using Lib.Core.Interfaces.Core;

namespace Lib.Common.Desktop.Interfaces;

public interface IDesktopSessionState<TDesktopApp> : IUISessionState<TDesktopApp>
    where TDesktopApp : IApp
{
    bool Visible { get; set; }
    bool Enabled { get; set; }

    double Scale { get; set; }
    int ScreenWidth { get; set; }
    int ScreenHeight { get; set; }

    DateTime? LastActivity { get; set; }

    void RefreshRibbon();
    void RefreshUI();
}