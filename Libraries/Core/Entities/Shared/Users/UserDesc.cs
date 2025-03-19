using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Lib.Common.Factories.Core;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Users;

public partial class UserDesc : UserRec, IDisposable, IStruct
{
    [IgnoreDataMember] public ConcurrentDictionary<string, object> Extended;

    public UserDesc()
    {
        Extended = new ConcurrentDictionary<string, object>();
    }

    public void Dispose()
    {
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