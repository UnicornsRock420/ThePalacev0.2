using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState : IDisposable
{
    Guid Id { get; }
    DateTime? LastActivity { get; set; }
    IConnectionState? ConnectionState { get; set; }
    
    object? SessionTag { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
}