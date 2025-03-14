using System.Collections.Concurrent;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState : IDisposable
{
    IApp<ISessionState> App { get; set; }

    Guid Id { get; }
    ConcurrentDictionary<string, object> Extended { get; }

    object? SessionTag { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
}