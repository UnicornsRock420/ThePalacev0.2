﻿using System.Collections;
using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Reflection;
using Lib.Common.Attributes.UI;
using Lib.Common.Client.Constants;
using Lib.Common.Desktop.Constants;
using Lib.Common.Desktop.Entities.Ribbon;
using Lib.Common.Desktop.Interfaces;
using Lib.Common.Desktop.Singletons;
using Lib.Common.Factories.Core;
using Lib.Core.Constants;
using Lib.Core.Entities.Scripting;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Entities.Shared.ServerInfo;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Enums;
using Lib.Core.Helpers.Core;
using Lib.Core.Interfaces.Core;
using Lib.Core.Singletons;
using Lib.Logging.Entities;
using Lib.Network.Factories;
using Lib.Network.Interfaces;
using Mod.Scripting.Iptscrae.Entities;
using Mod.Scripting.Iptscrae.Enums;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.Shared.Assets;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Helpers;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Client.Desktop.Singletons;
using Point = System.Drawing.Point;
using RegexConstants = Lib.Common.Constants.RegexConstants;
using RoomID = short;
using UserID = int;

namespace ThePalace.Client.Desktop.Entities.UI;

public class ClientDesktopSessionState : Disposable, IClientDesktopSessionState
{
    private const int CONST_INT_halfPropWidth = (int)AssetConstants.Values.DefaultPropWidth / 2;
    private const int CONST_INT_halfPropHeight = (int)AssetConstants.Values.DefaultPropHeight / 2;

    #region ctors

    public ClientDesktopSessionState()
    {
        _managedResources.Add(_uiLayers);

        foreach (var layer in _layerTypes)
            _uiLayers.TryAdd(layer, new LayerScreen(this, layer));

        var iptTracking = new IptTracking();
        ScriptTag = iptTracking;
        iptTracking.Variables.TryAdd("SESSIONSTATE", new IptMetaVariable
        {
            Flags = IptMetaVariableFlags.All,
            Variable = new IptVariable(IptVariableTypes.Hidden, this),
        });

        InitializeUIUserRec(UserDesc);

        var seed = (uint)Cipher.WizKeytoSeed(ClientConstants.RegCodeSeed);
        RegInfo.Crc = Cipher.ComputeLicenseCrc(seed);
        RegInfo.Counter = (uint)Cipher.GetSeedFromReg(seed, RegInfo.Crc);
        RegInfo.PuidCRC = RegInfo.Crc;
        RegInfo.PuidCtr = RegInfo.Counter;

        RegInfo.Reserved = ClientConstants.ClientAgent;
        RegInfo.UlUploadCaps = (UploadCapabilities)0x41;
        RegInfo.UlDownloadCaps = (DownloadCapabilities)0x0151;
        RegInfo.Ul2DEngineCaps = (Upload2DEngineCaps)0x01;
        RegInfo.Ul2DGraphicsCaps = (Upload2DGraphicsCaps)0x01;
    }

    ~ClientDesktopSessionState()
    {
        Dispose();
    }

    public override void Dispose()
    {
        Ribbon?.Values?.ToList()?.ForEach(b => b?.Dispose());
        Ribbon?.Clear();
        Ribbon = null;

        LastActivity = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Properties

    #region Object Info

    public IApp App { get; set; }
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime? LastActivity { get; set; }

    #endregion

    #region UI Info

    private static readonly IReadOnlyList<LayerScreenTypes> _layerTypes = Enum.GetValues<LayerScreenTypes>().AsReadOnly();
    private readonly DisposableDictionary<LayerScreenTypes, ILayerScreen> _uiLayers = new();
    public IReadOnlyDictionary<LayerScreenTypes, ILayerScreen> UILayers => _uiLayers.AsReadOnly();

    public bool Visible { get; set; } = true;
    public bool Enabled { get; set; } = true;

    public double Scale { get; set; } = 1.0D;
    public int ScreenWidth { get; set; } = DesktopConstants.AspectRatio.WidescreenDef.Default.Width;
    public int ScreenHeight { get; set; } = DesktopConstants.AspectRatio.WidescreenDef.Default.Height;

    public AssetSpec SelectedProp { get; set; } = null;
    public UserDesc SelectedUser { get; set; } = null;
    public HotspotDesc SelectedHotSpot { get; set; } = null;

    public HistoryManager History { get; } = new();
    public TabPage TabPage { get; set; } = null;
    public ConcurrentDictionary<Guid, ItemBase> Ribbon { get; internal set; } = new();

    #endregion

    #region User Info

    public IConnectionState<Socket>? ConnectionState { get; set; } =
        ConnectionManager.CreateConnectionState(
            AddressFamily.InterNetwork,
            instance: ConnectionManager.Current);

    public UserID UserId { get; set; } = 0;
    public UserDesc? UserDesc { get; set; } = new();
    public RegistrationRec? RegInfo { get; set; } = new();

    public object? SessionTag { get; set; } = null;
    public object? ScriptTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; set; } = new();

