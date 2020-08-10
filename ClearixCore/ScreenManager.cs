using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearixCore {
    class ScreenManager {
        public Dictionary<ScreenMeta, Screen> Screens { get; }

        public Screen CurrentScreen { 
            get {
                return (from kvp in Screens where kvp.Key.CurrentlyUsed select kvp.Value).FirstOrDefault ();
            } 
        }

        public ScreenManager() {
            Screens = new Dictionary<ScreenMeta, Screen> () {
                {new ScreenMeta() {
                    ScreenName = "SplashScreen",
                    CurrentlyUsed = true
                }, new SampleScreen() }
            };
        }

        public void SetCurrentScreen(String screenName) {
            bool foundScreen = false;

            foreach (ScreenMeta sm in Screens.Keys.Where (sm => sm.ScreenName.Equals (screenName)))
                foundScreen = true;

            if (foundScreen) {
                foreach (ScreenMeta sm in Screens.Keys)
                    sm.CurrentlyUsed = false;

                foreach (ScreenMeta sm in Screens.Keys.Where (sm => sm.ScreenName.Equals (screenName)))
                    sm.CurrentlyUsed = true;
            } else
                return;
        }
    }
}
