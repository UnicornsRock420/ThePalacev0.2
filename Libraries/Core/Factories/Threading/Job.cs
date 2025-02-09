namespace ThePalace.Core.Factories.Threading
{
    public partial class Job : IDisposable
    {
        private Job()
        {
            _token = new();
            Counter = 0;
            Failures = 0;
        }

        public Job(Action? cmd = null) : this()
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            _cmd = cmd;
        }

        ~Job() => this.Dispose();

        public void Dispose() { }

        private readonly Action? _cmd;

        protected readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;

        public int Counter { get; private set; }
        public int Failures { get; private set; }

        public object JobState { get; set; }

        public int Run()
        {
            Counter++;

            try
            {
                _cmd();

                return 0;
            }
            catch (Exception ex)
            {
                Failures++;

                return -1;
            }
        }

        public void Cancel() => _token.Cancel();
    }
}