    public static void InitializeUIUserRec(UserDesc user)
    {
        user.Extended.TryAdd(@"MessageQueue", new ConcurrentQueue<MsgBubble>());
        user.Extended.TryAdd(@"CurrentMessage", null);
    }

    #endregion

    #region Room Info

    public RoomID RoomId { get; set; }
    public RoomDesc RoomInfo { get; set; } = null;
    public ConcurrentDictionary<UserID, UserDesc> RoomUsers { get; set; } = new();

    #endregion

    #region Server Info

    public string? MediaUrl { get; set; } = string.Empty;
    public string? ServerName { get; set; } = string.Empty;
    public int ServerPopulation { get; set; } = 0;

    public ConcurrentDictionary<RoomID, ListRec> Rooms { get; set; } = [];
    public ConcurrentDictionary<UserID, ListRec> Users { get; set; } = [];

    #endregion

    #region ScreenLayer Consts

    private static readonly LayerScreenTypes[] CONST_allLayers = Enum.GetValues<LayerScreenTypes>()
        .Where(v => !new[] { LayerScreenTypes.Base, LayerScreenTypes.DimRoom }.Contains(v))
        .ToArray();

    private static readonly IReadOnlyList<ScriptEventTypes> CONST_EventTypes_UiRefresh = Enum.GetValues<ScriptEventTypes>()
        .Where(v => v.GetType()?.GetField(v.ToString())?.GetCustomAttributes<UIRefreshAttribute>()?.Any() ?? false)
        .ToList()
        .AsReadOnly();

    private static readonly IReadOnlyDictionary<ScriptEventTypes[], LayerScreenTypes[]> CONST_Event_LayerScreen_Types_Mappings =
        new Dictionary<ScriptEventTypes[], LayerScreenTypes[]>
        {
            { [ScriptEventTypes.MsgHttpServer, ScriptEventTypes.RoomLoad], [LayerScreenTypes.Base] },
            { [ScriptEventTypes.InChat], [LayerScreenTypes.Messages] },
            { [ScriptEventTypes.NameChange], [LayerScreenTypes.UserNametag] },
            { [ScriptEventTypes.FaceChange, ScriptEventTypes.MsgUserProp], [LayerScreenTypes.UserProp] },
            {
                [ScriptEventTypes.LoosePropAdded, ScriptEventTypes.LoosePropDeleted, ScriptEventTypes.LoosePropMoved],
                [LayerScreenTypes.LooseProp]
            },
            {
                [
                    ScriptEventTypes.Lock, ScriptEventTypes.MsgPictDel, ScriptEventTypes.MsgPictMove, ScriptEventTypes.MsgPictMove,
                    ScriptEventTypes.MsgPictNew, ScriptEventTypes.StateChange, ScriptEventTypes.UnLock
                ],
                [LayerScreenTypes.SpotImage]
            },
            {
                [
                    ScriptEventTypes.ColorChange, ScriptEventTypes.MsgUserDesc, ScriptEventTypes.MsgUserList,
                    ScriptEventTypes.MsgUserLog, ScriptEventTypes.UserEnter
                ],
                [LayerScreenTypes.UserProp, LayerScreenTypes.UserNametag]
            },
            { [ScriptEventTypes.MsgAssetSend], [LayerScreenTypes.UserProp, LayerScreenTypes.LooseProp] },
            {
                [ScriptEventTypes.SignOn, ScriptEventTypes.UserLeave, ScriptEventTypes.UserMove],
                [LayerScreenTypes.UserProp, LayerScreenTypes.UserNametag, LayerScreenTypes.Messages]
            },
            { [ScriptEventTypes.MsgDraw], [LayerScreenTypes.BottomPaint, LayerScreenTypes.TopPaint] },
            {
                [ScriptEventTypes.MsgSpotDel, ScriptEventTypes.MsgSpotMove, ScriptEventTypes.MsgSpotNew],
                [LayerScreenTypes.SpotBorder, LayerScreenTypes.SpotNametag, LayerScreenTypes.SpotImage]
            }
        }.AsReadOnly();

    #endregion

    #endregion

    #region ScreenLayer Methods

