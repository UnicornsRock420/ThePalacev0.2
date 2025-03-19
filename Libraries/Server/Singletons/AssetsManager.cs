using System.Collections.Concurrent;
using Lib.Common.Factories.Core;
using Lib.Common.Server.Interfaces;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Entities.Shared.Assets;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Singletons;

namespace Lib.Common.Server.Factories;

public class AssetsManager : SingletonDisposable<AssetsManager>
{
    public AssetsManager()
    {
        _managedResources.AddRange(
        [
            Assets
        ]);
    }

    ~AssetsManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        Assets = null;
    }

    public DisposableDictionary<int, AssetDesc> Assets { get; private set; } = new();
    public List<AssetSpec[]> Macros { get; private set; } = [];

    public void RegisterAsset(AssetDesc assetRec)
    {
        using (var @lock = LockContext.GetLock(Assets))
        {
            Current.Assets.TryAdd(assetRec.AssetSpec.Id, assetRec);
        }
    }

    public void FreeAssets(bool purge = false, params int[] propIDs)
    {
        using (var @lock = LockContext.GetLock(Assets))
        {
            var sessions = SessionManager.Current.Sessions.Values
                .Cast<IServerSessionState>()
                .ToList();

            var inUsePropIDs = new List<int>();

            sessions
                ?.Where(s => (s?.Users?.Count ?? 0) > 0)
                ?.SelectMany(s => s.Users.Values
                    ?.Where(u => (u?.UserDesc?.PropSpec?.Length ?? 0) > 0)
                    ?.Select(u => u.UserDesc.PropSpec
                        ?.Where(p => p?.Id != 0)
                        ?.Select(p => p.Id)))
                ?.Concat(sessions
                    ?.Where(s => (s?.Rooms?.Count ?? 0) > 0)
                    ?.SelectMany(s => s.Rooms.Values
                        ?.Where(r => (r?.LooseProps?.Count ?? 0) > 0)
                        ?.Select(r => r.LooseProps
                            ?.Where(l => l?.AssetSpec?.Id != 0)
                            ?.Select(l => l.AssetSpec.Id))))
                ?.Distinct()
                ?.ToList();

            var iQuery = Current.Assets.Values
                .Select(a => a.AssetSpec.Id)
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
            }
        }
    }

    public AssetDesc GetAsset(IUserSessionState sessionState, AssetSpec assetSpec, bool downloadAsset = false)
    {
        var assetID = assetSpec.Id;

        using (var @lock = LockContext.GetLock(Assets))
        {
            if (Current.Assets.TryGetValue(assetID, out var value))
            {
                return value;
            }

            if (downloadAsset)
                sessionState.Send(
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