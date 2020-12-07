using System;

using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class GameCore {
        private Boolean running;
        private Clock clock;
        private GameWindow gameWindow;
        private Single fpsd;
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
            gameWindow.Draw ();
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
                screenManager.CurrentScreen.CanProcessUserInput = !screenManager.CurrentScreen.CanProcessUserInput;
            }
            if (e.Code == Keyboard.Key.F8) {
                FaderAction action = gameWindow.fader.Action;

                if (action == FaderAction.FADING) {
                    gameWindow.fader.Action = FaderAction.IDLE;
                } else {
                    gameWindow.fader.Action = FaderAction.FADING;
                }
            }
        }

        private void CheckPlayerInputPressed ( Object sender, KeyEventArgs e ) {
            screenManager.CurrentScreen.CheckPlayerInputPressed (sender, e);
        }

        private void CheckPlayerInputReleased ( Object sender, KeyEventArgs e ) {
            screenManager.CurrentScreen.CheckPlayerInputReleased (sender, e);
        }
    }
}
