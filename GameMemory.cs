using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.ComponentUtil;

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

        private enum ExpectedDllSizes
        {
            PureFaction30d = 29945856,
            RedFaction1_20 = 29917184
        }

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
            _thread = Task.Factory.StartNew(MemoryReadThread);
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

        void MemoryReadThread()
        {
            Debug.WriteLine("[NoLoads] MemoryReadThread");

            while (!_cancelSource.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine("[NoLoads] Waiting for pf.exe...");

                    Process game;
                    while ((game = GetGameProcess()) == null)
                    {
                        Thread.Sleep(250);
                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    Debug.WriteLine("[NoLoads] Got games process!");

                    uint frameCounter = 0;

                    bool prevIsLoading = false;
                    bool prevIsMoviePlaying = false;
                    string prevLevelName = "";


                    bool loadingStarted = false;

                    while (!game.HasExited)
                    {
                        bool isLoading;
                        bool isMoviePlaying;
                        string levelName = "";
                        _levelNamePtr.DerefString(game, 10, out levelName);
                        levelName = levelName != null ? levelName.ToLower() : "";  //cause it can read null if the game started off fresh and then you'd try to convert it to lowercase and would get exception
                        _isLoadingPtr.Deref(game, out isLoading);
                        _binkMoviePlaying.Deref(game, out isMoviePlaying);

                        if (levelName != prevLevelName && levelName != null || isMoviePlaying != prevIsLoading)
                        {
                            for(int i=0; i<splitStates.Length; i++)
							{
                                if(!splitStates[i])
								{
                                    currentSplits[i].Check(in levelName, in prevLevelName, isMoviePlaying, ref splitStates[i]);
								}
							}
                        }


                        _isLoadingPtr.Deref(game, out isLoading);

                        if (isLoading != prevIsLoading || prevIsMoviePlaying != isMoviePlaying)
                        {
                            if (isLoading)
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load Start - {0}", frameCounter));

                                loadingStarted = true;

                                // pause game timer
                                _uiThread.Post(d =>
                                {
                                    if (this.OnLoadStarted != null)
                                    {
                                        this.OnLoadStarted(this, EventArgs.Empty);
                                    }
                                }, null);

                                if (levelName == "l1s1.rfl" && isMoviePlaying)
                                {
                                    //reset game timer
                                    _uiThread.Post(d =>
                                    {
                                        if (this.OnFirstLevelLoading != null)
                                        {
                                            this.OnFirstLevelLoading(this, EventArgs.Empty);
                                        }
                                    }, null);
                                }
                            }
                            else
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load End - {0}", frameCounter));
                                if (loadingStarted)
                                {
                                    loadingStarted = false;

                                    // unpause game timer
                                    _uiThread.Post(d =>
                                    {
                                        if (this.OnLoadFinished != null)
                                        {
                                            this.OnLoadFinished(this, EventArgs.Empty);
                                        }
                                    }, null);

                                    if (levelName == "l1s1.rfl")
                                    {
                                        // start game timer
                                        _uiThread.Post(d =>
                                        {
                                            if (this.OnPlayerGainedControl != null)
                                            {
                                                this.OnPlayerGainedControl(this, EventArgs.Empty);
                                            }
                                        }, null);
                                    }
                                }
                            }
                        }


                        Debug.WriteLineIf(levelName != prevLevelName, String.Format("[NoLoads] streamGroupId changed from {0} to {1} - {2}", prevLevelName, levelName, frameCounter));
                        prevLevelName = levelName;
                        prevIsLoading = isLoading;
                        prevIsMoviePlaying = isMoviePlaying;
                        
                        frameCounter++;

                        Thread.Sleep(15);

                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }

        private void Split(int split, uint frame)
        {
            Debug.WriteLine(String.Format("[NoLoads] split {0} - {1}", split, frame));
            _uiThread.Post(d =>
            {
                if (this.OnSplitCompleted != null)
                {
                    this.OnSplitCompleted(this, split, frame);
                }
            }, null);
        }

        Process GetGameProcess()
        {
            Process game = Process.GetProcesses().FirstOrDefault(p => (validExeNames.Any(x => x == p.ProcessName.ToLower()))
                && !p.HasExited && !_ignorePIDs.Contains(p.Id));
            if (game == null)
            {
                return null;
            }

            binkw32 = game.ModulesWow64Safe().FirstOrDefault(p => p.ModuleName.ToLower() == "binkw32.dll");
            if (binkw32 == null)
                return null;

            var mainModuleSize = game.MainModuleWow64Safe().ModuleMemorySize;

            if (mainModuleSize == (int)ExpectedDllSizes.PureFaction30d || mainModuleSize == (int)ExpectedDllSizes.RedFaction1_20)
            {

            }
            else
            {
                _ignorePIDs.Add(game.Id);
                _uiThread.Send(d => MessageBox.Show("Unexpected game version. Red Faction 1.20 (including DashFaction) or Pure Faction 3.0d is required", "LiveSplit.RedFaction",
                    MessageBoxButtons.OK, MessageBoxIcon.Error), null);
                return null;
            }

            return game;
        }
    }
}
