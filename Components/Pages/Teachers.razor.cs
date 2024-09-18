using BlazorAppServer.UnitOfWork;

namespace BlazorAppServer;

public partial class Teachers
{
    Teacher? teacher = new();
    List<Teacher>? teachers = new List<Teacher>();
    public Modal? modalComponent;
    bool editmode = false;
    [Inject] public ITeachersUnitOfWork? _teachersUnitOfWork { get; set; }
    protected async override Task OnInitializedAsync()
    {
        Console.WriteLine("OnInitializedAsync invoked.");

        teachers = await GetTeachersAsync();

        if (teacher is not null && teachers is not null && !teachers.Any())
        {
            teacher.Id = Guid.NewGuid();
            teacher.Name = "Wael Shehab Eldin";
            teacher.Mobile = "01207888335";
            teacher.Email = "wael@innotech.com.eg";
            teacher.Age = 44;
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

        ArgumentNullException.ThrowIfNull(teacher);

        string TeacherSerialized = teacher is null ? string.Empty : JsonSerializer.Serialize(teacher);
        Teacher? validTeacher = JsonSerializer.Deserialize<Teacher>(TeacherSerialized);

        if (validTeacher is null)
            throw new ArgumentNullException(nameof(validTeacher));

        Teacher? newTeacher = teachers?.FirstOrDefault(e => e.Name is not null && e.Name.Equals(validTeacher?.Name));
        if(editmode)
        {
            await _teachersUnitOfWork!.Update(validTeacher);
            teachers = await GetTeachersAsync();
            editmode = false;
            Clear();
            StateHasChanged();
            return;
        }
        if (newTeacher is null && _teachersUnitOfWork is not null)
        {
            validTeacher.Id = Guid.NewGuid();

            await _teachersUnitOfWork.Create(validTeacher);
        }
        // else if (_teachersUnitOfWork is not null)
        // {
        //     await _teachersUnitOfWork.Update(validTeacher);
        // }

        if (_teachersUnitOfWork is null) throw new Exception();
        Clear();    
        teachers = await GetTeachersAsync();
    }
    private async Task<List<Teacher>> GetTeachersAsync()
    {
        Console.WriteLine("GetTeachersAsync invoked.");

        if (_teachersUnitOfWork is not null)
            return (List<Teacher>)await _teachersUnitOfWork.ReadAll();
        else
            return new();
    }

    private void EditTeacher(Teacher toBeEditedTeacher)
    {
        Console.WriteLine("EditTeacher invoked.");

        string? TeacherSerialized = JsonSerializer.Serialize(toBeEditedTeacher);
        if (TeacherSerialized is not null)
            teacher = JsonSerializer.Deserialize<Teacher>(TeacherSerialized);
        editmode = true;
        StateHasChanged();
    }

    public async void DeleteTeacher(Teacher item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Console.WriteLine("DeleteTeacher invoked.");

        _teachersUnitOfWork?.Delete(item);
        teachers = await GetTeachersAsync();
        StateHasChanged();
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

        teacher = new Teacher();
        editmode = false;
    }
}
