using ThePalace.Common.Factories;

namespace ThePalace.Media.SoundPlayer
{
    public class SoundManager : Singleton<SoundManager>
    {
        private static readonly System.Media.SoundPlayer _player;

        static SoundManager()
        {
            _player = new();
        }

        public static void Load(string path)
        {
            if (File.Exists(path))
            {
                _player.SoundLocation = path;
                _player.Load();
            }
        }

        public static void Play(string? path = null)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                Load(path);
            }

            _player.Play();
        }

        public static void Stop() => _player.Stop();
    }
}