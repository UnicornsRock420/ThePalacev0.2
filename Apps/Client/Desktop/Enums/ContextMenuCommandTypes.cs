using System.ComponentModel;

namespace ThePalace.Client.Desktop.Enums;

public enum ContextMenuCommandTypes
{
    NONE,
    [Description("pin")] CMD_PIN,
    [Description("unpin")] CMD_UNPIN,
    [Description("gag")] CMD_GAG,
    [Description("ungag")] CMD_UNGAG,
    [Description("propgag")] CMD_PROPGAG,
    [Description("unpropgag")] CMD_UNPROPGAG,
    [Description("kill")] CMD_KILLUSER,
    CMD_USERMOVE,
    CMD_PROPDEL,
    CMD_SPOTDEL,
    UI_PROPSELECT,
    UI_SPOTSELECT,
    UI_USERSELECT
}