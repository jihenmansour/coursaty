using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Models
{
    public class CourseModel
    {

        public int Course_Id { get; set; }

        [Required]
        public string Name { get; set; }
        public System.DateTime Creation_Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }

        [Required]
        [Display(Name = "Trainer")]
        public Nullable<int> Trainer_Id { get; set; }
        public string TrainerName { get; set; }
        public string Image_ID { get; set; }
        private string image_ID
        {
            set
            {
                Image_ID = string.IsNullOrWhiteSpace(value) ? "empty.jpg" : value;
            }
            get
            {
             return Image_ID;
            }
        }

        public HttpPostedFileBase ImageFile { get; set; }
        public SelectList Trainers { get; set; }
        public SelectList Categories { get; set; }
    }

    public class CoursesListModel
    {
        public IEnumerable<CourseModel> Courses { get; set; }
        public IEnumerable<CategoryModel> categories { get; set; }
        public IEnumerable<TrainerModel> trainers { get; set; }
        public string Query { get; set; }

        [Display(Name ="Trainer")]
        public int TrainerId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public SelectList Trainers { get; set; }
        public SelectList Categories { get; set; }
    }



    public class CourseUnitModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Course_Id { get; set; }
        public string Description { get; set; }
    }
}