using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.RedFaction
{
	class GameMemory
	{
		public event EventHandler OnPlayerGainedControl;
		public event EventHandler OnLoadStarted;
		public event EventHandler OnFirstLevelLoading;
		public event EventHandler OnLoadFinished;
		public delegate void SplitCompletedEventHandler(object sender, int split, uint frame);
		public event SplitCompletedEventHandler OnSplitCompleted;

		private Task m_Thread;
		private CancellationTokenSource m_CancelSource;
		private SynchronizationContext m_UiThread;
		private List<int> m_IgnorePIDs;
		private RedFactionSettings m_Settings;

		private DeepPointer m_IsLoadingPtr;
		private DeepPointer m_LevelNamePtr;
		private DeepPointer m_BinkMoviePlaying;

		private readonly string[] validExeNames =
		{
			"pf",
			"rf",
			"rf_120na"
		};

		private readonly string[] validWindowTitles =
		{
			"Red Faction",
			"Dash Faction",
			"Pure Faction",
			"Alpine Faction"
		};

		public List<SplitStructOverall> currentSplits { get; set; }
		public bool[] splitStates { get; set; }

		public void ResetSplitStates()
		{
			for (int i = 0; i < currentSplits.Count; i++)
			{
				splitStates[i] = false;
			}
		}

		public GameMemory(RedFactionSettings componentSettings)
		{
			m_Settings = componentSettings;
			currentSplits = componentSettings.ModStates[componentSettings.ModIndex].Splits;
			splitStates = new bool[currentSplits.Count];

			m_IsLoadingPtr = new DeepPointer(0x13756AC); // == 1 if a loadscreen is happening
			m_LevelNamePtr = new DeepPointer(0x0246144, 0x0);
			m_BinkMoviePlaying = new DeepPointer("binkw32.dll", 0x41BD8);    //binkw32.dll+41BD8

			ResetSplitStates();

			m_IgnorePIDs = new List<int>();
		}

		public void StartMonitoring()
		{
			if (m_Thread != null && m_Thread.Status == TaskStatus.Running)
			{
				throw new InvalidOperationException();
			}
			if (!(SynchronizationContext.Current is WindowsFormsSynchronizationContext))
			{
				throw new InvalidOperationException("SynchronizationContext.Current is not a UI thread.");
			}

			m_UiThread = SynchronizationContext.Current;
			m_CancelSource = new CancellationTokenSource();
			m_Thread = Task.Run(() => MemoryReadThread(m_CancelSource.Token));
		}

		public void Stop()
		{
			if (m_CancelSource == null || m_Thread == null || m_Thread.Status != TaskStatus.Running)
			{
				return;
			}

			m_CancelSource.Cancel();
			m_Thread.Wait();
		}

		private void MemoryReadThread(CancellationToken cancellationToken)
		{
			Utils.WriteDebug("[NoLoads] MemoryReadThread");

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					Utils.WriteDebug("[NoLoads] Waiting for RF executable...");

					Process game = null;

					// wait for the game process (blocking wait until the process is found or canceled)
					while ((game = GetGameProcess()) == null)
					{
						if (cancellationToken.IsCancellationRequested)
						{
							return;
						}
					}

					Utils.WriteDebug("[NoLoads] Got game process!");

					uint frameCounter = 0;
					bool prevIsLoading = false;
					bool prevIsMoviePlaying = false;
					string prevLevelName = "";

					bool loadingStarted = false;

					// main game loop while the game is running
					while (!game.HasExited)
					{
						m_LevelNamePtr.DerefString(game, 32, out string levelName);
						levelName = levelName?.ToLower() ?? "";

						m_IsLoadingPtr.Deref(game, out bool isLoading);
						m_BinkMoviePlaying.Deref(game, out bool isMoviePlaying);

						// check for level change or bik movie state
						if (levelName != prevLevelName && !string.IsNullOrEmpty(levelName) || isMoviePlaying != prevIsMoviePlaying)
						{
							Utils.WriteDebugIf(levelName != prevLevelName, $"[NoLoads] Level change {prevLevelName} -> {levelName} (frame #{frameCounter})");

							for (int i = 0; i < splitStates.Length; i++)
							{
								if (!splitStates[i] && currentSplits[i].Check(in levelName, in prevLevelName, isMoviePlaying))
								{
									Split(i, frameCounter);
								}
							}
						}

						// handle loading states
						if (isLoading != prevIsLoading || prevIsMoviePlaying != isMoviePlaying)
						{
							if (isLoading)
							{
								Utils.WriteDebug("[NoLoads] Load Start - {frameCounter}");

								loadingStarted = true;

								// pause game timer
								m_UiThread.Post(d =>
								{
									this.OnLoadStarted?.Invoke(this, EventArgs.Empty);
								}, null);

								if (levelName == m_Settings.ModStates[m_Settings.ModIndex].FirstLevel.ToLower() && isMoviePlaying)
								{
									// reset game timer
									m_UiThread.Post(d =>
									{
										this.OnFirstLevelLoading?.Invoke(this, EventArgs.Empty);
									}, null);
								}
							}
							else
							{
								Utils.WriteDebug($"[NoLoads] Load End - {frameCounter}");
								if (loadingStarted)
								{
									loadingStarted = false;

									// unpause game timer
									m_UiThread.Post(d =>
									{
										this.OnLoadFinished?.Invoke(this, EventArgs.Empty);
									}, null);

									if (levelName == m_Settings.ModStates[m_Settings.ModIndex].FirstLevel.ToLower())
									{
										// start game timer
										m_UiThread.Post(d =>
										{
											this.OnPlayerGainedControl?.Invoke(this, EventArgs.Empty);
										}, null);
									}
								}
							}
						}

						prevLevelName = levelName;
						prevIsLoading = isLoading;
						prevIsMoviePlaying = isMoviePlaying;

						frameCounter++;

						Thread.Sleep(15); // non-blocking delay between each frame read

						if (cancellationToken.IsCancellationRequested)
						{
							return;
						}
					}
				}
				catch (Exception ex)
				{
					Utils.WriteDebug(ex.ToString());
					Thread.Sleep(1000); // blocking delay in case of error
				}
			}
		}

		private void Split(int split, uint frame)
		{
			Utils.WriteDebug($"[NoLoads] split {split} ({currentSplits[split].Name}) - {frame}");
			m_UiThread.Post(d =>
			{
				if (this.OnSplitCompleted != null)
				{
					this.OnSplitCompleted(this, split, frame);
				}
			}, null);
		}

		private Process GetGameProcess()
		{
			// find valid rf executables by exe name
			Process game = Process.GetProcesses().FirstOrDefault(p =>
				(validExeNames.Any(x => x == p.ProcessName.ToLower())) &&
				!p.HasExited &&
				!m_IgnorePIDs.Contains(p.Id)
			);

			// abort if no valid exe name found
			if (game == null)
			{
				return null;
			}

			string windowTitle = null;

			// keep checking until the window has a title, the process exits, or a 3-second timeout elapses
			var stopwatch = Stopwatch.StartNew();
			while (string.IsNullOrEmpty(windowTitle))
			{
				if (game.HasExited)
				{
					return null; // process exited before it got a title
				}

				if (stopwatch.ElapsedMilliseconds > 3000)
				{
					return null; // timeout after 3 seconds
				}

				Thread.Sleep(50); // wait 50ms before trying again
				windowTitle = game.MainWindowTitle?.Trim();
			}

			// confirm window title is valid, abort if not
			if (!validWindowTitles.Any(title => windowTitle.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0))
			{
				m_IgnorePIDs.Add(game.Id);
				m_UiThread.Send(d => MessageBox.Show($"Unexpected game window title.",
					"LiveSplit.RedFaction", MessageBoxButtons.OK, MessageBoxIcon.Error), null);
				return null;
			}

			// confirm binkw32.dll is loaded, abort if not
			var binkw32 = game.ModulesWow64Safe().FirstOrDefault(p => p.ModuleName.ToLower() == "binkw32.dll");
			if (binkw32 == null)
			{
				return null;
			}

			return game; // return valid RF game process
		}
	}
}
