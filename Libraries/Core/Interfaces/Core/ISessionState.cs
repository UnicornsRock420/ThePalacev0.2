using System.Collections.Concurrent;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState : ISessionState<IApp>
{
}

public interface ISessionState<TApp> : IDisposable
    where TApp : IApp
{
    TApp App { get; set; }

    Guid Id { get; }
    ConcurrentDictionary<string, object> Extended { get; }

    object? SessionTag { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
}