using KioskApi2.IndoorStatus;

namespace KioskApi2.DataAccess
{
    public interface IDatabaseManager
    {
        Task<IEnumerable<IndoorStatusData>> GetIndoorStatusData();
        void AddUpdateData<T>(T data);
    }
}