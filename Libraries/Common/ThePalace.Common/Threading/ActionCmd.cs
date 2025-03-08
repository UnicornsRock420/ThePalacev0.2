using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Common.Threading;

public class ActionCmd : ICmd, IDisposable
{
    public CmdFnc CmdFnc;
    public object[] Values;

    public void Dispose()
    {
        CmdFnc = null;
        Values = null;

        GC.SuppressFinalize(this);
    }
}