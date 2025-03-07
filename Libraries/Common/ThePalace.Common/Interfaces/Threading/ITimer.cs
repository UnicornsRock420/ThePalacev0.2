namespace ThePalace.Common.Interfaces.Threading
{
    public interface ITimer
    {
        event EventHandler Tick;

        object? Tag { get; set; }
        bool Enabled { get; set; }
        int Interval { get; set; }

        void Start();
        void Stop();
    }
}