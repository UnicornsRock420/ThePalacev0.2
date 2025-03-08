namespace ThePalace.Common.Exts.System;

public static class TaskExts
{
    //static TaskExts() { }

    public static void Forget(this Task task, int delay = 500)
    {
        Task.Delay(delay).ConfigureAwait(false);
    }

    public static class Types
    {
        public static readonly Type Task = typeof(Task);
    }
}