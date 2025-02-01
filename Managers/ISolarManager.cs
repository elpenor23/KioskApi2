using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface ISolarManager
    {
        Task<SolarData> GetSolarData();
    }
}