using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class PlayerEntity : Entity {
        private Dictionary<String, Boolean> MovementStates { get; }

        private Vector2f Velocity { get; }

        public PlayerEntity () : base () {
            this.MovementStates = new Dictionary<String, Boolean> {
                {"Left", false},
                {"Right", false},
                {"Up", false},
                {"Down", false}
            };
            this.Velocity = new Vector2f (3.0f, 3.0f);
        }

        public void CheckInputPressed (KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Right:
                    this.MovementStates ["Right"] = true;
                    break;
                case Keyboard.Key.Left:
                    this.MovementStates ["Left"] = true;
                    break;
                case Keyboard.Key.Up:
                    this.MovementStates ["Up"] = true;
                    break;
                case Keyboard.Key.Down:
                    this.MovementStates ["Down"] = true;
                    break;
            }
        }

        public void CheckInputReleased (KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Right:
                    this.MovementStates ["Right"] = false;
                    break;
                case Keyboard.Key.Left:
                    this.MovementStates ["Left"] = false;
                    break;
                case Keyboard.Key.Up:
                    this.MovementStates ["Up"] = false;
                    break;
                case Keyboard.Key.Down:
                    this.MovementStates ["Down"] = false;
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
        public override void Update (Single delta) {
            if (this.MovementStates ["Right"]) {
                this.Position += new Vector2f (this.Velocity.X * (delta * 100), 0.0f);
            }

            if (this.MovementStates ["Left"]) {
                this.Position += new Vector2f (-(this.Velocity.X * (delta * 100)), 0.0f);
            }

            if (this.MovementStates ["Up"]) {
                this.Position += new Vector2f (0.0f, -(this.Velocity.Y * (delta * 100)));
            }

            if (this.MovementStates ["Down"]) {
                this.Position += new Vector2f (0.0f, this.Velocity.Y * (delta * 100));
            }
        }
    }
}
