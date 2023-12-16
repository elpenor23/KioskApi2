namespace KioskApi.Models;

public class PersonsClothing(Person _person)
{
    public Person Person { get; set; } = _person;
    public List<IntensityClothing> Intensity { get; set; } = [];

}