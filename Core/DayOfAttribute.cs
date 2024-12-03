public class DayOfAttribute : Attribute
{
    public int Day { get; set; }

    public DayOfAttribute(int dayNumber)
    {
        Day = dayNumber;
    }
}