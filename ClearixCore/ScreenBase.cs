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
        public override void CheckGlobalInput (Object sender, KeyEventArgs e) {
            if (e.Code == Keyboard.Key.Y) {
                this.cameraRotation = -5.0f;
            } else if (e.Code == Keyboard.Key.U) {
                this.cameraRotation = 5.0f;
            }
            if (e.Code == Keyboard.Key.H) {
                this.cameraScalar = -0.001f;
            } else if (e.Code == Keyboard.Key.J) {
                this.cameraScalar = 0.001f;
            }
        }

        public override void CheckPlayerInputPressed (Object sender, KeyEventArgs e) {
            if (this.Active) {
                (this.Entities ["PlayerObject"] as PlayerEntity)?.CheckInputPressed (e);
            }
        }

        public override void CheckPlayerInputReleased (Object sender, KeyEventArgs e) {
            if (this.Active) {
                (this.Entities ["PlayerObject"] as PlayerEntity)?.CheckInputReleased (e);
            }
        }

        public override void Draw (RenderTarget target, RenderStates states) {
            this.Camera.Rotate (this.cameraRotation);
            this.Camera.Zoom (this.cameraScalar);
            this.Camera.Viewport = new FloatRect (0.0f, 0.0f, 200.0f, 200.0f);
            target.SetView (this.Camera);
        }

        public override void LoadAssets (String archiveFile) {
            return;
        }

        public override void Update (Single delta) {
            return;
        }
    }
}
