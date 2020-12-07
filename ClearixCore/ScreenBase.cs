using System;

using SFML.Graphics;
using SFML.Window;

namespace ClearixCore {
    /// <summary>
    /// A convenience class that both permits potential subclasses the ability to
    /// implement only the methods they really need as well as provide some concrete
    /// functions that each screen may need without having to repeat the process
    /// over and over again.
    /// </summary>
    class ScreenBase : Screen {
        public override void CheckGlobalInput ( Object sender, KeyEventArgs e ) {
            return;
        }

        public override void CheckPlayerInputPressed ( Object sender, KeyEventArgs e ) {
            if (this.CanProcessUserInput) {
                (Entities ["PlayerObject"] as PlayerEntity)?.CheckInputPressed (e);
            }
        }

        public override void CheckPlayerInputReleased ( Object sender, KeyEventArgs e ) {
            if (this.CanProcessUserInput) {
                (Entities ["PlayerObject"] as PlayerEntity)?.CheckInputReleased (e);
            }
        }

        public override void Draw ( RenderTarget target, RenderStates states ) {
            return;
        }

        public override void LoadAssets ( String archiveFile ) {
            return;
        }

        public override void Update ( Single delta ) {
            return;
        }
    }
}
