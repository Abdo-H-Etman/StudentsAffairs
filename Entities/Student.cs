namespace BlazorAppServer;

public class Student : BaseEntity
{
    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }

    public string? ScholarShipType { get; set; }
}
