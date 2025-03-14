using System.Collections.Concurrent;
using System.ComponentModel;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.Shared.Assets;
using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Client.Desktop.Helpers;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Common.Constants;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Desktop.Entities.UI;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Enums.App;
using ThePalace.Common.Helpers;
using ThePalace.Common.Interfaces.Threading;
using ThePalace.Common.Threading;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Scripting;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Entities.Threading;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Helpers.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Logging.Entities;
using ThePalace.Media.SoundPlayer;
using AssetID = int;
using Connection = ThePalace.Client.Desktop.Forms.Connection;
using HotspotID = short;
using RegexConstants = ThePalace.Common.Constants.RegexConstants;
using UserID = int;

#if WINDOWS10_0_17763_0_OR_GREATER
using Microsoft.Toolkit.Uwp.Notifications;
#endif

namespace ThePalace.Client.Desktop;

public class Program : SingletonDisposable<Program>, IApp<IDesktopSessionState>
{
    private static readonly Type CONST_TYPE_MSG_Header = typeof(MSG_Header);

    #region cStr

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "ThePalace.Media.dll");
        if (File.Exists(filePath))
            Assembly.LoadFile(filePath);

        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        EventBus.Current.Subscribe(AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName?.StartsWith("ThePalace.Common.Client") == true)
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.GetInterfaces().Contains(typeof(IEventHandler)) &&
                t.Namespace?.StartsWith("ThePalace.Common.Client.Entities.Business") == true)
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
                                                RegexConstants.ParseUrlOptions.IncludeProtocol |
                                                RegexConstants.ParseUrlOptions.IncludeIPEndPoint |
                                                RegexConstants.ParseUrlOptions.IncludeQueryString);
                                            if (match.Count < 3 ||
                                                match["Protocol"]?.ToLowerInvariant() != "palace") break;

                                            var hostname = match["Hostname"];
                                            var port = Convert.ToInt32(match["Port"]);

                                            Current.SessionState.ConnectionState.Connect(hostname, port);
                                            return;
                                        case NetworkCommandTypes.DISCONNECT:
                                        default:
                                            Current.SessionState.ConnectionState.Disconnect();
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
                                var delay = RndGenerator.Next(150, 350);

                                if ((Current?.SessionState?.ConnectionState?.BytesSend?.Length ?? 0) > 0)
                                {
                                    var msgBytes = Current?.SessionState?.ConnectionState?.BytesSend.Dequeue();
                                    Current.SessionState.ConnectionState.Send(msgBytes, directAccess: true);

                                    delay = RndGenerator.Next(75, 150);
                                }

                                cancellationToken.Token.ThrowIfCancellationRequested();

                                await Task.Delay(delay, cancellationToken.Token);
                            }
                        },

                        #endregion

                        #region Process BytesReceived Processor Sub-Task

                        async () =>
                        {
                            var cancellationToken = jobs[ThreadQueues.Network].TokenSource;
                            var msgHeader = (MSG_Header?)null;
                            var msgObj = (IProtocol?)null;
                            var eventType = (string?)null;
                            var msgType = (Type?)null;

                            while (!cancellationToken.IsCancellationRequested)
                            {
                                var delay = RndGenerator.Next(150, 350);

                                if ((Current?.SessionState?.ConnectionState?.BytesReceived?.Length ?? 0) > 0)
                                {
                                    if (msgHeader == null)
                                    {
                                        var msgHeaderBuffer = new byte[12];

                                        var bytesRead = Current?.SessionState?.ConnectionState?.BytesReceived.Read(msgHeaderBuffer, 0, msgHeaderBuffer.Length);
                                        if (bytesRead < msgHeaderBuffer.Length) throw new SocketException(-1, nameof(msgHeaderBuffer));

                                        using (var ms = new MemoryStream(msgHeaderBuffer))
                                        {
                                            msgHeader = new MSG_Header();
                                            if (msgHeader == null) throw new OutOfMemoryException(nameof(MSG_Header));

                                            ms.PalaceDeserialize(msgHeader, CONST_TYPE_MSG_Header);
                                        }
                                    }

                                    eventType = msgHeader.EventType.ToString();
                                    msgType = AppDomain.CurrentDomain.GetAssemblies()
                                        ?.Where(a => a.FullName.StartsWith("ThePalace"))
                                        ?.SelectMany(t => t.GetTypes())
                                        ?.Where(t => t.Name == eventType)
                                        ?.FirstOrDefault();
                                    if (msgType == null) throw new InvalidDataException(nameof(msgType));

                                    if (msgHeader != null &&
                                        msgHeader.Length > 0 &&
                                        Current?.SessionState?.ConnectionState?.BytesReceived.Length > msgHeader.Length &&
                                        msgObj == null)
                                    {
                                        var msgBuffer = new byte[msgHeader.Length];
                                        var bytesRead = Current?.SessionState?.ConnectionState?.BytesReceived.Read(msgBuffer, 0, msgBuffer.Length);
                                        if (bytesRead < msgHeader.Length) throw new SocketException(-1, nameof(msgBuffer));

                                        using (var ms = new MemoryStream(msgBuffer))
                                        {
                                            if (msgHeader.Length > 0)
                                            {
                                                msgObj = (IProtocol?)msgType.GetInstance();
                                                if (msgObj == null) throw new OutOfMemoryException(nameof(IProtocol));

                                                ms.PalaceDeserialize(
                                                    msgObj,
                                                    msgType);
                                            }
                                        }
                                    }
                                }

                                if (msgHeader != null &&
                                    msgType != null)
                                {
                                    var boType = EventBus.Current.GetType(msgType);
                                    if (boType == null) throw new InvalidDataException(nameof(msgType));

                                    EventBus.Current.Publish(
                                        Current,
                                        boType,
                                        new ProtocolEventParams
                                        {
                                            SourceID = Current?.SessionState?.UserId ?? 0,
                                            RefNum = msgHeader.RefNum,
                                            Request = msgObj
                                        });

                                    msgHeader = null;
                                    msgType = null;
                                    msgObj = null;

                                    delay = RndGenerator.Next(75, 150);
                                }

                                cancellationToken.Token.ThrowIfCancellationRequested();

                                await Task.Delay(delay, cancellationToken.Token);
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

                var assetDesc = AssetsManager.Current.GetAsset(Current.SessionState, assetCmd.AssetDesc.AssetRec.AssetSpec, true);
                if (assetDesc is not { Image: null }) return;

                await AssetDesc.Render(assetDesc);
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

    public Program()
    {
        _managedResources.AddRange(
            _contextMenu,
            SessionState);
    }

    public Program(Dictionary<ThreadQueues, IJob> jobs) : this()
    {
        _jobs = new(jobs);
    }

    ~Program()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        CONST_eventTypes.ToList().ForEach(t => ScriptEvents.Current.UnregisterEvent(t, RefreshScreen));

        if (SessionState.UIControls.GetValue(nameof(NotifyIcon)) is NotifyIcon trayIcon)
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

    public IDesktopSessionState SessionState { get; protected set; } = SessionManager.Current.CreateSession<DesktopSessionState>();

    protected ConcurrentDictionary<ThreadQueues, IJob> _jobs = new();

    public IReadOnlyDictionary<ThreadQueues, IJob> Jobs
    {
        get => _jobs.AsReadOnly();
        internal set => _jobs = new(value);
    }

    #endregion

    #region Form Methods

    public void RefreshScreen(object sender, EventArgs e)
    {
        if (sender is not IDesktopSessionState sessionState) return;

        if (e is not ScriptEvent scriptEvent) return;

        ((Job<ActionCmd>)_jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
        {
            CmdFnc = a =>
            {
                if (a[0] is not IDesktopSessionState sessionState) return null;

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
                    if (a[0] is not IDesktopSessionState sessionState) return null;

                    ShowAppForm();

                    var form = sessionState.GetForm();
                    if (form == null) return null;

                    if (sessionState.UIControls.GetValue(nameof(NotifyIcon)) is NotifyIcon trayIcon) return null;

                    trayIcon = new NotifyIcon
                    {
                        ContextMenuStrip = _contextMenu,
                        Icon = form.Icon,
                        Visible = true
                    };
                    sessionState.RegisterControl(nameof(NotifyIcon), trayIcon);

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

        SessionState.RegisterForm(nameof(Program), form);

        form.SessionState = SessionState;
        form.FormClosed += (sender, e) =>
            SessionState.UnregisterForm(nameof(Program), sender as FormBase);

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

            if (SessionState.GetControl("toolStrip") is ToolStrip toolStrip)
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

        var tabIndex = 0;

        if (SessionState.GetControl("toolStrip") is not ToolStrip toolStrip)
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
                SessionState.RegisterControl(nameof(toolStrip), toolStrip);

                toolStrip.Stretch = true;
                toolStrip.GripMargin = new Padding(0);
                toolStrip.ImageScalingSize = new Size(38, 38);
                toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
                toolStrip.RenderMode = ToolStripRenderMode.Professional;
                toolStrip.Renderer = new CustomToolStripRenderer();
                toolStrip.ItemClicked += toolStrip_ItemClicked;
            }
        }

        if (SessionState.GetControl("imgScreen") is not PictureBox imgScreen)
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
                        var point = new Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                SessionState.UserDesc.UserRec.RoomPos = point;

                                var user = (UserDesc?)null;
                                user = SessionState.RoomUsers.GetValueLocked(SessionState.UserId);
                                if (user != null)
                                {
                                    user.UserRec.RoomPos = point;
                                    user.Extended["CurrentMessage"] = null;

                                    if (user.Extended["MessageQueue"] is DisposableQueue<MsgBubble> queue)
                                        queue.Clear();

                                    SessionState.RefreshScreen(
                                        LayerScreenTypes.UserProp,
                                        LayerScreenTypes.UserNametag,
                                        LayerScreenTypes.Messages);

                                    SessionState.Send(
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
                                        if (roomUser.UserRec.UserId == 0 ||
                                            roomUser.UserRec.RoomPos == null)
                                        {
                                            continue;
                                        }
                                        else if (roomUser.UserRec.UserId != SessionState.UserId &&
                                                 point.IsPointInPolygon(roomUser.UserRec.RoomPos.GetBoundingBox(
                                                     new Size(
                                                         (int)AssetConstants.Values.DefaultPropWidth,
                                                         (int)AssetConstants.Values.DefaultPropHeight),
                                                     true)))
                                        {
                                            toolStripItem =
                                                _contextMenu.Items.Add($"Select User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_USERSELECT,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (!SessionState.UserDesc.IsModerator &&
                                                !SessionState.UserDesc.IsAdministrator) continue;

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Pin User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_PIN,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Unpin User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNPIN,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Gag User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_GAG,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Ungag User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNGAG,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Propgag User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_PROPGAG,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Unpropgag User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_UNPROPGAG,
                                                    roomUser.UserRec.UserId
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Kill User: {roomUser.UserRec.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.CMD_KILLUSER,
                                                    roomUser.UserRec.UserId
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
                                                _contextMenu.Items.Add($"Select Spot: {hotSpot.SpotInfo.HotspotID}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_SPOTSELECT,
                                                    hotSpot.SpotInfo.HotspotID
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (!SessionState.UserDesc.IsModerator &&
                                                !SessionState.UserDesc.IsAdministrator) continue;

                                            toolStripItem =
                                                _contextMenu.Items.Add($"Delete Spot: {hotSpot.SpotInfo.HotspotID}");
                                            if (toolStripItem == null) continue;

                                            toolStripItem.Tag = new object[]
                                            {
                                                ContextMenuCommandTypes.CMD_SPOTDEL,
                                                hotSpot.SpotInfo.HotspotID
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

                    if (SessionState.ConnectionState.IsConnected())
                    {
                        var point = new Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                        if ((SessionState.RoomUsers?.Count ?? 0) > 0)
                            foreach (var roomUser in SessionState.RoomUsers.Values)
                                if (roomUser.UserRec.UserId == 0 ||
                                    roomUser.UserRec.RoomPos == null)
                                {
                                    continue;
                                }
                                else if (point.IsPointInPolygon(
                                             roomUser.UserRec.RoomPos.GetBoundingBox(
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

                        if ((SessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                            foreach (var hotSpot in SessionState.RoomInfo.HotSpots)
                                if (point.IsPointInPolygon(hotSpot.Vortexes.ToArray()))
                                {
                                    imgScreen.Cursor = Cursors.Hand;
                                    break;
                                }
                    }
                };

                SessionState.RegisterControl(nameof(imgScreen), imgScreen);
            }
        }

        if (SessionState.GetControl("labelInfo") is not Label labelInfo)
        {
            labelInfo = FormsManager.Current.CreateControl<FormBase, Label>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                Margin = new Padding(0, 0, 0, 0)
            })?.FirstOrDefault();

            if (labelInfo != null)
                SessionState.RegisterControl(nameof(labelInfo), labelInfo);
        }

        if (SessionState.GetControl("txtInput") is not TextBox txtInput)
        {
            txtInput = FormsManager.Current.CreateControl<FormBase, TextBox>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = tabIndex++,
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
                                        if (a[0] is not IDesktopSessionState sessionState) return null;

                                        if (a[1] is not string text) return null;

                                        try
                                        {
                                            var atomlist = IptscraeEngine.Parse(
                                                sessionState.ScriptTag as IptTracking,
                                                text,
                                                false);
                                            IptscraeEngine.Executor(atomlist, sessionState.ScriptTag as IptTracking);
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
                                    SessionState.Send(
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

                SessionState.RegisterControl(nameof(txtInput), txtInput);
            }
        }

        SessionState.RefreshScreen(LayerScreenTypes.Base);
        SessionState.RefreshUI();

        ShowConnectionForm();
    }

    private void ShowConnectionForm(object sender = null, EventArgs e = null)
    {
        if (IsDisposed) return;

        var connectionForm = SessionState.GetForm<Connection>(nameof(Connection));
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
            connectionForm.FormClosed += (sender, e) => { SessionState.UnregisterForm(nameof(Connection), sender as FormBase); };

            if (connectionForm != null)
            {
                SessionState.RegisterForm(nameof(Connection), connectionForm);

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonDisconnect")
                        .FirstOrDefault() is Button buttonDisconnect)
                {
                    buttonDisconnect.Click += (sender, e) =>
                    {
                        if (SessionState.ConnectionState?.IsConnected() ?? false)
                            SessionState.ConnectionState.Disconnect();

                        var connectionForm = SessionState.GetForm(nameof(Connection));
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
                        var connectionForm = SessionState.GetForm(nameof(Connection));
                        if (connectionForm != null)
                        {
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

                            if (connectionForm.Controls
                                    .Cast<Control>()
                                    .Where(c => c.Name == "textBoxRoomID")
                                    .FirstOrDefault() is TextBox textBoxRoomID)
                            {
                                var roomID = (short)0;

                                if (!string.IsNullOrEmpty(textBoxRoomID.Text))
                                    roomID = Convert.ToInt16(textBoxRoomID.Text);

                                SessionState.RegInfo.DesiredRoom = roomID;
                            }

                            if (connectionForm.Controls
                                    .Cast<Control>()
                                    .Where(c => c.Name == "comboBoxAddresses")
                                    .FirstOrDefault() is ComboBox comboBoxAddresses &&
                                !string.IsNullOrWhiteSpace(comboBoxAddresses.Text))
                                ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new ActionCmd
                                {
                                    Flags = (int)NetworkCommandTypes.CONNECT,
                                    Values = [$"palace://{comboBoxAddresses.Text}"]
                                });

                            connectionForm.Close();
                        }
                    };

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonCancel")
                        .FirstOrDefault() is Button buttonCancel)
                    buttonCancel.Click += (sender, e) =>
                    {
                        var connectionForm = SessionState.GetForm(nameof(Connection));
                        connectionForm?.Close();
                    };

                if (connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "comboBoxUsernames")
                        .FirstOrDefault() is ComboBox comboBoxUsernames)
                {
                    if (SettingsManager.Current.Get<string[]>("GUI:Connection:Usernames") is string[] usernamesList)
                    {
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
                    if (SettingsManager.Current.Get<string[]>("GUI:Connection:Addresses") is string[] addressesList)
                    {
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

        if (!string.IsNullOrWhiteSpace(name))
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
                            var match = url.ParseUrl(RegexConstants.ParseUrlOptions.IncludeIPEndPoint | RegexConstants.ParseUrlOptions.IncludeQueryString);
                            if (match.Count < 2) break;

                            var hostname = match["Hostname"];
                            var port = Convert.ToInt32(match["Port"]);
                            var roomID = Convert.ToInt16(match["Path"]);

                            if ((SessionState.ConnectionState?.IsConnected() ?? false) &&
                                SessionState.ConnectionState?.HostAddr?.Address.ToString() == hostname &&
                                SessionState.ConnectionState.HostAddr.Port == port &&
                                roomID != 0)
                                SessionState.Send(
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

                    SessionState.Send(
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

                    SessionState.Send(
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

                    SessionState.Send(
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
                    ?.Where(s => s.SpotInfo.HotspotID == value)
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

                SessionState.Send(
                    SessionState.UserId,
                    new MSG_PROPDEL
                    {
                        PropNum = value
                    });
            }

                break;
            case ContextMenuCommandTypes.CMD_USERMOVE:
            {
                var value = values[1] as Core.Entities.Shared.Types.Point;

                SessionState.UserDesc.UserRec.RoomPos = value;

                var user = SessionState.RoomUsers.GetValueLocked(SessionState.UserId);
                if (user != null)
                {
                    user.UserRec.RoomPos = value;
                    user.Extended["CurrentMessage"] = null;

                    if (user.Extended["MessageQueue"] is DisposableQueue<MsgBubble> queue) queue.Clear();

                    SessionState.RefreshScreen(
                        LayerScreenTypes.UserProp,
                        LayerScreenTypes.UserNametag,
                        LayerScreenTypes.Messages);

                    SessionState.Send(
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
}