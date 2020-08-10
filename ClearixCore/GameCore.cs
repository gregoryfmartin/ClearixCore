using System;
using System.Collections.Generic;
using System.Text;

using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class GameCore {
        private bool running;
        private Clock clock;
        private GameWindow gameWindow;
        private float fpsd;
        private ScreenManager screenManager;

        public GameCore() {
            running = true;
            clock = null;
            gameWindow = null;
            screenManager = null;
        }

        public void Run () {
            Init ();

            while(running) {
                fpsd = clock.Restart ().AsSeconds ();

                Update ();
                Draw ();
            }

            Deinit ();
        }

        private void Init() {
            gameWindow = new GameWindow ();
            gameWindow.SetFramerateLimit (60);
            gameWindow.SetVerticalSyncEnabled (true);
            gameWindow.KeyDownHandler += CheckGlobalInput;
            gameWindow.KeyDownHandler += CheckPlayerInputPressed;
            gameWindow.KeyUpHandler += CheckPlayerInputReleased;

            screenManager = new ScreenManager ();

            clock = new Clock ();
        }

        private void Update() {
            gameWindow.Update (fpsd);
            screenManager.CurrentScreen.Update (fpsd);
            gameWindow.Renderables.Add (screenManager.CurrentScreen);
        }

        private void Draw() {
            gameWindow.Draw ();
        }

        private void Deinit() {
            gameWindow.Close ();
        }

        private void CheckGlobalInput(object sender, KeyEventArgs e) {
            if (e.Code == Keyboard.Key.Escape)
                running = false;
        }

        private void CheckPlayerInputPressed(object sender, KeyEventArgs e) {
            screenManager.CurrentScreen.CheckPlayerInputPressed (sender, e);
        }

        private void CheckPlayerInputReleased(object sender, KeyEventArgs e) {
            screenManager.CurrentScreen.CheckPlayerInputReleased (sender, e);
        }
    }
}
