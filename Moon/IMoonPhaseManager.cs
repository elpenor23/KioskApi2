

namespace KioskApi2.Moon
{
	public interface IMoonPhaseManager
	{
		Task<MoonData> GetMoonPhase(string lat, string lon);
	}
}