using System.Collections.Concurrent;
using Lib.Common.Desktop.Entities.UI;
using Lib.Common.Desktop.Factories.System.Windows.Forms;
using Lib.Common.Desktop.Forms.Generics;
using Lib.Common.Desktop.Interfaces;
using Lib.Common.Factories.Core;
using Lib.Common.Threading;
using Lib.Logging.Entities;

namespace Lib.Common.Desktop.Singletons;

public class FormsManager : SingletonDisposableApplicationContext<FormsManager>, IDisposable
{
    private static readonly string CONST_TypeFullName = typeof(FormBase).FullName;

    public FormsManager()
    {
        ThreadExit += (sender, e) => Dispose();
    }

    ~FormsManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        FormClosed?.Clear();

        foreach (var control in _forms.Values
                     .SelectMany(f => f.Controls.Cast<Control>())
                     .ToList())
        {
            try
            {
                control?.Dispose();
            }
            catch
            {
            }
        }

        _forms.Clear();

        base.Dispose();

        try
        {
            HotKeyManager.Current.Dispose();
        }
        catch
        {
        }

        try
        {
            TaskManager.Current.Dispose();
        }
        catch
        {
        }

        ExitThread();
    }

    public event EventHandler FormClosed;

    private readonly DisposableDictionary<string, FormBase> _forms = new();
    public IReadOnlyDictionary<string, FormBase> Forms => _forms.AsReadOnly();

    internal void _FormClosed(object sender, EventArgs e)
    {
        if (IsDisposed) return;

        var app = GetForm<FormBase>("IApp");

        if (sender is not FormBase _sender ||
            !UnregisterForm(_sender)) return;

        FormClosed?.Invoke(_sender, e);

        if (app == sender ||
            app == null)
        {
            TCF._TryDispose(
                    app,
                    this)
                .Execute();
        }
    }

    public bool RegisterForm(FormBase form, bool assignFormClosedHandler = true)
    {
        if (IsDisposed) return false;

        ArgumentNullException.ThrowIfNull(form, nameof(form));

        using (var @lock = LockContext.GetLock(_forms))
        {
            if (_forms.ContainsKey(form.Name)) return false;

            if (assignFormClosedHandler)
            {
                form.FormClosed += _FormClosed;
                form.Disposed += _FormClosed;
            }

            _forms.TryAdd(form.Name, form);

            return true;
        }

        return false;
    }

    public bool UnregisterForm(FormBase form)
    {
        if (IsDisposed) return false;

        ArgumentNullException.ThrowIfNull(form, nameof(form));

        using (var @lock = LockContext.GetLock(_forms))
        {
            try
            {
                var key = _forms
                    .Where(f => f.Value == form)
                    .Select(f => f.Key)
                    .FirstOrDefault();

                if (key == null) return false;

                _forms.TryRemove(key, out _);

                return true;
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);
            }
        }

        return false;
    }

    public T CreateForm<T>(FormCfg cfg, bool assignFormClosedHandler = true)
        where T : FormBase, new()
    {
        if (IsDisposed) return null;

        var form = new T
        {
            Name = cfg.Name ?? $"{CONST_TypeFullName}_{Guid.NewGuid()}",
            AutoScaleDimensions = cfg.AutoScaleDimensions,
            AutoScaleMode = cfg.AutoScaleMode,
            StartPosition = cfg.StartPosition
        };

        if (cfg.Activated != null)
            form.Activated += cfg.Activated;
        if (cfg.Load != null)
            form.Load += cfg.Load;
        if (cfg.Shown != null)
            form.Shown += cfg.Shown;
        if (cfg.GotFocus != null)
            form.GotFocus += cfg.GotFocus;
        if (cfg.MouseMove != null)
            form.MouseMove += cfg.MouseMove;
        if (cfg.FormClosed != null)
            form.FormClosed += cfg.FormClosed;

        RegisterForm(form, assignFormClosedHandler);
        UpdateForm(form, cfg);

        return form;
    }

    public T GetForm<T>(string name = null)
        where T : FormBase
    {
        if (IsDisposed) return null;

        if (name == null)
            return _forms.Values.OfType<T>().FirstOrDefault();
        if (_forms.TryGetValue(name, out var value)) return value as T;
        return null;
    }

    public static void UpdateForm<T>(T form, FormCfg cfg)
        where T : FormBase
    {
        ArgumentNullException.ThrowIfNull(form, nameof(form));

        form.SuspendLayout();

        form.ClientSize = cfg.Size;
        form.BackColor = cfg.BackColor;
        form.Padding = cfg.Padding;
        form.Margin = cfg.Margin;

        form.WindowState = cfg.WindowState;

        if (cfg.Title != null)
            form.Text = cfg.Title;

        form.ResumeLayout(false);
        form.PerformLayout();

        if (cfg.Visible) form.Show();
        else form.Hide();

        if (cfg.Focus) form.Focus();
    }

    public static void RegisterControl(FormBase parent, Control control)
    {
        ArgumentNullException.ThrowIfNull(parent, nameof(parent));
        ArgumentNullException.ThrowIfNull(control, nameof(control));

        parent.Controls.Add(control);
    }

    public static void RegisterControl<TForm, TControl>(TForm parent, TControl control)
        where TForm : FormBase
        where TControl : Control
    {
        ArgumentNullException.ThrowIfNull(parent, nameof(parent));
        ArgumentNullException.ThrowIfNull(control, nameof(control));

        parent.Controls.Add(control);
    }

    public TControl[] CreateControl<TForm, TControl>(TForm parent, bool visible = true, params ControlCfg[] cfgs)
        where TForm : FormBase
        where TControl : Control, new()
    {
        if (IsDisposed) return null;

        ArgumentNullException.ThrowIfNull(parent, nameof(parent));

        if (parent == null ||
            (cfgs?.Length ?? 0) < 1)
            return null;

        var results = new List<TControl>();

        parent.SuspendLayout();

        foreach (var cfg in cfgs)
        {
            var control = new TControl();
            switch (control)
            {
                case Button button:
                    button.UseVisualStyleBackColor = cfg.UseVisualStyleBackColor;
                    break;
                case CheckBox checkBox:
                    checkBox.Checked = cfg.Value != 0;
                    checkBox.UseVisualStyleBackColor = cfg.UseVisualStyleBackColor;
                    break;
                case PictureBox pictureBox:
                    pictureBox.BorderStyle = cfg.BorderStyle;
                    break;
                case ProgressBar progressBar:
                    progressBar.Value = cfg.Value;
                    break;
                case RichTextBox richTextBox:
                    richTextBox.MaxLength = cfg.MaxLength;
                    richTextBox.Multiline = cfg.Multiline;
                    break;
                case ScrollableControl scrollableControl when
                    cfg.Scroll != null:
                    scrollableControl.Scroll += cfg.Scroll;
                    break;
                case TextBox textBox:
                    textBox.MaxLength = cfg.MaxLength;
                    textBox.Multiline = cfg.Multiline;
                    break;
                case null:
                    continue;
            }

            control.Name = $"{typeof(TControl).FullName}_{Guid.NewGuid()}";

            if (cfg.Click != null)
                control.Click += cfg.Click;

            UpdateControl(control, cfg);

            RegisterControl(parent, control);

            results.Add(control);
        }

        parent.Visible = visible;

        parent.ResumeLayout(false);
        parent.PerformLayout();

        return results.ToArray();
    }

    public static void UpdateControl(Control control, ControlCfg cfg)
    {
        ArgumentNullException.ThrowIfNull(control, nameof(control));

        control.Visible = cfg.Visible;
        control.TabIndex = cfg.TabIndex;
        control.TabStop = cfg.TabStop;
        control.Location = cfg.WindowLocation;
        control.Padding = cfg.Padding;
        control.Margin = cfg.Margin;
        control.Size = cfg.Size;
        control.Text = cfg.Title;
    }

    public static void DestroyControl(FormBase parent, Control control)
    {
        ArgumentNullException.ThrowIfNull(parent, nameof(parent));
        ArgumentNullException.ThrowIfNull(control, nameof(control));

        parent.Controls.Remove(control);
    }

    public static TResult ShowModal<TForm, TResult>(IUISessionState sessionState)
        where TForm : ModalDialog<TResult>, IFormResult<TResult>, new()
    {
        using (var form = new TForm())
        {
            form.SessionState = sessionState;

            return form.ShowDialog() == DialogResult.OK ? form.Result : default;
        }
    }
}