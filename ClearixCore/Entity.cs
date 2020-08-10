using System;
using System.Collections.Generic;
using System.Text;

using SFML.Graphics;

namespace ClearixCore {
    abstract class Entity : Sprite {
        protected Entity() : base () { }

        public abstract void Update (float delta);
    }
}
