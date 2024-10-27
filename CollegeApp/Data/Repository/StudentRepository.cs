namespace CollegeApp.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDBContext _dBContext;

        public StudentRepository(CollegeDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }
    }
}