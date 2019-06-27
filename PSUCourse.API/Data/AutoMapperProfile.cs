using AutoMapper;
using PSUCourse.API.Models;
using PSUCourse.API.Dtos;

namespace PSUCourse.API.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            // Map the entity to the model
            CreateMap<Course, CourseProfessorDto>()
                .ForMember(prof => prof.ProfessorEmail, opt =>
                {
                    opt.MapFrom(p => p.Professor.Email);
                })
                .ForMember(prof => prof.ProfessorName, opt =>
                {
                    opt.MapFrom(p => p.Professor.ProfessorName);
                });
            CreateMap<CourseProfessorDto, Course>();
            CreateMap<CourseProfessorDto, Professor>()
                .ForMember(prof => prof.Email, opt =>
                {
                    opt.MapFrom(p => p.ProfessorEmail);
                })
                .ForMember(prof => prof.Id, opt =>
                {
                    opt.MapFrom(p => p.ProfessorID);
                });
            CreateMap<Professor, ProfessorDto>();
            CreateMap<CourseProfessor, CourseProfessorDto>();
            CreateMap<CourseProfessorDto, CourseProfessor>();
        }
    }
}