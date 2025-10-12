

namespace KioskApi2.Solar
{
    public interface ISolarManager
    {
        Task<SolarData?> GetSolarData();
    }
}