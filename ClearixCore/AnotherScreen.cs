﻿using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;

namespace ClearixCore {
    class AnotherScreen  : ScreenBase {
        public AnotherScreen() : base() {
            Name = "AnotherScreen";
            LoadAssets(@".\AnotherScreen.zip");

            ///
            /// If the following statement follows the one below it, the background entity is drawn above the other entities. I'll need to construct
            /// a layering construct soon to ensure that the drawing is performed the way I want it done.
            ///
            Entities.Add("Background", new EntityBase() {
                Texture = Assets.Textures["BG1Downscaled"],
                Position = new Vector2f(0.0f, 0.0f)
            });
            Entities.Add("PlayerObject", new PlayerEntity() {
                Texture = Assets.Textures["BlippiSquare"],
                TextureRect = new IntRect() {
                    Height = 24,
                    Left = 0,
                    Top = 0,
                    Width = 86
                },
                Position = new Vector2f(50.0f, 50.0f)
            });
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            foreach (KeyValuePair<String, Entity> kvp in Entities) {
                target.Draw(kvp.Value);
            }
        }

        public override void LoadAssets(String archiveFile) {
            Assets.LoadAssets(archiveFile);
        }

        public override void Update(Single delta) {
            foreach (KeyValuePair<String, Entity> kvp in Entities) {
                kvp.Value.Update(delta);
            }
        }
    }
}
