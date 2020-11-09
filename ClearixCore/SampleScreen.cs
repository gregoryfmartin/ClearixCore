using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;

namespace ClearixCore {
    class SampleScreen : ScreenBase {
        public SampleScreen () : base () {
            this.Name = "SampleScreen";
            this.LoadAssets (@".\SampleScreen.zip");
            this.Entities.Add ("PlayerObject", new PlayerEntity () {
                Texture = this.Assets.Textures ["BlippiSquare"],
                TextureRect = new IntRect () {
                    Height = 24,
                    Left = 0,
                    Top = 0,
                    Width = 86
                },
                Position = new Vector2f (50.0f, 50.0f)
            });
            this.Entities.Add ("SampleObject", new EntityBase () {
                Texture = this.Assets.Textures ["BlippiSquare"],
                TextureRect = new IntRect () {
                    Height = 24,
                    Left = 0,
                    Top = 0,
                    Width = 86
                },
                Position = new Vector2f (150.0f, 150.0f),
                Origin = new Vector2f (86 / 2, 24 / 2)
            });
            //this.Camera.Size = new Vector2f (800.0f, 600.0f);
        }

        public override void Draw (RenderTarget target, RenderStates states) {
            base.Draw (target, states);
            foreach (KeyValuePair<String, Entity> kvp in this.Entities) {
                target.Draw (kvp.Value);
            }
        }

        public override void LoadAssets (String archiveFile) {
            this.Assets.LoadAssets (archiveFile);
        }

        public override void Update (Single delta) {
            foreach (KeyValuePair<String, Entity> kvp in this.Entities) {
                kvp.Value.Update (delta);
            }
            this.Camera.Center = this.Entities ["SampleObject"].Position;

            Console.WriteLine ($"Camera Size for Screen {this.Name}: {this.Camera.Size.X}, {this.Camera.Size.Y}");
            Console.WriteLine ($"Player Entity Position: {this.Entities ["PlayerObject"].Position.X}, {this.Entities ["PlayerObject"].Position.Y}");
        }
    }
}
