using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DashboardModel
    {
        public List<PasswordModel> PasswordList { get; set; }
        public PasswordModel PasswordModel { get; set; }
    }
}