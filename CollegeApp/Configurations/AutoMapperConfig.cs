using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<StudentDTO, Student>()
                .ReverseMap();

            //Config for different property names
            //CreateMap<StudentDTO, Student>()
            //    .ForMember(n => n.StudentName, opt => opt.MapFrom(x => x.Name))
            //    .ReverseMap();

            //Config for ignoring some property
            //CreateMap<StudentDTO, Student>()
            //   .ForMember(n => n.StudentName, opt => opt.Ignore())
            //   .ReverseMap();

            //Config for transorm some property
            //CreateMap<StudentDTO, Student>()
            //.ReverseMap()
            //.AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No address found" : n);
        }
    }
}