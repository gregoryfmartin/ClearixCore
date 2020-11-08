using System;

using SFML.Graphics;
using SFML.System;

namespace ClearixCore {
    enum FaderState {
        TRANSPARENT,
        OPAQUE
    }

    enum FaderAction {
        IDLE,
        FADING
    }

    enum FaderSpeed : Byte {
        SUPER_SLOW = 1,
        SLOW = 5,
        MEDIUM = 15,
        FAST = 51,
        SUPER_FAST = 85
    }

    class Fader : RectangleShape {
        public FaderState State { get; set; }

        public FaderAction Action { get; set; }

        public FaderSpeed Speed { get; set; }

        private Single deltaCounter;

        public Fader () : base () {
            this.State = FaderState.TRANSPARENT;
            this.Action = FaderAction.IDLE;
            this.FillColor = new Color (255, 255, 255, 0);
            this.Speed = FaderSpeed.MEDIUM;
        }

        public Fader ( Vector2f size ) : base (size) {
            this.State = FaderState.TRANSPARENT;
            this.Action = FaderAction.IDLE;
            this.FillColor = new Color (255, 255, 255, 0);
            this.Speed = FaderSpeed.MEDIUM;
        }

        public void Update ( Single delta ) {
            Single frame = 1 / 60;
            Color fc = this.FillColor;

            this.deltaCounter += delta;
            if (this.deltaCounter >= frame) {
                this.deltaCounter = 0.0f;

                if (this.Action == FaderAction.FADING) {
                    if (this.State == FaderState.OPAQUE && this.FillColor.A > 0) {
                        fc.A -= (Byte)this.Speed; 
                        if (fc.A <= 0) {
                            this.Action = FaderAction.IDLE;
                        }
                    } else if (this.State == FaderState.TRANSPARENT && this.FillColor.A < 255) {
                        fc.A += (Byte)this.Speed; 
                        if (fc.A >= 255) {
                            this.Action = FaderAction.IDLE;
                        }
                    }
                }
            }

            if (this.State == FaderState.OPAQUE && fc.A <= 0) {
                this.State = FaderState.TRANSPARENT;
            } else if (this.State == FaderState.TRANSPARENT && fc.A >= 255) {
                this.State = FaderState.OPAQUE;
            }
            this.FillColor = fc;
        }
    }
}
