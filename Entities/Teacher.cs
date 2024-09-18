namespace BlazorAppServer;

public class Teacher : BaseEntity
{
    public int Age { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
}