using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using Lib.Common.Desktop.Constants;
using Lib.Common.Desktop.Entities.Core;
using Lib.Common.Desktop.Factories;
using Lib.Common.Enums.App;
using Lib.Common.Factories.Core;
using Lib.Common.Threading;
using Lib.Core.Constants;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Entities.Threading;
using Lib.Core.Enums;
using Lib.Core.Factories.Core;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using ThePalace.Client.Desktop.Interfaces;
using AssetDesc = ThePalace.Client.Desktop.Entities.Shared.Assets.AssetDesc;

namespace ThePalace.Client.Desktop.Factories;

public class AssetsManager : SingletonDisposable<AssetsManager>
{
    public AssetsManager()
    {
        _managedResources.AddRange(
        [
            SmileyFaces,
            Assets
        ]);

        ApiManager.Current.RegisterApi(nameof(ExecuteMacro), ExecuteMacro);
    }

    ~AssetsManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        SmileyFaces = null;
        Assets = null;
    }

    public DisposableDictionary<uint, Bitmap> SmileyFaces { get; private set; } = new();
    public DisposableDictionary<int, AssetDesc> Assets { get; private set; } = new();
    public List<AssetSpec[]> Macros { get; private set; } = [];

    private void ExecuteMacro(object sender = null, EventArgs e = null)
    {
        if (sender is not IUserSessionState sessionState) return;

        if (e is not ApiEvent apiEvent) return;

        var list = new List<AssetSpec>();

        if (apiEvent.HotKeyState is AssetSpec[] _assetSpecs1)
            list.AddRange(_assetSpecs1);
        if (apiEvent.EventState is AssetSpec[] _assetSpecs2)
            list.AddRange(_assetSpecs2);

        if (list.Count > 0)
            sessionState.Send(
                sessionState.UserId,
                new MSG_USERPROP
                {
                    AssetSpec = list
                        .Take(9)
                        .ToArray(),
                });
    }

    public void LoadSmilies(string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName)) return;

        var xPath = $"Lib.Media.Resources.smilies.{resourceName}";
        var imgSmileyFaces = (Bitmap?)null;

        using (var imgStream = AppDomain.CurrentDomain
                   .GetAssemblies()
                   .Where(a =>
                   {
                       try
                       {
                           using (var stream = a.GetManifestResourceStream(xPath))
                           {
                               return stream != null;
                           }
                       }
                       catch
                       {
                       }

                       return false;
                   })
                   .Select(a => a.GetManifestResourceStream(xPath))
                   .FirstOrDefault())
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
        using (var @lock = LockContext.GetLock(Assets))
        {
            Current.Assets.TryAdd(assetRec.AssetRec.AssetSpec.Id, assetRec);
        }
    }

    public void FreeAssets(bool purge = false, params int[] propIDs)
    {
        using (var @lock = LockContext.GetLock(Assets))
        {
            var sessions = SessionManager.Current.Sessions.Values
                .Cast<IClientDesktopSessionState>()
                .ToList();

            var inUsePropIDs = sessions
                ?.SelectMany(s => s?.RoomUsers?.Values
                    ?.Where(u => u?.UserRec?.PropSpec != null)
                    ?.SelectMany(u => u.UserRec.PropSpec))
                ?.Select(p => p?.Id ?? 0)
                ?.Concat(sessions
                    ?.Where(s => s?.RoomInfo?.LooseProps != null)
                    ?.SelectMany(s => s?.RoomInfo?.LooseProps
                        ?.Select(l => l?.AssetSpec))
                    .Select(p => p?.Id ?? 0)
                    ?.Distinct()
                    ?.Where(id => id != 0))
                ?.ToList() ?? [];

            var iQuery = Current.Assets.Values
                .Select(a => a.AssetRec.AssetSpec.Id)
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

                try
                {
                    prop.Image?.Dispose();
                }
                finally
                {
                    prop.Image = null;
                }
            }
        }
    }

    public AssetDesc GetAsset(IClientDesktopSessionState sessionState, AssetSpec assetSpec, bool downloadAsset = false)
    {
        var assetID = assetSpec.Id;

        using (var @lock = LockContext.GetLock(Assets))
        {
            if (Current.Assets.TryGetValue(assetID, out var value))
            {
                if (value.Image != null) return value;

                var job = (Job<AssetCmd>)sessionState.App.Jobs[ThreadQueues.Assets];

                if (job.Queue?.ToList()?.Any(i =>
                        i.AssetDesc?.AssetRec?.AssetSpec?.Id ==
                        value?.AssetRec?.AssetSpec?.Id) ?? false) return value;

                job.Enqueue(new AssetCmd
                {
                    AssetDesc = value,
                });

                return value;
            }

            if (downloadAsset)
                sessionState.Send<IClientDesktopSessionState, MSG_ASSETQUERY>(
                    sessionState.UserId,
                    new MSG_ASSETQUERY
                    {
                        AssetType = LegacyAssetTypes.RT_PROP,
                        AssetSpec = assetSpec,
                    });
        }

        return null;
    }
}