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
    /// <summary>
    /// The Asset Manager is responsible for loading all of the assets for a particular scope and providing hash lookup access to clients.
    /// 
    /// Types of assets are tightly intertwined with SFML. Custom asset types can be loaded by extending this class. However, asset types
    /// which fall outside the purview of SFML don't have any means of being translated for use in Clearix. Ergo, classes that seek to
    /// extend the functionality provided here will need to provide the following:
    /// 
    ///     1. Additional associative containers for the custom asset types.
    ///     2. Adjacent framework for interpreting, leveraging, and lifecycling the custom asset types.
    ///
    /// Asset Manager attempts to load assets in an asynchronous fashion. It currently uses C#'s TAP pattern, but it's not obvious if the
    /// program is actually loading the assets in an asynchronous manner or not. Without doubt, the actual opening of the archive file will
    /// be on the main thread since there doesn't appear to be any method in System.IO.Compression for opening a ZIP file asynchronously.
    /// The asynchronous operation actually occurs when the bytes of the current ZIP Entry are copied from the archive to memory. Once copied,
    /// they're used to initialize SFML constructs which are then stored in their appropriate associative containers.
    /// 
    /// Each associative container that houses SFML constructs is available read-only to the public. There are also several other convenience 
    /// members that can be used for monitoring and tracing the loading process as the asynchronous operations occur (similar to measuring
    /// legacy threading operations).
    /// 
    /// 
    /// </summary>
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
