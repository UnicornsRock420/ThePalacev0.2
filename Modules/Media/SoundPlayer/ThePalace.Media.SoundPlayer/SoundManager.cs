using LibVLCSharp.Shared;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using ThePalace.Common.Factories;

namespace ThePalace.Media.SoundPlayer;

public partial class SoundManager : Singleton<SoundManager>, IDisposable
{
    private const int CONST_INT_MaxPlayerCount = 30;
    private static readonly LibVLC _libVlc;
    private readonly MediaPlayer _libVlcPlayer;

    private class SndLog
    {
        private SndLog()
        {
            LastUsed = DateTime.UtcNow;
        }
        public SndLog(string path) : this()
        {
            Path = path;
            Media = new LibVLCSharp.Shared.Media(_libVlc, path);
        }

        public DateTime LastUsed { get; set; }
        public string Path { get; private set; }
        public LibVLCSharp.Shared.Media Media { get; private set; }
    }

    private readonly ConcurrentDictionary<string, SndLog> _libVlcMedia;

    static SoundManager()
    {
        _libVlc = new("--file-caching=0");  //--input-repeat=#
    }

    public SoundManager()
    {
        _libVlcPlayer = new(_libVlc);
        _libVlcMedia = new();
    }

    ~SoundManager() => this.Dispose();

    public void Dispose()
    {
        _libVlcPlayer.Dispose();

        _libVlcMedia.ToList().ForEach(m => { try { m.Value.Media.Dispose(); } catch { } });
        _libVlcMedia?.Clear();
    }

    public bool Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;

        if (File.Exists(path))
        {
            var filename = Path.GetFileNameWithoutExtension(path).ToUpperInvariant();
            var ext = Path.GetExtension(path).ToLowerInvariant().TrimStart('.');
            var _path = Path.GetFullPath(path);

            try
            {
                if (!_libVlcMedia.ContainsKey(filename) ||
                    _libVlcMedia[filename].Path != _path)
                {
                    using (var @lock = LockContext.GetLock(_libVlcMedia))
                    {
                        if (_libVlcMedia.ContainsKey(filename))
                        {
                            _libVlcMedia[filename].Media.Dispose();
                        }

                        if (_libVlcMedia.Count >= CONST_INT_MaxPlayerCount)
                        {
                            var _player = _libVlcMedia.OrderBy(m => m.Value.LastUsed).FirstOrDefault();
                            _player.Value.Media.Dispose();

                            _libVlcMedia.Remove(_player.Key);
                        }

                        _libVlcMedia[filename] = new SndLog(_path);
                    }
                }

                return true;
            }
            catch { }
        }

        return false;
    }

    public void Play(string? path = null)
    {
        if (Load(path))
        {
            var filename = Path.GetFileNameWithoutExtension(path).ToUpperInvariant();

            if (_libVlcMedia.ContainsKey(filename))
            {
                var media = _libVlcMedia[filename];

                media.LastUsed = DateTime.UtcNow;

                _libVlcPlayer.Play(media.Media);
            }
        }
    }

    public void Pause()
    {
        _libVlcPlayer.Pause();
    }

    public void Stop()
    {
        _libVlcPlayer.Stop();
    }

    [Flags]
    public enum PlaySoundFlags : int
    {
        SND_SYNC = 0x0000,/* play synchronously (default) */
        SND_ASYNC = 0x0001, /* play asynchronously */
        SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
        SND_MEMORY = 0x0004, /* pszSound points to a memory file */
        SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
        SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
        SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
        SND_ALIAS = 0x00010000,/* name is a registry alias */
        SND_ALIAS_ID = 0x00110000, /* alias is a pre d ID */
        SND_FILENAME = 0x00020000, /* name is file name */
        SND_RESOURCE = 0x00040004, /* name is resource name or atom */
        SND_PURGE = 0x0040,  /* purge non-static events for task */
        SND_APPLICATION = 0x0080, /* look for application specific association */
        SND_SENTRY = 0x00080000, /* Generate a SoundSentry event with this sound */
        SND_RING = 0x00100000, /* Treat this as a "ring" from a communications app - don't duck me */
        SND_SYSTEM = 0x00200000 /* Treat this as a system sound */
    }
    [DllImport("winmm.dll", EntryPoint = "PlaySound")]
    private static extern long _playSound(string file, int module, int flags);
    public void PlaySound(string path, PlaySoundFlags flags = PlaySoundFlags.SND_LOOP)
    {
        if (File.Exists(path))
        {
            _playSound(path, 0, (int)flags);
        }
    }
}