namespace ThePalace.Client.Desktop.Enums
{
    public enum ThreadQueues : short
    {
        GUI,
        Audio,
        Core,
        //Devices,
        Media,
        //Assets,
        Network_Receive,
        Network_Send,
        ScriptEngine,
#if WINDOWS10_0_17763_0_OR_GREATER
        Toast,
#endif
    }
}
