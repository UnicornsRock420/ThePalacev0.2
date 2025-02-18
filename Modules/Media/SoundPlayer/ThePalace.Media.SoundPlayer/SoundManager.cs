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

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        private static extern long _mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public SoundManager()
        {
            _players = new();
        }

        ~SoundManager() => this.Dispose();

        public void Dispose()
        {
            _players?.Clear();
        }

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