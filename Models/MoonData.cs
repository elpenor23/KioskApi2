namespace KioskApi.Models;

public class MoonData
{
    public MoonData(){}
    public MoonData(int index, string name, string icon, string length, DateTime? sunrise, DateTime sunset)
    {
        this.PhaseIndex = index;
        this.PhaseName = name;
        this.PhaseIcon = icon;
        this.DayLength = length;
        this.SunriseTime = sunrise;
        this.SunsetTime = sunset;
    }
    public int? PhaseIndex { get; set; }
    public string? PhaseName { get; set; }
    public string? PhaseIcon { get; set; }
    public string? DayLength { get; set; }
    public DateTime? SunriseTime { get; set; }
    public DateTime? SunsetTime { get; set; }
    public string? DayTimeFormatted
    {
        get
        {
            return SunriseTime?.ToString("h:mm") + " - " + SunsetTime?.ToString("h:mm");
        }
    } 
}