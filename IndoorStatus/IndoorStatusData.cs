namespace KioskApi2.IndoorStatus;

public class IndoorStatusData
{
    public string? Id { get; } = "1";
    public string? Data { get; set; } = "XX";
    public DateTime? LastSet { get; set; } = DateTime.Now;
    public string LastSetString
    {
        get
        {
            return (LastSet ?? DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}