    private void RefreshLayers(params LayerScreenTypes[] layers)
    {
        if (!Visible ||
            ScreenWidth < 1 ||
            ScreenHeight < 1) return;

        try
        {
            foreach (var layer in layers)
                lock (_uiLayers[layer])
                {
                    if (layer == LayerScreenTypes.Base) continue;

                    switch (layer)
                    {
                        case LayerScreenTypes.DimRoom:
                            if (_uiLayers[layer].Opacity >= 1F)
                            {
                                _uiLayers[layer].Unload();

                                break;
                            }

                            if (_uiLayers[layer].Image != null) break;

                            goto default;
                        default:
                            using (var g = _uiLayers[layer].Clear(ScreenWidth, ScreenHeight))
                            {
                                if (!_uiLayers[layer].Visible) continue;

                                g.InterpolationMode =
                                    SettingsManager.Current.Get<InterpolationMode>("GUI:General:" + nameof(InterpolationMode));
                                g.PixelOffsetMode =
                                    SettingsManager.Current.Get<PixelOffsetMode>("GUI:General:" + nameof(PixelOffsetMode));
                                g.SmoothingMode =
                                    SettingsManager.Current.Get<SmoothingMode>("GUI:General:" + nameof(SmoothingMode));

                                switch (layer)
                                {
                                    case LayerScreenTypes.LooseProp: ScreenLayer_LooseProp(g); break;
                                    case LayerScreenTypes.SpotImage: ScreenLayer_SpotImage(g); break;
                                    case LayerScreenTypes.BottomPaint: ScreenLayer_BottomPaint(g); break;
                                    case LayerScreenTypes.SpotNametag: ScreenLayer_SpotNametag(g); break;
                                    case LayerScreenTypes.UserProp: ScreenLayer_UserProp(g); break;
                                    case LayerScreenTypes.UserNametag: ScreenLayer_UserNametag(g); break;
                                    case LayerScreenTypes.ScriptedImage: ScreenLayer_ScriptedImage(g); break;
                                    case LayerScreenTypes.ScriptedText: ScreenLayer_ScriptedText(g); break;
                                    case LayerScreenTypes.SpotBorder: ScreenLayer_SpotBorder(g); break;
                                    case LayerScreenTypes.TopPaint: ScreenLayer_TopPaint(g); break;
                                    case LayerScreenTypes.DimRoom: ScreenLayer_DimRoom(g); break;
                                    case LayerScreenTypes.Messages: ScreenLayer_Messages(g); break;
                                }

                                g.Save();
                            }

                            break;
                    }
                }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    private void ScreenLayer_LooseProp(Graphics g)
    {
        var looseProps = RoomInfo?.LooseProps
            ?.ToList() ?? [];
        if (looseProps.Count > 0)
            foreach (var looseProp in looseProps)
            {
                var prop = AssetsManager.Current.GetAsset(this, looseProp.AssetSpec);
                if (prop?.Image == null) continue;

                //var x = looseProp.loc.h - (prop.Image.Width / 2);
                //var y = looseProp.loc.v - (prop.Image.Height / 2);
                var x = looseProp.Loc.HAxis;
                var y = looseProp.Loc.VAxis;

                //if (x < 0 ||
                //    y < 0) continue;

                if (prop.Image != null)
                    g.DrawImage(
                        prop.Image,
                        new Rectangle(
                            x,
                            y,
                            prop.Image.Width,
                            prop.Image.Height),
                        0, 0,
                        prop.Image.Width,
                        prop.Image.Height,
                        GraphicsUnit.Pixel);
            }
    }

    private void ScreenLayer_SpotImage(Graphics g)
    {
        if ((RoomInfo?.HotSpots?.Count ?? 0) > 0)
            foreach (var spot in RoomInfo.HotSpots)
            {
                var nbrStates = spot.States?.Count ?? 0;
                if (nbrStates < 1 ||
                    spot.State < 0 ||
                    spot.State >= nbrStates) continue;

                var state = spot.States[spot.State];
                if (state == null ||
                    state.PictID < 1) continue;

                var pictName = RoomInfo?.Pictures
                    ?.Where(p => p.PicID == state.PictID)
                    ?.Select(p => p.Name)
                    ?.FirstOrDefault();
                if (pictName == null) continue;

                var _pictName = RegexConstants.REGEX_FILESYSTEMCHARS.Replace(pictName, @"_");
                var filePath = Path.Combine(Environment.CurrentDirectory, "Media", _pictName);

                if (!File.Exists(filePath))
                {
                    var _serverName = RegexConstants.REGEX_FILESYSTEMCHARS.Replace(ServerName, @" ").Trim();
                    filePath = Path.Combine(Environment.CurrentDirectory, "Media", _serverName, _pictName);
                }

                if (!File.Exists(filePath)) continue;

                using (var pict = new Bitmap(filePath))
                {
                    g.DrawImage(
                        pict,
                        new Rectangle(
                            spot.Loc.HAxis - state.PicLoc.HAxis - pict.Width / 2,
                            spot.Loc.VAxis - state.PicLoc.VAxis - pict.Height / 2,
                            pict.Width,
                            pict.Height),
                        0, 0,
                        pict.Width,
                        pict.Height,
                        GraphicsUnit.Pixel);
                }
            }
    }

    private void ScreenLayer_BottomPaint(Graphics g)
    {
        ScreenLayer_Paint(g, false);
    }

    private void ScreenLayer_SpotNametag(Graphics g)
    {
        var spots = RoomInfo?.HotSpots
            ?.Where(h => (h.Flags & HotspotFlags.HS_ShowName) == HotspotFlags.HS_ShowName)
            ?.ToList() ?? [];
        if (spots.Count > 0)
        {
            var padding = 2;

            using (var font = new Font("Arial", 11))
            {
                foreach (var spot in spots)
                {
                    if (!string.IsNullOrWhiteSpace(spot.Name)) continue;

                    var textSize = TextRenderer.MeasureText(spot.Name, font);
                    var halfNameTagWidth = textSize.Width / 2;
                    var halfNameTagHeight = textSize.Height / 2;

                    var x = spot.Loc.HAxis - halfNameTagWidth - padding * 2;
                    var y = spot.Loc.VAxis + halfNameTagHeight * 3 - padding * 2;

                    //if (x < 0 ||
                    //    y < 0) continue;

                    g.FillRectangle(
                        Brushes.White,
                        new Rectangle(
                            x,
                            y,
                            textSize.Width + padding,
                            textSize.Height + padding));

                    g.DrawString(
                        spot.Name,
                        font,
                        Brushes.Black,
                        x, y);
                }
            }
        }
    }

    private void ScreenLayer_UserProp(Graphics g)
    {
        var users = (List<UserDesc>?)null;
        lock (RoomUsers)
        {
            users = RoomUsers.Values
                .Where(u =>
                    !(u.UserId < 1 ||
                      u.RoomPos == null))
                .ToList();
        }

        if (users.Count <= 0) return;

        foreach (var u in users)
        {
            var x = u.RoomPos.HAxis - CONST_INT_halfPropWidth;
            var y = u.RoomPos.VAxis - CONST_INT_halfPropHeight;

            if (x < -CONST_INT_halfPropWidth) x = -CONST_INT_halfPropWidth;
            else if (x > ScreenWidth + CONST_INT_halfPropWidth) x = ScreenWidth + CONST_INT_halfPropWidth;

            if (y < -CONST_INT_halfPropHeight) y = -CONST_INT_halfPropHeight;
            else if (y > ScreenHeight + CONST_INT_halfPropHeight) y = ScreenHeight + CONST_INT_halfPropHeight;

            //if (x < 0 ||
            //    y < 0) continue;

            var animatedProps = new List<AssetDesc>();
            var stillProps = new List<AssetDesc>();
            var hasPalindromeProp = false;
            var hasAnimatedProp = false;
            var hasHeadProp = false;

            var assetSpecs = u.PropSpec?.ToList() ?? [];
            if (assetSpecs.Count > 0)
                foreach (var assetSpec in assetSpecs)
                {
                    var asset = AssetsManager.Current.GetAsset(this, assetSpec);
                    if (asset == null) continue;

                    hasPalindromeProp |= asset.IsPalindrome;
                    hasAnimatedProp |= asset.IsAnimate;
                    hasHeadProp |= asset.IsHead;

                    if (asset.Image == null) continue;

                    if (asset.IsAnimate)
                        animatedProps.Add(asset);
                    else
                        stillProps.Add(asset);
                }

            if (!hasHeadProp)
            {
                var index = (uint)0;
                index += (uint)(u.FaceNbr % DesktopConstants.MaxNbrFaces);
                index += (uint)(u.ColorNbr % DesktopConstants.MaxNbrColors) << 8;
                var smileyFace = AssetsManager.Current.SmileyFaces[index];

                g.DrawImage(
                    smileyFace,
                    new Rectangle(
                        x, y,
                        smileyFace.Width,
                        smileyFace.Height),
                    0, 0,
                    smileyFace.Width,
                    smileyFace.Height,
                    GraphicsUnit.Pixel);
            }

            foreach (var prop in stillProps)
                if (prop.Image != null)
                {
                    var imgAttributes = (ImageAttributes?)null;

                    if (prop.IsGhost)
                    {
                        var matrix = new ColorMatrix
                        {
                            Matrix33 = 0.5F
                        };

                        imgAttributes = new ImageAttributes();
                        imgAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    }

                    g.DrawImage(
                        prop.Image,
                        new Rectangle(
                            x + prop.Offset.HAxis,
                            y + prop.Offset.VAxis,
                            prop.Image.Width,
                            prop.Image.Height),
                        0, 0,
                        prop.Image.Width,
                        prop.Image.Height,
                        GraphicsUnit.Pixel,
                        imgAttributes);
                }
        }
    }

    private void ScreenLayer_UserNametag(Graphics g)
    {
        var font = new Font("Arial", 11);
        var padding = 2;

        var users = (List<UserDesc>?)null;
        lock (RoomUsers)
        {
            users = RoomUsers.Values.ToList();
        }

        if ((users?.Count ?? 0) <= 0) return;

        foreach (var u in users)
        {
            if (u.UserId < 1 ||
                u.RoomPos == null) continue;

            var colour = DesktopConstants.NbrToColor(u.ColorNbr);
            using (var colourBrush = new SolidBrush(colour))
            {
                var textSize = TextRenderer.MeasureText(u.Name, font);
                var halfNameTagWidth = textSize.Width / 2;
                var halfNameTagHeight = textSize.Height / 2;

                var x = u.RoomPos.HAxis - halfNameTagWidth - padding * 2;
                var y = u.RoomPos.VAxis + halfNameTagHeight * 3 - padding * 2;

                if (x < -CONST_INT_halfPropWidth) x = -CONST_INT_halfPropWidth;
                else if (x > ScreenWidth + CONST_INT_halfPropWidth) x = ScreenWidth + CONST_INT_halfPropWidth;

                if (y < -CONST_INT_halfPropHeight) y = -CONST_INT_halfPropHeight;
                else if (y > ScreenHeight + CONST_INT_halfPropHeight) y = ScreenHeight + CONST_INT_halfPropHeight;

                //if (x < 0 ||
                //    y < 0) continue;

                g.FillRectangle(
                    Brushes.Black,
                    new Rectangle(
                        x,
                        y,
                        textSize.Width + padding,
                        textSize.Height + padding));

                g.DrawString(
                    u.Name,
                    font,
                    colourBrush,
                    x, y);
            }
        }
    }

    private void ScreenLayer_ScriptedImage(Graphics g)
    {
    }

    private void ScreenLayer_ScriptedText(Graphics g)
    {
    }

    private void ScreenLayer_SpotBorder(Graphics g)
    {
        var spots = RoomInfo?.HotSpots
            ?.Where(h => (h.Flags & HotspotFlags.HS_ShowFrame) == HotspotFlags.HS_ShowFrame)
            ?.ToList() ?? [];
        if (spots.Count > 0)
        {
            var pen = Pens.Black;
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;

            foreach (var h in spots)
                g.DrawPolygon(pen, h.Vortexes
                    .Select(v => new Point(v.HAxis, v.VAxis))
                    .ToArray());
        }
    }

    private void ScreenLayer_TopPaint(Graphics g)
    {
        ScreenLayer_Paint(g, true);
    }

    private void ScreenLayer_DimRoom(Graphics g)
    {
        g.FillRectangle(Brushes.Black, 0, 0, ScreenWidth, ScreenHeight);
    }

    private void ScreenLayer_Messages(Graphics g)
    {
        var users = RoomUsers.Values.ToList();
        if ((users?.Count ?? 0) <= 0) return;

        foreach (var u in users)
        {
            if (u.RoomPos == null) continue;

            if (u.Extended["MessageQueue"] is not DisposableQueue<MsgBubble> queue) continue;

            var msg = u.Extended["CurrentMessage"] as MsgBubble;
            if (msg != null)
                if ((msg.Type != BubbleTypes.Sticky || queue.Count > 0) &&
                    DateTime.Now.Subtract(msg.Accessed).TotalMilliseconds >= msg.Duration)
                {
                    u.Extended["CurrentMessage"] = null;
                    msg.Dispose();
                }

            if (msg == null)
            {
                if (queue.Count < 1) continue;
                queue.TryDequeue(out msg);

                if (msg == null) continue;

                u.Extended["CurrentMessage"] = msg;
            }

            if (msg == null) continue;
            if (!msg.Visible) continue;

            var loc = msg.Origin;

            var x = UserDesc.RoomPos.HAxis;
            var y = UserDesc.RoomPos.VAxis;

            if (x < -CONST_INT_halfPropWidth) x = -CONST_INT_halfPropWidth;
            else if (x > ScreenWidth + CONST_INT_halfPropWidth) x = (short)(ScreenWidth + CONST_INT_halfPropWidth);

            if (y < -CONST_INT_halfPropHeight) y = -CONST_INT_halfPropHeight;
            else if (y > ScreenHeight + CONST_INT_halfPropHeight) y = (short)(ScreenHeight + CONST_INT_halfPropHeight);

            if (x < 0 ||
                y < 0) continue;

            var image = msg.Render(this);

            // TODO:
        }
    }

    private void ScreenLayer_Paint(Graphics g, bool layer)
    {
        var helper = new GraphicsHelper(g);

        var drawCmds = RoomInfo?.DrawCmds
            ?.Where(d => d.Layer == layer)
            ?.Where(d => (d.Points?.Count ?? 0) > 0)
            ?.ToList() ?? [];
        if (drawCmds.Count > 0)
            foreach (var dc in drawCmds)
                switch (dc.Type)
                {
                    case DrawCmdTypes.DC_Path:
                    {
                        var colour = Color.FromArgb(255, dc.Red, dc.Green, dc.Blue);
                        using (var penColour = new Pen(colour, dc.PenSize))
                        using (var brushColour = new SolidBrush(colour))
                        {
                            penColour.StartCap = LineCap.Round;
                            penColour.EndCap = LineCap.Round;

                            helper.SetBrush(brushColour);
                            helper.SetPen(penColour);

                            if (dc.Filled)
                                helper.BeginPath();

                            var x = dc.Pos.HAxis;
                            var y = dc.Pos.VAxis;

                            helper.MoveTo(x, y);

                            foreach (var p in dc.Points)
                            {
                                x += p.HAxis;
                                y += p.VAxis;

                                helper.LineTo(x, y);
                            }

                            if (dc.Filled)
                                helper.Fill();

                            helper.Stroke();
                        }
                    }

                        break;
                    case DrawCmdTypes.DC_Ellipse:
                    {
                        //var colour = Color.FromArgb(255, dc.red, dc.green, dc.blue);
                        //using (var penColour = new Pen(colour, dc.penSize))
                        //using (var brushColour = new SolidBrush(colour))
                        //{
                        //    penColour.StartCap = LineCap.Round;
                        //    penColour.EndCap = LineCap.Round;

                        //    helper.SetBrush(brushColour);
                        //    helper.SetPen(penColour);

                        //    helper.BeginPath();

                        //    helper.DrawEllipse(dc.Rect);

                        //    if (dc.filled)
                        //        helper.Fill();

                        //    helper.Stroke();
                        //}

                        throw new NotImplementedException(nameof(DrawCmdTypes.DC_Ellipse));
                    }

                        break;
                    case DrawCmdTypes.DC_Text:
                    {
                        //var colour = Color.FromArgb(255, dc.red, dc.green, dc.blue);
                        //using (var penColour = new Pen(colour, dc.penSize))
                        //using (var brushColour = new SolidBrush(colour))
                        //{
                        //    penColour.StartCap = LineCap.Round;
                        //    penColour.EndCap = LineCap.Round;

                        //    helper.SetBrush(brushColour);
                        //    helper.SetPen(penColour);

                        //    helper.BeginPath();

                        //    helper.DrawText(dc.text, dc.pos.h, dc.pos.v);

                        //    if (dc.filled)
                        //        helper.Fill();

                        //    helper.Stroke();
                        //}

                        throw new NotImplementedException(nameof(DrawCmdTypes.DC_Text));
                    }

                        break;
                    case DrawCmdTypes.DC_Shape:
                    {
                        //var colour = Color.FromArgb(255, dc.red, dc.green, dc.blue);
                        //using (var penColour = new Pen(colour, dc.penSize))
                        //using (var brushColour = new SolidBrush(colour))
                        //{
                        //    penColour.StartCap = LineCap.Round;
                        //    penColour.EndCap = LineCap.Round;

                        //    helper.SetBrush(brushColour);
                        //    helper.SetPen(penColour);

                        //    helper.BeginPath();

                        //    // TODO:

                        //    if (dc.filled)
                        //        helper.Fill();

                        //    helper.Stroke();
                        //}

                        throw new NotImplementedException(nameof(DrawCmdTypes.DC_Shape));
                    }

                        break;
                }
    }

    #endregion

    #region UI Methods

    public void RefreshRibbon()
    {
        var isConnected = ConnectionState.IsConnected();

        if (((IDesktopApp)App).GetControl<ToolStrip>("toolStrip") is not ToolStrip toolStrip) return;

        toolStrip.Items.Clear();

        foreach (var ribbonItem in Ribbon.Values.ToList()) //SettingsManager.UserSettings.Ribbon
        {
            var nodeType = ribbonItem.GetType().Name;
            switch (nodeType)
            {
                case nameof(Separator):
                    toolStrip.Items.Add(new ToolStripSeparator
                    {
                        AutoSize = false,
                        Height = toolStrip.Height
                    });
                    break;
                default:
                    var item = new ToolStripButton();

                    //var binding = ApiManager.Current.ApiBindings.GetValue("toolStripDropdownlist_Click");
                    //if (binding != null)
                    //    item.Click += binding.Binding;

                    if (ribbonItem.Checkable)
                    {
                        var key = ribbonItem?.Id as Guid?;
                        if (key == null) return;

                        var ribbonItem2 = Ribbon.GetValue(key.Value);
                        if (ribbonItem2 != null)
                            item.Checked = ribbonItem2.Checked;
                    }

                    var condition = nodeType switch
                    {
                        nameof(DoorOutlines) or
                            nameof(UserNametags) or
                            nameof(Terminal) or
                            nameof(SuperUser) or
                            nameof(Draw) or
                            nameof(UsersList) or
                            nameof(RoomsList) or
                            nameof(Sounds) => isConnected,

                        nameof(GoBack) => History.History.Count > 0 &&
                                          (!History.Position.HasValue ||
                                           History.History.Keys.Min() !=
                                           History.Position.Value),

                        nameof(GoForward) => History.History.Count > 0 &&
                                             History.Position.HasValue &&
                                             History.History.Keys.Max() != History.Position.Value,

                        // nameof(Bookmarks) or
                        //     nameof(Connection) or
                        //     nameof(LiveDirectory) or
                        //     nameof(Chatlog) or
                        //     nameof(Tabs) or
                        _ => true,
                    };

                    switch (ribbonItem)
                    {
                        case StandardItem _standardItem:
                            item.Image = _standardItem.Icon.Image;
                            break;
                        case BooleanItem _booleanItem:
                            item.Image = _booleanItem.State ? _booleanItem.OnHoverIcon.Image : _booleanItem.OffHoverIcon.Image;
                            break;
                    }

                    if (item.Image != null)
                    {
                        item.Name = nodeType;
                        item.Enabled = condition;
                        item.ToolTipText = ribbonItem.Title;

                        toolStrip.Items.Add(item);
                    }

                    break;
            }
        }
    }

    public void LayerVisibility(bool visible, params LayerScreenTypes[] layers)
    {
        foreach (var layer in layers)
        {
            if (layer == LayerScreenTypes.Base) continue;

            _uiLayers[layer].Visible = visible;
        }
    }

    public void LayerOpacity(float opacity, params LayerScreenTypes[] layers)
    {
        foreach (var layer in layers)
        {
            if (layer == LayerScreenTypes.Base) continue;

            _uiLayers[layer].Opacity = opacity;
            _uiLayers[layer].Visible = opacity != 0F;
        }
    }

    public void RefreshScreen(params LayerScreenTypes[] layers)
    {
        if (layers.Length > 0)
            RefreshLayers(layers);

        try
        {
            var isConnected = ConnectionState.IsConnected();
            if (!isConnected)
            {
                foreach (var layer in _layerTypes
                             .Where(l => l != LayerScreenTypes.Base)
                             .ToList())
                {
                    _uiLayers[layer].Unload();
                }

                _uiLayers[LayerScreenTypes.Base].Load(
                    LayerSourceTypes.Resource,
                    "Lib.Media.Resources.backgrounds.aephixcorelogo.png");
            }
            else if (layers.Contains(LayerScreenTypes.Base))
            {
                var filePath = (string?)null;

                if (!string.IsNullOrWhiteSpace(MediaUrl) &&
                    !string.IsNullOrWhiteSpace(ServerName) &&
                    !string.IsNullOrWhiteSpace(RoomInfo.Picture))
                {
                    var fileName = RegexConstants.REGEX_FILESYSTEMCHARS.Replace(RoomInfo.Picture, @"_");
                    filePath = Path.Combine(Environment.CurrentDirectory, "Media", fileName);

                    if (!File.Exists(filePath))
                    {
                        var _serverName = RegexConstants.REGEX_FILESYSTEMCHARS.Replace(ServerName, @" ").Trim();
                        filePath = Path.Combine(Environment.CurrentDirectory, "Media", _serverName, fileName);
                    }

                    if (File.Exists(filePath))
                        _uiLayers[LayerScreenTypes.Base].Load(
                            LayerSourceTypes.Filesystem,
                            filePath);
                }

                if (RoomInfo.Picture !=
                    _uiLayers[LayerScreenTypes.Base].Image?.Tag?.ToString())
                    _uiLayers[LayerScreenTypes.Base].Load(
                        LayerSourceTypes.Resource,
                        "Lib.Media.Resources.backgrounds.clouds.jpg");
            }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }

        if (((IDesktopApp)App).GetControl<PictureBox>("imgScreen") is not PictureBox imgScreen) return;

        try
        {
            if (imgScreen.Image == null ||
                imgScreen.Image.Width != ScreenWidth ||
                imgScreen.Image.Height != ScreenHeight)
            {
                try
                {
                    imgScreen.Image?.Dispose();
                }
                catch
                {
                }

                var img = new Bitmap(ScreenWidth, ScreenHeight);
                img.MakeTransparent(Color.Transparent);

                using (var g = Graphics.FromImage(img))
                {
                    g.InterpolationMode =
                        SettingsManager.Current.Get<InterpolationMode>("GUI:General:" + nameof(InterpolationMode));
                    g.PixelOffsetMode =
                        SettingsManager.Current.Get<PixelOffsetMode>("GUI:General:" + nameof(PixelOffsetMode));
                    g.SmoothingMode =
                        SettingsManager.Current.Get<SmoothingMode>("GUI:General:" + nameof(SmoothingMode));

                    g.Clear(Color.Transparent);

                    foreach (var layer in _layerTypes)
                    {
                        if (!_uiLayers[layer].Visible ||
                            _uiLayers[layer].Opacity == 0F ||
                            _uiLayers[layer].Image == null) continue;

                        using (var @lock = LockContext.GetLock(_uiLayers[layer]))
                        {
                            var imgAttributes = (ImageAttributes?)null;

                            if (_uiLayers[layer].Opacity < 1.0F)
                            {
                                var matrix = new ColorMatrix
                                {
                                    Matrix33 = _uiLayers[layer].Opacity
                                };

                                imgAttributes = new ImageAttributes();
                                imgAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            }

                            g.DrawImage(
                                _uiLayers[layer].Image,
                                new Rectangle(
                                    0, 0,
                                    img.Width,
                                    img.Height),
                                0, 0,
                                _uiLayers[layer].Image.Width,
                                _uiLayers[layer].Image.Height,
                                GraphicsUnit.Pixel,
                                imgAttributes);
                        }
                    }

                    g.Save();
                }

                imgScreen.Image = img;
            }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    public void RefreshScriptEvent(ScriptEvent scriptEvent)
    {
        var screenLayers =
            (from layer
                    in CONST_Event_LayerScreen_Types_Mappings
                where layer.Key.Contains(scriptEvent.EventType)
                select layer.Value)
            .FirstOrDefault();

        if (screenLayers.Contains(LayerScreenTypes.Base))
            RefreshScreen(LayerScreenTypes.Base);

        if (CONST_EventTypes_UiRefresh.Contains(scriptEvent.EventType))
        {
            RefreshUI();
            RefreshRibbon();
        }

        RefreshScreen(
            !screenLayers.Contains(LayerScreenTypes.Base)
                ? screenLayers
                : CONST_allLayers);

        switch (scriptEvent.EventType)
        {
            case (int)ScriptEventTypes.RoomLoad:
                History.RegisterHistory(
                    $"{ServerName} - {RoomInfo.Name}",
                    $"palace://{ConnectionState.HostAddr.Address}:{ConnectionState.HostAddr.Port}/{RoomInfo.RoomID}");

                RefreshRibbon();

                ScriptEventBus.Current.Invoke(ScriptEventTypes.RoomReady, this, null, ScriptTag);
                ScriptEventBus.Current.Invoke(ScriptEventTypes.Enter, this, null, ScriptTag);

                break;
        }
    }

    public void RefreshUI()
    {
        var isConnected = ConnectionState.IsConnected();

        var app = (IDesktopApp)App;

        var form = app.GetForm();
        if (form == null) return;

        if (app.GetControl<ToolStrip>("toolStrip") is not ToolStrip toolStrip) return;

        if (app.GetControl<Label>("labelInfo") is not Label labelInfo) return;

        if (!isConnected)
        {
            form.Text = labelInfo.Text = @"Disconnected";
        }
        else
        {
            form.Text = $"Connected: {ServerName} - {RoomInfo.Name}";
            labelInfo.Text = $"Users ({RoomUsers.Count(u => u.Key != 0)}/{ServerPopulation})";
        }

        if (app.GetControl<PictureBox>("imgScreen") is PictureBox imgScreen)
        {
            toolStrip.Size = new Size(form.Width, form.Height);
            toolStrip.Location = new Point(0, 0);

            var width = ScreenWidth;
            var height = ScreenHeight;

            if (width < 1) width = DesktopConstants.AspectRatio.WidescreenDef.Default.Width;
            if (height < 1) height = DesktopConstants.AspectRatio.WidescreenDef.Default.Height;

            imgScreen.Size = new Size(width, height);
            if (toolStrip.Visible &&
                toolStrip.Dock == DockStyle.Top)
                imgScreen.Location = new Point(0, toolStrip.Location.Y + toolStrip.Height);
            else
                imgScreen.Location = new Point(0, 0);

            labelInfo.Size = new Size(width, 20);
            labelInfo.Location = new Point(0, imgScreen.Location.Y + imgScreen.Height);

            if (app.GetControl<TextBox>("txtInput") is TextBox txtInput)
            {
                txtInput.Size = new Size(width, 50);
                txtInput.Location = new Point(0, labelInfo.Location.Y + labelInfo.Height);
            }
        }
    }

    #endregion
}