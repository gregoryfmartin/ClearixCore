using System;
using System.Collections.Generic;
using System.Text;

using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class PlayerEntity : Entity {
        private Dictionary<String, Boolean> MovementStates { get; }

        private Vector2f Velocity { get; }

        public PlayerEntity() : base() {
            MovementStates = new Dictionary<String, Boolean> {
                {"Left", false},
                {"Right", false},
                {"Up", false},
                {"Down", false}
            };
            Velocity = new Vector2f(3.0f, 3.0f);
        }

        public void CheckInputPressed(KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Right:
                    MovementStates["Right"] = true;
                    break;
                case Keyboard.Key.Left:
                    MovementStates["Left"] = true;
                    break;
                case Keyboard.Key.Up:
                    MovementStates["Up"] = true;
                    break;
                case Keyboard.Key.Down:
                    MovementStates["Down"] = true;
                    break;
            }
        }

        public void CheckInputReleased(KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Right:
                    MovementStates["Right"] = false;
                    break;
                case Keyboard.Key.Left:
                    MovementStates["Left"] = false;
                    break;
                case Keyboard.Key.Up:
                    MovementStates["Up"] = false;
                    break;
                case Keyboard.Key.Down:
                    MovementStates["Down"] = false;
                    break;
            }
        }

        /// <summary>
        /// The scalar on the delta here is reliant upon the fact that the desired FPS is set to 60 and VSync is on.
        /// Otherwise, the delta becomes highly irregular and this scalar could easily result in neighbouring values
        /// ranging from 1 to 11. With VSync and nearly locked FPS, the value of Delta becomes considerably more
        /// predictable. There's definitely a better way of doing this, but I'm unsure what it would be at the moment.
        /// </summary>
        /// <param name="delta">The time between frames, taken from the main game loop through a multidelegate.</param>
        public override void Update(Single delta) {
            if (MovementStates["Right"]) {
                Position += new Vector2f(Velocity.X * (delta * 100), 0.0f);
            }

            if (MovementStates["Left"]) {
                Position += new Vector2f(-(Velocity.X * (delta * 100)), 0.0f);
            }

            if (MovementStates["Up"]) {
                Position += new Vector2f(0.0f, -(Velocity.Y * (delta * 100)));
            }

            if (MovementStates["Down"]) {
                Position += new Vector2f(0.0f, Velocity.Y * (delta * 100));
            }
        }
    }
}
