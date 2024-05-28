using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Controllers
{
    public class DefaultController : Controller
    {
        private readonly CourseService courseService;
        private readonly CategoryService categoryService;
        private readonly TrainerService trainerService;
        private readonly IMapper mapper;

        public DefaultController()
        {
            mapper = AutoMapperConfig.Mapper;
            courseService = new CourseService();
            categoryService = new CategoryService();
            trainerService = new TrainerService();

        }

        // GET: Default
        public ActionResult Index()
        {
            var courses = courseService.ReadAll();
            var categories = categoryService.ReadAll();
            var trainers  = trainerService.ReadAll();

            CoursesListModel mymodel = new CoursesListModel();
            mymodel.Courses = mapper.Map<List<Course>, List<CourseModel>>(courses);
            mymodel.categories = mapper.Map<List<Category>, List<CategoryModel>>(categories);
            mymodel.trainers = mapper.Map<List<Trainer>, List<TrainerModel>>(trainers);

            return View(mymodel);
        }
    }
}