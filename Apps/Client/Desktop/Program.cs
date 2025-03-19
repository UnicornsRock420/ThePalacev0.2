using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reflection;
using Lib.Common.Constants;
using Lib.Common.Desktop.Constants;
using Lib.Common.Desktop.Entities.UI;
using Lib.Common.Desktop.Factories;
using Lib.Common.Desktop.Forms.Core;
using Lib.Common.Desktop.Interfaces;
using Lib.Common.Enums.App;
using Lib.Common.Helpers;
using Lib.Common.Interfaces.Threading;
using Lib.Common.Threading;
using Lib.Core.Constants;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Entities.Network.Client.Users;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Entities.Threading;
using Lib.Core.Exts;
using Lib.Core.Factories.Core;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.EventsBus;
using Lib.Core.Interfaces.Network;
using Lib.Logging.Entities;
using Lib.Settings.Factories;
using Mod.Media.SoundPlayer;
using Mod.Scripting.Iptscrae.Attributes;
using Mod.Scripting.Iptscrae.Entities;
using Mod.Scripting.Iptscrae.Enums;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Client.Desktop.Helpers;
using ThePalace.Client.Desktop.Interfaces;
using AssetID = int;
using Connection = ThePalace.Client.Desktop.Forms.Connection;
using HotspotID = short;
using RegexConstants = Lib.Common.Constants.RegexConstants;
using UserID = int;

namespace ThePalace.Client.Desktop;

