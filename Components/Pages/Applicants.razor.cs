using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorAppServer;

public partial class Applicants
{
    Applicant? applicant = new();
    List<Applicant>? applicants = new List<Applicant>();
    public Modal? modalComponent;
    bool editmode = false;
    [Inject] public IApplicantsRepository? _applicantsRepository { get; set; }
    protected async override Task OnInitializedAsync()
    {
        Console.WriteLine("OnInitializedAsync invoked.");

        applicants = await GetApplicantsAsync();

        if (applicant is not null && applicants is not null && !applicants.Any())
        {
            applicant.Id = Guid.NewGuid();
            applicant.Name = "Wael Shehab Eldin";
            applicant.Mobile = "01207888335";
            applicant.Email = "wael@innotech.com.eg";
            applicant.Age = 44;
        }

        await base.OnInitializedAsync();
    }
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        Console.WriteLine($"OnAfterRenderAsync invoked. firstRender = {firstRender}");

        await base.OnAfterRenderAsync(firstRender);
    }
    private async Task HandleValidSubmit()
    {

        Console.WriteLine("HandleValidSubmit invoked.");

        ArgumentNullException.ThrowIfNull(applicant);

        string studentSerialized = applicant is null ? string.Empty : JsonSerializer.Serialize(applicant);
        Applicant? validApplicant = JsonSerializer.Deserialize<Applicant>(studentSerialized);

        if (validApplicant is null)
            throw new ArgumentNullException(nameof(validApplicant));

        Applicant? newApplicant = applicants?.FirstOrDefault(e => e.Email is not null && e.Email.Equals(validApplicant?.Email));
        if(editmode)
        {
            await _applicantsRepository!.Update(validApplicant);
            applicants = await GetApplicantsAsync();
            editmode = false;
            Clear();
            StateHasChanged();
            return;
        }
        if (newApplicant is null && _applicantsRepository is not null)
        {
            validApplicant.Id = Guid.NewGuid();

            await _applicantsRepository.Insert(validApplicant);
        }
        // else if (_applicantsRepository is not null)
        // {
        //     await _applicantsRepository.Update(validApplicant);
        //     applicants = await GetApplicantsAsync();
        //     return;
        // }

        if (_applicantsRepository is null) throw new Exception();
        Clear();
        applicants = await GetApplicantsAsync();
    }
    private async Task<List<Applicant>> GetApplicantsAsync()
    {
        Console.WriteLine("GetApplicantsAsync invoked.");

        if (_applicantsRepository is not null)
            return (List<Applicant>) await _applicantsRepository.GetAll();
        else
            return new();
    }

    private void EditApplicant(Applicant toBeEditedApplicant)
    {
        Console.WriteLine("EditApplicant invoked.");

        string? studentSerialized = JsonSerializer.Serialize(toBeEditedApplicant);
        if (studentSerialized is not null)
            applicant = JsonSerializer.Deserialize<Applicant>(studentSerialized);
        editmode = true;
        StateHasChanged();
    }

    public async Task DeleteApplicant(Applicant item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Console.WriteLine("DeleteApplicant invoked.");
         
        await _applicantsRepository!.Delete(item.Id);

        applicants = await GetApplicantsAsync();
        StateHasChanged();
        
        if (_applicantsRepository is null) throw new Exception();
    }

    private async void ShowModal()
    {
        Console.WriteLine("ShowModal invoked.");


        if (modalComponent is not null)
        {
            await modalComponent.ShowModal();
        }
        else
        {
            Console.WriteLine("modalComponent is null.");
        }
    }
    private void Clear()
    {
        Console.WriteLine("Clear invoked.");

        applicant = new Applicant();
    }
}