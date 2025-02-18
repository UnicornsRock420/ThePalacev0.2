namespace ThePalace.Common.Entities.Plugins
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