using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;

namespace ClearixCore {
    class GameWindow : RenderWindow {
        public List<Drawable> Renderables { get; }

        public delegate void KDH(Object sender, KeyEventArgs e);

        public delegate void KUH(Object sender, KeyEventArgs e);

        public KDH KeyDownHandler { get; set; }

        public KUH KeyUpHandler { get; set; }

        public GameWindow() : base(new VideoMode(800, 600), "") {
            Renderables = new List<Drawable>();

            KeyPressed += (s, e) => { KeyDownHandler(s, e); };
            KeyReleased += (s, e) => { KeyUpHandler(s, e); };

            KeyDownHandler += (s, e) => { };
            KeyUpHandler += (s, e) => { };
        }

        public void Update(Single delta) {
            DispatchEvents();
        }

        public void Draw() {
            Clear(Color.Black);

            if (Renderables.Count >= 1) {
                foreach (Drawable d in Renderables) {
                    Draw(d);
                }
            }

            Display();
        }
    }
}
