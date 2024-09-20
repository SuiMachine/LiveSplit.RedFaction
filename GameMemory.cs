using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.ComponentUtil;
using System.Runtime.InteropServices;

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

        private Task _thread;
        private CancellationTokenSource _cancelSource;
        private SynchronizationContext _uiThread;
        private List<int> _ignorePIDs;
        private RedFactionSettings _settings;

        private DeepPointer _isLoadingPtr;
        private DeepPointer _levelNamePtr;
        private DeepPointer _binkMoviePlaying;
        ProcessModuleWow64Safe binkw32;

        private string[] validExeNames =
        {
            "pf",
            "rf",
            "rf_120na"
        };

        private string[] validWindowTitles =
        {
            "Red Faction",
            "Dash Faction",
            "Pure Faction"
        };

        public List<SplitStructOverall> currentSplits { get; set; }
        public bool[] splitStates { get; set; }

        public void resetSplitStates()
        {
            for (int i = 0; i < currentSplits.Count; i++)
            {
                splitStates[i] = false;
            }
        }

        public GameMemory(RedFactionSettings componentSettings)
        {
            _settings = componentSettings;
            currentSplits = componentSettings.Mods[componentSettings.ModIndex].Splits;
            splitStates = new bool[currentSplits.Count];

            _isLoadingPtr = new DeepPointer(0x13756AC); // == 1 if a loadscreen is happening
            _levelNamePtr = new DeepPointer(0x0246144, 0x0);
            _binkMoviePlaying = new DeepPointer("binkw32.dll", 0x41BD8);    //binkw32.dll+41BD8

            resetSplitStates();

            _ignorePIDs = new List<int>();
        }

        public void StartMonitoring()
        {
            if (_thread != null && _thread.Status == TaskStatus.Running)
            {
                throw new InvalidOperationException();
            }
            if (!(SynchronizationContext.Current is WindowsFormsSynchronizationContext))
            {
                throw new InvalidOperationException("SynchronizationContext.Current is not a UI thread.");
            }

            _uiThread = SynchronizationContext.Current;
            _cancelSource = new CancellationTokenSource();
            _thread = Task.Run(() => MemoryReadThread(_cancelSource.Token));
        }

        public void Stop()
        {
            if (_cancelSource == null || _thread == null || _thread.Status != TaskStatus.Running)
            {
                return;
            }

            _cancelSource.Cancel();
            _thread.Wait();
        }

// asynchronous memory read
async Task MemoryReadThread(CancellationToken cancellationToken)
    {
        Debug.WriteLine("[NoLoads] MemoryReadThread");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Debug.WriteLine("[NoLoads] Waiting for RF executable...");

                Process game = null;

                // asynchronously wait for the game process
                while ((game = await GetGameProcess()) == null)
                {
                    await Task.Delay(250);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }

                Debug.WriteLine("[NoLoads] Got game process!");

                uint frameCounter = 0;
                bool prevIsLoading = false;
                bool prevIsMoviePlaying = false;
                string prevLevelName = "";

                bool loadingStarted = false;

                // main game loop while the game is running
                while (!game.HasExited)
                {
                    bool isLoading;
                    bool isMoviePlaying;
                    string levelName = "";
                    _levelNamePtr.DerefString(game, 32, out levelName);
                    levelName = levelName?.ToLower() ?? "";

                    _isLoadingPtr.Deref(game, out isLoading);
                    _binkMoviePlaying.Deref(game, out isMoviePlaying);

                    // check for level change or bik movie state
                    if (levelName != prevLevelName && !string.IsNullOrEmpty(levelName) || isMoviePlaying != prevIsLoading)
                    {
#if DEBUG
                    Debug.WriteIf(levelName != prevLevelName, $"[NoLoads] Level change {prevLevelName} -> {levelName} (frame #{frameCounter})");
#endif

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
#if DEBUG
                        Debug.WriteLine("[NoLoads] Load Start - {frameCounter}");
#endif

                            loadingStarted = true;

                            // pause game timer
                            _uiThread.Post(d =>
                            {
                                this.OnLoadStarted?.Invoke(this, EventArgs.Empty);
                            }, null);

                            if (levelName == _settings.Mods[_settings.ModIndex].FirstLevel.ToLower() && isMoviePlaying)
                            {
                                // reset game timer
                                _uiThread.Post(d =>
                                {
                                    this.OnFirstLevelLoading?.Invoke(this, EventArgs.Empty);
                                }, null);
                            }
                        }
                        else
                        {
#if DEBUG
                        Debug.WriteLine($"[NoLoads] Load End - {frameCounter}");
#endif
                            if (loadingStarted)
                            {
                                loadingStarted = false;

                                // unpause game timer
                                _uiThread.Post(d =>
                                {
                                    this.OnLoadFinished?.Invoke(this, EventArgs.Empty);
                                }, null);

                                if (levelName == _settings.Mods[_settings.ModIndex].FirstLevel.ToLower())
                                {
                                    // start game timer
                                    _uiThread.Post(d =>
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

                    // non-blocking delay between each frame read
                    await Task.Delay(15);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                await Task.Delay(1000); // non-blocking delay in case of error
            }
        }
    }


    private void Split(int split, uint frame)
        {
#if DEBUG
            Debug.WriteLine($"[NoLoads] split {split} ({currentSplits[split].Name}) - {frame}");
#endif
            _uiThread.Post(d =>
            {
                if (this.OnSplitCompleted != null)
                {
                    this.OnSplitCompleted(this, split, frame);
                }
            }, null);
        }

        async Task<Process> GetGameProcess()
        {
            // find valid rf executables by exe name
            Process game = Process.GetProcesses().FirstOrDefault(p =>
                (validExeNames.Any(x => x == p.ProcessName.ToLower())) &&
                !p.HasExited &&
                !_ignorePIDs.Contains(p.Id)
            );

            // abort if no valid exe name found
            if (game == null)
            {
                return null;
            }

            // asynchronous 500ms delay to give time for the window title to populate before we check it
            await Task.Delay(1000);

            // retrieve window title and trim spaces
            string windowTitle = game.MainWindowTitle?.Trim();

            // confirm window title is valid, abort if not
            if (string.IsNullOrEmpty(windowTitle) ||
                !validWindowTitles.Any(title => windowTitle.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                _ignorePIDs.Add(game.Id);
                _uiThread.Send(d => MessageBox.Show($"Unexpected game version.",
                    "LiveSplit.RedFaction", MessageBoxButtons.OK, MessageBoxIcon.Error), null);
                return null;
            }

            // connfirm binkw32.dll is loaded, abort if not
            var binkw32 = game.ModulesWow64Safe().FirstOrDefault(p => p.ModuleName.ToLower() == "binkw32.dll");
            if (binkw32 == null)
            {
                return null;
            }

            return game; // return valid rf game process
        }
    }
}
