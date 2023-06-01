using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RegistrationModel
    {
        public int ID { get; set; } 
        [Display(Name = "USERNAME")]
        public string USERNAME { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string PASSWORD { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string NAME { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string EMAIL { get; set; }
        public string CONTACT { get; set; }
        public string BIRTHDAY { get; set; }

        [Compare("PASSWORD", ErrorMessage = "Password does not match")]
        public string REPEAT_PASSWORD { get; set; }
    }
}