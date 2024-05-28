using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursesApp.Models
{
    public class CourseTrainee
    {
        public int TraineeId { get; set; }
        public int CourseId { get; set; }
        public DateTime Registration_Date { get; set; }
    }
}