using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ClearixCore {
    class GameWindow : RenderWindow {
        public List<Drawable> Renderables { get; }

        public delegate void KDH ( Object sender, KeyEventArgs e );

        public delegate void KUH ( Object sender, KeyEventArgs e );

        public KDH KeyDownHandler { get; set; }

        public KUH KeyUpHandler { get; set; }

        public Fader fader;

        public GameWindow () : base (new VideoMode (800, 600), "") {
            Renderables = new List<Drawable> ();

            KeyPressed += ( s, e ) => { KeyDownHandler (s, e); };
            KeyReleased += ( s, e ) => { KeyUpHandler (s, e); };

            KeyDownHandler += ( s, e ) => { };
            KeyUpHandler += ( s, e ) => { };

            this.fader = new Fader (new Vector2f (900.0f, 700.0f));
        }

        public void Update ( Single delta ) {
            this.fader.Update (delta);
            DispatchEvents ();
        }

        public void DrawStuff () {
            Clear (Color.Black);

            if (Renderables.Count >= 1) {
                foreach (Drawable d in Renderables) {
                    this.Draw (d);
                }
            }

            this.SetView (this.DefaultView);
            this.Draw (this.fader);

            Display ();
        }
    }
}
