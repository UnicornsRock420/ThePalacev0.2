using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.UI;

namespace ThePalace.Client.Desktop.Factories;

public class HistoryManager : SingletonDisposable<HistoryManager>
{
    private ConcurrentDictionary<DateTime, HistoryRecord> _history = new();

    public DateTime? Position;
    public IReadOnlyDictionary<DateTime, HistoryRecord> History => _history.AsReadOnly();

    ~HistoryManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        _history?.Clear();
        _history = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public void RegisterHistory(string title, string url)
    {
        lock (_history)
        {
            if (Position.HasValue &&
                History[Position.Value].Url.AbsoluteUri == url) return;

            if (Position.HasValue)
            {
                var keys = _history.Keys
                    .Where(k => k > Position.Value)
                    .ToList();
                foreach (var key in keys)
                    _history.TryRemove(key, out _);

                Position = null;
            }

            if (_history.Count >= 20)
            {
                var oldestKey = _history.Keys.Min();
                _history.TryRemove(oldestKey, out _);
            }

            Position = DateTime.Now;

            _history.TryAdd(Position.Value, new HistoryRecord(title, url));
        }
    }

    public string Back()
    {
        if (_history.Count == 0) return null;

        lock (_history)
        {
            if (!Position.HasValue)
                Position = _history.Keys.Max();

            var results = _history
                .Where(h => h.Key < Position.Value)
                .OrderByDescending(h => h.Key)
                .Take(1)
                .ToList();

            if (results.Count < 1) return null;

            var result = results.FirstOrDefault();
            Position = result.Key;

            return result.Value.Url.AbsoluteUri;
        }
    }

    public string Forward()
    {
        if (_history.Count == 0) return null;

        lock (_history)
        {
            if (!Position.HasValue)
            {
                Position = _history.Keys.Max();

                return null;
            }

            var results = _history
                .Where(h => h.Key > Position.Value)
                .OrderBy(h => h.Key)
                .Take(1)
                .ToList();

            if (results.Count < 1) return null;

            var result = results.FirstOrDefault();
            Position = result.Key;

            return result.Value.Url.AbsoluteUri;
        }
    }
}