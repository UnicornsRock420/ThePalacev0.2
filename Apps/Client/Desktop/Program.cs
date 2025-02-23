using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Entities.Gfx;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Desktop.Entities.UI;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Common.Threading;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Scripting;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts;
using ThePalace.Network.Factories;
using static ThePalace.Common.Threading.Job;

namespace ThePalace.Client.Desktop
{
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

            var task = (Task?)null;
            var uiLoaded = false;

            task = TaskManager.Current.CreateTask((Action<ConcurrentQueue<Cmd>>)(q =>
                {
                    // TODO: GUI

                    if (!uiLoaded)
                    {
                        uiLoaded = true;

                        var sessionState = new DesktopSessionState();
                        var app = new Program();

                        app.Initialize(sessionState);
                    }
                }),
                null,
                RunOptions.UseSleepInterval,
                new TimeSpan(750));
            if (task != null)
            {
                _jobs[ThreadQueues.GUI.ToString()] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Network_Receive
                },
                null,
                RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs[ThreadQueues.Network_Receive.ToString()] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Network_Send
                },
                null,
                RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs[ThreadQueues.Network_Send.ToString()] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Media
                },
                null,
                RunOptions.UseSleepInterval | RunOptions.RunNow);
            if (task != null)
            {
                _jobs[ThreadQueues.Media.ToString()] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Core

                    TaskManager.Current.Run();

                    FormsManager.Current.Dispose();
                },
                null,
                RunOptions.UseSleepInterval | RunOptions.RunNow);
            if (task != null)
            {
                _jobs[ThreadQueues.Core.ToString()] = task.Id;
            }

            Application.Run(FormsManager.Current);

            TaskManager.Current.Shutdown();
        }

        private ContextMenuStrip _contextMenu = new();
        private IDesktopSessionState _sessionState = null;

        private static readonly IptEventTypes[] CONST_eventTypes = Enum.GetValues<IptEventTypes>()
            .Where(v => v.GetType()?.GetField(v.ToString())?.GetCustomAttributes<ScreenRefreshAttribute>()?.Any() ?? false)
            .ToArray();

        private static readonly IptEventTypes[] CONST_uiRefreshEvents = Enum.GetValues<IptEventTypes>()
            .Where(v => v.GetType()?.GetField(v.ToString())?.GetCustomAttributes<UIRefreshAttribute>()?.Any() ?? false)
            .ToArray();

        private static readonly IReadOnlyDictionary<IptEventTypes[], ScreenLayers[]> CONST_EventLayerMappings = new Dictionary<IptEventTypes[], ScreenLayers[]>
        {
            { new IptEventTypes[] { IptEventTypes.MsgHttpServer, IptEventTypes.RoomLoad }, new ScreenLayers[] { ScreenLayers.Base } },
            { new IptEventTypes[] { IptEventTypes.InChat }, new ScreenLayers[] { ScreenLayers.Messages } },
            { new IptEventTypes[] { IptEventTypes.NameChange }, new ScreenLayers[] { ScreenLayers.UserNametag } },
            { new IptEventTypes[] { IptEventTypes.FaceChange, IptEventTypes.MsgUserProp }, new ScreenLayers[] { ScreenLayers.UserProp } },
            { new IptEventTypes[] { IptEventTypes.LoosePropAdded, IptEventTypes.LoosePropDeleted, IptEventTypes.LoosePropMoved }, new ScreenLayers[] { ScreenLayers.LooseProp } },
            { new IptEventTypes[] { IptEventTypes.Lock, IptEventTypes.MsgPictDel, IptEventTypes.MsgPictMove, IptEventTypes.MsgPictMove, IptEventTypes.MsgPictNew, IptEventTypes.StateChange, IptEventTypes.UnLock }, new ScreenLayers[] { ScreenLayers.SpotImage } },
            { new IptEventTypes[] { IptEventTypes.ColorChange, IptEventTypes.MsgUserDesc, IptEventTypes.MsgUserList, IptEventTypes.MsgUserLog, IptEventTypes.UserEnter }, new ScreenLayers[] { ScreenLayers.UserProp, ScreenLayers.UserNametag } },
            { new IptEventTypes[] { IptEventTypes.MsgAssetSend }, new ScreenLayers[] { ScreenLayers.UserProp, ScreenLayers.LooseProp } },
            { new IptEventTypes[] { IptEventTypes.SignOn, IptEventTypes.UserLeave, IptEventTypes.UserMove }, new ScreenLayers[] { ScreenLayers.UserProp, ScreenLayers.UserNametag, ScreenLayers.Messages } },
            { new IptEventTypes[] { IptEventTypes.MsgDraw }, new ScreenLayers[] { ScreenLayers.BottomPaint, ScreenLayers.TopPaint } },
            { new IptEventTypes[] { IptEventTypes.MsgSpotDel, IptEventTypes.MsgSpotMove, IptEventTypes.MsgSpotNew }, new ScreenLayers[] { ScreenLayers.SpotBorder, ScreenLayers.SpotNametag, ScreenLayers.SpotImage } },
        }.AsReadOnly();

        private static readonly ConcurrentDictionary<string, int> _jobs = new();

        public Program()
        {
            this._managedResources.Add(_contextMenu);
        }
        ~Program() => this.Dispose(false);

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            base.Dispose();

            foreach (var type in CONST_eventTypes)
                ScriptEvents.Current.UnregisterEvent(type, this.RefreshScreen);

            //var trayIcon = this._sessionState.UIControls.GetValue(nameof(NotifyIcon)) as NotifyIcon;
            //if (trayIcon != null)
            //{
            //    trayIcon.Visible = false;
            //    try { trayIcon.Dispose(); } catch { }
            //}
        }

        public void Initialize(IDesktopSessionState sessionState)
        {
            if (this.IsDisposed ||
                sessionState == null) return;

            this._sessionState = sessionState;

            foreach (var type in CONST_eventTypes)
            {
                ScriptEvents.Current.RegisterEvent(type, this.RefreshScreen);
            }

            //ApiManager.Current.RegisterApi(nameof(this.ShowConnectionForm), this.ShowConnectionForm);
            //ApiManager.Current.RegisterApi(nameof(this.toolStripDropdownlist_Click), this.toolStripDropdownlist_Click);
            //ApiManager.Current.RegisterApi(nameof(this.toolStripMenuItem_Click), this.toolStripMenuItem_Click);
            //ApiManager.Current.RegisterApi(nameof(this.contextMenuItem_Click), this.contextMenuItem_Click);

            ShowAppForm();

#if WINDOWS10_0_17763_0_OR_GREATER
            //TaskManager.Current.DispatchToast(new ToastCfg
            //{
            //    ExpirationTime = DateTime.Now.AddMinutes(1),
            //    Args = (IReadOnlyDictionary<string, object>)new Dictionary<string, object>
            //    {
            //        { "action", "whisperMsg" },
            //        { "connectionId", 123 },
            //        { "conversationId", 456 },
            //    },
            //    Text = (IReadOnlyList<string>)new List<string>
            //    {
            //        "Beat it like it owes you money!",
            //    },
            //});
#endif

            //TaskManager.Current.CreateTask(() =>
            //{
            //    var sessionState = this._sessionState as IUISessionState;
            //    if (sessionState == null) return;

            //    ShowAppForm();

            //    if (SysTrayIcon.Current.Value)
            //    {
            //        var form = sessionState.GetForm(nameof(Program2));
            //        if (form == null) return;

            //        var trayIcon = sessionState.UIControls.GetValue(nameof(NotifyIcon)) as NotifyIcon;
            //        if (trayIcon == null)
            //        {
            //            trayIcon = new NotifyIcon
            //            {
            //                ContextMenuStrip = new ContextMenuStrip(),
            //                Icon = form.Icon,
            //                Visible = true,
            //            };
            //            sessionState.RegisterControl(nameof(NotifyIcon), trayIcon);

            //            trayIcon.ContextMenuStrip.Items.Add("Exit", null, new EventHandler((sender, e) => TaskManager.Current.Dispose()));
            //        }
            //    }

            //    return;
            //}, null);

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

            //TaskManager.Current.CreateTask(() =>
            //{
            //    if (screenLayers.Contains(ScreenLayers.Base))
            //        sessionState.RefreshScreen(ScreenLayers.Base);

            //    if (uiRefresh)
            //    {
            //        sessionState.RefreshUI();
            //        sessionState.RefreshRibbon();
            //    }

            //    if (!screenLayers.Contains(ScreenLayers.Base))
            //        sessionState.RefreshScreen(screenLayers);
            //    else
            //        sessionState.RefreshScreen(new[] {
            //            ScreenLayers.LooseProp,
            //            ScreenLayers.SpotImage,
            //            ScreenLayers.BottomPaint,
            //            ScreenLayers.SpotNametag,
            //            ScreenLayers.UserProp,
            //            ScreenLayers.UserNametag,
            //            ScreenLayers.ScriptedImage,
            //            ScreenLayers.ScriptedText,
            //            ScreenLayers.SpotBorder,
            //            ScreenLayers.TopPaint,
            //            ScreenLayers.Messages, });

            //    switch (scriptEvent.EventType)
            //    {
            //        case IptEventTypes.RoomLoad:
            //            sessionState.History.RegisterHistory(
            //                $"{sessionState.ServerName} - {sessionState.RoomInfo.roomName}",
            //                $"palace://{sessionState.ConnectionState.Host}:{sessionState.ConnectionState.Port}/{sessionState.RoomInfo.roomID}");

            //            sessionState.RefreshRibbon();

            //            ScriptEvents.Current.Invoke(IptEventTypes.RoomReady, sessionState, null, sessionState.ScriptState);
            //            ScriptEvents.Current.Invoke(IptEventTypes.Enter, sessionState, null, sessionState.ScriptState);

            //            break;
            //    }

            //    return;
            //}, null);
        }

        private void ShowAppForm()
        {
            if (this.IsDisposed) return;

            var form = FormsManager.Current.CreateForm<FormDialog>(new FormCfg
            {
                //Load = new EventHandler((sender, e) => TaskManager.Current.Run(ThreadQueues.GUI)),
                WindowState = FormWindowState.Minimized,
                AutoScaleMode = AutoScaleMode.Font,
                AutoScaleDimensions = new SizeF(7F, 15F),
                Margin = new Padding(0, 0, 0, 0),
                Visible = false,
            });
            if (form == null) return;

            this._sessionState.RegisterForm(nameof(Program), form);

            form.SessionState = this._sessionState;
            form.FormClosed += new FormClosedEventHandler((sender, e) =>
                this._sessionState.UnregisterForm(nameof(Program), sender as FormBase));

            form.MouseMove += new MouseEventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseMove, this._sessionState, null, this._sessionState.State);
            });
            form.MouseUp += new MouseEventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseUp, this._sessionState, null, this._sessionState.State);
            });
            form.MouseDown += new MouseEventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseDown, this._sessionState, null, this._sessionState.State);
            });
            form.DragEnter += new DragEventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this._sessionState, null, this._sessionState.State);
            });
            form.DragLeave += new EventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this._sessionState, null, this._sessionState.State);
            });
            form.DragOver += new DragEventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

                ScriptEvents.Current.Invoke(IptEventTypes.MouseDrag, this._sessionState, null, this._sessionState.State);
            });
            form.Resize += new EventHandler((sender, e) =>
            {
                this._sessionState.LastActivity = DateTime.UtcNow;

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

                var toolStrip = this._sessionState.GetControl("toolStrip") as ToolStrip;
                if (toolStrip != null)
                {
                    toolStrip.Size = new Size(form.Width, form.Height);
                }
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

            var toolStrip = this._sessionState.GetControl("toolStrip") as ToolStrip;
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
                    this._sessionState.RegisterControl(nameof(toolStrip), toolStrip);

                    toolStrip.Stretch = true;
                    toolStrip.GripMargin = new Padding(0);
                    toolStrip.ImageScalingSize = new Size(38, 38);
                    toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
                    toolStrip.RenderMode = ToolStripRenderMode.Professional;
                    toolStrip.Renderer = new CustomToolStripRenderer();
                    toolStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
                }
            }

            var imgScreen = this._sessionState.GetControl("imgScreen") as PictureBox;
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
                        if (!AsyncTcpSocket.IsConnected(this._sessionState.ConnectionState))
                            ShowConnectionForm();
                        else
                        {
                            var point = new ThePalace.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                            switch (e.Button)
                            {
                                case MouseButtons.Left:
                                    this._sessionState.UserDesc.UserInfo.RoomPos = point;

                                    var user = null as UserDesc;
                                    user = this._sessionState.RoomUsers.GetValueLocked(this._sessionState.UserId);
                                    if (user != null)
                                    {
                                        user.UserInfo.RoomPos = point;
                                        user.Extended["CurrentMessage"] = null;

                                        var queue = user.Extended["MessageQueue"] as DisposableQueue<MsgBubble>;
                                        if (queue != null) queue.Clear();

                                        this._sessionState.RefreshScreen(
                                            ScreenLayers.UserProp,
                                            ScreenLayers.UserNametag,
                                            ScreenLayers.Messages);

                                        //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                                        //{
                                        //    EventType = EventTypes.MSG_USERMOVE,
                                        //    protocolSend = new MSG_USERMOVE
                                        //    {
                                        //        Pos = point,
                                        //    },
                                        //});
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

                                    if ((this._sessionState.RoomUsers?.Count ?? 0) > 0)
                                        foreach (var roomUser in this._sessionState.RoomUsers.Values)
                                            if (roomUser.UserInfo.UserId == 0 ||
                                                roomUser.UserInfo.RoomPos == null) continue;
                                            else if (roomUser.UserInfo.UserId != this._sessionState.UserId &&
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

                                                if (this._sessionState.UserDesc.IsModerator ||
                                                    this._sessionState.UserDesc.IsAdministrator)
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

                                    if ((this._sessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
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
                                        foreach (var looseProp in this._sessionState.RoomInfo.LooseProps)
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

                                    if ((this._sessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                                        foreach (var hotSpot in this._sessionState.RoomInfo.HotSpots)
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

                                                if (this._sessionState.UserDesc.IsModerator ||
                                                    this._sessionState.UserDesc.IsAdministrator)
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

                        if (AsyncTcpSocket.IsConnected(this._sessionState.ConnectionState))
                        {
                            var point = new ThePalace.Core.Entities.Shared.Types.Point((short)e.Y, (short)e.X);

                            if ((this._sessionState.RoomUsers?.Count ?? 0) > 0)
                                foreach (var roomUser in this._sessionState.RoomUsers.Values)
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

                            if ((this._sessionState.RoomInfo?.LooseProps?.Count ?? 0) > 0)
                                foreach (var looseProp in this._sessionState.RoomInfo.LooseProps)
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

                            if ((this._sessionState.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                                foreach (var hotSpot in this._sessionState.RoomInfo.HotSpots)
                                    if (point.IsPointInPolygon(hotSpot.Vortexes.ToArray()))
                                    {
                                        imgScreen.Cursor = Cursors.Hand;
                                        break;
                                    }
                        }
                    });

                    this._sessionState.RegisterControl(nameof(imgScreen), imgScreen);
                }
            }

            var labelInfo = this._sessionState.GetControl("labelInfo") as Label;
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
                    this._sessionState.RegisterControl(nameof(labelInfo), labelInfo);
            }

            var txtInput = this._sessionState.GetControl("txtInput") as TextBox;
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
                        this._sessionState.LastActivity = DateTime.UtcNow;

                        if (e.KeyCode == Keys.Tab)
                        {
                            e.Handled = true;

                            txtInput.Text = string.Empty;
                        }

                        if (!AsyncTcpSocket.IsConnected(this._sessionState?.ConnectionState))
                        {
                            this.ShowConnectionForm();

                            return;
                        }

                        ScriptEvents.Current.Invoke(IptEventTypes.KeyUp, this._sessionState, null, this._sessionState.State);

                        if (e.KeyCode == Keys.Enter)
                        {
                            e.Handled = true;

                            var text = txtInput.Text?.Trim();
                            txtInput.Text = string.Empty;

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                if (text[0] == '/')
                                {
                                    //TaskManager.Current.CreateTask(ThreadQueues.ScriptEngine, args =>
                                    //{
                                    //    var sessionState = args[0] as SessionState;
                                    //    if (sessionState == null) return null;

                                    //    var text = args[1] as string;
                                    //    if (text == null) return null;

                                    //    try
                                    //    {
                                    //        var atomlist = IptscraeEngine.Parser(
                                    //            sessionState.ScriptState as IptTracking,
                                    //            text,
                                    //            false);
                                    //        IptscraeEngine.Executor(atomlist, sessionState.ScriptState as IptTracking);
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        Debug.WriteLine(ex.Message);
                                    //    }

                                    //    return null;
                                    //}, this._sessionState, string.Concat(text.Skip(1)));
                                }
                                else
                                {
                                    var xTalk = new MSG_XTALK
                                    {
                                        Text = text,
                                    };
                                    var outboundPacket = new MSG_Header
                                    {
                                        EventType = EventTypes.MSG_XTALK,
                                    };

                                    //ScriptEvents.Current.Invoke(IptEventTypes.Chat, this._sessionState, outboundPacket, this._sessionState.ScriptState);
                                    //ScriptEvents.Current.Invoke(IptEventTypes.OutChat, this._sessionState, outboundPacket, this._sessionState.ScriptState);

                                    var iptTracking = this._sessionState.State as IptTracking;
                                    if (iptTracking != null)
                                    {
                                        if (iptTracking.Variables?.ContainsKey("CHATSTR") == true)
                                            xTalk.Text = iptTracking.Variables["CHATSTR"].Value.Value.ToString();

                                        //if (!string.IsNullOrWhiteSpace(xTalk.Text))
                                        //    TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, outboundPacket);
                                    }
                                }
                            }
                        }
                    });
                    txtInput.KeyDown += new KeyEventHandler((sender, e) =>
                    {
                        this._sessionState.LastActivity = DateTime.UtcNow;

                        if (e.KeyCode == Keys.Tab)
                        {
                            e.Handled = true;

                            txtInput.Text = string.Empty;
                        }

                        if (!AsyncTcpSocket.IsConnected(this._sessionState?.ConnectionState)) return;

                        ScriptEvents.Current.Invoke(IptEventTypes.KeyDown, this._sessionState, null, this._sessionState.State);
                    });

                    this._sessionState.RegisterControl(nameof(txtInput), txtInput);
                }
            }

            this._sessionState.RefreshScreen(ScreenLayers.Base);

            ShowConnectionForm();
        }
        private void ShowConnectionForm(object sender = null, EventArgs e = null)
        {
            if (this.IsDisposed) return;

            var connectionForm = this._sessionState.GetForm<Forms.Connection>(nameof(Forms.Connection));
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

                connectionForm.SessionState = this._sessionState;
                connectionForm.FormClosed += new FormClosedEventHandler((sender, e) =>
                {
                    this._sessionState.UnregisterForm(nameof(Forms.Connection), sender as FormBase);
                });

                if (connectionForm != null)
                {
                    this._sessionState.RegisterForm(nameof(Forms.Connection), connectionForm);

                    var buttonDisconnect = connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonDisconnect")
                        .FirstOrDefault() as Button;
                    if (buttonDisconnect != null)
                    {
                        buttonDisconnect.Click += new EventHandler((sender, e) =>
                        {
                            //if ((this._sessionState.ConnectionState?.IsConnected ?? false) == true)
                            //    ConnectionManager.Current.Disconnect(this._sessionState);

                            var connectionForm = this._sessionState.GetForm(nameof(Forms.Connection));
                            connectionForm?.Close();
                        });
                        buttonDisconnect.Visible = AsyncTcpSocket.IsConnected(this._sessionState?.ConnectionState);
                    }

                    var buttonConnect = connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "buttonConnect")
                        .FirstOrDefault() as Button;
                    if (buttonConnect != null)
                    {
                        buttonConnect.Click += new EventHandler((sender, e) =>
                        {
                            var connectionForm = this._sessionState.GetForm(nameof(Forms.Connection));
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
                                    this._sessionState.RegInfo.UserName = this._sessionState.RegInfo.UserName = comboBoxUsernames.Text;
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

                                    this._sessionState.RegInfo.DesiredRoom = roomID;
                                }

                                var comboBoxAddresses = connectionForm.Controls
                                    .Cast<Control>()
                                    .Where(c => c.Name == "comboBoxAddresses")
                                    .FirstOrDefault() as ComboBox;
                                //if (comboBoxAddresses != null &&
                                //    !string.IsNullOrWhiteSpace(comboBoxAddresses.Text))
                                //    TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.CONNECT, $"palace://{comboBoxAddresses.Text}");

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
                            var connectionForm = this._sessionState.GetForm(nameof(Forms.Connection));
                            connectionForm?.Close();
                        });
                    }

                    var comboBoxUsernames = connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "comboBoxUsernames")
                        .FirstOrDefault() as ComboBox;
                    if (comboBoxUsernames != null)
                    {
                        //var usernamesList = SettingsManager.Current.Settings[@"\GUI\Connection\Usernames"] as ISettingList;
                        //if (usernamesList != null)
                        //{
                        //    comboBoxUsernames.Items.AddRange(usernamesList.Text
                        //        .Select(v => new ComboboxItem
                        //        {
                        //            Text = v,
                        //            Value = v,
                        //        })
                        //        .ToArray());

                        //    comboBoxUsernames.Text = usernamesList.Text?.FirstOrDefault();
                        //}
                    }

                    var comboBoxAddresses = connectionForm.Controls
                        .Cast<Control>()
                        .Where(c => c.Name == "comboBoxAddresses")
                        .FirstOrDefault() as ComboBox;
                    if (comboBoxAddresses != null)
                    {
                        //var addressesList = SettingsManager.Current.Settings[@"\GUI\Connection\Addresses"] as ISettingList;
                        //if (addressesList != null)
                        //{
                        //    comboBoxAddresses.Items.AddRange(addressesList.Text
                        //        .Select(v => new ComboboxItem
                        //        {
                        //            Text = v,
                        //            Value = v,
                        //        })
                        //        .ToArray());

                        //    comboBoxAddresses.Text = addressesList.Text?.FirstOrDefault();
                        //}
                    }
                }
            }

            if (connectionForm != null)
            {
                connectionForm.TopMost = true;

                connectionForm.Show();
                connectionForm.Focus();

                this._sessionState.RefreshScreen(ScreenLayers.Base);
                this._sessionState.RefreshUI();
                this._sessionState.RefreshScreen();
                this._sessionState.RefreshRibbon();
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
                        if (AsyncTcpSocket.IsConnected(this._sessionState.ConnectionState) &&
                            (this._sessionState.History.History.Count > 0))
                        {
                            var url = null as string;

                            switch (name)
                            {
                                case nameof(GoBack):
                                    if ((!this._sessionState.History.Position.HasValue ||
                                        this._sessionState.History.History.Keys.Min() != this._sessionState.History.Position.Value))
                                        url = this._sessionState.History.Back();
                                    break;
                                case nameof(GoForward):
                                    if (this._sessionState.History.Position.HasValue &&
                                        this._sessionState.History.History.Keys.Max() != this._sessionState.History.Position.Value)
                                        url = this._sessionState.History.Forward();
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

                                //if (AsyncTcpSocket.IsConnected(this._sessionState.ConnectionState) &&
                                //    this._sessionState.ConnectionState.Hostname == host &&
                                //    this._sessionState.ConnectionState.Port == port &&
                                //    roomID != 0)
                                //    TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                                //    {
                                //        EventType = EventTypes.MSG_ROOMGOTO,
                                //        protocolSend = new MSG_ROOMGOTO
                                //        {
                                //            dest = roomID,
                                //        },
                                //    });
                                //else TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.CONNECT, url);
                            }
                        }

                        break;
                    case nameof(Connection):
                        ApiManager.Current.ApiBindings.GetValue("ShowConnectionForm")?.Binding(this._sessionState, null);
                        break;
                    case nameof(Chatlog):
                        ApiManager.Current.ApiBindings.GetValue("ShowLogForm")?.Binding(this._sessionState, null);
                        break;
                    case nameof(UsersList):
                        ApiManager.Current.ApiBindings.GetValue("ShowUserListForm")?.Binding(this._sessionState, null);
                        break;
                    case nameof(RoomsList):
                        ApiManager.Current.ApiBindings.GetValue("ShowRoomListForm")?.Binding(this._sessionState, null);
                        break;
                    case nameof(Bookmarks):
                        break;
                    case nameof(LiveDirectory):
                        break;
                    case nameof(DoorOutlines):
                        break;
                    case nameof(UserNametags):
                        break;
                    case nameof(Tabs):
                        break;
                    case nameof(Terminal):
                        break;
                    case nameof(SuperUser):
                        break;
                    case nameof(Draw):
                        break;
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

            if (this._sessionState.UserDesc.IsModerator ||
                this._sessionState.UserDesc.IsAdministrator)
                switch (cmd)
                {
                    case ContextMenuCommandTypes.CMD_PIN:
                    case ContextMenuCommandTypes.CMD_UNPIN:
                    case ContextMenuCommandTypes.CMD_GAG:
                    case ContextMenuCommandTypes.CMD_UNGAG:
                    case ContextMenuCommandTypes.CMD_PROPGAG:
                    case ContextMenuCommandTypes.CMD_UNPROPGAG:
                        {
                            var value = (UInt32)values[1];

                            //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                            //{
                            //    EventType = EventTypes.MSG_WHISPER,
                            //    protocolSend = new MSG_WHISPER
                            //    {
                            //        TargetID = value,
                            //        Text = $"`{cmd.GetDescription()}",
                            //    },
                            //});
                        }

                        break;
                    case ContextMenuCommandTypes.MSG_KILLUSER:
                        {
                            var value = (UInt32)values[1];

                            //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                            //{
                            //    EventType = EventTypes.MSG_KILLUSER,
                            //    protocolSend = new MSG_KILLUSER
                            //    {
                            //        TargetID = value,
                            //    },
                            //});
                        }

                        break;
                    case ContextMenuCommandTypes.MSG_SPOTDEL:
                        {
                            var value = (short)values[1];

                            //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                            //{
                            //    EventType = EventTypes.MSG_SPOTDEL,
                            //    protocolSend = new MSG_SPOTDEL
                            //    {
                            //        SpotID = value,
                            //    },
                            //});
                        }

                        break;
                }

            switch (cmd)
            {
                case ContextMenuCommandTypes.UI_SPOTSELECT:
                    {
                        var value = (Int32)values[1];

                        this._sessionState.SelectedHotSpot = this._sessionState.RoomInfo?.HotSpots
                            ?.Where(s => s.SpotInfo.HotspotID == value)
                            ?.FirstOrDefault();
                    }

                    break;
                case ContextMenuCommandTypes.UI_PROPSELECT:
                    {
                        var value = (Int32)values[1];

                        this._sessionState.SelectedProp = this._sessionState.RoomInfo?.LooseProps
                            ?.Where(s => s.AssetSpec.Id == value)
                            ?.Select(s => s.AssetSpec)
                            ?.FirstOrDefault();
                    }

                    break;
                case ContextMenuCommandTypes.UI_USERSELECT:
                    {
                        var value = (UInt32)values[1];

                        this._sessionState.SelectedUser = this._sessionState.RoomUsers.GetValueLocked(value);
                    }

                    break;
                case ContextMenuCommandTypes.MSG_PROPDEL:
                    {
                        var value = (Int32)values[1];

                        //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                        //{
                        //    EventType = EventTypes.MSG_PROPDEL,
                        //    protocolSend = new MSG_PROPDEL
                        //    {
                        //        propNum = value,
                        //    },
                        //});
                    }

                    break;
                case ContextMenuCommandTypes.MSG_USERMOVE:
                    {
                        var value = values[1] as ThePalace.Core.Entities.Shared.Types.Point;

                        this._sessionState.UserDesc.UserInfo.RoomPos = value;

                        var user = null as UserDesc;
                        user = this._sessionState.RoomUsers.GetValueLocked(this._sessionState.UserId);
                        if (user != null)
                        {
                            user.UserInfo.RoomPos = value;
                            user.Extended["CurrentMessage"] = null;

                            var queue = user.Extended["MessageQueue"] as DisposableQueue<MsgBubble>;
                            if (queue != null) queue.Clear();

                            this._sessionState.RefreshScreen(
                                ScreenLayers.UserProp,
                                ScreenLayers.UserNametag,
                                ScreenLayers.Messages);

                            //TaskManager.Current.CreateTask(ThreadQueues.Network, null, this._sessionState, NetworkCommandTypes.SEND, new MSG_Header
                            //{
                            //    EventType = EventTypes.MSG_USERMOVE,
                            //    protocolSend = new MSG_USERMOVE
                            //    {
                            //        Pos = value,
                            //    },
                            //});
                        }
                    }

                    break;
            }
        }
    }
}