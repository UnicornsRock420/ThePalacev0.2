namespace ThePalace.Core.Interfaces.Core;

public interface IApp<out TSessionState>
    where TSessionState : ISessionState
{
    TSessionState SessionState { get; }

    void Initialize();
}