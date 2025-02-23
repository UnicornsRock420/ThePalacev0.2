using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ThePalace.Client.Desktop.Entities.Gfx;
using ThePalace.Client.Desktop.Entities.Ribbon;
using ThePalace.Client.Desktop.Entities.Shared.Assets;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Enums.Palace;
using ThePalace.Network.Factories;
using ThePalace.Network.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Core
{
    public partial class DesktopSessionState : IDesktopSessionState
    {
        private static readonly IReadOnlyList<ScreenLayers> _layerTypes = Enum.GetValues<ScreenLayers>().AsReadOnly();

        private DisposableDictionary<string, IDisposable> _uiControls = new();
        public IReadOnlyDictionary<string, IDisposable> UIControls => this._uiControls.AsReadOnly();

        // UI Info
        public bool Visible { get; set; } = true;
        public DateTime? LastActivity { get; set; } = null;
        public HistoryManager History { get; private set; } = new();
        public TabPage TabPage { get; set; } = null;
        public double Scale { get; set; }
        public int ScreenWidth { get; set; } = DesktopConstants.AspectRatio.WidescreenDef.Default.Width;
        public int ScreenHeight { get; set; } = DesktopConstants.AspectRatio.WidescreenDef.Default.Height;

        public AssetSpec SelectedProp { get; set; } = null;
        public UserRec SelectedUser { get; set; } = null;
        public HotspotRec SelectedHotSpot { get; set; } = null;

        private DisposableDictionary<ScreenLayers, ScreenLayer> _uiLayers = new();
        public IReadOnlyDictionary<ScreenLayers, ScreenLayer> UILayers => _uiLayers.AsReadOnly();

        public RoomDesc RoomInfo { get; set; }
        public ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; }
        UserDesc IDesktopSessionState.SelectedUser { get; set; }
        HotspotDesc IDesktopSessionState.SelectedHotSpot { get; set; }

        public ConcurrentDictionary<string, object> Extended => throw new NotImplementedException();

        public object? ScriptState { get; set; }

        public Guid Id => throw new NotImplementedException();

        public uint UserId { get; set; }
        public IConnectionState? ConnectionState { get; set; }
        public UserDesc? UserDesc { get; set; }
        public RegistrationRec? RegInfo { get; set; }
        public object? State { get; set; }

        public string? MediaUrl { get; set; }
        public string? ServerName { get; set; }
        public int ServerPopulation { get; set; }

        public void RefreshUI()
        {
            var isConnected = AsyncTcpSocket.IsConnected(this.ConnectionState);

            var form = GetForm("SessionStateManager");
            if (form == null) return;

            var toolStrip = GetControl("toolStrip") as ToolStrip;
            if (toolStrip == null) return;

            var labelInfo = GetControl("labelInfo") as Label;
            if (labelInfo == null) return;

            if (!isConnected)
            {
                form.Text = labelInfo.Text = @"Disconnected";
            }
            else
            {
                form.Text = $"Connected: {this.ServerName} - {this.RoomInfo.Name}";
                labelInfo.Text = $"Users ({this.RoomUsers.Count(u => u.Key != 0)}/{this.ServerPopulation})";
            }

            var imgScreen = GetControl("imgScreen") as PictureBox;
            if (imgScreen != null)
            {
                toolStrip.Size = new Size(form.Width, form.Height);
                toolStrip.Location = new System.Drawing.Point(0, 0);

                var width = this.ScreenWidth;
                var height = this.ScreenHeight;

                if (width < 1) width = DesktopConstants.AspectRatio.WidescreenDef.Default.Width;
                if (height < 1) height = DesktopConstants.AspectRatio.WidescreenDef.Default.Height;

                imgScreen.Size = new Size(width, height);
                if (toolStrip.Visible &&
                    toolStrip.Dock == DockStyle.Top)
                    imgScreen.Location = new System.Drawing.Point(0, toolStrip.Location.Y + toolStrip.Height);
                else
                    imgScreen.Location = new System.Drawing.Point(0, 0);

                labelInfo.Size = new Size(width, 20);
                labelInfo.Location = new System.Drawing.Point(0, imgScreen.Location.Y + imgScreen.Height);

                var txtInput = GetControl("txtInput") as TextBox;
                if (txtInput != null)
                {
                    txtInput.Size = new Size(width, 50);
                    txtInput.Location = new System.Drawing.Point(0, labelInfo.Location.Y + labelInfo.Height);
                }
            }
        }
        public void RefreshRibbon()
        {
            var isConnected = AsyncTcpSocket.IsConnected(this.ConnectionState);

            var toolStrip = GetControl("toolStrip") as ToolStrip;
            if (toolStrip == null) return;

            toolStrip.Items.Clear();

            foreach (var ribbonItem in SettingsManager.UserSettings.Ribbon)
            {
                var nodeType = ribbonItem.GetType().Name;
                switch (nodeType)
                {
                    case nameof(Separator):
                        toolStrip.Items.Add(new ToolStripSeparator
                        {
                            AutoSize = false,
                            Height = toolStrip.Height,
                        });
                        break;
                    default:
                        var item = new ToolStripButton();

                        //var binding = ApiManager.Current.ApiBindings.GetValue("toolStripDropdownlist_Click");
                        //if (binding != null)
                        //    item.Click += binding.Binding;

                        if (ribbonItem.Checkable)
                        {
                            var ribbonItem2 = SettingsManager.SystemSettings.Ribbon.GetValue(nodeType);
                            if (ribbonItem2 != null)
                                item.Checked = ribbonItem2.Checked;
                        }

                        var condition = true;
                        switch (nodeType)
                        {
                            //case nameof(Bookmarks):
                            //case nameof(Connection):
                            //case nameof(LiveDirectory):
                            //case nameof(Chatlog):
                            //case nameof(Tabs):
                            //    condition = true;
                            //    break;
                            case nameof(DoorOutlines):
                            case nameof(UserNametags):
                            case nameof(Terminal):
                            case nameof(SuperUser):
                            case nameof(Draw):
                            case nameof(UsersList):
                            case nameof(RoomsList):
                            case nameof(Sounds):
                                condition = isConnected;
                                break;
                            case nameof(GoBack):
                                condition =
                                    (History.History.Count > 0) &&
                                    (!History.Position.HasValue || History.History.Keys.Min() != History.Position.Value);
                                break;
                            case nameof(GoForward):
                                condition =
                                    (History.History.Count > 0) &&
                                    History.Position.HasValue &&
                                    History.History.Keys.Max() != History.Position.Value;
                                break;
                        }

                        if (ribbonItem is StandardItem _standardItem)
                            item.Image = _standardItem.Image;
                        else if (ribbonItem is BooleanItem _booleanItem)
                            item.Image = _booleanItem.State ? _booleanItem.OnImage : _booleanItem.OffImage;

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

        public FormBase GetForm(string friendlyName)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName))
                return this._uiControls.GetValue(friendlyName) as FormBase;

            return null;
        }
        public T GetForm<T>(string friendlyName)
            where T : FormBase
        {
            if (!string.IsNullOrWhiteSpace(friendlyName))
                return this._uiControls.GetValue(friendlyName) as T;

            return default;
        }

        public void RegisterForm(string friendlyName, FormBase form)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                form != null)
                this._uiControls?.TryAdd(friendlyName, form);
        }

        public void UnregisterForm(string friendlyName, FormBase form)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                form != null)
                this._uiControls?.TryRemove(friendlyName, out var _);
        }

        public Control GetControl(string friendlyName)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName))
                return this._uiControls.GetValue(friendlyName) as Control;

            return null;
        }

        public void RegisterControl(string friendlyName, Control control)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                control != null)
                this._uiControls?.TryAdd(friendlyName, control);
        }

        public void RegisterControl(string friendlyName, IDisposable control)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                control != null)
                this._uiControls?.TryAdd(friendlyName, control);
        }

        public void UnregisterForm(string friendlyName, Control control)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                control != null)
                this._uiControls?.TryRemove(friendlyName, out var _);
        }

        public void UnregisterForm(string friendlyName, IDisposable control)
        {
            if (!string.IsNullOrWhiteSpace(friendlyName) &&
                control != null)
                this._uiControls?.TryRemove(friendlyName, out var _);
        }

        public void RefreshScreen(params ScreenLayers[] layers)
        {
            if (layers.Length > 0)
                this.RefreshLayers(layers);

            try
            {
                var isConnected = AsyncTcpSocket.IsConnected(this.ConnectionState);
                if (!isConnected)
                {
                    foreach (var layer in _layerTypes)
                    {
                        if (layer == ScreenLayers.Base) continue;

                        this._uiLayers[layer].Unload();
                    }

                    this._uiLayers[ScreenLayers.Base].Load(this, LayerLoadingTypes.Resource, "ThePalace.Core.Desktop.Core.Resources.backgrounds.aephixcorelogo.png");
                }
                else if (layers.Contains(ScreenLayers.Base))
                {
                    var filePath = null as string;

                    if (!string.IsNullOrWhiteSpace(this.MediaUrl) &&
                        !string.IsNullOrWhiteSpace(this.ServerName) &&
                        !string.IsNullOrWhiteSpace(this.RoomInfo.Picture))
                    {
                        var fileName = ThePalace.Common.Constants.RegexConstants.REGEX_FILESYSTEMCHARS.Replace(this.RoomInfo.Picture, @"_");
                        filePath = Path.Combine(Environment.CurrentDirectory, "Media", fileName);

                        if (!File.Exists(filePath))
                        {
                            var _serverName = ThePalace.Common.Constants.RegexConstants.REGEX_FILESYSTEMCHARS.Replace(this.ServerName, @" ").Trim();
                            filePath = Path.Combine(Environment.CurrentDirectory, "Media", _serverName, fileName);
                        }

                        if (File.Exists(filePath))
                            this._uiLayers[ScreenLayers.Base].Load(this, LayerLoadingTypes.Filesystem, filePath);
                    }

                    if (this.RoomInfo.Picture !=
                        this._uiLayers[ScreenLayers.Base].Image?.Tag?.ToString())
                        this._uiLayers[ScreenLayers.Base].Load(this, LayerLoadingTypes.Resource, "ThePalace.Core.Desktop.Core.Resources.backgrounds.clouds.jpg");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"RefreshScreen.Base: {ex.Message}");
#endif
            }

            var imgScreen = GetControl("imgScreen") as PictureBox;
            if (imgScreen != null)
                try
                {
                    //var img = null as Bitmap;
                    //if (imgScreen.Image == null ||
                    //    imgScreen.Image.Width != this.ScreenWidth ||
                    //    imgScreen.Image.Height != this.ScreenHeight)
                    //{
                    //    try { imgScreen.Image?.Dispose(); } catch { }
                    var img = new Bitmap(this.ScreenWidth, this.ScreenHeight);
                    img.MakeTransparent(Color.Transparent);
                    //}

                    using (var g = Graphics.FromImage(img))
                    {
                        g.InterpolationMode = SettingsManager.Current.GetOption<InterpolationMode>(@"\GUI\General\" + nameof(InterpolationMode));
                        g.PixelOffsetMode = SettingsManager.Current.GetOption<PixelOffsetMode>(@"\GUI\General\" + nameof(PixelOffsetMode));
                        g.SmoothingMode = SettingsManager.Current.GetOption<SmoothingMode>(@"\GUI\General\" + nameof(SmoothingMode));

                        g.Clear(Color.Transparent);

                        foreach (var layer in _layerTypes)
                        {
                            if (!this._uiLayers[layer].Visible ||
                                this._uiLayers[layer].Opacity == 0F ||
                                this._uiLayers[layer].Image == null) continue;

                            lock (this._uiLayers[layer])
                            {
                                var imgAttributes = null as ImageAttributes;

                                if (this._uiLayers[layer].Opacity < 1.0F)
                                {
                                    var matrix = new ColorMatrix
                                    {
                                        Matrix33 = this._uiLayers[layer].Opacity,
                                    };

                                    imgAttributes = new();
                                    imgAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                }

                                g.DrawImage(
                                    this._uiLayers[layer].Image,
                                    new Rectangle(
                                        0, 0,
                                        img.Width,
                                        img.Height),
                                    0, 0,
                                    this._uiLayers[layer].Image.Width,
                                    this._uiLayers[layer].Image.Height,
                                    GraphicsUnit.Pixel,
                                    imgAttributes);
                            }
                        }

                        g.Save();
                    }

                    imgScreen.Image = img;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                }
        }

        public void LayerVisibility(bool visible, params ScreenLayers[] layers)
        {
            foreach (var layer in layers)
            {
                if (layer == ScreenLayers.Base) continue;

                this._uiLayers[layer].Visible = visible;
            }
        }

        public void LayerOpacity(float opacity, params ScreenLayers[] layers)
        {
            foreach (var layer in layers)
            {
                if (layer == ScreenLayers.Base) continue;

                this._uiLayers[layer].Opacity = opacity;
                this._uiLayers[layer].Visible = opacity != 0F;
            }
        }

        private void RefreshLayers(params ScreenLayers[] layers)
        {
            if (!this.Visible ||
                this.ScreenWidth < 1 ||
                this.ScreenHeight < 1) return;

            try
            {
                foreach (var layer in layers)
                {
                    lock (this._uiLayers[layer])
                    {
                        if (layer == ScreenLayers.Base) continue;

                        switch (layer)
                        {
                            case ScreenLayers.DimRoom:
                                if (this._uiLayers[layer].Opacity == 1F)
                                {
                                    this._uiLayers[layer].Unload();

                                    break;
                                }
                                else if (this._uiLayers[layer].Image != null) break;

                                goto default;
                            default:
                                using (var g = this._uiLayers[layer].Initialize(this.ScreenWidth, this.ScreenHeight))
                                {
                                    if (!this._uiLayers[layer].Visible) continue;

                                    g.InterpolationMode = SettingsManager.Current.GetOption<InterpolationMode>(@"\GUI\General\" + nameof(InterpolationMode));
                                    g.PixelOffsetMode = SettingsManager.Current.GetOption<PixelOffsetMode>(@"\GUI\General\" + nameof(PixelOffsetMode));
                                    g.SmoothingMode = SettingsManager.Current.GetOption<SmoothingMode>(@"\GUI\General\" + nameof(SmoothingMode));

                                    switch (layer)
                                    {
                                        case ScreenLayers.LooseProp: this.ScreenLayer_LooseProp(g); break;
                                        case ScreenLayers.SpotImage: this.ScreenLayer_SpotImage(g); break;
                                        case ScreenLayers.BottomPaint: this.ScreenLayer_BottomPaint(g); break;
                                        case ScreenLayers.SpotNametag: this.ScreenLayer_SpotNametag(g); break;
                                        case ScreenLayers.UserProp: this.ScreenLayer_UserProp(g); break;
                                        case ScreenLayers.UserNametag: this.ScreenLayer_UserNametag(g); break;
                                        case ScreenLayers.ScriptedImage: this.ScreenLayer_ScriptedImage(g); break;
                                        case ScreenLayers.ScriptedText: this.ScreenLayer_ScriptedText(g); break;
                                        case ScreenLayers.SpotBorder: this.ScreenLayer_SpotBorder(g); break;
                                        case ScreenLayers.TopPaint: this.ScreenLayer_TopPaint(g); break;
                                        case ScreenLayers.DimRoom: this.ScreenLayer_DimRoom(g); break;
                                        case ScreenLayers.Messages: this.ScreenLayer_Messages(g); break;
                                    }

                                    g.Save();
                                }

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
        private void ScreenLayer_LooseProp(Graphics g)
        {
            var looseProps = this.RoomInfo?.LooseProps
                ?.ToList() ?? [];
            if (looseProps.Count > 0)
                foreach (var looseProp in looseProps)
                {
                    var prop = AssetsManager.Current.GetAsset(this, looseProp.AssetSpec);
                    if (prop == null) continue;

                    if (prop.Image == null)
                        try
                        {
                            prop.Image = AssetDesc.Render(prop);
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Debug.WriteLine(ex.Message);
#endif

                            continue;
                        }

                    if (prop.Image == null) continue;

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
            if ((this.RoomInfo?.HotSpots?.Count ?? 0) > 0)
                foreach (var spot in this.RoomInfo.HotSpots)
                {
                    var nbrStates = spot.States?.Count ?? 0;
                    if (nbrStates < 1 ||
                        spot.SpotInfo.State < 0 ||
                        spot.SpotInfo.State >= nbrStates) continue;

                    var state = spot.States[spot.SpotInfo.State];
                    if (state == null ||
                        state.StateInfo.PictID < 1) continue;

                    var pictName = this.RoomInfo?.Pictures
                        ?.Where(p => p.PicID == state.StateInfo.PictID)
                        ?.Select(p => p.Name)
                        ?.FirstOrDefault();
                    if (pictName == null) continue;

                    var _pictName = ThePalace.Common.Constants.RegexConstants.REGEX_FILESYSTEMCHARS.Replace(pictName, @"_");
                    var filePath = Path.Combine(Environment.CurrentDirectory, "Media", _pictName);

                    if (!File.Exists(filePath))
                    {
                        var _serverName = ThePalace.Common.Constants.RegexConstants.REGEX_FILESYSTEMCHARS.Replace(this.ServerName, @" ").Trim();
                        filePath = Path.Combine(Environment.CurrentDirectory, "Media", _serverName, _pictName);
                    }

                    if (!File.Exists(filePath)) continue;

                    using (var pict = new Bitmap(filePath))
                        g.DrawImage(
                            pict,
                            new Rectangle(
                                spot.SpotInfo.Loc.HAxis - state.StateInfo.PicLoc.HAxis - (pict.Width / 2),
                                spot.SpotInfo.Loc.VAxis - state.StateInfo.PicLoc.VAxis - (pict.Height / 2),
                                pict.Width,
                                pict.Height),
                            0, 0,
                            pict.Width,
                            pict.Height,
                            GraphicsUnit.Pixel);
                }
        }
        private void ScreenLayer_BottomPaint(Graphics g) =>
            ScreenLayer_Paint(g, false);
        private void ScreenLayer_SpotNametag(Graphics g)
        {
            var spots = this.RoomInfo?.HotSpots
                ?.Where(h => ((HotspotFlags)h.SpotInfo.Flags & HotspotFlags.HS_ShowName) == HotspotFlags.HS_ShowName)
                ?.ToList() ?? new();
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

                        var x = spot.SpotInfo.Loc.HAxis - halfNameTagWidth - padding * 2;
                        var y = spot.SpotInfo.Loc.VAxis + halfNameTagHeight * 3 - padding * 2;

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
            var halfPropWidth = (int)AssetConstants.Values.DefaultPropWidth / 2;
            var halfPropHeight = (int)AssetConstants.Values.DefaultPropHeight / 2;

            var users = null as List<UserDesc>;
            lock (this.RoomUsers)
                users = this.RoomUsers.Values
                    .Where(u =>
                        !(u.UserInfo.UserId < 1 ||
                        u.UserInfo.RoomPos == null))
                    .ToList();
            if (users.Count > 0)
                foreach (var u in users)
                {
                    var x = u.UserInfo.RoomPos.HAxis - halfPropWidth;
                    var y = u.UserInfo.RoomPos.VAxis - halfPropHeight;

                    if (x < -halfPropWidth) x = -halfPropWidth;
                    else if (x > this.ScreenWidth + halfPropWidth) x = this.ScreenWidth + halfPropWidth;

                    if (y < -halfPropHeight) y = -halfPropHeight;
                    else if (y > this.ScreenHeight + halfPropHeight) y = this.ScreenHeight + halfPropHeight;

                    //if (x < 0 ||
                    //    y < 0) continue;

                    var animatedProps = new List<AssetDesc>();
                    var stillProps = new List<AssetDesc>();
                    var hasPalindromeProp = false;
                    var hasAnimatedProp = false;
                    var hasHeadProp = false;

                    var assetSpecs = u.UserInfo.PropSpec?.ToList() ?? [];
                    if (assetSpecs.Count > 0)
                        foreach (var assetSpec in assetSpecs)
                        {
                            var asset = AssetsManager.Current.GetAsset(this, assetSpec);
                            if (asset == null) continue;

                            hasPalindromeProp |= asset.AssetInfo.IsPalindrome;
                            hasAnimatedProp |= asset.AssetInfo.IsAnimate;
                            hasHeadProp |= asset.AssetInfo.IsHead;

                            if (asset.Image == null)
                                try
                                {
                                    asset.Image = AssetDesc.Render(asset);
                                }
                                catch (Exception ex)
                                {
#if DEBUG
                                    Debug.WriteLine(ex.Message);
#endif
                                }

                            if (asset.Image != null)
                            {
                                if (asset.AssetInfo.IsAnimate)
                                    animatedProps.Add(asset);
                                else
                                    stillProps.Add(asset);
                            }
                        }

                    if (!hasHeadProp)
                    {
                        var index = (uint)0;
                        index += (uint)(u.UserInfo.FaceNbr % DesktopConstants.MaxNbrFaces);
                        index += (uint)(u.UserInfo.ColorNbr % DesktopConstants.MaxNbrColors) << 8;
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
                            var imgAttributes = null as ImageAttributes;

                            if (prop.AssetInfo.IsGhost)
                            {
                                var matrix = new ColorMatrix
                                {
                                    Matrix33 = 0.5F,
                                };

                                imgAttributes = new();
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
            var halfPropWidth = (int)AssetConstants.Values.DefaultPropWidth / 2;
            var halfPropHeight = (int)AssetConstants.Values.DefaultPropHeight / 2;

            var font = new Font("Arial", 11);
            var padding = 2;

            var users = null as List<UserDesc>;
            lock (this.RoomUsers)
                users = this.RoomUsers.Values.ToList();

            if ((users?.Count ?? 0) > 0)
                foreach (var u in users)
                {
                    if (u.UserInfo.UserId < 1 ||
                        u.UserInfo.RoomPos == null) continue;

                    var colour = DesktopConstants.NbrToColor(u.UserInfo.ColorNbr);
                    using (var colourBrush = new SolidBrush(colour))
                    {
                        var textSize = TextRenderer.MeasureText(u.UserInfo.Name, font);
                        var halfNameTagWidth = textSize.Width / 2;
                        var halfNameTagHeight = textSize.Height / 2;

                        var x = u.UserInfo.RoomPos.HAxis - halfNameTagWidth - padding * 2;
                        var y = u.UserInfo.RoomPos.VAxis + halfNameTagHeight * 3 - padding * 2;

                        if (x < -halfPropWidth) x = -halfPropWidth;
                        else if (x > this.ScreenWidth + halfPropWidth) x = this.ScreenWidth + halfPropWidth;

                        if (y < -halfPropHeight) y = -halfPropHeight;
                        else if (y > this.ScreenHeight + halfPropHeight) y = this.ScreenHeight + halfPropHeight;

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
                            u.UserInfo.Name,
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
            var spots = this.RoomInfo?.HotSpots
                ?.Where(h => (h.SpotInfo.Flags & HotspotFlags.HS_ShowFrame) == HotspotFlags.HS_ShowFrame)
                ?.ToList() ?? [];
            if (spots.Count > 0)
            {
                var pen = Pens.Black;
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                foreach (var h in spots)
                    g.DrawPolygon(pen, h.Vortexes
                        .Select(v => new System.Drawing.Point(v.HAxis, v.VAxis))
                        .ToArray());
            }
        }
        private void ScreenLayer_TopPaint(Graphics g) =>
            ScreenLayer_Paint(g, true);
        private void ScreenLayer_DimRoom(Graphics g)
        {
            g.FillRectangle(Brushes.Black, 0, 0, this.ScreenWidth, this.ScreenHeight);
        }
        private void ScreenLayer_Messages(Graphics g)
        {
            var users = null as List<UserDesc>;
            lock (this.RoomUsers)
                users = this.RoomUsers.Values.ToList();

            if ((users?.Count ?? 0) > 0)
                foreach (var u in users)
                {
                    if (u.UserInfo.RoomPos == null) continue;

                    var queue = u.Extended["MessageQueue"] as DisposableQueue<MsgBubble>;
                    if (queue == null) continue;

                    var msg = u.Extended["CurrentMessage"] as MsgBubble;
                    if (msg != null)
                    {
                        if ((msg.Type != BubbleTypes.Sticky || queue.Count > 0) &&
                            DateTime.Now.Subtract(msg.Accessed).TotalMilliseconds >= msg.Duration)
                        {
                            u.Extended["CurrentMessage"] = null;
                            msg.Dispose();
                        }
                    }

                    if (msg == null)
                    {
                        if (queue.Count < 1) continue;
                        else queue.TryDequeue(out msg);

                        if (msg == null) continue;

                        u.Extended["CurrentMessage"] = msg;
                    }

                    if (msg == null) continue;
                    else if (!msg.Visible) continue;

                    var loc = msg.Origin;

                    //var halfPropWidth = (int)AssetConstants.Values.DefaultPropWidth / 2;
                    //var halfPropHeight = (int)AssetConstants.Values.DefaultPropHeight / 2;

                    //if (x < -halfPropWidth) x = (short)-halfPropWidth;
                    //else if (x > (sessionState.ScreenWidth + halfPropWidth)) x = (short)(sessionState.ScreenWidth + halfPropWidth);

                    //if (y < -halfPropHeight) y = (short)-halfPropHeight;
                    //else if (y > (sessionState.ScreenHeight + halfPropHeight)) y = (short)(sessionState.ScreenHeight + halfPropHeight);

                    //if (x < 0 ||
                    //    y < 0) continue;

                    var image = msg.Render();

                    // TODO:
                }
        }
        private void ScreenLayer_Paint(Graphics g, bool layer)
        {
            var helper = new GraphicsHelper(g);

            var drawCmds = this.RoomInfo?.DrawCmds
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
    }
}