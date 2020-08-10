using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Audio;
using SFML.Graphics;

namespace ClearixCore {
    class AssetManager {
        public Dictionary<string, Texture> Textures { get; }

        public Dictionary<string, Font> Fonts { get; }

        public Dictionary<string, SoundBuffer> SoundEffects { get; }

        public Dictionary<string, Music> Songs { get; }

        public bool AssetsLoading { get { return assetsLoading; } }

        public bool AssetsLoaded { get { return assetsLoaded; } }

        public int NumAssetsToLoad { get { return numAssetsToLoad; } }

        public int NumAssetsLoaded { get { return numAssetsLoaded; } }

        private bool assetsLoading;

        private bool assetsLoaded;

        private int numAssetsToLoad;

        private int numAssetsLoaded;

        public AssetManager () {
            Textures = new Dictionary<string, Texture> ();
            Fonts = new Dictionary<string, Font> ();
            SoundEffects = new Dictionary<string, SoundBuffer> ();
            Songs = new Dictionary<string, Music> ();
            assetsLoading = false;
            assetsLoaded = false;
            numAssetsToLoad = 0;
            numAssetsLoaded = 0;
        }

        public void LoadAssets (string archiveFileName) {
            try {
                ZipArchive archive = ZipFile.OpenRead (archiveFileName);

                // I have no idea if this is going to freaking work.
                numAssetsToLoad = archive.Entries.SelectMany (s => s.Name).Where (s => s.ToString ().Split (new char [] { '.' }).Length > 1).ToList ().Count;

                foreach(ZipArchiveEntry entry in archive.Entries) {
                    string [] fname = entry.Name.Split (new char [] { '.' });
                    if(fname.Length > 1) {
                        switch(fname[1]) {
                            case "png":
                            case "jpg":
                            case "jpeg":
                                Textures.Add (fname [0], (CopyAssetMem<Texture> (entry)).Result);
                                numAssetsLoaded++;
                                break;
                            case "otf":
                                Fonts.Add (fname [0], (CopyAssetMem<Font> (entry)).Result);
                                numAssetsLoaded++;
                                break;
                            case "ogg":
                                Songs.Add (fname [0], (CopyAssetMem<Music> (entry)).Result);
                                numAssetsLoaded++;
                                break;
                            case "wav":
                                SoundEffects.Add (fname [0], (CopyAssetMem<SoundBuffer> (entry)).Result);
                                numAssetsLoaded++;
                                break;
                            default:
                                /*
                                 * This could cause a problem where the number of assets to load won't match the expected
                                 * amount, and could result in a false positive sent to the caller. In production code,
                                 * this would be removed since all kinds of assets would be known and contained in the archive.
                                 */ 
                                break;
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                Environment.Exit (-99);
            }
        }

        private async Task<T> CopyAssetMem<T> (ZipArchiveEntry entry) {
            byte [] b;
            using (MemoryStream ms = new MemoryStream ()) {
                await entry.Open ().CopyToAsync (ms);
                b = ms.ToArray ();
            }

            return (T)Activator.CreateInstance (typeof (T), b);
        }
    }
}
