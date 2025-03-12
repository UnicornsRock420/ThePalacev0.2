using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState : IDisposable
{
    IApp<ISessionState> App { get; set; }

    Guid Id { get; }
    IConnectionState? ConnectionState { get; set; }

    object? SessionTag { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
}