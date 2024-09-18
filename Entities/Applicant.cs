namespace BlazorAppServer;

public class Applicant : BaseEntity
{
    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public int Age { get; set; }

    public string? SecondarySchoolName { get; set; }
}
