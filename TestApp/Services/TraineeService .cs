using CoursesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursesApp.Services
{
    public interface ITraineeService
    {
        Trainee Create(Trainee trainee);
    }
    public class TraineeService : ITraineeService
    {

        private readonly Courses_DBEntities1 courses_DBEntities1;

        public TraineeService()
        {
            courses_DBEntities1= new Courses_DBEntities1();
        }

        public Trainee Create(Trainee trainee)
        {
            courses_DBEntities1.Trainees.Add(trainee);

            int savingResult = courses_DBEntities1.SaveChanges();
            if(savingResult > 0)
            {
                return trainee;
            }
            return null;
            
        }
    }
}