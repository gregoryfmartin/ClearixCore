using System;

using SFML.Graphics;

namespace ClearixCore {
    /// <summary>
    /// This class is supposed to be the base class for all abstract entities that could exist in the game
    /// world. I haven't yet decided to add anything else to this yet and it's currently an abstraction of
    /// the SFML Sprite class.
    /// </summary>
    abstract class Entity : Sprite {
        /// <summary>
        /// Does nothing other than call the superclass constructor.
        /// </summary>
        protected Entity() : base() { }

        /// <summary>
        /// Contains any update logic.
        /// </summary>
        /// <param name="delta">Delta between frames.</param>
        public abstract void Update(Single delta);
    }
}
