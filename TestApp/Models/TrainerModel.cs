using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoursesApp.Models
{
    public class TrainerModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please, write Email in correct format!")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string Website { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}