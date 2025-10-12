
using System.Text.Json.Serialization;

namespace KioskApi2.Solar;

public class SolarData
{
    [JsonPropertyName("overview")]
    public required Overview Overview { get; set; }
}
public class CurrentPower
{
    [JsonPropertyName("power")]
    public double Power { get; set; }
}

public class LastDayData
{
    [JsonPropertyName("energy")]
    public double Energy { get; set; }
}

public class LastMonthData
{
    [JsonPropertyName("energy")]
    public double Energy { get; set; }
}

public class LastYearData
{
    [JsonPropertyName("energy")]
    public double Energy { get; set; }
}

public class LifeTimeData
{
    [JsonPropertyName("energy")]
    public double Energy { get; set; }

    [JsonPropertyName("revenue")]
    public double Revenue { get; set; }
}

public class Overview
{
    [JsonPropertyName("lastUpdateTime")]
    public required string LastUpdateTime { get; set; }

    [JsonPropertyName("lifeTimeData")]
    public required LifeTimeData LifeTimeData { get; set; }

    [JsonPropertyName("lastYearData")]
    public required LastYearData LastYearData { get; set; }

    [JsonPropertyName("lastMonthData")]
    public required LastMonthData LastMonthData { get; set; }

    [JsonPropertyName("lastDayData")]
    public required LastDayData LastDayData { get; set; }

    [JsonPropertyName("currentPower")]
    public required CurrentPower CurrentPower { get; set; }

    [JsonPropertyName("measuredBy")]
    public required string MeasuredBy { get; set; }
}