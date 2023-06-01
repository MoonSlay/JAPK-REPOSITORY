using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PasswordModel
    {
        public int ID { get; set; }
        public string SITE_NAME { get; set; }
        public string SITE_PASSWORD { get; set; }
        public string DEC_PASSWORD { get; set; }
        public string USER_NAME { get; set; }
        public string EMAIL { get; set; }
        public int CONTACT_NUMBER { get; set; }
    }
}