using System;
using System.Collections.Generic;
using System.Text;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class SampleScreen : Screen {
        public SampleScreen() : base() {
            LoadAssets (@".\SampleScreen.zip");
            Entities.Add ("PlayerObject", new PlayerEntity () {
                Texture = Assets.Textures ["BlippiSquare"],
                TextureRect = new IntRect () {
                    Height = 24,
                    Left = 0,
                    Top = 0,
                    Width = 86
                },
                Position = new Vector2f (50.0f, 50.0f)
            });
        }

        public override void Draw (RenderTarget target, RenderStates states) {
            foreach (KeyValuePair<string, Entity> kvp in Entities)
                target.Draw (kvp.Value);
        }

        public override void LoadAssets (string archiveFile) {
            Assets.LoadAssets (archiveFile);
        }

        public override void Update (float delta) {
            foreach (KeyValuePair<string, Entity> kvp in Entities)
                kvp.Value.Update (delta);
        }

        public override void CheckGlobalInput (object sender, KeyEventArgs e) {
            return;
        }

        public override void CheckPlayerInputPressed (object sender, KeyEventArgs e) {
            (Entities ["PlayerObject"] as PlayerEntity)?.CheckInputPressed (e);
        }

        public override void CheckPlayerInputReleased (object sender, KeyEventArgs e) {
            (Entities ["PlayerObject"] as PlayerEntity)?.CheckInputReleased (e);
        }
    }
}
