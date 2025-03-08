namespace ThePalace.Client.Desktop.Entities.UI;

public class HistoryRecord
{
    public string Title;

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

    public DateTime Created { get; private set; }
    public Uri Url { get; private set; }
}