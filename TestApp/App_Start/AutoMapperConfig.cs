using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace CoursesApp
{
    public class AutoMapperConfig
    {

        public static IMapper Mapper { get; private set; }
        public static void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Category, CategoryModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(e => e.ID))
                .ForMember(dst => dst.ParentId, src => src.MapFrom(e => e.Category2.Parent_Id))
                .ForMember(dst => dst.ParentName, src => src.MapFrom(e => e.Category2.Name))
                .ReverseMap();

                cfg.CreateMap<Trainer, TrainerModel>().ReverseMap();


                cfg.CreateMap<Course, CourseModel>()
                .ForMember(dst => dst.TrainerName, src => src.MapFrom(e => e.Trainer.Name))
                .ForMember(dst => dst.Category_Name, src => src.MapFrom(e => e.Category.Name))
                .ReverseMap();


                cfg.CreateMap<Trainee, TraineeModel>().ReverseMap();


                cfg.CreateMap<Trainee_Courses, TraineeCourseModel>()
                .ForMember(dst => dst.CourseId, src => src.MapFrom(c => c.Course_Id))
                .ForMember(dst => dst.Registration_Date, src => src.MapFrom(c => c.Registration_Date))
                .ForMember(dst => dst.Trainee, src => src.MapFrom(c => c.Trainee));


                cfg.CreateMap<Course_Units, CourseUnitModel>()
                .ForMember(dst => dst.Course_Id, src => src.MapFrom(c => c.Course_Id))
                .ForMember(dst => dst.Name, src => src.MapFrom(c => c.Name))
                .ReverseMap();

            });

            Mapper = config.CreateMapper();
        }
    }
}