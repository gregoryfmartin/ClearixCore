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
            Screens = new List<Screen> () {
                new SampleScreen(),
                new AnotherScreen()
            };
            currentScreen = Screens [0];
        }

        public void ChangeCurrentScreen ( String screenName, GameWindow gameWindow ) {
            Boolean foundScreen = false;
            Screen r = null;

            foreach (Screen s in Screens) {
                if (s.Name.Equals (screenName)) {
                    foundScreen = true;
                    r = s;
                }
            }

            if (foundScreen) {
                if (currentScreen == null) {
                    currentScreen = r;
                    previousScreen = null;
                } else {
                    if ((gameWindow.Renderables.Find (x => (x as Screen).Equals (currentScreen)) is Screen t)) {
                        gameWindow.Renderables.Remove (t);
                    }
                    previousScreen = currentScreen;
                    currentScreen = r;
                }
            }
        }

        public void Update ( Single delta, GameWindow gameWindow ) {
            currentScreen.Update (delta);

            if (!(gameWindow.Renderables.Find (x => (x as Screen).Equals (currentScreen)) is Screen t)) {
                gameWindow.Renderables.Add (currentScreen);
            }
        }
    }
}
