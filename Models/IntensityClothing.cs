using KioskApi2.Enums;

namespace KioskApi2.Models;

public class IntensityClothing
{
    public IntensityClothing(Intensity intensity)
    {
        this.Intensity = intensity;
        this.Clothes = new List<ClothingItem>();
    }

    public string IntensityString
    {
        get
        {
            return this.Intensity.ToString();
        }
    }
    public Intensity Intensity { get; set; }
    public List<ClothingItem> Clothes { get; set; }

}