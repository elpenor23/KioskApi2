
namespace KioskApi2.Clothing
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

		List<string> GetBodyParts();

		Clothing GetClothing();
	}
}