namespace CollegeApp.Data
{
    public class Student
    {
        public string Address { get; set; }

        public virtual Department? Department { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime DOB { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }

        public string StudentName { get; set; }
    }
}