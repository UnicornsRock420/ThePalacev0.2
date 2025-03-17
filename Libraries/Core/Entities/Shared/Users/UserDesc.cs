using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Lib.Common.Factories.Core;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Users;

public partial class UserDesc : IDisposable, IStruct
{
    [IgnoreDataMember] public ConcurrentDictionary<string, object> Extended;

    public UserRec UserRec;

    public UserDesc()
    {
        UserRec = new UserRec();
        Extended = new ConcurrentDictionary<string, object>();
    }

    public void Dispose()
    {
        UserRec = null;

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