namespace KioskApi2.HttpClients.Models;

public class ApiOptions
{
	public required string URL { get; set; }
	public string QueryString { get; set; } = string.Empty;
	public HeaderData? HeaderData { get; set; }
}

public class HeaderData
{
	public required string AuthToken { get; set; }
}