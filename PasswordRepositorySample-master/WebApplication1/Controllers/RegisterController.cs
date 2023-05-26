 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel _registration)
        {

            if(ModelState.IsValid)
            {
                using (PassRepoDbEntities entities = new PassRepoDbEntities())
                {
                    var userData = entities.TBL_LOGIN.Where(x=> x.USERNAME == _registration.USERNAME).FirstOrDefault();

                    if (userData != null)
                    {
                        ViewBag.ErrorMessage = "Username already exist.";
                        return View(_registration);
                    }

                    var ePassword = Helpers.Encryption.Encrypt(_registration.PASSWORD);

                    var nUser = new TBL_LOGIN
                    {
                        USERNAME = _registration.USERNAME,
                        PASSWORD = ePassword,
                        DATE_CREATED = DateTime.Now,
                    };

                    entities.TBL_LOGIN.Add(nUser);

                    if(entities.SaveChanges() > 0)
                    {
                        var nUserDetails = new TBL_USER_DETAILS
                        {
                            UID = nUser.ID,
                            USER_EMAIL = _registration.EMAIL,
                            NAME = _registration.NAME,
                            DATE_CREATED = DateTime.Now
                        };

                        entities.TBL_USER_DETAILS.Add(nUserDetails);

                        if(entities.SaveChanges() > 0)
                        {
                            ViewBag.SuccessMessage = "User registered!";
                            return View();
                        }

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Something went wrong. Please try again.";
                        return View();
                    }

                }
            }

            return View(_registration);

        }

    }
}