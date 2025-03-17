using Lib.Core.Interfaces.Core;

namespace Lib.Common.Desktop.Interfaces;

public interface IUISessionState<TApp> : ISessionState<TApp>
    where TApp : IApp
{
}