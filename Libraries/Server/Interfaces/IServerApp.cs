using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Interfaces;

public interface IServerApp : IApp
{
    IServerSessionState<IServerApp> ServerSessionState { get; }
}