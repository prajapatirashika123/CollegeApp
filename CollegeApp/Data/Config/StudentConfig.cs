using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);
            builder.Property(n => n.StudentName).HasMaxLength(250);
            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasData(new List<Student>()
            {
                new Student {
                    Address="123,4,qasjh",
                    DOB = new DateTime(2022,12,3),
                    Email="aih@gmail.com",
                    Id =1,
                    StudentName="AVc",
                },
                new Student {
                    Address="wnx.s8,we",
                    DOB = new DateTime(2022,8,23),
                    Email="iyt@gmail.com",
                    Id =2,
                    StudentName="Iyt",
                },
            });
        }
    }
}