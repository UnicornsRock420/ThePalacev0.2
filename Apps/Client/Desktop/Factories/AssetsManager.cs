using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using ThePalace.Client.Desktop.Entities.Shared.Assets;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Common.Factories;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Factories.Core;

namespace ThePalace.Client.Desktop.Factories;

public class AssetsManager : SingletonDisposable<AssetsManager>
{
    public AssetsManager()
    {
        _managedResources.AddRange(
            new IDisposable[]
            {
                SmileyFaces,
                Assets
            });

        ApiManager.Current.RegisterApi(nameof(ExecuteMacro), ExecuteMacro);
    }

    public Type ResourceType { get; set; }

    public DisposableDictionary<uint, Bitmap> SmileyFaces { get; private set; } = new();
    public DisposableDictionary<int, AssetDesc> Assets { get; private set; } = new();
    public List<AssetSpec[]> Macros { get; private set; } = new();

    ~AssetsManager()
    {
        Dispose(false);
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        SmileyFaces = null;
        Assets = null;
    }

    private void ExecuteMacro(object sender = null, EventArgs e = null)
    {
        if (sender is not IUISessionState sessionState) return;

        if (e is not ApiEvent apiEvent) return;

        var list = new List<AssetSpec>();

        if (apiEvent.HotKeyState is AssetSpec[] _assetSpecs1)
            list.AddRange(_assetSpecs1);
        if (apiEvent.EventState is AssetSpec[] _assetSpecs2)
            list.AddRange(_assetSpecs2);

        //if (list.Count > 0)
        //    ConnectionManager.Current.Send(sessionState, new MSG_Header
        //    {
        //        EventType = EventTypes.MSG_USERPROP,
        //        protocolSend = new MSG_USERPROP
        //        {
        //            AssetSpec = list
        //                .Take(9)
        //                .ToList(),
        //        },
        //    });
    }

    public void LoadSmilies(string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName)) return;

        var imgSmileyFaces = null as Bitmap;

        using (var imgStream = ResourceType
                   ?.Assembly
                   ?.GetManifestResourceStream($"ThePalace.Core.Desktop.Core.Resources.smilies.{resourceName}"))
        {
            if (imgStream == null) return;

            imgSmileyFaces = new Bitmap(imgStream);
            if (imgSmileyFaces == null) return;

            if (SmileyFaces.Count > 0)
            {
                foreach (var smileyFace in SmileyFaces.Values)
                    try
                    {
                        smileyFace.Dispose();
                    }
                    catch
                    {
                    }

                SmileyFaces.Clear();
            }
        }

        var deltaX = (uint)(imgSmileyFaces.Width / DesktopConstants.MaxNbrFaces);
        var deltaY = (uint)(imgSmileyFaces.Height / DesktopConstants.MaxNbrColors);

        for (var x = (uint)0; x < imgSmileyFaces.Width; x += deltaX)
        for (var y = (uint)0; y < imgSmileyFaces.Height; y += deltaY)
        {
            var result = new Bitmap((int)AssetConstants.Values.DefaultPropWidth,
                (int)AssetConstants.Values.DefaultPropHeight);

            using (var canvas = Graphics.FromImage(result))
            {
                canvas.InterpolationMode = InterpolationMode.NearestNeighbor;
                canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                canvas.SmoothingMode = SmoothingMode.HighQuality;

                canvas.DrawImage(
                    imgSmileyFaces,
                    new Rectangle(
                        0, 0,
                        (int)AssetConstants.Values.DefaultPropWidth,
                        (int)AssetConstants.Values.DefaultPropHeight),
                    new Rectangle(
                        (int)x, (int)y,
                        (int)deltaX,
                        (int)deltaY),
                    GraphicsUnit.Pixel);

                canvas.Save();
            }

            var index = (uint)0;
            index += x / deltaX % DesktopConstants.MaxNbrFaces;
            index += (y / deltaY % DesktopConstants.MaxNbrColors) << 8;

            SmileyFaces.TryAdd(index, result);
        }

        imgSmileyFaces?.Dispose();
        imgSmileyFaces = null;
    }

    public void RegisterAsset(AssetDesc assetRec)
    {
        lock (Current.Assets)
        {
            Current.Assets.TryAdd(assetRec.AssetInfo.AssetSpec.Id, assetRec);
        }
    }

    public void FreeAssets(bool purge = false, params int[] propIDs)
    {
        lock (Current.Assets)
        {
            var sessions = SessionManager.Current.Sessions.Values
                .Cast<IDesktopSessionState>()
                .ToList();

            var inUsePropIDs = sessions
                ?.SelectMany(s => s?.RoomUsers?.Values
                    ?.Where(u => u?.UserInfo?.PropSpec != null)
                    ?.SelectMany(u => u.UserInfo.PropSpec))
                ?.Select(p => p?.Id ?? 0)
                ?.Concat(sessions
                    ?.Where(s => s?.RoomInfo?.LooseProps != null)
                    ?.SelectMany(s => s?.RoomInfo?.LooseProps
                        ?.Select(l => l?.AssetSpec))
                    .Select(p => p?.Id ?? 0)
                    ?.Distinct()
                    ?.Where(id => id != 0))
                ?.ToList() ?? new List<int>();

            var iQuery = Current.Assets.Values
                .Select(a => a.AssetInfo.AssetSpec.Id)
                .AsQueryable();

            if (propIDs.Length > 0)
                iQuery = iQuery.Where(id => propIDs.Contains(id));

            var toPurgePropIDs = iQuery
                .Where(id => !inUsePropIDs.Contains(id))
                .ToList();
            foreach (var propID in toPurgePropIDs)
            {
                var prop = Assets[propID];

                if (purge)
                    Assets.TryRemove(propID, out prop);

                //try { prop.Image?.Dispose(); prop.Image = null; } catch { }
            }
        }
    }

    public AssetDesc GetAsset(IDesktopSessionState sessionState, AssetSpec assetSpec, bool downloadAsset = false)
    {
        var assetID = assetSpec.Id;

        lock (Current.Assets)
        {
            //if (Current.Assets.ContainsKey(assetID))
            //    return Current.Assets[assetID];
            //else if (downloadAsset)
            //    TaskManager.Current.DownloadAsset(sessionState, assetSpec);
        }

        return null;
    }
}