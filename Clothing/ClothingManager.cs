
using KioskApi2.Weather;

using Microsoft.Extensions.Options;

namespace KioskApi2.Clothing;
public class ClothingManager(IOptions<PersonOptions> optPerson, IOptions<Clothing> optClothing, IOptions<Adjustments> optAdjustments, Serilog.ILogger logger, IWeatherManager weatherManager) : IClothingManager
{
	private readonly PersonOptions _personOptions = optPerson.Value;
	private readonly Clothing _clothingOptions = optClothing.Value;
	private readonly Adjustments _adjustmentOptions = optAdjustments.Value;
	public async Task<IEnumerable<PersonsClothing>> GetCalculatedClothing(
		string feels,
		string ids,
		string names,
		string colors,
		string lat,
		string lon)
	{

		logger.Debug("Calculating Clothing!");

		string[] arrayFeels = feels.Split(",");
		string[] arrayIds = ids.Split(",");
		string[] arrayName = names.Split(",");
		string[] arrayColor = colors.Split(",");

		if (!AreAllTheSameLength(arrayFeels, arrayIds, arrayName, arrayColor))
		{
			//Input arrays are screwy bail
			throw new Exception("Input Arrays DO NOT MATCH!");
		}

		var people = CreatePeople(arrayFeels, arrayIds, arrayName, arrayColor);
		var weather = await weatherManager.GetWeather(lat, lon);


		var list = CalculateClothing(people, weather, _personOptions.Intensities);

		return list;
	}

	private List<PersonsClothing> CalculateClothing(List<Person> people, WeatherItem weather, List<string> intensities)
	{
		var calculatedClothing = new List<PersonsClothing>();

		foreach (var person in people)
		{
			var pc = CalculateIntensities(person, weather, intensities);
			calculatedClothing.Add(pc);
		}

		return calculatedClothing;
	}
	private PersonsClothing CalculateIntensities(Person person, WeatherItem weather, List<string> intensities)
	{
		var pc = new PersonsClothing(person);

		foreach (var intense in intensities)
		{
			if (!Enum.TryParse<Enums.Intensity>(intense, true, out var intensity))
				intensity = Enums.Intensity.Normal;

			person.Feel ??= Enums.Feel.None;

			var adjustedTemp = AdjustTemperature(weather, intensity, person.Feel.Value);

			var clothing = GetClothingForAdjustedTemp(adjustedTemp, intensity, weather.CurrentMain ?? "No Data", weather.TimeOfDay);

			pc.Intensity.Add(clothing);
		}
		return pc;
	}

