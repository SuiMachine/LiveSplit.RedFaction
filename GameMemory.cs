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
        public enum SplitArea : int
        {
            None,
            Chapter1,
            Chapter2,
            Chapter3,
            Chapter4,
            Chapter5,
            Chapter6,
            Chapter7,
            Chapter8,
            Chapter9,
            Chapter10,
            Chapter11,
            Chapter12,
            Chapter13,
            Chapter14,
            Chapter15,
            Chapter16,
            Chapter17,
            Chapter18,
            Chapter19,
            Bomb
        }

        public event EventHandler OnPlayerGainedControl;
        public event EventHandler OnLoadStarted;
        public event EventHandler OnFirstLevelLoading;
        public event EventHandler OnLoadFinished;
        public delegate void SplitCompletedEventHandler(object sender, SplitArea type, uint frame);
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

        private static class LevelName
        {
            public const string Chapter1Start = "l1s1.rfl";
            public const string Chapter1Exit = "l1s3.rfl";
            public const string Chapter2Start = "l2s1.rfl";
            public const string Chapter2Exit = "l2s3.rfl";
            public const string Chapter3Start = "l3s1.rfl";
            public const string Chapter3Exit = "l3s4.rfl";
            public const string Chapter4Start = "l4s1a.rfl";
            public const string Chapter4StartB = "l4s1b.rfl";
            public const string Chapter4Exit = "l4s4.rfl";
            public const string Chapter5Start = "l5s1.rfl";
            public const string Chapter5Exit = "l5s4.rfl";
            public const string Chapter6Start = "l6s1.rfl";         //Administration
            public const string Chapter6Exit = "l6s3.rfl";
            public const string Chapter7Start = "l7s1.rfl";         //Backstage
            public const string Chapter7Exit = "l7s4.rfl";
            public const string Chapter8Start = "l8s1.rfl";         //Medical Labs
            public const string Chapter8End = "l8s4.rfl";
            public const string Chapter9Start = "l9s1.rfl";         //Caves
            public const string Chapter9End = "l9s4.rfl";
            public const string Chapter10Start = "l10s1.rfl";         //Zoo
            public const string Chapter10End = "l10s4.rfl";
            public const string Chapter11Start = "l11s1.rfl";         //Capek Secret Facility
            public const string Chapter11End = "l11s3.rfl";
            public const string Chapter12Start = "l12s1.rfl";         //Canion
            public const string Chapter13Start = "l13s1.rfl";         //Satelite Control
            public const string Chapter13End = "l13s3.rfl";
            public const string Chapter14Start = "l14s1.rfl";         //Missile Command Center
            public const string Chapter14End = "l14s3.rfl";
            public const string Chapter15Start = "l15s1.rfl";         //Catch a shuttle
            public const string Chapter15End = "l15s4.rfl";
            public const string Chapter16Start = "l17s1.rfl";         //Spacestation (chapter 16 missing in game's files)
            public const string Chapter16End = "l17s4.rfl";
            public const string Chapter17Start = "l18s1.rfl";         //Back on Mars
            public const string Chapter17End = "l18s3.rfl";
            public const string Chapter18Start = "l19s1.rfl";         //Prision
            public const string Chapter18End = "l19s3.rfl";
            public const string Chapter19Start = "l20s1.rfl";         //Merc Base
            public const string Chapter19End = "l20s2.rfl";
            public const string Bomb = "l20s3.rfl";           //A bomb

        }

        private enum ExpectedDllSizes
        {
            PureFaction30d = 29945856
        }

        public bool[] splitStates { get; set; }

        public void resetSplitStates()
        {
            for (int i = 0; i <= (int)SplitArea.Bomb; i++)
            {
                splitStates[i] = false;
            }

        }

        public GameMemory(RedFactionSettings componentSettings)
        {
            _settings = componentSettings;
            splitStates = new bool[(int)SplitArea.Bomb + 1];

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
                    string prevStreamGroupId = String.Empty;


                    bool loadingStarted = false;

                    while (!game.HasExited)
                    {
                        bool isLoading;
                        bool isMoviePlaying;
                        string streamGroupId = String.Empty;
                        _levelNamePtr.DerefString(game, 10, out streamGroupId);
                        streamGroupId = streamGroupId.ToLower();
                        _isLoadingPtr.Deref(game, out isLoading);
                        _binkMoviePlaying.Deref(game, out isMoviePlaying);

                        if (streamGroupId != prevStreamGroupId && streamGroupId != null || isMoviePlaying != prevIsLoading)
                        {
                            if (prevStreamGroupId == LevelName.Chapter1Exit && streamGroupId == LevelName.Chapter2Start)        //Mines
                            {
                                Split(SplitArea.Chapter1, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter2Exit && streamGroupId == LevelName.Chapter3Start)   //Barracks
                            {
                                Split(SplitArea.Chapter2, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter3Exit && streamGroupId == LevelName.Chapter4Start)   //Reception & Docks
                            {
                                Split(SplitArea.Chapter3, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter4Exit && streamGroupId == LevelName.Chapter5Start)   //Ventilation
                            {
                                Split(SplitArea.Chapter4, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter5Exit && streamGroupId == LevelName.Chapter6Start)  //Geothermal Plant
                            {
                                Split(SplitArea.Chapter5, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter6Exit && streamGroupId == LevelName.Chapter7Start)
                            {
                                Split(SplitArea.Chapter6, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter7Exit && streamGroupId == LevelName.Chapter8Start)
                            {
                                Split(SplitArea.Chapter7, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter8End && streamGroupId == LevelName.Chapter9Start)
                            {
                                Split(SplitArea.Chapter8, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter9End && streamGroupId == LevelName.Chapter10Start)
                            {
                                Split(SplitArea.Chapter9, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter10End && streamGroupId == LevelName.Chapter11Start)
                            {
                                Split(SplitArea.Chapter10, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter11End && streamGroupId == LevelName.Chapter12Start)
                            {
                                Split(SplitArea.Chapter11, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter12Start && streamGroupId == LevelName.Chapter13Start)
                            {
                                Split(SplitArea.Chapter12, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter13End && streamGroupId == LevelName.Chapter14Start)
                            {
                                Split(SplitArea.Chapter13, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter14End && streamGroupId == LevelName.Chapter15Start)
                            {
                                Split(SplitArea.Chapter14, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter15End && streamGroupId == LevelName.Chapter16Start)
                            {
                                Split(SplitArea.Chapter15, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter16End && streamGroupId == LevelName.Chapter17Start)
                            {
                                Split(SplitArea.Chapter16, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter17End && streamGroupId == LevelName.Chapter18Start)
                            {
                                Split(SplitArea.Chapter17, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter18End && streamGroupId == LevelName.Chapter19Start)
                            {
                                Split(SplitArea.Chapter18, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.Chapter19End && streamGroupId == LevelName.Bomb)
                            {
                                Split(SplitArea.Chapter19, frameCounter);
                            }
                            else if (streamGroupId == LevelName.Bomb && isMoviePlaying)
                            {
                                Split(SplitArea.Bomb, frameCounter);
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

                                if (streamGroupId == LevelName.Chapter1Start && isMoviePlaying)
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

                                    if (streamGroupId == LevelName.Chapter1Start)
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


                        Debug.WriteLineIf(streamGroupId != prevStreamGroupId, String.Format("[NoLoads] streamGroupId changed from {0} to {1} - {2}", prevStreamGroupId, streamGroupId, frameCounter));
                        prevStreamGroupId = streamGroupId;
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

        private void Split(SplitArea split, uint frame)
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
            Process game = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.ToLower() == "pf" && !p.HasExited && !_ignorePIDs.Contains(p.Id));
            if (game == null)
            {
                return null;
            }

            binkw32 = game.ModulesWow64Safe().FirstOrDefault(p => p.ModuleName.ToLower() == "binkw32.dll");
            if (binkw32 == null)
                return null;

            if (game.MainModuleWow64Safe().ModuleMemorySize == (int)ExpectedDllSizes.PureFaction30d)
            {

            }
            else
            {
                _ignorePIDs.Add(game.Id);
                _uiThread.Send(d => MessageBox.Show("Unexpected game version. Red Faction (Pure Faction 3.0d) is required", "LiveSplit.RedFaction",
                    MessageBoxButtons.OK, MessageBoxIcon.Error), null);
                return null;
            }

            return game;
        }
    }
}
