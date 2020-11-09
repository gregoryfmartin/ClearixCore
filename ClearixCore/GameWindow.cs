using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class GameWindow : RenderWindow {
        public List<Drawable> Renderables { get; }

        public delegate void KDH (Object sender, KeyEventArgs e);

        public delegate void KUH (Object sender, KeyEventArgs e);

        public KDH KeyDownHandler { get; set; }

        public KUH KeyUpHandler { get; set; }

        public Fader MyFader;

        public GameWindow () : base (new VideoMode (800, 600), "") {
            this.Renderables = new List<Drawable> ();

            this.KeyPressed += (s, e) => { this.KeyDownHandler (s, e); };
            this.KeyReleased += (s, e) => { this.KeyUpHandler (s, e); };

            this.KeyDownHandler += (s, e) => { };
            this.KeyUpHandler += (s, e) => { };

            this.MyFader = new Fader (new Vector2f (900.0f, 700.0f));
        }

        public void Update (Single delta) {
            this.MyFader.Update (delta);
            this.DispatchEvents ();
        }

        public void DrawStuff () {
            this.Clear (Color.Black);

            if (this.Renderables.Count >= 1) {
                foreach (Drawable d in this.Renderables) {
                    this.Draw (d);
                }
            }

            this.SetView (this.DefaultView);
            this.Draw (this.MyFader);

            this.Display ();
        }
    }
}
