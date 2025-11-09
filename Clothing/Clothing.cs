using System.Text.Json.Serialization;

namespace KioskApi2.Clothing;

public class Clothing
{
	public static readonly string Location = "Clothing";

	[JsonPropertyName("Head")]
	public required List<ClothingItem> Head { get; set; }

	[JsonPropertyName("OuterTorso")]
	public required List<ClothingItem> OuterTorso { get; set; }

	[JsonPropertyName("InnerTorso")]
	public required List<ClothingItem> InnerTorso { get; set; }

	[JsonPropertyName("Legs")]
	public required List<ClothingItem> Legs { get; set; }

	[JsonPropertyName("Hands")]
	public required List<ClothingItem> Hands { get; set; }
}