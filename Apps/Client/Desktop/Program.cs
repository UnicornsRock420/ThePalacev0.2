using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Desktop.Entities.UI;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Common.Interfaces.Threading;
using ThePalace.Common.Threading;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Scripting;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Threading;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Factories.Scripting;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Logging.Entities;
using ThePalace.Network.Factories;

namespace ThePalace.Client.Desktop;

public partial class Program : Disposable
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var app = (Program?)null;
        var job = (IJob?)null;

        #region Jobs
        job = TaskManager.Current.CreateTask<ActionCmd>(q =>
            {
                // TODO: GUI

                if (!q.IsEmpty &&
                    q.TryDequeue(out var cmd))
                {
                    if (cmd.Values != null)
                        cmd.CmdFnc(cmd.Values);
                    else
                        cmd.CmdFnc();
                }
            },
            jobState: null,
            opts: RunOptions.UseTimer,
            timer: new UITimer
            {
                Enabled = true,
            });
        if (job != null)
        {
            _jobs[ThreadQueues.GUI] = job;

            ((Job<ActionCmd>)job).Enqueue(new ActionCmd
            {
                CmdFnc = a =>
                {
                    app = new Program();

                    return null;
                },
            });
        }

        job = TaskManager.Current.CreateTask<ActionCmd>(q =>
            {
                if (!q.IsEmpty &&
                    q.TryDequeue(out var cmd))
                {
                    if (cmd.Values != null)
                        cmd.CmdFnc(cmd.Values);
                    else
                        cmd.CmdFnc();
                }

                while ((app?.SessionState?.ConnectionState?.BytesReceived?.Length ?? 0) > 0)
                {
                    // TODO: Network

                }
            },
            null,
            RunOptions.UseResetEvent);
        if (job != null)
        {
            _jobs[ThreadQueues.Network] = job;

            AsyncTcpSocket.DataReceived += (o, a) => _jobs[ThreadQueues.Network].ResetEvent.Set();
        }

        job = TaskManager.Current.CreateTask<MediaCmd>(q =>
            {
                if (!q.IsEmpty &&
                    q.TryDequeue(out var mediaCmd))
                {
                    // TODO: Media

                }
            },
            null,
            RunOptions.UseResetEvent);
        if (job != null)
        {
            _jobs[ThreadQueues.Media] = job;
        }

        job = TaskManager.Current.CreateTask<AssetCmd>(q =>
        {
            if (!q.IsEmpty &&
                q.TryDequeue(out var assetCmd))
            {
                if (!AssetsManager.Current.Assets.ContainsKey(assetCmd.AssetInfo.AssetInfo.AssetSpec.Id))
                {
                    // TODO: Assets

                }
            }
        },
            null,
            RunOptions.UseResetEvent);
        if (job != null)
        {
            _jobs[ThreadQueues.Assets] = job;
        }

        job = TaskManager.Current.CreateTask(q => TaskManager.Current.Run(resources: FormsManager.Current),
            null,
            RunOptions.UseSleepInterval | RunOptions.RunNow);
        if (job != null)
        {
            _jobs[ThreadQueues.Core] = job;
        }
        #endregion

        Application.Run(FormsManager.Current);
        TaskManager.Current.Shutdown();
    }

    private ContextMenuStrip _contextMenu = new();
    public IDesktopSessionState SessionState { get; protected set; } = SessionManager.Current.CreateSession<DesktopSessionState>();

    private static readonly ScreenLayers[] CONST_allLayers = Enum.GetValues<ScreenLayers>()
        .Where(v => !new[] { ScreenLayers.Base, ScreenLayers.DimRoom }.Contains(v))
        .ToArray();

    private static readonly IReadOnlyList<IptEventTypes> CONST_eventTypes = Enum.GetValues<IptEventTypes>()
        .Where(v => v.GetType()?.GetField(v.ToString())?.GetCustomAttributes<ScreenRefreshAttribute>()?.Any() ?? false)
        .ToList()
        .AsReadOnly();

    private static readonly IReadOnlyList<IptEventTypes> CONST_uiRefreshEvents = Enum.GetValues<IptEventTypes>()
        .Where(v => v.GetType()?.GetField(v.ToString())?.GetCustomAttributes<UIRefreshAttribute>()?.Any() ?? false)
        .ToList()
        .AsReadOnly();

    private static readonly IReadOnlyDictionary<IptEventTypes[], ScreenLayers[]> CONST_EventLayerMappings = new Dictionary<IptEventTypes[], ScreenLayers[]>
    {
        { [IptEventTypes.MsgHttpServer, IptEventTypes.RoomLoad], [ScreenLayers.Base] },
        { [IptEventTypes.InChat], [ScreenLayers.Messages] },
        { [IptEventTypes.NameChange], [ScreenLayers.UserNametag] },
        { [IptEventTypes.FaceChange, IptEventTypes.MsgUserProp], [ScreenLayers.UserProp] },
        { [IptEventTypes.LoosePropAdded, IptEventTypes.LoosePropDeleted, IptEventTypes.LoosePropMoved], [ScreenLayers.LooseProp] },
        { [IptEventTypes.Lock, IptEventTypes.MsgPictDel, IptEventTypes.MsgPictMove, IptEventTypes.MsgPictMove, IptEventTypes.MsgPictNew, IptEventTypes.StateChange, IptEventTypes.UnLock], [ScreenLayers.SpotImage] },
        { [IptEventTypes.ColorChange, IptEventTypes.MsgUserDesc, IptEventTypes.MsgUserList, IptEventTypes.MsgUserLog, IptEventTypes.UserEnter], [ScreenLayers.UserProp, ScreenLayers.UserNametag] },
        { [IptEventTypes.MsgAssetSend], [ScreenLayers.UserProp, ScreenLayers.LooseProp] },
        { [IptEventTypes.SignOn, IptEventTypes.UserLeave, IptEventTypes.UserMove], [ScreenLayers.UserProp, ScreenLayers.UserNametag, ScreenLayers.Messages] },
        { [IptEventTypes.MsgDraw], [ScreenLayers.BottomPaint, ScreenLayers.TopPaint] },
        { [IptEventTypes.MsgSpotDel, IptEventTypes.MsgSpotMove, IptEventTypes.MsgSpotNew], [ScreenLayers.SpotBorder, ScreenLayers.SpotNametag, ScreenLayers.SpotImage] },
    }.AsReadOnly();

    private static readonly ConcurrentDictionary<ThreadQueues, IJob> _jobs = new();
    public static IReadOnlyDictionary<ThreadQueues, IJob> Jobs => _jobs.AsReadOnly();

    public Program()
    {
        this._managedResources.Add(_contextMenu);

        this.Initialize();
    }
    ~Program() => this.Dispose(false);

    public override void Dispose()
    {
        if (this.IsDisposed) return;

        base.Dispose();

        foreach (var type in CONST_eventTypes)
            ScriptEvents.Current.UnregisterEvent(type, this.RefreshScreen);

        var trayIcon = this.SessionState.UIControls.GetValue(nameof(NotifyIcon)) as NotifyIcon;
        if (trayIcon != null)
        {
            trayIcon.Visible = false;
            try { trayIcon.Dispose(); } catch { }
        }
    }

    public void Initialize()
    {
        if (this.IsDisposed) return;

        foreach (var type in CONST_eventTypes)
        {
            ScriptEvents.Current.RegisterEvent(type, this.RefreshScreen);
        }

        ApiManager.Current.RegisterApi(nameof(this.ShowConnectionForm), this.ShowConnectionForm);
        ApiManager.Current.RegisterApi(nameof(this.toolStripDropdownlist_Click), this.toolStripDropdownlist_Click);
        ApiManager.Current.RegisterApi(nameof(this.toolStripMenuItem_Click), this.toolStripMenuItem_Click);
        ApiManager.Current.RegisterApi(nameof(this.contextMenuItem_Click), this.contextMenuItem_Click);

        ShowAppForm();

#if WINDOWS10_0_17763_0_OR_GREATER
        //var toast = new ToastCfg
        //{
        //    ExpirationTime = DateTime.Now.AddMinutes(1),
        //    Args = new Dictionary<string, object>
        //    {
        //        { "action", "whisperMsg" },
        //        { "connectionId", 123 },
        //        { "conversationId", 456 },
        //    }.AsReadOnly(),
        //    Text = new List<string>
        //    {
        //        "Beat it like it owes you money!",
        //    }.AsReadOnly(),
        //};
        //TaskManager.Current.DispatchToast(toast);
#endif

        ((Job<ActionCmd>)_jobs[ThreadQueues.GUI])?.Enqueue(
            new ActionCmd
            {
                CmdFnc = a =>
                {
                    var sessionState = a[0] as IDesktopSessionState;
                    if (sessionState == null) return null;

                    ShowAppForm();

                    var form = sessionState.GetForm(nameof(Program));
                    if (form == null) return null;

                    var trayIcon = sessionState.UIControls.GetValue(nameof(NotifyIcon)) as NotifyIcon;
                    if (trayIcon == null)
                    {
                        trayIcon = new NotifyIcon
                        {
                            ContextMenuStrip = new ContextMenuStrip(),
                            Icon = form.Icon,
                            Visible = true,
                        };
                        sessionState.RegisterControl(nameof(NotifyIcon), trayIcon);

                        trayIcon.ContextMenuStrip.Items.Add("Exit", null, new EventHandler((sender, e) => TaskManager.Current.Dispose()));
                    }

                    return null;
                },
                Values = [this.SessionState],
            });

        return;
    }

    public void RefreshScreen(object sender, EventArgs e)
    {
        var sessionState = sender as IDesktopSessionState;
        if (sessionState == null) return;

        var scriptEvent = e as ScriptEvent;
        if (scriptEvent == null) return;

        var uiRefresh = CONST_uiRefreshEvents.Contains(scriptEvent.EventType);

        var screenLayers = null as ScreenLayers[];
        foreach (var layer in CONST_EventLayerMappings)
        {
            if (CONST_EventLayerMappings.Keys.Contains(scriptEvent.EventType))
            {
                screenLayers = layer.Value;

                break;
            }
        }

        ((Job<ActionCmd>)_jobs[ThreadQueues.GUI]).Enqueue(new ActionCmd
        {
            CmdFnc = a =>
            {
                if (screenLayers.Contains(ScreenLayers.Base))
                    sessionState.RefreshScreen(ScreenLayers.Base);

                if (uiRefresh)
                {
                    sessionState.RefreshUI();
                    sessionState.RefreshRibbon();
                }

                if (!screenLayers.Contains(ScreenLayers.Base))
                    sessionState.RefreshScreen(screenLayers);
                else
                    sessionState.RefreshScreen(CONST_allLayers);

                switch (scriptEvent.EventType)
                {
                    case IptEventTypes.RoomLoad:
                        sessionState.History.RegisterHistory(
                            $"{sessionState.ServerName} - {sessionState.RoomInfo.Name}",
                            $"palace://{sessionState.ConnectionState.HostAddr.Address}:{sessionState.ConnectionState.HostAddr.Port}/{sessionState.RoomInfo.RoomInfo.RoomID}");

                        sessionState.RefreshRibbon();

                        ScriptEvents.Current.Invoke(IptEventTypes.RoomReady, sessionState, null, sessionState.ScriptState);
                        ScriptEvents.Current.Invoke(IptEventTypes.Enter, sessionState, null, sessionState.ScriptState);

                        break;
                }

                return null;
            },
        });
    }

    private void ShowAppForm()
    {
        if (this.IsDisposed) return;

        var form = FormsManager.Current.CreateForm<FormDialog>(new FormCfg
        {
            Load = new EventHandler((sender, e) => _jobs[ThreadQueues.GUI].Run()),
            WindowState = FormWindowState.Minimized,
            AutoScaleMode = AutoScaleMode.Font,
            AutoScaleDimensions = new SizeF(7F, 15F),
            Margin = new Padding(0, 0, 0, 0),
            Visible = false,
        });
        if (form == null) return;

        this.SessionState.RegisterForm(nameof(Program), form);

        form.SessionState = this.SessionState;
        form.FormClosed += new FormClosedEventHandler((sender, e) =>
            this.SessionState.UnregisterForm(nameof(Program), sender as FormBase));

        form.MouseMove += new MouseEventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseMove, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.MouseUp += new MouseEventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseUp, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.MouseDown += new MouseEventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDown, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.DragEnter += new DragEventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.DragLeave += new EventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.DragOver += new DragEventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this.SessionState, null, this.SessionState.ScriptState);
        });
        form.Resize += new EventHandler((sender, e) =>
        {
            this.SessionState.LastActivity = DateTime.UtcNow;

            var form = sender as FormBase;
            if (form == null ||
                form.WindowState != FormWindowState.Normal) return;

            var screenWidth = (Screen.PrimaryScreen?.Bounds.Width ?? 0);
            var screenHeight = (Screen.PrimaryScreen?.Bounds.Height ?? 0);

            if (form.Location.X < 0 ||
                form.Location.Y < 0 ||
                form.Location.X > screenWidth ||
                form.Location.Y > screenHeight)
            {
                form.ClientSize = new Size(screenWidth - 16, screenHeight - 16);
                form.Location = new Point(0, 0);
            }

            form.Location = new Point(
                (screenWidth / 2) - (form.Width / 2),
                (screenHeight / 2) - (form.Height / 2));

            var toolStrip = this.SessionState.GetControl("toolStrip") as ToolStrip;
            if (toolStrip != null)
            {
                toolStrip.Size = new Size(form.Width, form.Height);
            }

            this.SessionState.RefreshUI();
        });

        FormsManager.UpdateForm(form, new FormCfg
        {
            Size = new Size(
                DesktopConstants.AspectRatio.WidescreenDef.Default.Width,
                DesktopConstants.AspectRatio.WidescreenDef.Default.Height),
            WindowState = FormWindowState.Normal,
            Visible = true,
            Focus = true,
        });

        var tabIndex = 0;

        var toolStrip = this.SessionState.GetControl("toolStrip") as ToolStrip;
        if (toolStrip == null)
        {
            toolStrip = FormsManager.Current.CreateControl<FormBase, ToolStrip>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                //Size = new Size(800, 25),
                Margin = new Padding(0, 0, 0, 0),
            })?.FirstOrDefault();

            if (toolStrip != null)
            {
                this.SessionState.RegisterControl(nameof(toolStrip), toolStrip);

                toolStrip.Stretch = true;
                toolStrip.GripMargin = new Padding(0);
                toolStrip.ImageScalingSize = new Size(38, 38);
                toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
                toolStrip.RenderMode = ToolStripRenderMode.Professional;
                toolStrip.Renderer = new CustomToolStripRenderer();
                toolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
            }
        }

        var imgScreen = this.SessionState.GetControl("imgScreen") as PictureBox;
        if (imgScreen == null)
        {
            imgScreen = FormsManager.Current.CreateControl<FormBase, PictureBox>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Margin = new Padding(0, 0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle,
            })?.FirstOrDefault();

            if (imgScreen != null)
            {
                imgScreen.MouseClick += new MouseEventHandler((sender, e) =>
                {
                    if (!this.SessionState.ConnectionState.IsConnected())
                        ShowConnectionForm();
                    else
                    {
                        var point = new ThePalace.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                        switch (e.Button)
                        {
                            case MouseButtons.Left:
                                this.SessionState.UserDesc.UserInfo.RoomPos = point;

                                var user = null as UserDesc;
                                user = this.SessionState.RoomUsers.GetValueLocked(this.SessionState.UserId);
                                if (user != null)
                                {
                                    user.UserInfo.RoomPos = point;
                                    user.Extended["CurrentMessage"] = null;

                                    var queue = user.Extended["MessageQueue"] as DisposableQueue<MsgBubble>;
                                    if (queue != null) queue.Clear();

                                    this.SessionState.RefreshScreen(
                                        ScreenLayers.UserProp,
                                        ScreenLayers.UserNametag,
                                        ScreenLayers.Messages);

                                    ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                                    {
                                        CmdFnc = a =>
                                        {
                                            var sessionState = a[0] as IDesktopSessionState;
                                            if (sessionState == null) return null;

                                            var point = a[1] as ThePalace.Core.Entities.Shared.Types.Point;
                                            if (point == null) return null;

                                            sessionState.Send(new MSG_USERMOVE
                                            {
                                                Pos = point,
                                            });

                                            return null;
                                        },
                                        Values = [this.SessionState, point],
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
                                        ContextMenuCommandTypes.MSG_USERMOVE,
                                        point,
                                    };
                                    toolStripItem.Click += contextMenuItem_Click;
                                }

                                if ((this.SessionState.RoomUsers?.Count ?? 0) > 0)
                                    foreach (var roomUser in this.SessionState.RoomUsers.Values)
                                        if (roomUser.UserInfo.UserId == 0 ||
                                            roomUser.UserInfo.RoomPos == null) continue;
                                        else if (roomUser.UserInfo.UserId != this.SessionState.UserId &&
                                                 BinaryOpsExts.IsPointInPolygon(
                                                     point,
                                                     roomUser.UserInfo.RoomPos.GetBoundingBox(
                                                         new Size(
                                                             (int)AssetConstants.Values.DefaultPropWidth,
                                                             (int)AssetConstants.Values.DefaultPropHeight),
                                                         true)))
                                        {
                                            toolStripItem = _contextMenu.Items.Add($"Select User: {roomUser.UserInfo.Name}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_USERSELECT,
                                                    roomUser.UserInfo.UserId,
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (this.SessionState.UserDesc.IsModerator ||
                                                this.SessionState.UserDesc.IsAdministrator)
                                            {
                                                toolStripItem = _contextMenu.Items.Add($"Pin User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_PIN,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Unpin User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_UNPIN,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Gag User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_GAG,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Ungag User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_UNGAG,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Propgag User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_PROPGAG,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Unpropgag User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.CMD_UNPROPGAG,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }

                                                toolStripItem = _contextMenu.Items.Add($"Kill User: {roomUser.UserInfo.Name}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.MSG_KILLUSER,
                                                        roomUser.UserInfo.UserId,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }
                                            }
                                        }

                                if ((this.SessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
                                {
                                    toolStripItem = _contextMenu.Items.Add("Delete All Props");
                                    if (toolStripItem != null)
                                    {
                                        toolStripItem.Tag = new object[]
                                        {
                                            ContextMenuCommandTypes.MSG_PROPDEL,
                                            -1,
                                        };
                                        toolStripItem.Click += contextMenuItem_Click;
                                    }

                                    var j = 0;
                                    foreach (var looseProp in this.SessionState.RoomInfo.LooseProps)
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
                                            toolStripItem = _contextMenu.Items.Add($"Select Prop: {looseProp.AssetSpec.Id}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_PROPSELECT,
                                                    j,
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            toolStripItem = _contextMenu.Items.Add($"Delete Prop: {looseProp.AssetSpec.Id}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.MSG_PROPDEL,
                                                    j,
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }
                                        }

                                        j++;
                                    }
                                }

                                if ((this.SessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                                    foreach (var hotSpot in this.SessionState.RoomInfo.HotSpots)
                                        if (point.IsPointInPolygon(hotSpot.Vortexes.ToArray()))
                                        {
                                            toolStripItem = _contextMenu.Items.Add($"Select Spot: {hotSpot.SpotInfo.HotspotID}");
                                            if (toolStripItem != null)
                                            {
                                                toolStripItem.Tag = new object[]
                                                {
                                                    ContextMenuCommandTypes.UI_SPOTSELECT,
                                                    hotSpot.SpotInfo.HotspotID,
                                                };
                                                toolStripItem.Click += contextMenuItem_Click;
                                            }

                                            if (this.SessionState.UserDesc.IsModerator ||
                                                this.SessionState.UserDesc.IsAdministrator)
                                            {
                                                toolStripItem = _contextMenu.Items.Add($"Delete Spot: {hotSpot.SpotInfo.HotspotID}");
                                                if (toolStripItem != null)
                                                {
                                                    toolStripItem.Tag = new object[]
                                                    {
                                                        ContextMenuCommandTypes.MSG_SPOTDEL,
                                                        hotSpot.SpotInfo.HotspotID,
                                                    };
                                                    toolStripItem.Click += contextMenuItem_Click;
                                                }
                                            }
                                        }

                                _contextMenu.Show(Cursor.Position);

                                break;
                        }
                    }
                });
                imgScreen.MouseMove += new MouseEventHandler((sender, e) =>
                {
                    imgScreen.Cursor = Cursors.Default;

                    if (this.SessionState.ConnectionState.IsConnected())
                    {
                        var point = new ThePalace.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                        if ((this.SessionState.RoomUsers?.Count ?? 0) > 0)
                            foreach (var roomUser in this.SessionState.RoomUsers.Values)
                                if (roomUser.UserInfo.UserId == 0 ||
                                    roomUser.UserInfo.RoomPos == null) continue;
                                else if (point.IsPointInPolygon(
                                             roomUser.UserInfo.RoomPos.GetBoundingBox(
                                                 new Size(
                                                     (int)AssetConstants.Values.DefaultPropWidth,
                                                     (int)AssetConstants.Values.DefaultPropHeight),
                                                 true)))
                                {
                                    imgScreen.Cursor = Cursors.Hand;
                                    break;
                                }

                        if ((this.SessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
                            foreach (var looseProp in this.SessionState.RoomInfo.LooseProps)
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

                        if ((this.SessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                            foreach (var hotSpot in this.SessionState.RoomInfo.HotSpots)
                                if (point.IsPointInPolygon(hotSpot.Vortexes.ToArray()))
                                {
                                    imgScreen.Cursor = Cursors.Hand;
                                    break;
                                }
                    }
                });

                this.SessionState.RegisterControl(nameof(imgScreen), imgScreen);
            }
        }

        var labelInfo = this.SessionState.GetControl("labelInfo") as Label;
        if (labelInfo == null)
        {
            labelInfo = FormsManager.Current.CreateControl<FormBase, Label>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = 0, //tabIndex++,
                Title = string.Empty,
                Margin = new Padding(0, 0, 0, 0),
            })?.FirstOrDefault();

            if (labelInfo != null)
                this.SessionState.RegisterControl(nameof(labelInfo), labelInfo);
        }

        var txtInput = this.SessionState.GetControl("txtInput") as TextBox;
        if (txtInput == null)
        {
            txtInput = FormsManager.Current.CreateControl<FormBase, TextBox>(form, true, new ControlCfg
            {
                Visible = true,
                TabIndex = tabIndex++,
                Title = string.Empty,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = Color.FromKnownColor(KnownColor.DimGray),
                Multiline = true,
                MaxLength = 255,
            })?.FirstOrDefault();

            if (txtInput != null)
            {
                txtInput.LostFocus += new EventHandler((sender, e) =>
                {
                    txtInput.BackColor = Color.FromKnownColor(KnownColor.LightGray);
                });
                txtInput.GotFocus += new EventHandler((sender, e) =>
                {
                    txtInput.BackColor = Color.FromKnownColor(KnownColor.White);
                });
                txtInput.KeyUp += new KeyEventHandler((sender, e) =>
                {
                    this.SessionState.LastActivity = DateTime.UtcNow;

                    if (e.KeyCode == Keys.Tab)
                    {
                        e.Handled = true;

                        txtInput.Text = string.Empty;
                    }

                    if (!this.SessionState?.ConnectionState?.IsConnected() ?? false)
                    {
                        this.ShowConnectionForm();

                        return;
                    }

                    ScriptEvents.Current.Invoke(IptEventTypes.KeyUp, this.SessionState, null, this.SessionState.ScriptState);

                    if (e.KeyCode == Keys.Enter)
                    {
                        e.Handled = true;

                        var text = txtInput.Text?.Trim();
                        txtInput.Text = string.Empty;

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            if (text[0] == '/')
                            {
                                ((Job<ActionCmd>)_jobs[ThreadQueues.ScriptEngine]).Enqueue(new ActionCmd
                                {
                                    CmdFnc = a =>
                                    {
                                        var sessionState = a[0] as IDesktopSessionState;
                                        if (sessionState == null) return null;

                                        var text = a[1] as string;
                                        if (text == null) return null;

                                        try
                                        {
                                            var atomlist = IptscraeEngine.Parser(
                                                sessionState.ScriptState as IptTracking,
                                                text,
                                                false);
                                            IptscraeEngine.Executor(atomlist, sessionState.ScriptState as IptTracking);
                                        }
                                        catch (Exception ex)
                                        {
                                            LoggerHub.Current.Error(ex);
                                        }

                                        return null;
                                    },
                                    Values = [this.SessionState, string.Concat(text.Skip(1))],
                                });
                            }
                            else
                            {
                                var xTalk = new MSG_XTALK
                                {
                                    Text = text,
                                };

                                ScriptEvents.Current.Invoke(IptEventTypes.Chat, this.SessionState, xTalk, this.SessionState.ScriptState);
                                ScriptEvents.Current.Invoke(IptEventTypes.OutChat, this.SessionState, xTalk, this.SessionState.ScriptState);

                                var iptTracking = this.SessionState.ScriptState as IptTracking;
                                if (iptTracking != null)
                                {
                                    if (iptTracking.Variables?.ContainsKey("CHATSTR") == true)
                                        xTalk.Text = iptTracking.Variables["CHATSTR"].Value.Value.ToString();

                                    if (!string.IsNullOrWhiteSpace(xTalk.Text))
                                    {
                                        ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new ActionCmd
                                        {
                                            CmdFnc = a =>
                                            {
                                                var sessionState = a[0] as IDesktopSessionState;
                                                if (sessionState == null) return null;

                                                var xTalk = a[1] as MSG_XTALK;
                                                if (xTalk == null) return null;

                                                sessionState?.Send(xTalk);

                                                return null;
                                            },
                                            Values = [this.SessionState, xTalk],
                                        });
                                    }
                                }
                            }
                        }
                    }
                });
                txtInput.KeyDown += new KeyEventHandler((sender, e) =>
                {
                    this.SessionState.LastActivity = DateTime.UtcNow;

                    if (e.KeyCode == Keys.Tab)
                    {
                        e.Handled = true;

                        txtInput.Text = string.Empty;
                    }

                    if (!this.SessionState?.ConnectionState?.IsConnected() ?? false) return;

                    ScriptEvents.Current.Invoke(IptEventTypes.KeyDown, this.SessionState, null, this.SessionState.ScriptState);
                });

                this.SessionState.RegisterControl(nameof(txtInput), txtInput);
            }
        }

        this.SessionState.RefreshScreen(ScreenLayers.Base);
        this.SessionState.RefreshUI();

        ShowConnectionForm();
    }
    private void ShowConnectionForm(object sender = null, EventArgs e = null)
    {
        if (this.IsDisposed) return;

        var connectionForm = this.SessionState.GetForm<Forms.Connection>(nameof(Forms.Connection));
        if (connectionForm == null)
        {
            connectionForm = FormsManager.Current.CreateForm<Forms.Connection>(
                new FormCfg
                {
                    AutoScaleDimensions = new SizeF(7F, 15F),
                    AutoScaleMode = AutoScaleMode.Font,
                    WindowState = FormWindowState.Normal,
                    StartPosition = FormStartPosition.CenterScreen,
                    Margin = new Padding(0, 0, 0, 0),
                    Size = new Size(303, 182),
                    Visible = true,
                });
            if (connectionForm == null) return;

            connectionForm.SessionState = this.SessionState;
            connectionForm.FormClosed += new FormClosedEventHandler((sender, e) =>
            {
                this.SessionState.UnregisterForm(nameof(Forms.Connection), sender as FormBase);
            });

            if (connectionForm != null)
            {
                this.SessionState.RegisterForm(nameof(Forms.Connection), connectionForm);

                var buttonDisconnect = connectionForm.Controls
                    .Cast<Control>()
                    .Where(c => c.Name == "buttonDisconnect")
                    .FirstOrDefault() as Button;
                if (buttonDisconnect != null)
                {
                    buttonDisconnect.Click += new EventHandler((sender, e) =>
                    {
                        if ((this.SessionState.ConnectionState?.IsConnected() ?? false) == true)
                            this.SessionState.ConnectionState.Socket?.DropConnection();

                        var connectionForm = this.SessionState.GetForm(nameof(Forms.Connection));
                        connectionForm?.Close();
                    });
                    buttonDisconnect.Visible = this.SessionState?.ConnectionState?.IsConnected() ?? false;
                }

                var buttonConnect = connectionForm.Controls
                    .Cast<Control>()
                    .Where(c => c.Name == "buttonConnect")
                    .FirstOrDefault() as Button;
                if (buttonConnect != null)
                {
                    buttonConnect.Click += new EventHandler((sender, e) =>
                    {
                        var connectionForm = this.SessionState.GetForm(nameof(Forms.Connection));
                        if (connectionForm != null)
                        {
                            //var checkBoxNewTab = connectionForm.Controls
                            //    .Cast<Control>()
                            //    .Where(c => c.Name == "checkBoxNewTab")
                            //    .FirstOrDefault() as CheckBox;
                            //if (checkBoxNewTab?.Checked == true)
                            //{
                            //}

                            var comboBoxUsernames = connectionForm.Controls
                                .Cast<Control>()
                                .Where(c => c.Name == "comboBoxUsernames")
                                .FirstOrDefault() as ComboBox;
                            if (comboBoxUsernames != null)
                            {
                                this.SessionState.RegInfo.UserName = this.SessionState.RegInfo.UserName = comboBoxUsernames.Text;
                            }

                            var textBoxRoomID = connectionForm.Controls
                                .Cast<Control>()
                                .Where(c => c.Name == "textBoxRoomID")
                                .FirstOrDefault() as TextBox;
                            if (textBoxRoomID != null)
                            {
                                var roomID = (short)0;

                                if (!string.IsNullOrEmpty(textBoxRoomID.Text))
                                    roomID = Convert.ToInt16(textBoxRoomID.Text);

                                this.SessionState.RegInfo.DesiredRoom = roomID;
                            }

                            var comboBoxAddresses = connectionForm.Controls
                                .Cast<Control>()
                                .Where(c => c.Name == "comboBoxAddresses")
                                .FirstOrDefault() as ComboBox;
                            if (comboBoxAddresses != null &&
                                !string.IsNullOrWhiteSpace(comboBoxAddresses.Text))
                                ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                                {
                                    CmdFnc = a =>
                                    {
                                        var sessionState = a[0] as IDesktopSessionState;
                                        if (sessionState == null) return null;

                                        var url = a[1] as string;
                                        if (url == null) return null;

                                        if (IPEndPoint.TryParse(url, out var result))
                                        {
                                            sessionState.ConnectionState.Connect(result);
                                        }

                                        return null;
                                    },
                                    Values = [this.SessionState, $"palace://{comboBoxAddresses.Text}"],
                                });

                            connectionForm.Close();
                        }
                    });
                }

                var buttonCancel = connectionForm.Controls
                    .Cast<Control>()
                    .Where(c => c.Name == "buttonCancel")
                    .FirstOrDefault() as Button;
                if (buttonCancel != null)
                {
                    buttonCancel.Click += new EventHandler((sender, e) =>
                    {
                        var connectionForm = this.SessionState.GetForm(nameof(Forms.Connection));
                        connectionForm?.Close();
                    });
                }

                var comboBoxUsernames = connectionForm.Controls
                    .Cast<Control>()
                    .Where(c => c.Name == "comboBoxUsernames")
                    .FirstOrDefault() as ComboBox;
                if (comboBoxUsernames != null)
                {
                    var usernamesList = SettingsManager.Current.Settings[@"\GUI\Connection\Usernames"] as ISettingList;
                    if (usernamesList != null)
                    {
                        comboBoxUsernames.Items.AddRange(usernamesList.Text
                            .Select(v => new ComboboxItem
                            {
                                Text = v,
                                Value = v,
                            })
                            .ToArray());

                        comboBoxUsernames.Text = usernamesList.Text?.FirstOrDefault();
                    }
                }

                var comboBoxAddresses = connectionForm.Controls
                    .Cast<Control>()
                    .Where(c => c.Name == "comboBoxAddresses")
                    .FirstOrDefault() as ComboBox;
                if (comboBoxAddresses != null)
                {
                    var addressesList = SettingsManager.Current.Settings[@"\GUI\Connection\Addresses"] as ISettingList;
                    if (addressesList != null)
                    {
                        comboBoxAddresses.Items.AddRange(addressesList.Text
                            .Select(v => new ComboboxItem
                            {
                                Text = v,
                                Value = v,
                            })
                            .ToArray());

                        comboBoxAddresses.Text = addressesList.Text?.FirstOrDefault();
                    }
                }
            }
        }

        if (connectionForm != null)
        {
            connectionForm.TopMost = true;

            connectionForm.Show();
            connectionForm.Focus();

            this.SessionState.RefreshScreen(ScreenLayers.Base);
            this.SessionState.RefreshUI();
            this.SessionState.RefreshScreen();
            this.SessionState.RefreshRibbon();
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
                    if (this.SessionState.ConnectionState.IsConnected() &&
                        (this.SessionState.History.History.Count > 0))
                    {
                        var url = null as string;

                        switch (name)
                        {
                            case nameof(GoBack):
                                if ((!this.SessionState.History.Position.HasValue ||
                                     this.SessionState.History.History.Keys.Min() != this.SessionState.History.Position.Value))
                                    url = this.SessionState.History.Back();
                                break;
                            case nameof(GoForward):
                                if (this.SessionState.History.Position.HasValue &&
                                    this.SessionState.History.History.Keys.Max() != this.SessionState.History.Position.Value)
                                    url = this.SessionState.History.Forward();
                                break;
                        }

                        if (url != null &&
                            ThePalace.Core.Constants.RegexConstants.REGEX_PALACEURL.IsMatch(url))
                        {
                            var match = ThePalace.Core.Constants.RegexConstants.REGEX_PALACEURL.Match(url);
                            if (match.Groups.Count < 2) break;

                            var host = match.Groups[1].Value;
                            var port = match.Groups.Count > 2 &&
                                       !string.IsNullOrWhiteSpace(match.Groups[2].Value)
                                ? Convert.ToUInt16(match.Groups[2].Value) : (ushort)0;
                            var roomID = match.Groups.Count > 3 &&
                                         !string.IsNullOrWhiteSpace(match.Groups[3].Value)
                                ? Convert.ToInt16(match.Groups[3].Value) : (short)0;

                            if ((this.SessionState.ConnectionState?.IsConnected() ?? false) &&
                                this.SessionState.ConnectionState?.HostAddr?.Address.ToString() == host &&
                                this.SessionState.ConnectionState.HostAddr.Port == port &&
                                roomID != 0)
                                ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                                {
                                    CmdFnc = a =>
                                    {
                                        var sessionState = a[0] as IDesktopSessionState;
                                        if (sessionState == null) return null;

                                        var roomID = a[1] as short?;
                                        if (roomID == null) return null;

                                        sessionState?.Send(new MSG_ROOMGOTO
                                        {
                                            Dest = roomID.Value,
                                        });

                                        return null;
                                    },
                                    Values = [this.SessionState, roomID],
                                });
                            else
                                ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                                {
                                    CmdFnc = a =>
                                    {
                                        var sessionState = a[0] as IDesktopSessionState;
                                        if (sessionState == null) return null;

                                        var url = a[1] as string;
                                        if (url == null) return null;

                                        if (IPEndPoint.TryParse(url, out var result))
                                        {
                                            ConnectionManager.Connect(sessionState.ConnectionState, result);
                                        }

                                        return null;
                                    },
                                    Values = [this.SessionState, url],
                                });
                        }
                    }

                    break;
                case nameof(Connection):
                    ApiManager.Current.ApiBindings.GetValue("ShowConnectionForm")?.Binding(this.SessionState, null);
                    break;
                case nameof(Chatlog):
                    ApiManager.Current.ApiBindings.GetValue("ShowLogForm")?.Binding(this.SessionState, null);
                    break;
                case nameof(UsersList):
                    ApiManager.Current.ApiBindings.GetValue("ShowUserListForm")?.Binding(this.SessionState, null);
                    break;
                case nameof(RoomsList):
                    ApiManager.Current.ApiBindings.GetValue("ShowRoomListForm")?.Binding(this.SessionState, null);
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
        var contextMenuItem = sender as ToolStripMenuItem;
        if (contextMenuItem == null) return;

        var values = contextMenuItem.Tag as object[];
        if (values == null) return;

        var cmd = (ContextMenuCommandTypes)values[0];

        if (this.SessionState.UserDesc.IsModerator ||
            this.SessionState.UserDesc.IsAdministrator)
            switch (cmd)
            {
                case ContextMenuCommandTypes.CMD_PIN:
                case ContextMenuCommandTypes.CMD_UNPIN:
                case ContextMenuCommandTypes.CMD_GAG:
                case ContextMenuCommandTypes.CMD_UNGAG:
                case ContextMenuCommandTypes.CMD_PROPGAG:
                case ContextMenuCommandTypes.CMD_UNPROPGAG:
                    {
                        var value = (Int32)values[1];

                        ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                        {
                            CmdFnc = a =>
                            {
                                var sessionState = a[0] as IDesktopSessionState;
                                if (sessionState == null) return null;

                                var value = a[1] as Int32?;
                                if (value == null) return null;

                                var cmd = a[2] as ContextMenuCommandTypes?;
                                if (cmd == null) return null;

                                sessionState.Send(new MSG_WHISPER
                                {
                                    TargetID = value.Value,
                                    Text = $"`{cmd.Value.GetDescription()}",
                                });

                                return null;
                            },
                            Values = [this.SessionState, value, cmd],
                        });
                    }

                    break;
                case ContextMenuCommandTypes.MSG_KILLUSER:
                    {
                        var value = (UInt32)values[1];

                        ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                        {
                            CmdFnc = a =>
                            {
                                var sessionState = a[0] as IDesktopSessionState;
                                if (sessionState == null) return null;

                                var value = a[1] as UInt32?;
                                if (value == null) return null;

                                sessionState.Send(new MSG_KILLUSER
                                {
                                    TargetID = value.Value,
                                });

                                return null;
                            },
                            Values = [this.SessionState, value],
                        });
                    }

                    break;
                case ContextMenuCommandTypes.MSG_SPOTDEL:
                    {
                        var value = (short)values[1];

                        ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                        {
                            CmdFnc = a =>
                            {
                                var sessionState = a[0] as IDesktopSessionState;
                                if (sessionState == null) return null;

                                var value = a[1] as short?;
                                if (value == null) return null;

                                sessionState.Send(new MSG_SPOTDEL
                                {
                                    SpotID = value.Value,
                                });

                                return null;
                            },
                            Values = [this.SessionState, value],
                        });
                    }

                    break;
            }

        switch (cmd)
        {
            case ContextMenuCommandTypes.UI_SPOTSELECT:
                {
                    var value = (Int32)values[1];

                    this.SessionState.SelectedHotSpot = this.SessionState.RoomInfo?.HotSpots
                        ?.Where(s => s.SpotInfo.HotspotID == value)
                        ?.FirstOrDefault();
                }

                break;
            case ContextMenuCommandTypes.UI_PROPSELECT:
                {
                    var value = (Int32)values[1];

                    this.SessionState.SelectedProp = this.SessionState.RoomInfo?.LooseProps
                        ?.Where(s => s.AssetSpec.Id == value)
                        ?.Select(s => s.AssetSpec)
                        ?.FirstOrDefault();
                }

                break;
            case ContextMenuCommandTypes.UI_USERSELECT:
                {
                    var value = (UInt32)values[1];

                    this.SessionState.SelectedUser = this.SessionState.RoomUsers.GetValueLocked(value);
                }

                break;
            case ContextMenuCommandTypes.MSG_PROPDEL:
                {
                    var value = (Int32)values[1];

                    ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                    {
                        CmdFnc = a =>
                        {
                            var sessionState = a[0] as IDesktopSessionState;
                            if (sessionState == null) return null;

                            var value = a[1] as Int32?;
                            if (value == null) return null;

                            sessionState.Send(new MSG_PROPDEL
                            {
                                PropNum = value.Value,
                            });

                            return null;
                        },
                        Values = [this.SessionState, value],
                    });
                }

                break;
            case ContextMenuCommandTypes.MSG_USERMOVE:
                {
                    var value = values[1] as ThePalace.Core.Entities.Shared.Types.Point;

                    this.SessionState.UserDesc.UserInfo.RoomPos = value;

                    var user = null as UserDesc;
                    user = this.SessionState.RoomUsers.GetValueLocked(this.SessionState.UserId);
                    if (user != null)
                    {
                        user.UserInfo.RoomPos = value;
                        user.Extended["CurrentMessage"] = null;

                        var queue = user.Extended["MessageQueue"] as DisposableQueue<MsgBubble>;
                        if (queue != null) queue.Clear();

                        this.SessionState.RefreshScreen(
                            ScreenLayers.UserProp,
                            ScreenLayers.UserNametag,
                            ScreenLayers.Messages);

                        ((Job<ActionCmd>)_jobs[ThreadQueues.Network]).Enqueue(new()
                        {
                            CmdFnc = a =>
                            {
                                var sessionState = a[0] as IDesktopSessionState;
                                if (sessionState == null) return null;

                                var value = a[1] as ThePalace.Core.Entities.Shared.Types.Point;
                                if (value == null) return null;

                                sessionState.Send(new MSG_USERMOVE
                                {
                                    Pos = value,
                                });

                                return null;
                            },
                            Values = [this.SessionState, value],
                        });
                    }
                }

                break;
        }
    }
}