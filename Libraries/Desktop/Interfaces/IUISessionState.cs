using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IUISessionState<TApp> : ISessionState<TApp>
    where TApp : IApp
{
}