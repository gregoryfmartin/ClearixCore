using System;

using SFML.System;
using SFML.Window;

namespace ClearixCore {
    /// <summary>
    /// Represents the governor for the engine itself. This is essentially the bottom-layer of the engine
    /// that operates the rest of the abstractions on higher levels.
    /// </summary>
    class GameCore {
        /// <summary>
        /// States if the game is running or not. This is the governor for the game loop.
        /// </summary>
        private Boolean running;

        /// <summary>
        /// The primary game clock.
        /// </summary>
        private Clock clock;

        /// <summary>
        /// The primary render window that the user will interact with.
        /// </summary>
        private GameWindow gameWindow;

        /// <summary>
        /// FPS Delta; obtained each frame.
        /// </summary>
        private Single fpsd;

        /// <summary>
        /// A ScreenManager instance for managing screens.
        /// </summary>
        private ScreenManager screenManager;

        public GameCore () {
            this.running = true;
            this.clock = null;
            this.gameWindow = null;
            this.screenManager = null;
        }

        public void Run () {
            this.Init ();

            while (this.running) {
                this.fpsd = this.clock.Restart ().AsSeconds ();

                this.Update ();
                this.Draw ();
            }

            this.Deinit ();
        }

        private void Init () {
            this.gameWindow = new GameWindow ();
            this.gameWindow.SetFramerateLimit (60);
            this.gameWindow.SetVerticalSyncEnabled (true);
            this.gameWindow.KeyDownHandler += CheckGlobalInput;
            this.gameWindow.KeyDownHandler += CheckPlayerInputPressed;
            this.gameWindow.KeyUpHandler += CheckPlayerInputReleased;

            this.screenManager = new ScreenManager ();

            this.clock = new Clock ();
        }

        private void Update () {
            this.gameWindow.Update (this.fpsd);
            this.screenManager.Update (this.fpsd, this.gameWindow);
        }

        private void Draw () {
            this.gameWindow.DrawStuff ();
        }

        private void Deinit () {
            this.gameWindow.Close ();
        }

        private void CheckGlobalInput (Object sender, KeyEventArgs e) {
            if (e.Code == Keyboard.Key.Escape) {
                this.running = false;
            }
            if (e.Code == Keyboard.Key.F6) {
                if (this.screenManager.CurrentScreen.Name.Equals ("SampleScreen")) {
                    this.screenManager.ChangeCurrentScreen ("AnotherScreen", this.gameWindow);
                } else if (this.screenManager.CurrentScreen.Name.Equals ("AnotherScreen")) {
                    this.screenManager.ChangeCurrentScreen ("SampleScreen", this.gameWindow);
                }
            }
            if (e.Code == Keyboard.Key.F7) {
                this.screenManager.CurrentScreen.Active = !this.screenManager.CurrentScreen.Active;
            }
            if (e.Code == Keyboard.Key.F8) {
                FaderAction action = this.gameWindow.MyFader.Action;

                if (action == FaderAction.FADING) {
                    this.gameWindow.MyFader.Action = FaderAction.IDLE;
                } else {
                    this.gameWindow.MyFader.Action = FaderAction.FADING;
                }
            }
            this.screenManager.CurrentScreen.CheckGlobalInput (sender, e);
        }

        private void CheckPlayerInputPressed (Object sender, KeyEventArgs e) {
            this.screenManager.CurrentScreen.CheckPlayerInputPressed (sender, e);
        }

        private void CheckPlayerInputReleased (Object sender, KeyEventArgs e) {
            this.screenManager.CurrentScreen.CheckPlayerInputReleased (sender, e);
        }
    }
}
