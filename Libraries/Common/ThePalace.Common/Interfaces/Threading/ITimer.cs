namespace ThePalace.Common.Interfaces.Threading;

public interface ITimer
{
    object? Tag { get; set; }
    bool Enabled { get; set; }
    int Interval { get; set; }
    event EventHandler Tick;

    void Start();
    void Stop();
}