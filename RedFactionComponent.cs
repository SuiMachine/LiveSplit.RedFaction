using LiveSplit.Model;
using LiveSplit.TimeFormatters;
using LiveSplit.UI.Components;
using LiveSplit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

namespace LiveSplit.RedFaction
{
    class RedFactionComponent : LogicComponent
    {
        public override string ComponentName
        {
            get { return "RedFaction"; }
        }

        public RedFactionSettings Settings { get; set; }

        public bool Disposed { get; private set; }
        public bool IsLayoutComponent { get; private set; }

        private TimerModel _timer;
        private GameMemory _gameMemory;
        private LiveSplitState _state;

        public RedFactionComponent(LiveSplitState state, bool isLayoutComponent)
        {
            _state = state;
            this.IsLayoutComponent = isLayoutComponent;

            this.Settings = new RedFactionSettings();

           _timer = new TimerModel { CurrentState = state };
           _timer.CurrentState.OnPause += timer_OnStart;

            _gameMemory = new GameMemory(this.Settings);
            _gameMemory.OnFirstLevelLoading += gameMemory_OnFirstLevelLoading;
            _gameMemory.OnPlayerGainedControl += gameMemory_OnPlayerGainedControl;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnSplitCompleted += gameMemory_OnSplitCompleted;
            state.OnStart += State_OnStart;
            _gameMemory.StartMonitoring();
        }

        public override void Dispose()
        {
            this.Disposed = true;

            _state.OnStart -= State_OnStart;
            _timer.CurrentState.OnStart -= timer_OnStart;

            if (_gameMemory != null)
            {
                _gameMemory.Stop();
            }

        }

        private void timer_OnStart(object sender, EventArgs e)
        {
            _timer.InitializeGameTime();
        }

        void State_OnStart(object sender, EventArgs e)
        {
            _gameMemory.resetSplitStates();
        }

        void gameMemory_OnFirstLevelLoading(object sender, EventArgs e)
        {
            if (this.Settings.AutoReset)
            {
                _timer.Reset();
            }
        }

        void gameMemory_OnPlayerGainedControl(object sender, EventArgs e)
        {
            if (this.Settings.AutoStart)
            {
                _timer.Start();
            }
        }

        void gameMemory_OnLoadStarted(object sender, EventArgs e)
        {
            _state.IsGameTimePaused = true;
        }

        void gameMemory_OnLoadFinished(object sender, EventArgs e)
        {
            _state.IsGameTimePaused = false;
        }

        void gameMemory_OnSplitCompleted(object sender, GameMemory.SplitArea split, uint frame)
        {
            Debug.WriteLineIf(split != GameMemory.SplitArea.None, String.Format("[NoLoads] Trying to split {0}, State: {1} - {2}", split, _gameMemory.splitStates[(int)split], frame));
            if (_state.CurrentPhase == TimerPhase.Running && !_gameMemory.splitStates[(int)split] &&
                ((split == GameMemory.SplitArea.Chapter1 && this.Settings.sC01) ||
                (split == GameMemory.SplitArea.Chapter2 && this.Settings.sC02) ||
                (split == GameMemory.SplitArea.Chapter3 && this.Settings.sC03) ||
                (split == GameMemory.SplitArea.Chapter4 && this.Settings.sC04) ||
                (split == GameMemory.SplitArea.Chapter5 && this.Settings.sC05) ||
                (split == GameMemory.SplitArea.Chapter6 && this.Settings.sC06) ||
                (split == GameMemory.SplitArea.Chapter7 && this.Settings.sC07) ||
                (split == GameMemory.SplitArea.Chapter8 && this.Settings.sC08) ||
                (split == GameMemory.SplitArea.Chapter9 && this.Settings.sC09) ||
                (split == GameMemory.SplitArea.Chapter10 && this.Settings.sC10) ||
                (split == GameMemory.SplitArea.Chapter11 && this.Settings.sC11) ||
                (split == GameMemory.SplitArea.Chapter12 && this.Settings.sC12) ||
                (split == GameMemory.SplitArea.Chapter13 && this.Settings.sC13) ||
                (split == GameMemory.SplitArea.Chapter14 && this.Settings.sC14) ||
                (split == GameMemory.SplitArea.Chapter15 && this.Settings.sC15) ||
                (split == GameMemory.SplitArea.Chapter16 && this.Settings.sC16) ||
                (split == GameMemory.SplitArea.Chapter17 && this.Settings.sC17) ||
                (split == GameMemory.SplitArea.Chapter18 && this.Settings.sC18) ||
                (split == GameMemory.SplitArea.Chapter19 && this.Settings.sC19) ||
                (split == GameMemory.SplitArea.Bomb && this.Settings.sCBomb)))
            {
                Trace.WriteLine(String.Format("[NoLoads] {0} Split - {1}", split, frame));
                _timer.Split();
                _gameMemory.splitStates[(int)split] = true;
            }
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return this.Settings.GetSettings(document);
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            return this.Settings;
        }

        public override void SetSettings(XmlNode settings)
        {
            this.Settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
        //public override void RenameComparison(string oldName, string newName) { }
    }
}
