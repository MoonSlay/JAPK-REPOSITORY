using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                if (Session["user_id"] == null)
                {
                    return RedirectToAction("Index", "Login");

                }
                var userId = (Int32)Session["user_id"];
                var passwordData = entities.TBL_USER_PASSWORDS.Where(x => x.USER_ID == userId && x.IS_DELETED == false).ToList();

                var passwordList = new List<PasswordModel>();

                foreach (var pass in passwordData)
                {
                    ////decrypt password
                    var dec_Pass = Helpers.Encryption.DecryptString(pass.SITE_PASSWORD);
                    passwordList.Add(new PasswordModel
                    {
                        ID = pass.ID,
                        SITE_NAME = pass.SITE_NAME,
                        SITE_PASSWORD = pass.SITE_PASSWORD,
                        DEC_PASSWORD = dec_Pass,
                        USER_NAME = pass.USER_NAME,
                        EMAIL = pass.EMAIL,
                        CONTACT_NUMBER = (int)pass.CONTACT_NUMBER,
                    });;
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

            //encrypt added password
            var ePassword = Helpers.Encryption.Encrypt(_MODEL.SITE_PASSWORD);

            var data = new TBL_USER_PASSWORDS()
            {
                SITE_NAME = _MODEL.SITE_NAME,
                SITE_PASSWORD = ePassword,
                DATE_CREATED = DateTime.Now,
                IS_DELETED = false,
                DATE_MODIFIED = DateTime.Now,
                USER_ID = userId,
                USER_NAME = _MODEL.USER_NAME,
                EMAIL = _MODEL.EMAIL,
                CONTACT_NUMBER = _MODEL.CONTACT_NUMBER,
            };

            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                entities.TBL_USER_PASSWORDS.Add(data);

                if (entities.SaveChanges() >= 1)
                {
                    //SUCCESS
                    TempData["SuccessMessage"] = "Successfully added the Account to the database.";
                    return RedirectToAction("Index");
                }
                else
                {
                    //FAIL
                    TempData["ErrorMessage"] = "Error in creating Account data";
                    return RedirectToAction("Index");

                }

            }
        }

        //delete account controller
        [HttpPost]
        public ActionResult DeleteAccount(int userId)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                var data = entities.TBL_USER_PASSWORDS.Where(x => x.ID == userId).FirstOrDefault();

                if (data == null)
                {
                    return Json(new { msg = "Account not found." });
                }
                data.IS_DELETED = true;

                if (entities.SaveChanges() >= 1)
                {
                    return Json(new { msg = "Account removed from database" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        //update account controller
        [HttpPost]
        public ActionResult UpdateAccount(int userId, string sitePass, string siteName, string userName, string email, int contact)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                var data = entities.TBL_USER_PASSWORDS.Where(x => x.ID == userId).FirstOrDefault();

                //decrypt password
                //var dec_Pass = Helpers.Encryption.DecryptString(data.SITE_PASSWORD);

                //encrpyt modified password
                var ePassword = Helpers.Encryption.Encrypt(sitePass);

                if (data == null)
                {
                    return Json(new { msg = "Account not found" });
                }

                data.SITE_NAME = siteName;
                data.SITE_PASSWORD = ePassword;
                data.USER_NAME = userName;
                data.EMAIL = email;
                data.CONTACT_NUMBER = contact;

                if (entities.SaveChanges() >= 1)
                {
                    return Json(new { msg = "Modified Account details" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }


        [HttpPost]
        public ActionResult EditProfile(int userId, string name, string birthday, string contact, string email)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                var data = entities.TBL_USER_DETAILS.Where(x => x.ID == userId).FirstOrDefault();

                if (data == null)
                {
                    return Json(new { msg = "Account not found" });
                }

                data.NAME = name;
                data.USER_EMAIL = email;
                data.BIRTHDAY = birthday;
                data.CONTACT = contact;

                if (entities.SaveChanges() >= 1)
                {
                    return Json(new { msg = "Modified Account details" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        public ActionResult Restore()
        {
            return View();  
        }


        [HttpPost]
        public ActionResult RestoreEntry(int userId)
        {

            using (JAPKDBEntities entities = new JAPKDBEntities())
            {

                var data = entities.TBL_USER_PASSWORDS.Where(x => x.ID == userId).FirstOrDefault();

                if (data == null)
                {
                    return Json(new { msg = "Entry not found (how???)" });
                }


                data.IS_DELETED = false;


                if (entities.SaveChanges() >= 1)
                {
                    //Success Message
                    return Json(new { msg = "Entry deleted" });
                }
                else
                {
                    //Error Message
                    return Json(new { msg = "An error occurred(Controller)" });
                }

            }
        }

        [HttpPost]
        public ActionResult PermDeleteEntry(int userId)
        {

            using (JAPKDBEntities entities = new JAPKDBEntities())
            {

                var data = entities.TBL_USER_PASSWORDS.Where(x => x.ID == userId).FirstOrDefault();

                if (data == null)
                {
                    return Json(new { msg = "Entry not found (how???)" });
                }

                data.IS_DELETED = true;
                if (entities.SaveChanges() >= 1)
                {
                    //Success Message
                    return Json(new { msg = "Entry deleted" });
                }
                else
                {
                    //Error Message
                    return Json(new { msg = "An error occurred(Controller)" });
                }

            }
        }
}