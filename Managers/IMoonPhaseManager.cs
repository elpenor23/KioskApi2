using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface IMoonPhaseManager
    {
        Task<MoonData> GetMoonPhase(string lat, string lon);
    }
}