namespace KioskApi2.Clothing;

public class PersonsClothing(Person _person)
{
    public Person Person { get; set; } = _person;
    public List<IntensityClothing> Intensity { get; set; } = [];

}