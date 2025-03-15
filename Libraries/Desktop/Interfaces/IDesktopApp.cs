using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IDesktopApp : IApp
{
    IReadOnlyDictionary<string, IDisposable> UIControls { get; }
    FormBase GetForm(string? friendlyName = null);
    T GetForm<T>(string? friendlyName = null) where T : FormBase;
    void RegisterForm(string friendlyName, FormBase form);
    void RegisterForm<T>(string friendlyName, T form) where T : FormBase;
    void UnregisterForm(string friendlyName, FormBase form);
    void UnregisterForm<T>(string friendlyName, T form) where T : FormBase;
    Control GetControl(string? friendlyName = null);
    T GetControl<T>(string? friendlyName = null) where T : Control;
    void RegisterControl(string friendlyName, Control control);
    void RegisterControl<T>(string friendlyName, T control) where T : Control;
    void RegisterControl(string friendlyName, IDisposable control);
    void UnregisterControl(string friendlyName, Control control);
    void UnregisterControl<T>(string friendlyName, T control) where T : Control;
    void UnregisterControl(string friendlyName, IDisposable control);
}