	private IntensityClothing GetClothingForAdjustedTemp(double adjustedTemp, Enums.Intensity intensity, string conditions, Enums.TimeOfDay timeOfDay)
	{

		var finalList = new IntensityClothing(intensity);

		FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, _clothingOptions.Head, ref finalList);
		FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, _clothingOptions.InnerTorso, ref finalList);
		FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, _clothingOptions.OuterTorso, ref finalList);
		FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, _clothingOptions.Legs, ref finalList);
		FindCorrectItemForBodyPart(adjustedTemp, intensity, conditions, timeOfDay, _clothingOptions.Hands, ref finalList);

		return finalList;
	}

	private static void FindCorrectItemForBodyPart(double adjustedTemp, Enums.Intensity intensity, string conditions, Enums.TimeOfDay timeOfDay, List<ClothingItem> clothingList, ref IntensityClothing finalList)
	{
		foreach (var item in clothingList)
		{
			if (item.MinTemp <= adjustedTemp && adjustedTemp <= item.MaxTemp)
			{
				//check if we need to eliminate this sucker for any reason
				if (
					(item.Conditions != null && !item.Conditions.Contains(conditions)) ||
					(item.Special != null && RemoveForSpecialConditions(item.Special, conditions, timeOfDay, intensity)))
				{
					continue;
				}

				finalList.Clothes.Add(item);
				break;
			}
		}
	}

	private static bool RemoveForSpecialConditions(string specialCondition, string conditions, Enums.TimeOfDay timeOfDay, Enums.Intensity intensity)
	{

		if (specialCondition == "sunny")
		{
			//bail if it is not a sunny day
			if (!conditions.Contains("Clear") || timeOfDay != Enums.TimeOfDay.Day)
			{
				return false;
			}
		}

		if (specialCondition == "not_rain")
		{
			//bail if it is raining
			if (conditions.Contains("Rain"))
			{
				return false;
			}
		}

		if (specialCondition == "race")
		{
			//bail if it is not a race
			if (intensity != Enums.Intensity.Race)
			{
				return false;
			}
		}
		return false;
	}
	private double AdjustTemperature(WeatherItem weather, Enums.Intensity intensity, Enums.Feel feel)
	{

		if (weather.CurrentTemp == null)
		{
			return 0;
		}

		double timeOfDayAndConditionsAdjustment = TimeOfDayAndPrecipitationTempAdjustment(weather);
		double windAdjustment = WindTemperatureAdjustment(weather);
		double intensityAdjustment = IntensityAdjustment(intensity);
		double feelAdjustment = FeelAdjustment(feel);

		double finalAdjustment = weather.CurrentTemp.Value +
								timeOfDayAndConditionsAdjustment +
								windAdjustment +
								intensityAdjustment +
								feelAdjustment;

		return finalAdjustment;
	}

	private double FeelAdjustment(Enums.Feel feel)
	{
		double feelAdjustment = 0;

		switch (feel)
		{
			case Enums.Feel.Cool:
				feelAdjustment = _adjustmentOptions.Feel.Cool;
				break;
			case Enums.Feel.Warm:
				feelAdjustment = _adjustmentOptions.Feel.Warm;
				break;
		}

		return feelAdjustment;
	}
	private double IntensityAdjustment(Enums.Intensity intensity)
	{
		double intensityAdjustment = 0;

		switch (intensity)
		{
			case Enums.Intensity.Race:
				intensityAdjustment = _adjustmentOptions.Intensity.Race;
				break;
			case Enums.Intensity.Workout:
				intensityAdjustment = _adjustmentOptions.Intensity.HardWorkout;
				break;
			case Enums.Intensity.Long:
				intensityAdjustment = _adjustmentOptions.Intensity.LongRun;
				break;
		}

		return intensityAdjustment;
	}
	private double WindTemperatureAdjustment(WeatherItem weather)
	{
		double windAdjustment = 0;

		if (weather.CurrentWindSpeed == null) { return 0; }

		Enums.WindType windType = GetWindType((int)weather.CurrentWindSpeed.Value);

		switch (windType)
		{
			case Enums.WindType.Light:
				windAdjustment = _adjustmentOptions.Wind.LightWind;
				break;
			case Enums.WindType.Medium:
				windAdjustment = _adjustmentOptions.Wind.Windy;
				break;
			case Enums.WindType.Heavy:
				windAdjustment = _adjustmentOptions.Wind.HeavyWind;
				break;
		}

		return windAdjustment;
	}

	private Enums.WindType GetWindType(int windSpeed)
	{
		var wind = _adjustmentOptions.WindSpeed;

		if (wind.LightMin <= windSpeed && windSpeed <= wind.LightMax)
			return Enums.WindType.Light;

		if (wind.WindMin <= windSpeed && windSpeed <= wind.WindMax)
			return Enums.WindType.Medium;

		if (wind.HeavyMin <= windSpeed && windSpeed <= wind.HeavyMax)
			return Enums.WindType.Heavy;

		return Enums.WindType.None;
	}
	private double TimeOfDayAndPrecipitationTempAdjustment(WeatherItem weather)
	{
		double timeOfDayAndConditionsAdjustment = 0;
		switch (weather.TimeOfDay)
		{
			case Enums.TimeOfDay.Day:
				switch (weather.CurrentMain)
				{
					case "Clear":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.ClearDay;
						break;
					case "Clouds":
					case "Mist":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.PartiallyCloudyDay;
						break;
				}
				break;
			case Enums.TimeOfDay.Dawn:
			case Enums.TimeOfDay.Dusk:
				switch (weather.CurrentMain)
				{
					case "Clear":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.ClearDuskDawn;
						break;
					case "Clouds":
					case "Mist":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.PartiallyCloudyDuskDawn;
						break;
				}
				break;
			default:
				switch (weather.CurrentMain)
				{
					case "Rain":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.Rain;
						break;
					case "Drizzle":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.LightRain;
						break;
					case "Snow":
						timeOfDayAndConditionsAdjustment += _adjustmentOptions.TimeofdayPrecipitation.Snow;
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
			if (!Enum.TryParse<Enums.Feel>(arrayFeels[i], true, out var feel))
			{
				feel = Enums.Feel.None;
			}

			var person = new Person
			{
				Id = arrayIds[i],
				Feel = feel,
				Name = arrayName[i],
				Color = arrayColor[i]
			};
			people.Add(person);
		}

		return people;
	}

	public List<string> GetBodyParts()
	{
		return _personOptions.BodyParts;
	}

	public Clothing GetClothing()
	{
		return _clothingOptions;
	}

	private static bool AreAllTheSameLength(params Array[] arrays)
	{
		return arrays.All(a => a.Length == arrays[0].Length);
	}
}