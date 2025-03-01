using System;

namespace ThePalace.Client.Desktop.Entities;

public partial class HistoryRecord
{
    public DateTime Created { get; private set; }
    public string Title;
    public Uri Url { get; private set; } = null;

    private HistoryRecord()
    {
        Created = DateTime.UtcNow;
    }
    public HistoryRecord(string url) : this()
    {
        Url = new Uri(url);
    }
    public HistoryRecord(string title, string url) : this(url)
    {
        Title = title;
    }
}