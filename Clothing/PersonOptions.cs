
namespace KioskApi2.Clothing;

public class PersonOptions
{
	public static readonly string Location = "PersonOptions";
	public required List<string> BodyParts { get; set; }
	public required List<string> Intensities { get; set; }
}