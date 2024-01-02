using KioskApi2.Models;
using KioskApi2.Enums;
using KioskApi2.Utilities;

namespace KioskApi2.Managers;
public class ClothingManager(IConfiguration configuration)
{
    private readonly IConfiguration Configuration = configuration;
    private readonly WeatherManager weatherManager = new(configuration);

    public async Task<IEnumerable<PersonsClothing>> GetCalcuatedClothing(
        string feels,
        string ids,
        string names,
        string colors,
        string lat,
        string lon)
    {

        string[] arrayFeels = feels.Split(",");
        string[] arrayIds = ids.Split(",");
        string[] arrayName = names.Split(",");
        string[] arrayColor = colors.Split(",");

        if (!Arrays.AreAllTheSameLength(arrayFeels, arrayIds, arrayName, arrayColor))
        {
            //Input arrays are screwy bail
            throw new Exception("Input Arrays DO NOT MATCH!");
        }

        var people = CreatePeople(arrayFeels, arrayIds, arrayName, arrayColor);
        var weather = await weatherManager.GetWeather(lat, lon);
        var intensities = Configuration.GetSection("Intensities")?.GetChildren()?.Select(x => x.Value)?.ToList() ?? [];

        if (intensities == null) { return new List<PersonsClothing>(); }

        var list = await CalculateClothing(people, weather, intensities);

        return list;
    }

    private async Task<List<PersonsClothing>> CalculateClothing(List<Person> people, WeatherItem weather, List<string?> intensities)
    {
        var calculatedClothing = new List<PersonsClothing>();

        if (weather == null || weather.CurrentMain == null) { return calculatedClothing; }

        foreach (var person in people)
        {
            var pc = await CalculateIntensities(person, weather, intensities);
            calculatedClothing.Add(pc);
        }

        return calculatedClothing;
    }
    private async Task<PersonsClothing> CalculateIntensities(Person person, WeatherItem weather, List<string?> intensities)
    {
        var pc = new PersonsClothing(person);
        foreach (var intense in intensities)
        {
            var intensity = Enums.Enums.ConvertStringToIntensity(intense);
            person.Feel ??= Feel.None;

            var adjustedTemp = AdjustTemperature(weather, intensity, person.Feel.Value);

            var clothing = await GetClothingForAdjustedTemp(adjustedTemp, intensity, weather.CurrentMain ?? "No Data", weather.TimeOfDay);

            pc.Intensity.Add(clothing);
        }
        return pc;
    }

    private async Task<IntensityClothing> GetClothingForAdjustedTemp(decimal adjustedTemp, Intensity intensity, string conditions, TimeOfDay timeOfDay)
    {

        var finalList = new IntensityClothing(intensity);

        var bodyParts = await GetBodyParts();

        foreach (var bp in bodyParts)
        {
            //no data then bail
            if (bp == null || bp.Id == null) { continue; }

            var clothingList = GetClothingList(bp.Id).Clothing;
            FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, clothingList, ref finalList);
        }

