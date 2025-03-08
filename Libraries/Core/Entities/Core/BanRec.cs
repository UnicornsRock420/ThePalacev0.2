namespace ThePalace.Core.Database.Core.Model;

public class BanRec
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public string? IPAddress { get; set; }
    public int? RegCtr { get; set; }
    public int? RegCrc { get; set; }
    public int? Puidctr { get; set; }
    public int? Puidcrc { get; set; }
    public string? Note { get; set; }
}