using ThePalace.Core.Factories.Types;

namespace ThePalace.Core.Factories.Threading
{
    public partial class CancellationTokenFactory : IDisposable
    {
        static CancellationTokenFactory()
        {
            _globalToken = new();
        }

        public CancellationTokenFactory()
        {
            _subTokens = new();
        }

        ~CancellationTokenFactory() => Dispose();

        public void Dispose()
        {
            if (_globalToken.IsCancellationRequested == false)
            {
                Shutdown();

                Thread.Sleep(1500);
            }

            _globalToken.Dispose();
        }

        private static readonly CancellationTokenSource _globalToken;
        private Root<Guid, CancellationTokenSource> _subTokens;

        public CancellationToken GlobalToken => _globalToken.Token;

        public void Shutdown()
        {
            try { _globalToken.Cancel(); } catch { }
        }

        public static CancellationTokenSource NewToken() => new();

        public static void NewLinkedToken(out CancellationTokenSource newToken, out CancellationTokenSource linkedToken, params CancellationToken[] tokens)
        {
            newToken = new();

            var _tokens = new List<CancellationToken>(tokens);
            _tokens.Add(_globalToken.Token);
            _tokens.Add(newToken.Token);

            linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_tokens.ToArray());
        }

        public static CancellationTokenSource LinkTokens(List<CancellationToken> tokens)
        {
            tokens.Add(_globalToken.Token);

            return CancellationTokenSource.CreateLinkedTokenSource(tokens.ToArray());
        }

        public static CancellationTokenSource LinkTokens(params CancellationToken[] tokens)
        {
            var _tokens = new List<CancellationToken>(tokens);
            _tokens.Add(_globalToken.Token);

            return CancellationTokenSource.CreateLinkedTokenSource(_tokens.ToArray());
        }

        public static bool CascadingCancel(Guid id)
        {
            return false;
        }
    }
}