        return finalList;
    }

    private static void FindCorrectItemForBodyPart(decimal adjustedTemp, Intensity intensity, string conditions, TimeOfDay timeOfDay, List<ClothingItem> clothingList, ref IntensityClothing finalList)
    {
        foreach (var item in clothingList)
        {
            //if we don't have the data we need bail
            if (item == null || item.MinTemp == null || item.MaxTemp == null) { continue; }

            if (item.MinTemp.Value <= adjustedTemp && adjustedTemp <= item.MaxTemp.Value)
            {
                var addItem = true;

                //check if we need to eliminate this sucker for any reason
                if (
                    (item.Conditions != null && !item.Conditions.Contains(conditions)) ||
                    (item.Special != null && RemoveForSpecialConditions(item.Special, conditions, timeOfDay, intensity)))
                {
                    addItem = false;
                    continue;
                }

                if (addItem)
                {
                    finalList.Clothes.Add(item);
                    break;
                }

            }
        }
    }

    private static bool RemoveForSpecialConditions(string specialCondition, string conditions, TimeOfDay timeOfDay, Intensity intensity)
    {

        if (specialCondition == "sunny")
        {
            //bail if it is not a sunny day
            if (conditions == null || !conditions.Contains("Clear") || timeOfDay != TimeOfDay.Day)
            {
                return false;
            }
        }

        if (specialCondition == "not_rain")
        {
            //bail if it is raining
            if (conditions == null || conditions.Contains("Rain"))
            {
                return false;
            }
        }

        if (specialCondition == "race")
        {
            //bail if it is not a race
            if (intensity != Intensity.Race)
            {
                return false;
            }
        }
        return false;
    }
    private decimal AdjustTemperature(WeatherItem weather, Intensity intensity, Feel feel)
    {

        if (weather.CurrentTemp == null)
        {
            return 0;
        }

        decimal timeOfDayAndConditionsAdjustment = TimeOfDayAndPrecipitationTempAdustment(weather);
        decimal windAdjustment = WindTemperatureAdjustment(weather);
        decimal intensityAdjustment = IntensityAdjustment(intensity);
        decimal feelAdjustment = FeelAdjustment(feel);

        decimal finalAdjustment = weather.CurrentTemp.Value +
                                timeOfDayAndConditionsAdjustment +
                                windAdjustment +
                                intensityAdjustment +
                                feelAdjustment;

        return finalAdjustment;
    }

    private decimal FeelAdjustment(Feel feel)
    {
        decimal feelAdjustment = 0;

        switch (feel)
        {
            case Feel.Cool:
                feelAdjustment = decimal.Parse(Configuration["Adjustments:feel:cool"] ?? "0");
                break;
            case Feel.Warm:
                feelAdjustment = decimal.Parse(Configuration["Adjustments:feel:warm"] ?? "0");
                break;
        }

        return feelAdjustment;
    }
    private decimal IntensityAdjustment(Intensity intensity)
    {
        decimal intensityAdjustment = 0;

        switch (intensity)
        {
            case Intensity.Race:
                intensityAdjustment = decimal.Parse(Configuration["Adjustments:intensity:race"] ?? "0");
                break;
            case Intensity.Workout:
                intensityAdjustment = decimal.Parse(Configuration["Adjustments:intensity:hard_workout"] ?? "0");
                break;
            case Intensity.Long:
                intensityAdjustment = decimal.Parse(Configuration["Adjustments:intensity:long_run"] ?? "0");
                break;
        }

        return intensityAdjustment;
    }
    private decimal WindTemperatureAdjustment(WeatherItem weather)
    {
        decimal windAdjustment = 0;

        if (weather.CurrentWindSpeed == null) { return 0; }

        WindType windType = GetWindType(weather.CurrentWindSpeed.Value);

        switch (windType)
        {
            case WindType.Light:
                windAdjustment = decimal.Parse(Configuration["Adjustments:wind:light_wind"] ?? "0");
                break;
            case WindType.Medium:
                windAdjustment = decimal.Parse(Configuration["Adjustments:wind:windy"] ?? "0");
                break;
            case WindType.Heavy:
                windAdjustment = decimal.Parse(Configuration["Adjustments:wind:heavy_wind"] ?? "0");
                break;
        }

        return windAdjustment;
    }

    private WindType GetWindType(int windSpeed)
    {
        WindType windType = WindType.None;

        //GET CONFIGS
        decimal lightMin = decimal.Parse(Configuration["Adjustments:wind_speed:light_min"] ?? "0");
        decimal lightMax = decimal.Parse(Configuration["Adjustments:wind_speed:light_max"] ?? "0");
        decimal medMin = decimal.Parse(Configuration["Adjustments:wind_speed:wind_min"] ?? "0");
        decimal medMax = decimal.Parse(Configuration["Adjustments:wind_speed:wind_max"] ?? "0");
        decimal heavyMin = decimal.Parse(Configuration["Adjustments:wind_speed:heavy_min"] ?? "0");
        decimal heavyMax = decimal.Parse(Configuration["Adjustments:wind_speed:heavy_max"] ?? "0");

        if (lightMin <= windSpeed && windSpeed <= lightMax)
        {
            windType = WindType.Light;
        }
        else if (medMin <= windSpeed && windSpeed <= medMax)
        {
            windType = WindType.Medium;
        }
        else if (heavyMin <= windSpeed && windSpeed <= heavyMax)
        {
            windType = WindType.Heavy;
        }

        return windType;
    }
    private decimal TimeOfDayAndPrecipitationTempAdustment(WeatherItem weather)
    {
        decimal timeOfDayAndConditionsAdjustment = 0;
        switch (weather.TimeOfDay)
        {
            case TimeOfDay.Day:
                switch (weather.CurrentMain)
                {
                    case "Clear":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:clear_day"] ?? "0");
                        break;
                    case "Clouds":
                    case "Mist":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:partially_cloudy_day"] ?? "0");
                        break;
                }
                break;
            case TimeOfDay.Dawn:
            case TimeOfDay.Dusk:
                switch (weather.CurrentMain)
                {
                    case "Clear":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:clear_dusk_dawn"] ?? "0");
                        break;
                    case "Clouds":
                    case "Mist":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:partially_cloudy_dusk_dawn"] ?? "0");
                        break;
                }
                break;
            default:
                switch (weather.CurrentMain)
                {
                    case "Rain":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:rain"] ?? "0");
                        break;
                    case "Drizzle":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:light_rain"] ?? "0");
                        break;
                    case "Snow":
                        timeOfDayAndConditionsAdjustment += decimal.Parse(Configuration["Adjustments:timeofday_precipitation:snow"] ?? "0");
                        break;
                }
                break;
        }

        return timeOfDayAndConditionsAdjustment;
    }
    private static List<Person> CreatePeople(string[] arrayFeels, string[] arrayIds, string[] arrayName, string[] arrayColor)
    {
        var people = new List<Person>();
        for (int i = 0; i < arrayFeels.Length; i++)
        {
            var person = new Person
            {
                Id = arrayIds[i],
                Feel = Enums.Enums.ConvertStringToFeel(arrayFeels[i]),
                Name = arrayName[i],
                Color = arrayColor[i]
            };
            people.Add(person);
        }

        return people;
    }

    public async Task<IEnumerable<BodyPart>> GetBodyParts()
    {
        var bodyParts = new List<BodyPart>();

        if (bodyParts.Count == 0)
        {
            bodyParts = await Task.Run(() => DefaultBodyParts());
        }

        return bodyParts;
    }

    public async Task<List<ClothingList>> GetClothing()
    {
        var fullList = new List<ClothingList>();
        if (fullList.Count == 0)
        {
            fullList = await this.DefaultClothing();
        }

        return fullList;
    }

    private List<BodyPart> DefaultBodyParts()
    {
        var bodyParts = new List<BodyPart>();

        var defaultbodyPartsList = Configuration.GetSection("BodyParts")?.GetChildren()?.Select(x => x.Value)?.ToList();

        if (defaultbodyPartsList == null) { return bodyParts; }

        foreach (var id in defaultbodyPartsList)
        {
            var bp = new BodyPart();
            bp.Id = id;

            bodyParts.Add(bp);
        }

        return bodyParts;
    }
    private async Task<List<ClothingList>> DefaultClothing()
    {
        var bodyParts = await GetBodyParts();
        var fullList = new List<ClothingList>();

        foreach (var id in bodyParts)
        {
            if (id == null || id.Id == null) { continue; }

            var clothingItemList = GetClothingList(id.Id);
            fullList.Add(clothingItemList);
        }

        return fullList;
    }

    private ClothingList GetClothingList(string bodyPart)
    {
        var cl = new ClothingList
        {
            Id = bodyPart
        };

        var key = string.Format("Clothing:{0}", bodyPart);
        var clothingItemSubList = Configuration.GetSection(key)?.GetChildren()?.Select(x => x)?.ToList();

        if (clothingItemSubList == null) { return cl; }

        foreach (var item in clothingItemSubList)
        {
            var ci = new ClothingItem
            {
                Id = item["id"],
                MinTemp = decimal.Parse(item["min_temp"] ?? "0"),
                MaxTemp = decimal.Parse(item["max_temp"] ?? "100"),
                Title = item["title"],
                Special = item["special"]
            };
            cl.Clothing.Add(ci);
        }

        return cl;
    }
}