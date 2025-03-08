using System.Collections.Concurrent;
using System.Runtime.Serialization;
using ThePalace.Common.Factories;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared;

public partial class UserDesc : IDisposable, IStruct
{
    [IgnoreDataMember] public ConcurrentDictionary<string, object> Extended;

    public UserRec UserInfo;

    public UserDesc()
    {
        UserInfo = new UserRec();
        Extended = new ConcurrentDictionary<string, object>();
    }

    public void Dispose()
    {
        UserInfo = null;

        Extended
            ?.Values
            ?.Where(_ => _ is IDisposable)
            ?.Cast<IDisposable>()
            ?.ToList()
            ?.ForEach(_ => TCF
                .Options(false)
                .Try(() => _.Dispose())
                .Execute());
        Extended?.Clear();
        Extended = null;

        GC.SuppressFinalize(this);
    }

    ~UserDesc()
    {
        Dispose();
    }
}