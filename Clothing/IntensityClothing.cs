namespace KioskApi2.Clothing;

public class IntensityClothing(Enums.Intensity intensity)
{
	public string IntensityString
	{
		get
		{
			return this.Intensity.ToString();
		}
	}
	public Enums.Intensity Intensity { get; set; } = intensity;
	public List<ClothingItem> Clothes { get; set; } = [];

}