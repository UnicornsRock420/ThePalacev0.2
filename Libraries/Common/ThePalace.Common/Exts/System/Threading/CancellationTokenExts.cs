namespace System.Threading;

public static class CancellationTokenExts
{
    //static CancellationTokenExts() { }

    public static void ForceCancel(ref this CancellationToken cancellationToken, Func<bool>? condition = null)
    {
        if (condition?.Invoke() != false)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationToken = cts.Token;
            cts.Cancel();
        }
    }

    public static class Types
    {
        public static readonly Type CancellationToken = typeof(CancellationToken);
        public static readonly Type CancellationTokenList = typeof(List<CancellationToken>);
        public static readonly Type CancellationTokenSource = typeof(CancellationTokenSource);
        public static readonly Type CancellationTokenSourceList = typeof(List<CancellationTokenSource>);
    }
}