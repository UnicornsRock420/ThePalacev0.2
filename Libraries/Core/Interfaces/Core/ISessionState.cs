using System.Collections.Concurrent;
using Lib.Common.Interfaces.Core;

namespace Lib.Core.Interfaces.Core;

public interface ISessionState : IDisposable, IID
{
    IApp App { get; set; }

    ConcurrentDictionary<string, object> Extended { get; }

    object? SessionTag { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
}