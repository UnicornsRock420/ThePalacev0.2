﻿namespace Lib.Common.Enums.App;

public enum ThreadQueues : short
{
    Core,
    Network,
    GUI,
    Audio,
    Media,
    Assets,
    Scripting,
    Plugins,
    Toast, // Requires: WINDOWS10_0_17763_0_OR_GREATER
}