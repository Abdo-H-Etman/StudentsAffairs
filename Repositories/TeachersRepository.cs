
namespace BlazorAppServer;

public class TeachersRepository : Repository<Teacher>,ITeachersRepository
{
    public TeachersRepository(StudentsAffairsDbContext context) : base(context) { }

}