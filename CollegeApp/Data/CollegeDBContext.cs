using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
        }

        private DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new List<Student>()
            {
                new Student {
                    Id =1,
                    StudentName="AVc",
                    Email="aih@gmail.com",
                    DOB = new DateTime(2022,12,3),
                    Address="123,4,qasjh"
                },
                new Student {
                    Id =2,
                    StudentName="Iyt",
                    Email="iyt@gmail.com",
                    DOB = new DateTime(2022,8,23),
                    Address="wnx.s8,we"
                },
            });
            // base.OnModelCreating(modelBuilder);
        }
    }
}