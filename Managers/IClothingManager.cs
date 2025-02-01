using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface IClothingManager
    {
        Task<IEnumerable<PersonsClothing>> GetCalculatedClothing(
        string feels,
        string ids,
        string names,
        string colors,
        string lat,
        string lon);

        Task<IEnumerable<BodyPart>> GetBodyParts();

        Task<List<ClothingList>> GetClothing();
    }
}