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
        public string USERNAME { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string PASSWORD { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string dec_Pass { get; set; }
        public string NAME { get; set; }
        //[Required(ErrorMessage = "This field is required")]
        public string EMAIL { get; set; }
        public int CONTACT { get; set; }
        public string BIRTHDAY { get; set; }

        [Compare("PASSWORD", ErrorMessage = "Password does not match")]
        public string REPEAT_PASSWORD { get; set; }
    }
}