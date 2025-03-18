using Lib.Core.Interfaces.Core;

namespace Lib.Common.Server.Interfaces;

public interface IServerApp : IApp
{
    IServerSessionState ServerSessionState { get; }
}