using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface IIndoorStatusManager
    {
        Task<IndoorStatusData> GetIndoorStatus();
        Task<IndoorStatusData> SaveIndoorStatus(string status);
    }
}