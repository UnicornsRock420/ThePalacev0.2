using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace ThePalace.Core.Entities.Shared
{
    public partial class UserRec
    {
        [IgnoreDataMember]
        public ConcurrentDictionary<string, object> Extended;
    }
}