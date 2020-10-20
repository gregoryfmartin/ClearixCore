using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearixCore {
    class ScreenManager {
        public List<Screen> Screens { get; }

        public Screen CurrentScreen {
            get {
                return (from s in Screens where s.CurrentlyUsed select s).FirstOrDefault();
            }
        }

        public ScreenManager() {
            Screens = new List<Screen>() {
                new SampleScreen() {
                    CurrentlyUsed = true
                },
                new AnotherScreen() {
                    CurrentlyUsed = false
                }
            };
        }

        public void ChangeCurrentScreen(String screenName, GameWindow gameWindow) {
            Boolean foundScreen = false;

            foreach (Screen s in Screens) {
                if (s.Name.Equals(screenName)) {
                    foundScreen = true;
                }
            }

            if (foundScreen) {
                if ((gameWindow.Renderables.Find(x => (x as Screen).Equals(CurrentScreen)) is Screen t)) {
                    gameWindow.Renderables.Remove(t);
                }

                foreach (Screen s in Screens) {
                    s.CurrentlyUsed = false;
                }
                foreach (Screen s in Screens) {
                    if (s.Name.Equals(screenName)) {
                        s.CurrentlyUsed = true;
                    }
                }
            } else {
                return;
            }
        }

        public void Update(Single delta, GameWindow gameWindow) {
            CurrentScreen.Update(delta);

            if (!(gameWindow.Renderables.Find(x => (x as Screen).Equals(CurrentScreen)) is Screen t)) {
                gameWindow.Renderables.Add(CurrentScreen);
            }
        }

        //public Dictionary<ScreenMeta, Screen> Screens { get; }

        //public Screen CurrentScreen { 
        //    get {
        //        return (from kvp in Screens where kvp.Key.CurrentlyUsed select kvp.Value).FirstOrDefault ();
        //    } 
        //}

        //public ScreenManager() {
        //    Screens = new Dictionary<ScreenMeta, Screen> () {
        //        {new ScreenMeta() {
        //            ScreenName = "SplashScreen",
        //            CurrentlyUsed = true
        //        }, new SampleScreen() }
        //    };
        //}

        //public void SetCurrentScreen(String screenName) {
        //    bool foundScreen = false;

        //    foreach (ScreenMeta sm in Screens.Keys.Where (sm => sm.ScreenName.Equals (screenName)))
        //        foundScreen = true;

        //    if (foundScreen) {
        //        foreach (ScreenMeta sm in Screens.Keys)
        //            sm.CurrentlyUsed = false;

        //        foreach (ScreenMeta sm in Screens.Keys.Where (sm => sm.ScreenName.Equals (screenName)))
        //            sm.CurrentlyUsed = true;
        //    } else
        //        return;
        //}
    }
}
