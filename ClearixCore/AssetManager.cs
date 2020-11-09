using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
    /// program is actually loading the assets in an asynchronous manner or not. Without doubt, the opening of the archive file will
    /// be on the main thread since there doesn't appear to be any method in System.IO.Compression for opening a ZIP file asynchronously.
    /// The asynchronous operation actually occurs when the bytes of the current ZIP Entry are copied from the archive to memory. Once copied,
    /// they're used to initialize SFML constructs which are then stored in their appropriate associative containers.
    /// 
    /// Each associative container that houses SFML constructs is available read-only to the public. There are also several other convenience 
    /// members that can be used for monitoring and tracing the loading process as the asynchronous operations occur (similar to measuring
    /// legacy threading operations).
    /// 
    /// The Asset Manager also provides a method for monitoring the status of loading. It attempts to do this by enumerating the files in the target
    /// archive that have valid extensions (generally, directories in the archive won't have extensions). The obvious problem with this is that if a
    /// file is enumerated that has an extension type which isn't supported, it still counts toward the total number of assets that are to be loaded.
    /// As of now, there's currently no known workaround for this.
    /// </summary>
    class AssetManager {
        /// <summary>
        /// The collection for all Textures this Asset Manager will handle.
        /// </summary>
        public Dictionary<String, Texture> Textures { get; }

        /// <summary>
        /// The collection for all Fonts this Asset Manager will handle.
        /// </summary>
        public Dictionary<String, Font> Fonts { get; }

        /// <summary>
        /// The collection for all SoundBuffers this Asset Manager will handle.
        /// </summary>
        public Dictionary<String, SoundBuffer> SoundEffects { get; }

        /// <summary>
        /// The collection for all Musics this Asset Manager will handle.
        /// </summary>
        public Dictionary<String, Music> Songs { get; }

        /// <summary>
        /// Accessor which states if the Asset Manager is currently loading assets or not.
        /// </summary>
        public Boolean AssetsLoading { get { return assetsLoading; } }

        /// <summary>
        /// Accessor which states if the Asset Manager has finished loading assets or not.
        /// </summary>
        public Boolean AssetsLoaded { get { return assetsLoaded; } }

        /// <summary>
        /// Accessor which states the total number of assets this Asset Manager has to load.
        /// </summary>
        public Int32 NumAssetsToLoad { get { return numAssetsToLoad; } }

        /// <summary>
        /// Accessor which states the total number of assets loaded thus far.
        /// </summary>
        public Int32 NumAssetsLoaded { get { return numAssetsLoaded; } }

        /// <summary>
        /// Internal variable for <see cref="AssetsLoading"/>.
        /// </summary>
        private Boolean assetsLoading;

        /// <summary>
        /// Internal variable for <see cref="AssetsLoaded"/>.
        /// </summary>
        private Boolean assetsLoaded;

        /// <summary>
        /// Internal variable for <see cref="NumAssetsToLoad"/>.
        /// </summary>
        private Int32 numAssetsToLoad;

        /// <summary>
        /// Internal variable for <see cref="NumAssetsLoaded"/>.
        /// </summary>
        private Int32 numAssetsLoaded;

        /// <summary>
        /// Default Constructor. Does nothing out of the ordinary.
        /// </summary>
        public AssetManager() {
            this.Textures = new Dictionary<String, Texture>();
            this.Fonts = new Dictionary<String, Font>();
            this.SoundEffects = new Dictionary<String, SoundBuffer>();
            this.Songs = new Dictionary<String, Music>();
            this.assetsLoading = false;
            this.assetsLoaded = false;
            this.numAssetsToLoad = 0;
            this.numAssetsLoaded = 0;
        }

        /// <summary>
        /// Controls the process of loading the assets from an archive file into memory. This function first determines
        /// how many assets there are to load. The way it does this is a little juvenile, so I'm sure I'm going to need
        /// a more sophisticated method for doing this. It's accomplished by performing a LINQ query against the entry
        /// collection to determine the number of entries that have a file extension. Variable aspects about this are
        /// currently not considered, such as the kind of extension or the full path of the asset (this one in particular
        /// would require that the structure of the archive be well defined before hand).
        /// 
        /// There's another potential caveat here in that numAssetsLoaded is incremented in this method, and is done
        /// so directly after a call to an asynchronous operation. The increment of this counter may not accurately reflect
        /// the loading efforts done behind the scenes, and may result in future checks that rely on this measurement to be
        /// inaccurate. Further testing will need performed to verify that this actually is the case.
        /// 
        /// While this method is running, the status of its loading can be monitored by calling methods through the public
        /// properties. There may be a better way of doing this, but like other things, it'll require a bit more
        /// research.
        /// 
        /// When this method completes, each asset bank contained in the AssetManager should be populated with the resources
        /// located within the archive that was provided to this method.
        /// </summary>
        /// <param name="archiveFileName">The path to an archive file that contains assets one wishes to load.</param>
        public void LoadAssets(String archiveFileName) {
            try {
                ZipArchive archive = ZipFile.OpenRead(archiveFileName);

                // I have no idea if this is going to freaking work.
                numAssetsToLoad = archive.Entries.SelectMany(s => s.Name).Where(s => s.ToString().Split(new Char[] { '.' }).Length > 1).ToList().Count;

                foreach (ZipArchiveEntry entry in archive.Entries) {
                    String[] fname = entry.Name.Split(new Char[] { '.' });
                    if (fname.Length > 1) {
                        switch (fname[1]) {
                            case "png":
                            case "jpg":
                            case "jpeg":
                                this.Textures.Add(fname[0], (this.CopyAssetMem<Texture>(entry)).Result);
                                //numAssetsLoaded++;
                                break;
                            case "otf":
                                this.Fonts.Add(fname[0], (this.CopyAssetMem<Font>(entry)).Result);
                                //numAssetsLoaded++;
                                break;
                            case "ogg":
                                this.Songs.Add(fname[0], (this.CopyAssetMem<Music>(entry)).Result);
                                //numAssetsLoaded++;
                                break;
                            case "wav":
                                this.SoundEffects.Add(fname[0], (this.CopyAssetMem<SoundBuffer>(entry)).Result);
                                //numAssetsLoaded++;
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
                Console.WriteLine(e.Message);
                Environment.Exit(-99);
            }
        }

        /// <summary>
        /// Actually copies the memory from the archive entry to a new instance of the asset type
        /// specified by the caller. This function performs the asynchronous operation on account of
        /// the argument supporting the feature.
        /// </summary>
        /// <typeparam name="T">The asset type that the caller wants to interpret the raw data as.</typeparam>
        /// <param name="entry">The entry from the archive containing the raw data.</param>
        /// <returns>A fully instantiated instance of a specified type derived from raw data.</returns>
        private async Task<T> CopyAssetMem<T>(ZipArchiveEntry entry) {
            Byte[] b;
            using (MemoryStream ms = new MemoryStream()) {
                await entry.Open().CopyToAsync(ms);
                b = ms.ToArray();
            }
            this.numAssetsLoaded++;

            return (T)Activator.CreateInstance(typeof(T), b);
        }
    }
}
