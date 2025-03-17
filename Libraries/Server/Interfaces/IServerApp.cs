using Lib.Core.Interfaces.Core;

namespace Lib.Common.Server.Interfaces;

public interface IServerApp : IApp
{
    IServerSessionState<IServerApp> ServerSessionState { get; }
}