using System;
using System.Collections.Generic;
using System.Text;

using SFML.Graphics;
using SFML.Window;

namespace ClearixCore {
    abstract class Screen : Drawable {
        public AssetManager Assets { get; }

        public Dictionary<string, Entity> Entities { get; }

        protected Screen () {
            Assets = new AssetManager ();
            Entities = new Dictionary<string, Entity> ();
        }

        public abstract void Draw (RenderTarget target, RenderStates states);

        public abstract void LoadAssets (string archiveFile);

        public abstract void Update (float delta);

        public abstract void CheckGlobalInput (object sender, KeyEventArgs e);

        public abstract void CheckPlayerInputPressed (object sender, KeyEventArgs e);

        public abstract void CheckPlayerInputReleased (object sender, KeyEventArgs e);
    }
}
