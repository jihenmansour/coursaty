using CoursesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursesApp.Services
{
    public interface ITraineeCourseService
    {
        IEnumerable<Trainee_Courses> GetTrainees(int? courseId = null);
        int Create(Trainee_Courses courseTrainee);
    }
    public class TraineeCourseService : ITraineeCourseService
    {

        private readonly Courses_DBEntities1 db;

        public TraineeCourseService()
        {
            db= new Courses_DBEntities1();
        }

        public int Create(Trainee_Courses courseTrainee)
        {
            db.Trainee_Courses.Add(courseTrainee);

            return db.SaveChanges();
        }

        public IEnumerable<Trainee_Courses> GetTrainees(int? courseId = null)
        {
           return db.Trainee_Courses.Where(t => 
           courseId == null || t.Course_Id== courseId);
        }
    }
}