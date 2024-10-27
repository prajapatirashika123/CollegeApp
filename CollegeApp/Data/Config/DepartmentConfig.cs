using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.DepartmentName).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Description).IsRequired(false).HasMaxLength(500);

            builder.HasData(new List<Department>()
            {
                new Department {
                    Description="ECE Department",
                    DepartmentName = "ECE",
                    Id =1,
                },
                new Department {
                    Description="CSE Department",
                    DepartmentName = "CSE",
                    Id =2,
                },
            });
        }
    }
}