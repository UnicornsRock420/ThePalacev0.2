using System;
using System.Collections.Generic;

namespace ThePalace.Client.Desktop.Entities
{
#if WINDOWS10_0_17763_0_OR_GREATER
    public partial class ToastCfg
    {
        public DateTimeOffset ExpirationTime;
        public IReadOnlyDictionary<string, object> Args;
        public IReadOnlyList<string> Text;
    }
#endif
}
