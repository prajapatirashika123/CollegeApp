namespace CollegeApp.Data
{
    public class Department
    {
        public string DepartmentName { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}