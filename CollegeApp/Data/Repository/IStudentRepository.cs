namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<int> Create(Student student);

        Task<bool> Delete(Student studentToDelete);

        Task<List<Student>> GetAll();

        Task<Student> GetById(int id, bool useNoTracking = false);

        Task<Student> GetByName(string name);

        Task<int> Update(Student student);
    }
}