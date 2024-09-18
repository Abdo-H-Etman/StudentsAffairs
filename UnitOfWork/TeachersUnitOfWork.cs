namespace BlazorAppServer.UnitOfWork;

public class TeachersUnitOfWork : UnitOfWork<Teacher> ,ITeachersUnitOfWork
{
    private readonly StudentsAffairsDbContext _context;
    private readonly ITeachersRepository _teachersRepository;
    public TeachersUnitOfWork(StudentsAffairsDbContext context, ITeachersRepository teachersRepository) : base (context, teachersRepository)
    {
        _context = context;
        _teachersRepository = teachersRepository;
    }


}