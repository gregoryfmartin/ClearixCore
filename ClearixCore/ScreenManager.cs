using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearixCore {
    class ScreenManager {
        public List<Screen> Screens { get; }

        public Screen CurrentScreen { get { return currentScreen; } }
        private Screen currentScreen;

        public Screen PreviousScreen { get { return previousScreen; } }
        private Screen previousScreen;

        public ScreenManager () {
            this.Screens = new List<Screen> () {
                new SampleScreen(),
                new AnotherScreen()
            };
            this.currentScreen = this.Screens [0];
        }

        public void ChangeCurrentScreen (String screenName, GameWindow gameWindow) {
            Boolean foundScreen = false;
            Screen r = null;

            foreach (Screen s in this.Screens) {
                if (s.Name.Equals (screenName)) {
                    foundScreen = true;
                    r = s;
                }
            }

            if (foundScreen) {
                if (this.currentScreen == null) {
                    this.currentScreen = r;
                    this.previousScreen = null;
                } else {
                    if ((gameWindow.Renderables.Find (x => x is Screen && (x as Screen).Equals (this.currentScreen)) is Screen t)) {
                        gameWindow.Renderables.Remove (t);
                    }
                    this.previousScreen = this.currentScreen;
                    this.currentScreen = r;
                }
            }
        }

        public void Update (Single delta, GameWindow gameWindow) {
            this.currentScreen.Update (delta);

            if (!(gameWindow.Renderables.Find (x => x is Screen && (x as Screen).Equals (this.currentScreen)) is Screen t)) {
                gameWindow.Renderables.Add (this.currentScreen);
            }
            //if (!(gameWindow.Renderables.Find (x => (x as Screen).Equals (currentScreen)) is Screen t)) {
            //    gameWindow.Renderables.Add (currentScreen);
            //}
        }
    }
}
