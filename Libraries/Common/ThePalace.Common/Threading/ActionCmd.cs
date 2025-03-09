﻿using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Common.Threading;

public class ActionCmd : ICmd, IDisposable
{
    public CmdFnc CmdFnc { get; set; }
    public object[] Values { get; set; }
    public uint Flags { get; set; } = 0;

    public void Dispose()
    {
        CmdFnc = null;
        Values = null;

        GC.SuppressFinalize(this);
    }
}