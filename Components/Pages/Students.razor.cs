using BlazorAppServer.UnitOfWork;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace BlazorAppServer;

public partial class Students
{
    Student? student = new();
    List<Student>? students = new List<Student>();
    public Modal? modalComponent;
    bool editmode = false;
    [Inject] public IStudentsUnitOfWork? _studentsUnitOfWork { get; set; }
    protected async override Task OnInitializedAsync()
    {
        Console.WriteLine("OnInitializedAsync invoked.");

        students = await GetStudentsAsync();

        if (student is not null && students is not null && !students.Any())
        {
            student.Id = Guid.NewGuid();
            student.Name = "Wael Shehab Eldin";
            student.Mobile = "01207888335";
            student.Email = "wael@innotech.com.eg";
            student.Age = 44;
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

        ArgumentNullException.ThrowIfNull(student);

        string studentSerialized = student is null ? string.Empty : JsonSerializer.Serialize(student);
        Student? validStudent = JsonSerializer.Deserialize<Student>(studentSerialized);

        if (validStudent is null)
            throw new ArgumentNullException(nameof(validStudent));

        Student? newStudent = students?.FirstOrDefault(e => e.Email is not null && e.Email.Equals(validStudent?.Email));

        if(editmode)
        {
            try
            {
                await _studentsUnitOfWork!.Update(validStudent);
                students = await GetStudentsAsync();
                editmode = false;
                Clear();
                StateHasChanged();
                return;
            }
            catch
            {
                throw new InvalidDataException();
            }
        }
        if (newStudent is null && _studentsUnitOfWork is not null)
        {
            validStudent.Id = Guid.NewGuid();

            await _studentsUnitOfWork.Create(validStudent);
        }
        // else if (_studentsUnitOfWork is not null)
        // {
        //     await _studentsUnitOfWork.Update(validStudent);
        //     students = await GetStudentsAsync();
        //     return;
        // }

        if (_studentsUnitOfWork is null) throw new Exception();
        Clear();
        students = await GetStudentsAsync();
    }
    private async Task<List<Student>> GetStudentsAsync()
    {
        Console.WriteLine("GetStudentsAsync invoked.");

        if (_studentsUnitOfWork is not null)
            return (List<Student>)await _studentsUnitOfWork.ReadAll();
        else
            return new();
    }

    private void EditStudent(Student toBeEditedStudent)
    {
        Console.WriteLine("EditStudent invoked.");

        string? studentSerialized = JsonSerializer.Serialize(toBeEditedStudent);
        if (studentSerialized is not null)
            student = JsonSerializer.Deserialize<Student>(studentSerialized);
        editmode = true;
        StateHasChanged();
    }

    public async void DeleteStudent(Student item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Console.WriteLine("DeleteStudent invoked.");
        await _studentsUnitOfWork!.Delete(item.Id);
        students = await GetStudentsAsync();
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

        student = new Student();
    }
}