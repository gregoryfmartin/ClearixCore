using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;

namespace ClearixCore {
    abstract class Screen : Drawable {
        public AssetManager Assets { get; }

        public Dictionary<String, Entity> Entities { get; }

        public String Name { get; set; }

        //public Boolean CurrentlyUsed { get; set; }

        protected Screen() {
            Assets = new AssetManager();
            Entities = new Dictionary<String, Entity>();
            Name = "";
            //CurrentlyUsed = false;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);

        public abstract void LoadAssets(String archiveFile);

        public abstract void Update(Single delta);

        public abstract void CheckGlobalInput(Object sender, KeyEventArgs e);

        public abstract void CheckPlayerInputPressed(Object sender, KeyEventArgs e);

        public abstract void CheckPlayerInputReleased(Object sender, KeyEventArgs e);
    }
}