public class Program : SingletonDisposable<Program>, IDesktopApp
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
    private static readonly Type CONST_TYPE_MSG_Header = typeof(MSG_Header);

    #region Program::Main()

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        var libAssemblies = Directory.GetFiles(Environment.CurrentDirectory, "Lib.*.dll", SearchOption.TopDirectoryOnly).ToList();
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(a =>
        {
            try
            {
                libAssemblies.Remove($"{a.FullName}.dll");
            }
            catch
            {
            }
        });
        libAssemblies.ForEach(p =>
        {
            try
            {
                if (File.Exists(p))
                    Assembly.LoadFile(p);
            }
            catch
            {
            }
        });

        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        EventBus.Current.Subscribe(AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName?.StartsWith("Lib.Common.Client") == true)
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.GetInterfaces().Contains(CONST_TYPE_IEventHandler) &&
                t.Namespace?.StartsWith("Lib.Common.Client.Entities.Business") == true)
            .ToArray());

        var jobs = new Dictionary<ThreadQueues, IJob>();

        #region Jobs

        jobs[ThreadQueues.GUI] = TaskManager.Current.CreateJob<ActionCmd>(q =>
            {
                if (q.IsEmpty ||
                    !q.TryDequeue(out var cmd)) return;

                if (cmd.Values != null)
                    cmd.CmdFnc(cmd.Values);
                else
                    cmd.CmdFnc();
            },
            opts: RunOptions.UseTimer,
            timer: new UITimer
            {
                Enabled = true
            });
        ((Job<ActionCmd>)jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
        {
            CmdFnc = a =>
            {
                Current.Jobs = jobs;
                Current.Initialize();

                return Current;
            },
        });

        jobs[ThreadQueues.Network] = TaskManager.Current.CreateJob<ActionCmd>(q =>
            {
                Task.WaitAll(
                    TaskManager.StartMany(
                        jobs[ThreadQueues.Network].Token,

                        #region Command Processor Sub-Task

                        async () =>
                        {
                            var cancellationToken = jobs[ThreadQueues.Network].TokenSource;

                            while (!cancellationToken.IsCancellationRequested)
                            {
                                if (q.TryDequeue(out var cmd))
                                    switch ((NetworkCommandTypes)cmd.Flags)
                                    {
                                        case NetworkCommandTypes.CONNECT:
                                            var url = cmd.Values[0] as string;
                                            if (!RegexConstants.REGEX_PARSE_URL.IsMatch(url)) return;

                                            var match = url.ParseUrl(
                                                RegexConstants.ParseUrlOptions.IncludeBaseUrl |
                                                RegexConstants.ParseUrlOptions.IncludePath |
                                                RegexConstants.ParseUrlOptions.ModifierToLowerInvariant);
                                            if (match.Count < 3 ||
                                                match["Protocol"] != "palace") break;

                                            var hostname = match["Hostname"];
                                            var port = Convert.ToInt32(match["Port"]);

                                            Current.SessionState.ConnectionState.Connect(hostname, port);
                                            return;
                                        case NetworkCommandTypes.DISCONNECT:
                                            Current.SessionState.ConnectionState.Disconnect();
                                            return;
                                        default:
                                            return;
                                    }

                                cancellationToken.Token.ThrowIfCancellationRequested();

                                if (!(Current?.SessionState?.ConnectionState?.IsConnected() ?? false) ||
                                    q.IsEmpty)
                                    await Task.Delay(RndGenerator.Next(75, 250), cancellationToken.Token);
                            }
                        },

                        #endregion

                        #region Process BytesSend Processor Sub-Task

                        async () =>
                        {
                            var cancellationToken = jobs[ThreadQueues.Network].TokenSource;

                            while (!cancellationToken.IsCancellationRequested)
                            {
                                if ((Current?.SessionState?.ConnectionState?.BytesSend?.Length ?? 0) < 1)
                                {
                                    await Task.Delay(RndGenerator.Next(250, 450), cancellationToken.Token);

                                    continue;
                                }

                                var msgBytes = Current?.SessionState?.ConnectionState?.BytesSend.Dequeue();
                                if ((msgBytes?.Length ?? 0) > 0)
                                    Current.SessionState.ConnectionState.Send(msgBytes, directAccess: true);

                                cancellationToken.Token.ThrowIfCancellationRequested();

                                await Task.Delay(RndGenerator.Next(35, 75), cancellationToken.Token);
                            }
                        },

                        #endregion

                        #region Process BytesReceived Processor Sub-Task

                        async () =>
                        {
                            var msgTypes = AppDomain.CurrentDomain.GetAssemblies()
                                ?.Where(a => a.FullName.StartsWith("Lib.Core"))
                                ?.SelectMany(t => t.GetTypes()
                                    .Where(t => t.Name.StartsWith("MSG_"))) ?? [];

                            var cancellationToken = jobs[ThreadQueues.Network].TokenSource;
                            var msgHeader = new MSG_Header();

                            var eventType = (string?)null;
                            var msgObj = (IProtocol?)null;
                            var msgType = (Type?)null;

                            while (!cancellationToken.IsCancellationRequested)
                            {
                                if ((Current?.SessionState?.ConnectionState?.BytesReceived?.Length ?? 0) < 1)
                                {
                                    await Task.Delay(RndGenerator.Next(250, 450), cancellationToken.Token);

                                    continue;
                                }

                                if (msgHeader.EventType == 0 &&
                                    eventType == null &&
                                    msgType == null)
                                {
                                    try
                                    {
                                        var msgHeaderBuffer = new byte[12];
                                        var bytesRead = Current?.SessionState?.ConnectionState?.BytesReceived?.Read(msgHeaderBuffer, 0, msgHeaderBuffer.Length);
                                        if (bytesRead < msgHeaderBuffer.Length) throw new SocketException(-1, nameof(msgHeaderBuffer));

                                        using (var ms = new MemoryStream(msgHeaderBuffer))
                                        {
                                            ms.PalaceDeserialize(msgHeader, CONST_TYPE_MSG_Header);
                                        }

                                        eventType = msgHeader.EventType.ToString()?.Trim() ?? string.Empty;
                                        if (msgHeader.EventType == 0 ||
                                            string.IsNullOrWhiteSpace(eventType)) throw new InvalidDataException(nameof(eventType));

                                        msgType = msgTypes.FirstOrDefault(t => t.Name == eventType);
                                        if (msgType == null) throw new InvalidDataException(nameof(msgType));
                                    }
                                    catch
                                    {
                                        Current?.SessionState?.ConnectionState?.Disconnect();
                                    }
                                }

                                if (msgType != null &&
                                    msgObj == null &&
                                    msgHeader.Length > 0 &&
                                    Current?.SessionState?.ConnectionState?.BytesReceived.Length >= msgHeader.Length)
                                {
                                    var msgBuffer = new byte[msgHeader.Length];
                                    var bytesRead = Current?.SessionState?.ConnectionState?.BytesReceived?.Read(msgBuffer, 0, msgBuffer.Length);
                                    if (bytesRead < msgHeader.Length) throw new SocketException(-1, nameof(msgBuffer));

                                    msgObj = (IProtocol?)msgType.GetInstance();
                                    if (msgObj == null) throw new OutOfMemoryException(nameof(IProtocol));

                                    using (var ms = new MemoryStream(msgBuffer))
                                    {
                                        ms.PalaceDeserialize(msgObj, msgType);
                                    }
                                }

                                if (msgHeader.EventType != 0 &&
                                    msgType != null)
                                {
                                    try
                                    {
                                        var boType = EventBus.Current.GetType(msgType);
                                        if (boType == null) throw new InvalidDataException(nameof(msgType));

                                        EventBus.Current.Publish(
                                            Current.SessionState,
                                            boType,
                                            new ProtocolEventParams
                                            {
                                                SourceID = Current?.SessionState?.UserId ?? 0,
                                                RefNum = msgHeader.RefNum,
                                                Request = msgObj
                                            });
                                    }
                                    catch
                                    {
                                        Current?.SessionState?.ConnectionState?.Disconnect();
                                    }
                                    finally
                                    {
                                        msgHeader.EventType = 0;
                                        msgHeader.Length = 0;
                                        msgHeader.RefNum = 0;

                                        eventType = null;
                                        msgType = null;
                                        msgObj = null;
                                    }
                                }

                                cancellationToken.Token.ThrowIfCancellationRequested();

                                await Task.Delay(RndGenerator.Next(35, 75), cancellationToken.Token);
                            }
                        }

                        #endregion

                    ), jobs[ThreadQueues.Network].Token);
            },
            opts: RunOptions.UseSleepInterval,
            sleepInterval: TimeSpan.FromMilliseconds(500));

        jobs[ThreadQueues.Assets] = TaskManager.Current.CreateJob<AssetCmd>(async q =>
            {
                if (q.IsEmpty ||
                    !q.TryDequeue(out var assetCmd)) return;

                // TODO: Assets

                //var assetDesc = AssetsManager.Current.GetAsset(Current.SessionState, assetCmd.AssetDesc.AssetRec.AssetSpec, true);
                //if (assetDesc is not { Image: null }) return;

                //await AssetDesc.Render(assetDesc);
            },
            opts: RunOptions.UseResetEvent);

        jobs[ThreadQueues.Media] = TaskManager.Current.CreateJob<MediaCmd>(q =>
            {
                if (q.IsEmpty ||
                    !q.TryDequeue(out var mediaCmd)) return;

                // TODO: Media
            },
            opts: RunOptions.UseResetEvent);

        jobs[ThreadQueues.Audio] = TaskManager.Current.CreateJob<MediaCmd>(async q =>
            {
                if (q.IsEmpty ||
                    !q.TryDequeue(out var mediaCmd)) return;

                SoundManager.Current.PlaySound(mediaCmd.Path);
            },
            opts: RunOptions.UseResetEvent);

        jobs[ThreadQueues.Core] = TaskManager.Current.CreateJob<AssetCmd>(q => TaskManager.Current.Run(resources: FormsManager.Current),
            opts: RunOptions.UseSleepInterval | RunOptions.RunNow,
            sleepInterval: TimeSpan.FromMilliseconds(500));

#if WINDOWS10_0_17763_0_OR_GREATER
        jobs[ThreadQueues.Toast] = TaskManager.Current.CreateJob<ToastCfg>(q =>
            {
                if (!q.IsEmpty &&
                    q.TryDequeue(out var toastCfg))
                    ToastCfg.Dispatch(toastCfg);
            },
            opts: RunOptions.UseSleepInterval,
            sleepInterval: TimeSpan.FromMilliseconds(750));
