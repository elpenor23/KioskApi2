namespace KioskApi2.Clothing;

public class ClothingItem
{
	[ConfigurationKeyName("id")]
	public required string Id { get; set; }

	[ConfigurationKeyName("min_temp")]
	public double MinTemp { get; set; }

	[ConfigurationKeyName("max_temp")]
	public double MaxTemp { get; set; }

	[ConfigurationKeyName("title")]
	public required string Title { get; set; }

	[ConfigurationKeyName("special")]
	public string? Special { get; set; }

	[ConfigurationKeyName("conditions")]
	public required string Conditions { get; set; }
}