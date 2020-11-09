using System;
using System.Collections.Generic;
using System.Text;

namespace ClearixCore {
    [Obsolete ("This class is no longer required. Please avoid directly using it.", true)]
    class ScreenMeta {
        public string ScreenName { get; set; }

        public bool CurrentlyUsed { get; set; }
    }
}
