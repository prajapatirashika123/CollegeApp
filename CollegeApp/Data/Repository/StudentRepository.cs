using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dBContext;

        public StudentRepository(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<int> Create(Student student)
        {
            await _dBContext.Students.AddAsync(student);
            await _dBContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> Delete(Student studentToDelete)
        {
            _dBContext.Students.Remove(studentToDelete);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAll()
        {
            return await _dBContext.Students.ToListAsync();
        }

        public async Task<Student> GetById(int id, bool useNoTracking = false)
        {
            if (useNoTracking)
                return await _dBContext.Students.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            else
                return await _dBContext.Students.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByName(string name)
        {
            return await _dBContext.Students.Where(x => x.StudentName == name).FirstOrDefaultAsync();
        }

        public async Task<int> Update(Student student)
        {
            _dBContext.Update(student);
            await _dBContext.SaveChangesAsync();
            return student.Id;
        }
    }
}