#endif

        #endregion

        Application.Run(FormsManager.Current);
        TaskManager.Current.Shutdown();
    }

    #endregion

    #region ctors

    public Program()
    {
        SessionState = SessionManager.Current.CreateSession<ClientDesktopSessionState, IDesktopApp>(this);

        _managedResources.AddRange(
            _uiControls,
            _contextMenu,
            SessionState);

        FormsManager.Current.FormClosed += _FormClosed;
        SessionState.ConnectionState.ConnectionEstablished += _ConnectionEstablished;
        SessionState.ConnectionState.ConnectionDisconnected += _ConnectionDisconnected;
    }

    ~Program()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        CONST_eventTypes.ToList().ForEach(t => ScriptEvents.Current.UnregisterEvent(t, RefreshScreen));

        if (UIControls.GetValue(nameof(NotifyIcon)) is NotifyIcon trayIcon)
        {
            trayIcon.Visible = false;

            try
            {
                trayIcon.Dispose();
            }
            catch
            {
            }
        }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Properties

    private static readonly IReadOnlyList<IptEventTypes> CONST_eventTypes = Enum.GetValues<IptEventTypes>()
        .Where(v => v.GetType()
            ?.GetField(v.ToString())
            ?.GetCustomAttributes<ScreenRefreshAttribute>()
            ?.Any() ?? false)
        .ToList()
        .AsReadOnly();

    protected readonly ContextMenuStrip _contextMenu = new();

    public IClientDesktopSessionState SessionState { get; protected set; }

    protected ConcurrentDictionary<ThreadQueues, IJob> _jobs = new();

    public IReadOnlyDictionary<ThreadQueues, IJob> Jobs
    {
        get => _jobs.AsReadOnly();
        internal set => _jobs = new(value);
    }

    private readonly DisposableDictionary<string, IDisposable> _uiControls = new();
    public IReadOnlyDictionary<string, IDisposable> UIControls => _uiControls.AsReadOnly();

    #endregion

    #region Form Methods

    public void RefreshScreen(object sender, EventArgs e)
    {
        if (sender is not IClientDesktopSessionState sessionState) return;

        if (e is not ScriptEvent scriptEvent) return;

        ((Job<ActionCmd>)_jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
        {
            CmdFnc = a =>
            {
                if (a[0] is not IClientDesktopSessionState sessionState) return null;

                if (a[1] is not ScriptEvent scriptEvent) return null;

                sessionState.RefreshScriptEvent(scriptEvent);

                return null;
            },
            Values = [sessionState, scriptEvent],
        });
    }

    public void Initialize()
    {
        if (IsDisposed) return;

        CONST_eventTypes.ToList().ForEach(t => ScriptEvents.Current.RegisterEvent(t, RefreshScreen));

        ApiManager.Current.RegisterApi(nameof(ShowConnectionForm), ShowConnectionForm);
        ApiManager.Current.RegisterApi(nameof(toolStripDropdownlist_Click), toolStripDropdownlist_Click);
        ApiManager.Current.RegisterApi(nameof(toolStripMenuItem_Click), toolStripMenuItem_Click);
        ApiManager.Current.RegisterApi(nameof(contextMenuItem_Click), contextMenuItem_Click);

#if WINDOWS10_0_17763_0_OR_GREATER
        ((Job<ToastCfg>)Jobs[ThreadQueues.Toast])?.Enqueue(new ToastCfg
        {
            ExpirationTime = DateTime.Now.AddMinutes(1),
            Args = new Dictionary<string, object>
            {
                { "action", "whisperMsg" },
                { "connectionId", 123 },
                { "conversationId", 456 },
            }.AsReadOnly(),
            Text = new List<string>
            {
                "Beat it like it owes you money!",
            }.AsReadOnly(),
        });
#endif

        ((Job<ActionCmd>)_jobs[ThreadQueues.GUI])?.Enqueue(
            new ActionCmd
            {
                CmdFnc = a =>
                {
                    if (a[0] is not IClientDesktopSessionState sessionState) return null;

                    ShowAppForm();

                    var form = GetForm();
                    if (form == null) return null;

                    if (UIControls.GetValue(nameof(NotifyIcon)) is NotifyIcon trayIcon) return null;

                    trayIcon = new NotifyIcon
                    {
                        ContextMenuStrip = _contextMenu,
                        Icon = form.Icon,
                        Visible = true
                    };
                    RegisterControl(nameof(NotifyIcon), trayIcon);

                    trayIcon.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => TaskManager.Current.Dispose());

                    return null;
                },
                Values = [SessionState]
            });
    }

    private void ShowAppForm()
    {
        if (IsDisposed) return;

        var form = FormsManager.Current.CreateForm<FormDialog>(new FormCfg
        {
            Load = (sender, e) => _jobs[ThreadQueues.GUI].Run(),
            WindowState = FormWindowState.Minimized,
            AutoScaleMode = AutoScaleMode.Font,
            AutoScaleDimensions = new SizeF(7F, 15F),
            Margin = new Padding(0, 0, 0, 0),
            Visible = false
        });
        if (form == null) return;

        RegisterForm(nameof(Program), form);

        form.SessionState = SessionState;
        form.FormClosed += (sender, e) =>
            UnregisterForm(nameof(Program), sender as FormBase);

        form.MouseMove += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseMove, SessionState, null, SessionState.ScriptTag);
        };
        form.MouseUp += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseUp, SessionState, null, SessionState.ScriptTag);
        };
        form.MouseDown += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDown, SessionState, null, SessionState.ScriptTag);
        };
        form.DragEnter += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, SessionState, null, SessionState.ScriptTag);
        };
        form.DragLeave += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, SessionState, null, SessionState.ScriptTag);
        };
        form.DragOver += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, SessionState, null, SessionState.ScriptTag);
        };
        form.Resize += (sender, e) =>
        {
            SessionState.LastActivity = DateTime.UtcNow;

            if (sender is not FormBase { WindowState: FormWindowState.Normal } form) return;

            var screenWidth = Screen.PrimaryScreen?.Bounds.Width ?? 0;
            var screenHeight = Screen.PrimaryScreen?.Bounds.Height ?? 0;

            if (form.Location.X < 0 ||
                form.Location.Y < 0 ||
                form.Location.X > screenWidth ||
                form.Location.Y > screenHeight)
            {
                form.ClientSize = new Size(screenWidth - 16, screenHeight - 16);
                form.Location = new Point(0, 0);
            }

            form.Location = new Point(
                screenWidth / 2 - form.Width / 2,
                screenHeight / 2 - form.Height / 2);

            if (GetControl("toolStrip") is ToolStrip toolStrip)
                toolStrip.Size = new Size(form.Width, form.Height);

            SessionState.RefreshUI();
        };

        FormsManager.UpdateForm(form, new FormCfg
        {
            Size = new Size(
                DesktopConstants.AspectRatio.WidescreenDef.Default.Width,
                DesktopConstants.AspectRatio.WidescreenDef.Default.Height),
            WindowState = FormWindowState.Normal,
            Visible = true,
            Focus = true
        });

        //var tabIndex = 0;

        if (GetControl("toolStrip") is not ToolStrip toolStrip)
        {
            toolStrip = FormsManager.Current.CreateControl<FormBase, ToolStrip>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                //Size = new Size(800, 25),
                Margin = new Padding(0, 0, 0, 0)
            })?.FirstOrDefault();

            if (toolStrip != null)
            {
                RegisterControl(nameof(toolStrip), toolStrip);

                toolStrip.Stretch = true;
                toolStrip.GripMargin = new Padding(0);
                toolStrip.ImageScalingSize = new Size(38, 38);
                toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
                toolStrip.RenderMode = ToolStripRenderMode.Professional;
                toolStrip.Renderer = new CustomToolStripRenderer(SessionState);
                toolStrip.ItemClicked += toolStrip_ItemClicked;
            }
        }

        if (GetControl("imgScreen") is not PictureBox imgScreen)
        {
            imgScreen = FormsManager.Current.CreateControl<FormBase, PictureBox>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Margin = new Padding(0, 0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle
            })?.FirstOrDefault();

            if (imgScreen != null)
            {
                imgScreen.MouseClick += (sender, e) =>
                {
                    if (!SessionState.ConnectionState.IsConnected())
                    {
                        ShowConnectionForm();
                    }
                    else
                    {
                        var point = new Lib.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                SessionState.UserDesc.RoomPos = point;

                                var user = (UserDesc?)null;
                                user = SessionState.RoomUsers.GetValueLocked(SessionState.UserId);
                                if (user != null)
                                {
                                    user.RoomPos = point;
                                    user.Extended["CurrentMessage"] = null;

                                    if (user.Extended["MessageQueue"] is DisposableQueue<MsgBubble> queue)
                                        queue.Clear();

                                    SessionState.RefreshScreen(
                                        LayerScreenTypes.UserProp,
                                        LayerScreenTypes.UserNametag,
                                        LayerScreenTypes.Messages);

                                    SessionState.Send<IClientDesktopSessionState, MSG_USERMOVE>(
                                        SessionState.UserId,
                                        new MSG_USERMOVE
                                        {
                                            Pos = point
                                        });
                                }

                                break;
                            case MouseButtons.Right:
                                _contextMenu.Items.Clear();

                                var toolStripItem = _contextMenu.Items.Add($"Move here: {point.HAxis}, {point.VAxis}");
                                if (toolStripItem != null)
                                {
                                    toolStripItem.Tag = new object[]
                                    {
                                        ContextMenuCommandTypes.CMD_USERMOVE,
                                        point
                                    };
                                    toolStripItem.Click += contextMenuItem_Click;
                                }

                                if ((SessionState.RoomUsers?.Count ?? 0) > 0)
                                    foreach (var roomUser in SessionState.RoomUsers.Values)
                                        if (roomUser.UserId == 0 ||
                                            roomUser.RoomPos == null)
                                        {
                                            continue;
                                        }
                                        else if (roomUser.UserId != SessionState.UserId &&
                                                 point.IsPointInPolygon(roomUser.RoomPos.GetBoundingBox(
                                                     new Size(
                                                         (int)AssetConstants.Values.DefaultPropWidth,
                                                         (int)AssetConstants.Values.DefaultPropHeight),
                                                     true)))
                                        {
                                            toolStripItem =
                                                _contextMenu.Items.Add($"Select User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_USERSELECT,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (!SessionState.UserDesc.IsModerator &&
                                                !SessionState.UserDesc.IsAdministrator) continue;

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Pin User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_PIN,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Unpin User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNPIN,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Gag User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_GAG,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Ungag User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNGAG,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Propgag User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_PROPGAG,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Unpropgag User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNPROPGAG,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Kill User: {roomUser.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_KILLUSER,
                                                    roomUser.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }
                                        }

                                if ((SessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
                                {
                                    toolStripItem = _contextMenu.Items.Add("Delete All Props");
                                    if (toolStripItem != null)
                                    {
                                        toolStripItem.Tag = new object[]
                                        {
                                            ContextMenuCommandTypes.CMD_PROPDEL,
                                            -1
                                        };
                                        toolStripItem.Click += contextMenuItem_Click;
                                    }

                                    var j = 0;
                                    foreach (var looseProp in SessionState.RoomInfo.LooseProps)
                                    {
                                        if (looseProp.Loc == null) continue;

                                        var prop = AssetsManager.Current.Assets.GetValueLocked(looseProp.AssetSpec.Id);
                                        if (prop == null) continue;

                                        if (point.IsPointInPolygon(
                                                looseProp.Loc.GetBoundingBox(
                                                    new Size(
                                                        prop.Width,
                                                        prop.Height),
                                                    true)))
                                        {
                                            toolStripItem =
                                                _contextMenu.Items.Add($"Select Prop: {looseProp.AssetSpec.Id}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_PROPSELECT,
                                                    j
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Delete Prop: {looseProp.AssetSpec.Id}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_PROPDEL,
                                                    j
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }
                                        }

                                        j++;
                                    }
                                }

                                if ((SessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                                    foreach (var hotSpot in SessionState.RoomInfo.HotSpots)
                                        if (point.IsPointInPolygon(hotSpot.Vortexes.ToArray()))
                                        {
                                            toolStripItem =
                                                _contextMenu.Items.Add($"Select Spot: {hotSpot.HotspotID}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_SPOTSELECT,
                                                    hotSpot.HotspotID
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (!SessionState.UserDesc.IsModerator &&
                                                !SessionState.UserDesc.IsAdministrator) continue;

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Delete Spot: {hotSpot.HotspotID}");
                                            if (toolStripItem == null) continue;

                                            toolStripItem.Tag = new object[]
                                            {
                                                ContextMenuCommandTypes.CMD_SPOTDEL,
                                                hotSpot.HotspotID
                                            };
                                            toolStripItem.Click += contextMenuItem_Click;
                                        }

                                _contextMenu.Show(Cursor.Position);

                                break;
                        }
                    }
                };
                imgScreen.MouseMove += (sender, e) =>
                {
                    imgScreen.Cursor = Cursors.Default;

                    if (!SessionState.ConnectionState.IsConnected()) return;

                    var point = new Lib.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                    if ((SessionState.RoomUsers?.Count ?? 0) > 0)
                        foreach (var roomUser in SessionState.RoomUsers.Values)
                            if (roomUser.UserId == 0 ||
                                roomUser.RoomPos == null)
                            {
                                continue;
                            }
                            else if (point.IsPointInPolygon(
                                         roomUser.RoomPos.GetBoundingBox(
                                             new Size(
                                                 (int)AssetConstants.Values.DefaultPropWidth,
                                                 (int)AssetConstants.Values.DefaultPropHeight),
                                             true)))
                            {
                                imgScreen.Cursor = Cursors.Hand;
                                break;
                            }

                    if ((SessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
                        foreach (var looseProp in SessionState.RoomInfo.LooseProps)
                        {
                            if (looseProp.Loc == null) continue;

                            var prop = AssetsManager.Current.Assets.GetValueLocked(looseProp.AssetSpec.Id);
                            if (prop == null) continue;

                            if (point.IsPointInPolygon(
                                    looseProp.Loc.GetBoundingBox(
                                        new Size(
                                            prop.Width,
                                            prop.Height),
                                        true)))
                            {
                                imgScreen.Cursor = Cursors.Hand;
                                break;
                            }
                        }

                    if ((SessionState.RoomInfo?.HotSpots?.Count ?? 0) <= 0) return;

                    foreach (var hotSpot in SessionState.RoomInfo.HotSpots
                                 .Where(hotSpot => point.IsPointInPolygon(hotSpot.Vortexes.ToArray())))
                    {
                        imgScreen.Cursor = Cursors.Hand;
                        break;
                    }
                };

                RegisterControl(nameof(imgScreen), imgScreen);
            }
        }

        if (GetControl("labelInfo") is not Label labelInfo)
        {
            labelInfo = FormsManager.Current.CreateControl<FormBase, Label>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                Margin = new Padding(0, 0, 0, 0)
            })?.FirstOrDefault();

            if (labelInfo != null)
                RegisterControl(nameof(labelInfo), labelInfo);
        }

        if (GetControl("txtInput") is not TextBox txtInput)
        {
            txtInput = FormsManager.Current.CreateControl<FormBase, TextBox>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = Color.FromKnownColor(KnownColor.DimGray),
                Multiline = true,
                MaxLength = 255
            })?.FirstOrDefault();

            if (txtInput != null)
            {
                txtInput.LostFocus += (sender, e) => { txtInput.BackColor = Color.FromKnownColor(KnownColor.LightGray); };
                txtInput.GotFocus += (sender, e) => { txtInput.BackColor = Color.FromKnownColor(KnownColor.White); };
                txtInput.KeyUp += (sender, e) =>
                {
                    SessionState.LastActivity = DateTime.UtcNow;

                    if (e.KeyCode == Keys.Tab)
                    {
                        e.Handled = true;

                        txtInput.Text = string.Empty;
                    }

                    if (!SessionState?.ConnectionState?.IsConnected() ?? false)
                    {
                        ShowConnectionForm();

                        return;
                    }

                    ScriptEvents.Current.Invoke(IptEventTypes.KeyUp, SessionState, null, SessionState.ScriptTag);

                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;

                        var text = txtInput.Text?.Trim();
                        txtInput.Text = string.Empty;

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            if (text[0] == '/')
                            {
                                ((Job<ActionCmd>)_jobs[ThreadQueues.Scripting]).Enqueue(new ActionCmd
                                {
                                    CmdFnc = a =>
                                    {
                                        if (a[0] is not IClientDesktopSessionState sessionState) return null;

                                        if (a[1] is not string text) return null;

                                        var iptTracking = sessionState.ScriptTag as IptTracking;

                                        try
                                        {
                                            var atomlist =
                                                IptscraeEngine.Parse(
                                                    iptTracking,
                                                    text,
                                                    false);
                                            IptscraeEngine.Executor(
                                                atomlist,
                                                iptTracking);
                                        }
                                        catch (Exception ex)
                                        {
                                            LoggerHub.Current.Error(ex);
                                        }

                                        return null;
                                    },
                                    Values = [SessionState, string.Concat(text.Skip(1))]
                                });
                            }
                            else
                            {
                                var xTalk = new MSG_XTALK
                                {
                                    Text = text
                                };

                                ScriptEvents.Current.Invoke(IptEventTypes.Chat, SessionState, xTalk, SessionState.ScriptTag);
                                ScriptEvents.Current.Invoke(IptEventTypes.OutChat, SessionState, xTalk, SessionState.ScriptTag);

                                if (SessionState.ScriptTag is not IptTracking iptTracking) return;

                                if (iptTracking.Variables?.TryGetValue("CHATSTR", out var variable) is true)
                                    xTalk.Text = variable.Variable.Value.ToString();

                                if (!string.IsNullOrWhiteSpace(xTalk.Text))
                                    SessionState.Send<IClientDesktopSessionState, MSG_XTALK>(
                                        SessionState.UserId,
                                        xTalk);
                            }
                        }
                    }
                };
                txtInput.KeyDown += (sender, e) =>
                {
                    SessionState.LastActivity = DateTime.UtcNow;

                    if (e.KeyCode == Keys.Tab)
                    {
                        e.Handled = true;

                        txtInput.Text = string.Empty;
                    }

                    if (!SessionState?.ConnectionState?.IsConnected() ?? false) return;

                    ScriptEvents.Current.Invoke(IptEventTypes.KeyDown, SessionState, null, SessionState.ScriptTag);
                };

                RegisterControl(nameof(txtInput), txtInput);
            }
        }

        SessionState.RefreshScreen(LayerScreenTypes.Base);
        SessionState.RefreshUI();

        ShowConnectionForm();
    }

    private void ShowConnectionForm(object sender = null, EventArgs e = null)
    {
        if (IsDisposed) return;

        var connectionForm = GetForm<Connection>(nameof(Connection));
        if (connectionForm == null)
        {
            connectionForm = FormsManager.Current.CreateForm<Connection>(
                new FormCfg
                {
                    AutoScaleDimensions = new SizeF(7F, 15F),
                    AutoScaleMode = AutoScaleMode.Font,
                    WindowState = FormWindowState.Normal,
                    StartPosition = FormStartPosition.CenterScreen,
                    Margin = new Padding(0, 0, 0, 0),
                    Size = new Size(303, 182),
                    Visible = true
                });
            if (connectionForm == null) return;

            connectionForm.SessionState = SessionState;
            connectionForm.FormClosed += (sender, e) => { UnregisterForm(nameof(Connection), sender as FormBase); };

            if (connectionForm != null)
            {
                RegisterForm(nameof(Connection), connectionForm);

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonDisconnect")
                        .FirstOrDefault() is Button buttonDisconnect)
                {
                    buttonDisconnect.Click += (sender, e) =>
                    {
                        if (SessionState.ConnectionState?.IsConnected() ?? false)
                            SessionState.ConnectionState.Disconnect();

                        var connectionForm = GetForm(nameof(Connection));
                        connectionForm?.Close();
                    };
                    buttonDisconnect.Visible = SessionState?.ConnectionState?.IsConnected() ?? false;
                }

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonConnect")
                        .FirstOrDefault() is Button buttonConnect)
                    buttonConnect.Click += (sender, e) =>
                    {
                        var connectionForm = GetForm(nameof(Connection));
                        if (connectionForm != null)
                        {
                            var roomID = (short)0;

                            var checkBoxNewTab = connectionForm.Controls
                                .Cast<Control>()
                                .Where(c => c.Name == "checkBoxNewTab")
                                .FirstOrDefault() as CheckBox;
                            if (checkBoxNewTab?.Checked == true)
                            {
                                // TODO: Load new client tab
                            }

                            if (connectionForm.Controls
                                    .Cast<Control>()
                                    .Where(c => c.Name == "comboBoxUsernames")
                                    .FirstOrDefault() is ComboBox comboBoxUsernames)
                                SessionState.RegInfo.UserName = SessionState.RegInfo.UserName = comboBoxUsernames.Text;

                            if (connectionForm.Controls.Cast<Control>()
                                    .Where(c => c.Name == "textBoxRoomID")
                                    .FirstOrDefault() is TextBox textBoxRoomID)
                            {
                                if (!string.IsNullOrEmpty(textBoxRoomID.Text))
                                    roomID = Convert.ToInt16(textBoxRoomID.Text);

                                SessionState.RegInfo.DesiredRoom = roomID;
                            }

                            if (connectionForm.Controls
                                    .Cast<Control>()
                                    .Where(c => c.Name == "comboBoxAddresses")
                                    .FirstOrDefault() is ComboBox comboBoxAddresses &&
                                !string.IsNullOrWhiteSpace(comboBoxAddresses.Text))
                            {
                                var url = roomID > 0 ? $"palace://{comboBoxAddresses.Text}/{roomID}" : $"palace://{comboBoxAddresses.Text}";

                                if (SettingsManager.Current.Get<string[]>("GUI:Connection:Addresses") is string[] list)
                                {
                                    var addressesList = new UniqueList<string>(50, list);
                                    addressesList.Add(comboBoxAddresses.Text);

                                    SettingsManager.Current.Set<string[]>(@"Config\UserSettings.json", "GUI:Connection:Addresses", addressesList.ToArray());

                                    comboBoxAddresses.Items.Clear();
                                    comboBoxAddresses.Items.AddRange(addressesList
                                        .Select(v => new ComboboxItem
                                        {
                                            Text = v,
                                            Value = v
                                        })
                                        .ToArray());

                                    comboBoxAddresses.Text = addressesList?.FirstOrDefault();
                                }

                                ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new ActionCmd
                                {
                                    Flags = (int)NetworkCommandTypes.CONNECT,
                                    Values = [url]
                                });
                            }

                            connectionForm.Close();
                        }
                    };

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonCancel")
                        .FirstOrDefault() is Button buttonCancel)
                    buttonCancel.Click += (sender, e) =>
                    {
                        var connectionForm = GetForm(nameof(Connection));
                        connectionForm?.Close();
                    };

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "comboBoxUsernames")
                        .FirstOrDefault() is ComboBox comboBoxUsernames)
                {
                    if (SettingsManager.Current.Get<string[]>("GUI:Connection:Usernames") is string[] list)
                    {
                        var usernamesList = new UniqueList<string>(50, list);

                        comboBoxUsernames.Items.AddRange(usernamesList
                            .Select(v => new ComboboxItem
                            {
                                Text = v,
                                Value = v
                            })
                            .ToArray());

                        comboBoxUsernames.Text = usernamesList?.FirstOrDefault();
                    }
                }

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "comboBoxAddresses")
                        .FirstOrDefault() is ComboBox comboBoxAddresses)
                {
                    if (SettingsManager.Current.Get<string[]>("GUI:Connection:Addresses") is string[] list)
                    {
                        var addressesList = new UniqueList<string>(50, list);

                        comboBoxAddresses.Items.AddRange(addressesList
                            .Select(v => new ComboboxItem
                            {
                                Text = v,
                                Value = v
                            })
                            .ToArray());

                        comboBoxAddresses.Text = addressesList?.FirstOrDefault();
                    }
                }
            }
        }

        if (connectionForm != null)
        {
            connectionForm.TopMost = true;

            connectionForm.Show();
            connectionForm.Focus();

            SessionState.RefreshScreen(LayerScreenTypes.Base);
            SessionState.RefreshUI();
            SessionState.RefreshScreen();
            SessionState.RefreshRibbon();
        }
    }

    private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
        var name = e?.ClickedItem?.Name;

        if (string.IsNullOrWhiteSpace(name)) return;

        switch (name)
        {
            case nameof(GoBack):
            case nameof(GoForward):
                if (SessionState.ConnectionState.IsConnected() &&
                    SessionState.History.History.Count > 0)
                {
                    var url = (string?)null;

                    switch (name)
                    {
                        case nameof(GoBack):
                            if (!SessionState.History.Position.HasValue ||
                                SessionState.History.History.Keys.Min() != SessionState.History.Position.Value)
                                url = SessionState.History.Back();
                            break;
                        case nameof(GoForward):
                            if (SessionState.History.Position.HasValue &&
                                SessionState.History.History.Keys.Max() != SessionState.History.Position.Value)
                                url = SessionState.History.Forward();
                            break;
                    }

                    if (url != null &&
                        RegexConstants.REGEX_PARSE_URL.IsMatch(url))
                    {
                        var match = url.ParseUrl(RegexConstants.ParseUrlOptions.IncludeIPEndPoint | RegexConstants.ParseUrlOptions.IncludeQuery);
                        if (match.Count < 2) break;

                        var hostname = match["Hostname"];
                        var port = Convert.ToInt32(match["Port"]);
                        var roomID = Convert.ToInt16(match["Path"]);

                        if ((SessionState.ConnectionState?.IsConnected() ?? false) &&
                            SessionState.ConnectionState?.HostAddr?.Address.ToString() == hostname &&
                            SessionState.ConnectionState.HostAddr.Port == port &&
                            roomID != 0)
                            SessionState.Send<IClientDesktopSessionState, MSG_ROOMGOTO>(
                                SessionState.UserId,
                                new MSG_ROOMGOTO
                                {
                                    Dest = roomID
                                });
                        else
                            ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new ActionCmd
                            {
                                Flags = (int)NetworkCommandTypes.CONNECT,
                                Values = [url]
                            });
                    }
                }

                break;
            case nameof(Entities.Ribbon.Connection):
                ApiManager.Current.ApiBindings.GetValue("ShowConnectionForm")?.Binding(SessionState, null);
                break;
            case nameof(Chatlog):
                ApiManager.Current.ApiBindings.GetValue("ShowLogForm")?.Binding(SessionState, null);
                break;
            case nameof(UsersList):
                ApiManager.Current.ApiBindings.GetValue("ShowUserListForm")?.Binding(SessionState, null);
                break;
            case nameof(RoomsList):
                ApiManager.Current.ApiBindings.GetValue("ShowRoomListForm")?.Binding(SessionState, null);
                break;
            case nameof(Bookmarks):
            case nameof(LiveDirectory):
            case nameof(DoorOutlines):
            case nameof(UserNametags):
            case nameof(Tabs):
            case nameof(Terminal):
            case nameof(SuperUser):
            case nameof(Draw):
            case nameof(Sounds):
                break;
        }
    }

    private void toolStripDropdownlist_Click(object sender = null, EventArgs e = null)
    {
    }

    private void toolStripMenuItem_Click(object sender = null, EventArgs e = null)
    {
    }

    private void contextMenuItem_Click(object sender = null, EventArgs e = null)
    {
        if (sender is not ToolStripMenuItem contextMenuItem) return;

        if (contextMenuItem.Tag is not object[] values) return;

        var cmd = (ContextMenuCommandTypes)values[0];

        if (SessionState.UserDesc.IsModerator ||
            SessionState.UserDesc.IsAdministrator)
            switch (cmd)
            {
                case ContextMenuCommandTypes.CMD_PIN:
                case ContextMenuCommandTypes.CMD_UNPIN:
                case ContextMenuCommandTypes.CMD_GAG:
                case ContextMenuCommandTypes.CMD_UNGAG:
                case ContextMenuCommandTypes.CMD_PROPGAG:
                case ContextMenuCommandTypes.CMD_UNPROPGAG:
                {
                    var value = (UserID)values[1];

                    SessionState.Send<IClientDesktopSessionState, MSG_WHISPER>(
                        SessionState.UserId,
                        new MSG_WHISPER
                        {
                            TargetID = value,
                            Text = $"`{cmd.GetDescription()}"
                        });
                }

                    break;
                case ContextMenuCommandTypes.CMD_KILLUSER:
                {
                    var value = (UserID)values[1];

                    SessionState.Send<IClientDesktopSessionState, MSG_KILLUSER>(
                        SessionState.UserId,
                        new MSG_KILLUSER
                        {
                            TargetID = value
                        });
                }

                    break;
                case ContextMenuCommandTypes.CMD_SPOTDEL:
                {
                    var value = (HotspotID)values[1];

                    SessionState.Send<IClientDesktopSessionState, MSG_SPOTDEL>(
                        SessionState.UserId,
                        new MSG_SPOTDEL
                        {
                            SpotID = value
                        });
                }

                    break;
            }

        switch (cmd)
        {
            case ContextMenuCommandTypes.UI_SPOTSELECT:
            {
                var value = (HotspotID)values[1];

                SessionState.SelectedHotSpot = SessionState.RoomInfo?.HotSpots
                    ?.Where(s => s.HotspotID == value)
                    ?.FirstOrDefault();
            }

                break;
            case ContextMenuCommandTypes.UI_PROPSELECT:
            {
                var value = (AssetID)values[1];

                SessionState.SelectedProp = SessionState.RoomInfo?.LooseProps
                    ?.Where(s => s.AssetSpec.Id == value)
                    ?.Select(s => s.AssetSpec)
                    ?.FirstOrDefault();
            }

                break;
            case ContextMenuCommandTypes.UI_USERSELECT:
            {
                var value = (UserID)values[1];

                SessionState.SelectedUser = SessionState.RoomUsers.GetValueLocked(value);
            }

                break;
            case ContextMenuCommandTypes.CMD_PROPDEL:
            {
                var value = (AssetID)values[1];

                SessionState.Send<IClientDesktopSessionState, MSG_PROPDEL>(
                    SessionState.UserId,
                    new MSG_PROPDEL
                    {
                        PropNum = value
                    });
            }

                break;
            case ContextMenuCommandTypes.CMD_USERMOVE:
            {
                var value = values[1] as Lib.Core.Entities.Shared.Types.Point;

                SessionState.UserDesc.RoomPos = value;

                var user = SessionState.RoomUsers.GetValueLocked(SessionState.UserId);
                if (user != null)
                {
                    user.RoomPos = value;
                    user.Extended["CurrentMessage"] = null;

                    if (user.Extended["MessageQueue"] is DisposableQueue<MsgBubble> queue) queue.Clear();

                    SessionState.RefreshScreen(
                        LayerScreenTypes.UserProp,
                        LayerScreenTypes.UserNametag,
                        LayerScreenTypes.Messages);

                    SessionState.Send<IClientDesktopSessionState, MSG_USERMOVE>(
                        SessionState.UserId,
                        new MSG_USERMOVE
                        {
                            Pos = value
                        });
                }
            }

                break;
        }
    }

    #endregion

    #region UI Methods

    private void _FormClosed(object sender, EventArgs e)
    {
        if (IsDisposed) return;

        if (sender is not Form form) return;

        var key = _uiControls
            .Where(c => c.Value == form)
            .Select(c => c.Key)
            .FirstOrDefault();
        if (key != null)
            _uiControls.TryRemove(key, out _);
    }

    private void _ConnectionEstablished(object sender, EventArgs e)
    {
        if (IsDisposed) return;

        if (this == sender)
            ((Job<ActionCmd>)Jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
            {
                CmdFnc = a =>
                {
                    if (a[0] is not IClientDesktopSessionState sessionState) return null;

                    sessionState.RefreshUI();
                    sessionState.RefreshRibbon();

                    return null;
                },
                Values = [sender]
            });
    }

    private void _ConnectionDisconnected(object sender, EventArgs e)
    {
        if (IsDisposed) return;

        if (this == sender)
            ((Job<ActionCmd>)Jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
            {
                CmdFnc = a =>
                {
                    if (a[0] is not IClientDesktopSessionState sessionState) return null;

                    sessionState.RefreshScreen();
                    sessionState.RefreshUI();
                    sessionState.RefreshRibbon();

                    return null;
                },
                Values = [sender]
            });
    }

    // FormsManager.Current.FormClosed += _FormClosed;
    // ConnectionState.ConnectionEstablished += _ConnectionEstablished;
    // ConnectionState.ConnectionDisconnected += _ConnectionDisconnected;

    #endregion

    #region Form/Control Methods

    public FormBase GetForm(string? friendlyName = null)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName))
            return _uiControls.GetValue(friendlyName) as FormBase;

        return _uiControls.Values
            ?.Where(f => f is FormBase)
            ?.FirstOrDefault() as FormBase;
    }

    public T GetForm<T>(string? friendlyName = null)
        where T : FormBase
    {
        if (!string.IsNullOrWhiteSpace(friendlyName))
            return _uiControls.GetValue(friendlyName) as T;

        return _uiControls.Values
            ?.Where(f => f is T)
            ?.FirstOrDefault() as T;
    }

    public void RegisterForm(string friendlyName, FormBase form)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            form != null)
            _uiControls?.TryAdd(friendlyName, (IDisposable)form);
    }

    public void RegisterForm<T>(string friendlyName, T form)
        where T : FormBase
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            form != null)
            _uiControls?.TryAdd(friendlyName, (IDisposable)form);
    }

    public void UnregisterForm(string friendlyName, FormBase form)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            form != null)
            _uiControls?.TryRemove(friendlyName, out _);
    }

    public void UnregisterForm<T>(string friendlyName, T form)
        where T : FormBase
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            form != null)
            _uiControls?.TryRemove(friendlyName, out _);
    }

    public Control GetControl(string? friendlyName = null)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName))
            return _uiControls.GetValue(friendlyName) as Control;

        return _uiControls.Values
            ?.Where(c => c is Control)
            ?.FirstOrDefault() as Control;
    }

    public T GetControl<T>(string? friendlyName = null)
        where T : Control
    {
        if (!string.IsNullOrWhiteSpace(friendlyName))
            return _uiControls.GetValue(friendlyName) as T;

        return _uiControls.Values
            ?.Where(f => f is T)
            ?.FirstOrDefault() as T;
    }

    public void RegisterControl(string friendlyName, Control control)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryAdd(friendlyName, (IDisposable)control);
    }

    public void RegisterControl<T>(string friendlyName, T control)
        where T : Control
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryAdd(friendlyName, (IDisposable)control);
    }

    public void RegisterControl(string friendlyName, IDisposable control)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryAdd(friendlyName, control);
    }

    public void UnregisterControl(string friendlyName, Control control)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryRemove(friendlyName, out _);
    }

    public void UnregisterControl<T>(string friendlyName, T control)
        where T : Control
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryRemove(friendlyName, out _);
    }

    public void UnregisterControl(string friendlyName, IDisposable control)
    {
        if (!string.IsNullOrWhiteSpace(friendlyName) &&
            control != null)
            _uiControls?.TryRemove(friendlyName, out _);
    }

    #endregion
}