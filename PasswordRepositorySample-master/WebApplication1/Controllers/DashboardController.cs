using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            using (PassRepoDbEntities entities = new PassRepoDbEntities())
            {
                if (Session["user_id"] == null)
                {
                    return RedirectToAction("Index", "Login");

                }
                var userId = (Int32)Session["user_id"];
                var passwordData = entities.TBL_USER_PASSWORDS.Where(x => x.USER_ID == userId).ToList();

                var passwordList = new List<PasswordModel>();

                foreach (var pass in passwordData)
                {
                    passwordList.Add(new PasswordModel
                    {
                        ID = pass.ID,
                        SITE_NAME = pass.SITE_NAME,
                        SITE_PASSWORD = pass.SITE_PASSWORD,
                    });
                }

                var dashboardModel = new DashboardModel
                {
                    PasswordList = passwordList,
                    PasswordModel = new PasswordModel()
                };

                return View(dashboardModel);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PasswordModel _MODEL)
        {
            var userId = (Int32)Session["user_id"];

            var ePassword = Helpers.Encryption.Encrypt(_MODEL.SITE_PASSWORD);

            var data = new TBL_USER_PASSWORDS()
            {
                SITE_NAME = _MODEL.SITE_NAME,
                SITE_PASSWORD = ePassword,
                DATE_CREATED = DateTime.Now,
                IS_DELETED = false,
                DATE_MODIFIED = DateTime.Now,
                USER_ID = userId
            };

            using (PassRepoDbEntities entities = new PassRepoDbEntities())
            {
                entities.TBL_USER_PASSWORDS.Add(data);

                if (entities.SaveChanges() >= 1)
                {
                    //SUCCESS
                    TempData["SuccessMessage"] = "Successfully added the student to the database.";
                    return RedirectToAction("Index");
                }
                else
                {
                    //FAIL
                    TempData["ErrorMessage"] = "Error in creating student data";
                    return RedirectToAction("Index");

                }

            }
        }
    }
}