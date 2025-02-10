using System.Collections.Concurrent;
using System.Runtime.Serialization;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared
{
    public partial class UserDesc : IDisposable, IStruct
    {
        public UserDesc()
        {
            this.UserInfo = new();
            this.Extended = new();
        }

        ~UserDesc() => this.Dispose();

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

        public UserRec UserInfo;

        [IgnoreDataMember]
        public ConcurrentDictionary<string, object> Extended;
    }
}