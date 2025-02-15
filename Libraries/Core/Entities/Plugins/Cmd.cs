using System;

namespace ThePalace.Core.Entities.Plugins
{
    public partial class Cmd : IDisposable
    {
        public CmdFnc CmdFnc;
        public object[] Values;

        public void Dispose()
        {
            CmdFnc = null;
            Values = null;
        }
    }
}
