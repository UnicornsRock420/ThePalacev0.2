namespace ThePalace.Core.Factories.Threading
{
    public static class CancellationTokenFactory
    {
        public static CancellationTokenSource NewToken() => new();

        public static void NewLinkedToken(out CancellationTokenSource newToken, out CancellationTokenSource linkedToken, params CancellationToken[] tokens)
        {
            newToken = new();

            var _tokens = new List<CancellationToken>(tokens);
            _tokens.Add(TaskManager.GlobalToken);
            _tokens.Add(newToken.Token);

            linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_tokens.ToArray());
        }

        public static CancellationTokenSource LinkTokens(List<CancellationToken> tokens)
        {
            tokens.Add(TaskManager.GlobalToken);

            return CancellationTokenSource.CreateLinkedTokenSource(tokens.ToArray());
        }

        public static CancellationTokenSource LinkTokens(params CancellationToken[] tokens)
        {
            var _tokens = new List<CancellationToken>(tokens);
            _tokens.Add(TaskManager.GlobalToken);

            return CancellationTokenSource.CreateLinkedTokenSource(_tokens.ToArray());
        }

        public static bool CascadingCancel(Guid id)
        {
            return false;
        }
    }
}