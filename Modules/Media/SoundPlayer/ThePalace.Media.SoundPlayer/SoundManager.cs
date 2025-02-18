using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using ThePalace.Common.Factories;

namespace ThePalace.Media.SoundPlayer
{
    public partial class SoundManager : Singleton<SoundManager>, IDisposable
    {
        private const int CONST_INT_MaxPlayerCount = 30;
        private readonly ConcurrentDictionary<string, Tuple<DateTime, string>> _players;

        public SoundManager()
        {
            _players = new();
        }

        ~SoundManager() => this.Dispose();

        public void Dispose()
        {
            _players?.Clear();
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

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        private static extern long _mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public bool Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            var player = (System.Media.SoundPlayer?)null;

            if (File.Exists(path))
            {
                var filename = Path.GetFileNameWithoutExtension(path).ToUpperInvariant();
                var ext = Path.GetExtension(path).ToLowerInvariant().TrimStart('.');
                var _path = Path.GetFullPath(path);

                try
                {
                    if (!_players.ContainsKey(filename) ||
                        _players[filename].Item2 != _path)
                    {
                        using (var @lock = LockContext.GetLock(_players))
                        {
                            if (_players.ContainsKey(filename))
                            {
                                _mciSendString($"close {filename}", null, 0, IntPtr.Zero);
                            }

                            if (_players.Count >= CONST_INT_MaxPlayerCount)
                            {
                                var _player = _players.OrderBy(p => p.Value.Item1).FirstOrDefault();
                                _mciSendString($"close {filename}", null, 0, IntPtr.Zero);

                                _players.Remove(_player.Key);
                            }

                            var type = ext == "mp3" ? "type mpegvideo" : string.Empty;

                            _mciSendString($"open \"{path}\" {type} as {filename}", null, 0, IntPtr.Zero);

                            _players[filename] = new(DateTime.UtcNow, _path);
                        }
                    }

                    return true;
                }
                catch { }
            }

            return false;
        }

        public void Play(string? path = null, bool loop = false)
        {
            if (Load(path))
            {
                var filename = Path.GetFileNameWithoutExtension(path).ToUpperInvariant();
                var loopCmd = loop ? "REPEAT" : string.Empty;

                if (_players.ContainsKey(filename))
                {
                    _mciSendString($"play {filename} {loopCmd}", null, 0, IntPtr.Zero); //{_players[filename].Item3}
                }
            }
        }

        public void Stop()
        {
            foreach (var p in _players.ToList())
            {
                _mciSendString($"stop {p.Key.ToUpperInvariant()}", null, 0, IntPtr.Zero);
            }
        }
    }
}