
namespace KioskApi2.Clothing;

public class Adjustments
{
    public static readonly string Location = "Adjustments";
    
    [ConfigurationKeyName("timeofday_precipitation")]
    public required TimeofdayPrecipitation TimeofdayPrecipitation { get; set; }

    [ConfigurationKeyName("wind")]
    public required Wind Wind { get; set; }

    [ConfigurationKeyName("intensity")]
    public required Intensity Intensity { get; set; }

    [ConfigurationKeyName("feel")]
    public required Feel Feel { get; set; }

    [ConfigurationKeyName("wind_speed")]
    public required WindSpeed WindSpeed { get; set; }

    [ConfigurationKeyName("comfort_data")]
    public required ComfortData ComfortData { get; set; }
}

public class ComfortData
{
    [ConfigurationKeyName("cold_min_temp")]
    public int ColdMinTemp { get; set; }

    [ConfigurationKeyName("comfortable_max_dew_point")]
    public int ComfortableMaxDewPoint { get; set; }

    [ConfigurationKeyName("perfect_temp_min")]
    public int PerfectTempMin { get; set; }

    [ConfigurationKeyName("perfect_temp_max")]
    public int PerfectTempMax { get; set; }

    [ConfigurationKeyName("comfortable_max_temp")]
    public int ComfortableMaxTemp { get; set; }

    [ConfigurationKeyName("sticky_max_dew_point")]
    public int StickyMaxDewPoint { get; set; }

    [ConfigurationKeyName("oppressive_max_dew_point")]
    public int OppressiveMaxDewPoint { get; set; }
}

public class Feel
{
    [ConfigurationKeyName("cool")]
    public int Cool { get; set; }

    [ConfigurationKeyName("warm")]
    public int Warm { get; set; }
}

public class Intensity
{
    [ConfigurationKeyName("race")]
    public int Race { get; set; }

    [ConfigurationKeyName("hard_workout")]
    public int HardWorkout { get; set; }

    [ConfigurationKeyName("long_run")]
    public int LongRun { get; set; }
}

public class TimeofdayPrecipitation
{
    [ConfigurationKeyName("clear_day")]
    public int ClearDay { get; set; }

    [ConfigurationKeyName("partially_cloudy_day")]
    public int PartiallyCloudyDay { get; set; }

    [ConfigurationKeyName("clear_dusk_dawn")]
    public int ClearDuskDawn { get; set; }

    [ConfigurationKeyName("partially_cloudy_dusk_dawn")]
    public int PartiallyCloudyDuskDawn { get; set; }

    [ConfigurationKeyName("light_rain")]
    public int LightRain { get; set; }

    [ConfigurationKeyName("rain")]
    public int Rain { get; set; }

    [ConfigurationKeyName("snow")]
    public int Snow { get; set; }
}

public class Wind
{
    [ConfigurationKeyName("light_wind")]
    public int LightWind { get; set; }

    [ConfigurationKeyName("windy")]
    public int Windy { get; set; }

    [ConfigurationKeyName("heavy_wind")]
    public int HeavyWind { get; set; }
}

public class WindSpeed
{
    [ConfigurationKeyName("light_min")]
    public double LightMin { get; set; }

    [ConfigurationKeyName("light_max")]
    public double LightMax { get; set; }

    [ConfigurationKeyName("wind_min")]
    public double WindMin { get; set; }

    [ConfigurationKeyName("wind_max")]
    public double WindMax { get; set; }

    [ConfigurationKeyName("heavy_min")]
    public double HeavyMin { get; set; }

    [ConfigurationKeyName("heavy_max")]
    public double HeavyMax { get; set; }
}
