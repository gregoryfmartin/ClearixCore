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
            running = true;
            clock = null;
            gameWindow = null;
            screenManager = null;
        }

        public void Run () {
            Init ();

            while (running) {
                fpsd = clock.Restart ().AsSeconds ();

                Update ();
                Draw ();
            }

            Deinit ();
        }

        private void Init () {
            gameWindow = new GameWindow ();
            gameWindow.SetFramerateLimit (60);
            gameWindow.SetVerticalSyncEnabled (true);
            gameWindow.KeyDownHandler += CheckGlobalInput;
            gameWindow.KeyDownHandler += CheckPlayerInputPressed;
            gameWindow.KeyUpHandler += CheckPlayerInputReleased;

            screenManager = new ScreenManager ();

            clock = new Clock ();
        }

        private void Update () {
            gameWindow.Update (fpsd);
            screenManager.Update (fpsd, gameWindow);
        }

        private void Draw () {
            gameWindow.DrawStuff ();
        }

        private void Deinit () {
            gameWindow.Close ();
        }

        private void CheckGlobalInput ( Object sender, KeyEventArgs e ) {
            if (e.Code == Keyboard.Key.Escape) {
                running = false;
            }
            if (e.Code == Keyboard.Key.F6) {
                if (screenManager.CurrentScreen.Name.Equals ("SampleScreen")) {
                    screenManager.ChangeCurrentScreen ("AnotherScreen", gameWindow);
                } else if (screenManager.CurrentScreen.Name.Equals ("AnotherScreen")) {
                    screenManager.ChangeCurrentScreen ("SampleScreen", gameWindow);
                }
            }
            if (e.Code == Keyboard.Key.F7) {
                screenManager.CurrentScreen.Active = !screenManager.CurrentScreen.Active;
            }
            if (e.Code == Keyboard.Key.F8) {
                FaderAction action = gameWindow.fader.Action;

                if (action == FaderAction.FADING) {
                    gameWindow.fader.Action = FaderAction.IDLE;
                } else {
                    gameWindow.fader.Action = FaderAction.FADING;
                }
            }
            screenManager.CurrentScreen.CheckGlobalInput (sender, e);
        }

        private void CheckPlayerInputPressed ( Object sender, KeyEventArgs e ) {
            screenManager.CurrentScreen.CheckPlayerInputPressed (sender, e);
        }

        private void CheckPlayerInputReleased ( Object sender, KeyEventArgs e ) {
            screenManager.CurrentScreen.CheckPlayerInputReleased (sender, e);
        }
    }
}
