using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using Microsoft.AspNet.Identity;

namespace CoursesApp.Controllers
{
    public class CourseController : Controller
    {

        private readonly CourseService courseService;
        private readonly CategoryService categoryService;
        private readonly TrainerService trainerService;
        private readonly TraineeCourseService traineeCourseService;
        private readonly IMapper mapper;

        public CourseController()
        {
            mapper = AutoMapperConfig.Mapper;
            courseService = new CourseService();
            categoryService = new CategoryService();
            trainerService = new TrainerService();
            traineeCourseService = new TraineeCourseService();

        }

        //// GET: Default
        //public ActionResult Index()
        //{
        //    var courses = courseService.ReadAll();
        //    return View(mapper.Map<List<Course>, List<CourseModel>>(courses));
        //}

        // GET: Course
        public ActionResult Index(string query = null, int? categoryId = null, int? trainerId = null)
        {
            var coursesListData = new CoursesListModel();
            InitSelectList(ref coursesListData);


            var coursesList = courseService.ReadAll(query, trainerId, categoryId);


            var mappedCoursesList = mapper.Map<List<CourseModel>>(coursesList);
            coursesListData.Courses = mappedCoursesList;


            return View(coursesListData);
        }

        public ActionResult Info(int? Id)
        {
            if (Id == null || Id == 0)
                return HttpNotFound("This course not found!");

            var courseInfo = courseService.Get(Id.Value);
            if (courseInfo == null)
                return HttpNotFound("This course not found!");

            var mappedCourseInfo = mapper.Map<Course, CourseModel>(courseInfo);

            return View(mappedCourseInfo);
        }

        [HttpPost]
        public ActionResult Subscribe(TraineeCourseModel model, int Id)
        {
            try
            {
                var course = courseService.Get(Id);

                model.CourseId = Id;
                model.Trainee_Id = Id;

                var courseTrainee = mapper.Map<Trainee_Courses>(model);

                var TraineeCourse = traineeCourseService.Create(courseTrainee);

                if (TraineeCourse >= 1)
                {
                    return Json(new { saved = true });
                }
                else
                {
                    return Json(new { saved = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { saved = false, message = ex.Message });
            }
        }


        private void InitSelectList(ref CourseModel courseModel)
        {
            var mappedCategoriesList = GetCategories();
            courseModel.Categories = new SelectList(mappedCategoriesList, "Id", "Name");

            var mappedTrainersList = GetTrainers();
            courseModel.Trainers = new SelectList(mappedTrainersList, "ID", "Name");
        }

        private void InitSelectList(ref CoursesListModel coursesList)
        {
            var mappedCategoriesList = GetCategories();
            coursesList.Categories = new SelectList(mappedCategoriesList, "Id", "Name");

            var mappedTrainersList = GetTrainers();
            coursesList.Trainers = new SelectList(mappedTrainersList, "ID", "Name");
        }

        private IEnumerable<CategoryModel> GetCategories()
        {
            var categories = categoryService.ReadAll();
            return mapper.Map<IEnumerable<CategoryModel>>(categories);
        }

        private IEnumerable<TrainerModel> GetTrainers()
        {
            var trainers = trainerService.ReadAll();
            return mapper.Map<IEnumerable<TrainerModel>>(trainers);
        }

    }
}