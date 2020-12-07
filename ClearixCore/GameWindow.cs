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

        //public RectangleShape Fader { get; }

        //private Single deltaCounter;

        public Fader MyFader;

        public GameWindow () : base (new VideoMode (800, 600), "") {
            Renderables = new List<Drawable> ();

            KeyPressed += ( s, e ) => { KeyDownHandler (s, e); };
            KeyReleased += ( s, e ) => { KeyUpHandler (s, e); };

            KeyDownHandler += ( s, e ) => { };
            KeyUpHandler += ( s, e ) => { };

            this.MyFader = new Fader (new Vector2f (900.0f, 700.0f));
            //this.Fader = new RectangleShape (new SFML.System.Vector2f (900.0f, 700.0f));
            //this.fader.FillColor = new Color (255, 255, 255, 0);

            //this.deltaCounter = 0.0f;
        }

        public void Update ( Single delta ) {
            //this.deltaCounter += delta;
            //Console.WriteLine ("deltaCounter Value: " + this.deltaCounter.ToString ());

            this.MyFader.Update (delta);
            DispatchEvents ();
        }

        public void Draw () {
            Clear (Color.Black);

            if (Renderables.Count >= 1) {
                foreach (Drawable d in Renderables) {
                    Draw (d);
                }
            }

            this.Draw (this.MyFader);

            Display ();
        }
    }
}
