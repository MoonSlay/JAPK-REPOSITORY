using Microsoft.Ajax.Utilities;
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
        public ActionResult Homepage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(PasswordModel _MODEL)
        {
            var userId = (Int32)Session["user_id"];

            //encrypt added password
            var ePassword = Helpers.Encryption.Encrypt(_MODEL.SITE_PASSWORD);
            var dec_Pass = Helpers.Encryption.DecryptString(_MODEL.SITE_PASSWORD);
            var data = new TBL_USER_PASSWORDS()
            {
                SITE_NAME = _MODEL.SITE_NAME,
                SITE_PASSWORD = ePassword,
                DEC_PASSWORD = dec_Pass,
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
                    return RedirectToAction("Manage");
                }
                else
                {
                    //FAIL
                    TempData["ErrorMessage"] = "Error in creating Account data";
                    return RedirectToAction("Manage");

                }

            }
        }

        public ActionResult LearnMore()
        {
            return View();
        }

        public ActionResult Manage()
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
                        IS_DELETED = pass.IS_DELETED,
                        USER_NAME = pass.USER_NAME,
                        EMAIL = pass.EMAIL,
                        CONTACT_NUMBER = (int)pass.CONTACT_NUMBER,
                    }); ;
                }

                var dashboardModel = new DashboardModel
                {
                    PasswordList = passwordList,
                    PasswordModel = new PasswordModel()
                };

                return View(dashboardModel);

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
                    return Json(new { msg = "Account Successfully Modified" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        public ActionResult Restore()
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                if (Session["user_id"] == null)
                {
                    return RedirectToAction("Index", "Login");

                }
                var userId = (Int32)Session["user_id"];
                var passwordData = entities.TBL_USER_PASSWORDS.Where(x => x.USER_ID == userId && x.IS_DELETED == true).ToList();

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
                        IS_DELETED = pass.IS_DELETED,
                        USER_NAME = pass.USER_NAME,
                        EMAIL = pass.EMAIL,
                        CONTACT_NUMBER = (int)pass.CONTACT_NUMBER,
                    }); ;
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
        public ActionResult RestoreAccount(int userId)
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
                    return Json(new { msg = "Account Successfully Restored" });
                }
                else
                {
                    //Error Message
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        public ActionResult Profile()
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                if (Session["user_id"] == null)
                {
                    return RedirectToAction("Index", "Login");

                }
                var uId = (Int32)Session["user_id"];

                var registrationData = entities.TBL_USER_DETAILS.Where(x => x.UID == uId).ToList();
                var registrationList = new List<RegistrationModel>();

                foreach (var reg in registrationData)
                {
                    registrationList.Add(new RegistrationModel
                    {
                        ID = reg.UID.Value,
                        NAME = reg.NAME,
                        EMAIL = reg.EMAIL,
                        BIRTHDAY = reg.BIRTHDAY,
                        CONTACT = (int)reg.CONTACT,
                    }); ;
                }

                var DashboardModel = new DashboardModel
                {
                    RegistrationList = registrationList,
                    RegistrationModel = new RegistrationModel()
                };

                return View(DashboardModel);
            }
        }

        //Edit profile
        [HttpPost]
        public ActionResult EditProfile(int uId, string nAme, string eMail, string bIrthday, int cOntact)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                var reg = entities.TBL_USER_DETAILS.Where(x => x.UID == uId).FirstOrDefault();
                var log = entities.TBL_LOGIN.Where(x => x.ID == uId).FirstOrDefault();

                if (reg == null)
                {
                    return Json(new { msg = "Account not found" });
                }

                reg.NAME = nAme;
                reg.EMAIL = eMail;
                reg.BIRTHDAY = bIrthday;
                reg.CONTACT = cOntact;

                if (entities.SaveChanges() >= 1)
                {
                    return Json(new { msg = "Account Successfully Edited" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        public ActionResult LoginDetails()
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                if (Session["user_id"] == null)
                {
                    return RedirectToAction("Index", "Login");

                }
                var uId = (Int32)Session["user_id"];
                var loginData = entities.TBL_LOGIN.Where(x => x.ID == uId).ToList();
                var registrationList = new List<RegistrationModel>();

                foreach (var log in loginData)
                {
                    registrationList.Add(new RegistrationModel
                    {
                        ID = log.ID,
                        USERNAME = log.USERNAME,
                        PASSWORD = log.PASSWORD,

                    }); ;
                }

                var DashboardModel = new DashboardModel
                {
                    RegistrationList = registrationList,
                    RegistrationModel = new RegistrationModel()
                };

                return View(DashboardModel);
            }
        }

        public ActionResult EditUsername(int uId, string uSername)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {

                var log = entities.TBL_LOGIN.Where(x => x.ID == uId).FirstOrDefault();

                if (log == null)
                {
                    return Json(new { msg = "Account not found" });
                }

                log.USERNAME = uSername;

                if (entities.SaveChanges() >= 1)
                {
                    return Json(new { msg = "Account Successfully Edited" });
                }
                else
                {
                    return Json(new { msg = "An error occurred" });
                }

            }
        }

        //Edit profile
        [HttpPost]
        public ActionResult ChangePassword(int uId, string oldpass, string newpass)
        {
            using (JAPKDBEntities entities = new JAPKDBEntities())
            {
                var log = entities.TBL_LOGIN.Where(x => x.ID == uId).FirstOrDefault();
                var New = Helpers.Encryption.Encrypt(newpass);
                var Old = Helpers.Encryption.Encrypt(oldpass);
                if (log == null)
                {
                    return Json(new { msg = "Account not found" });
                }

                if (log.PASSWORD == Old)
                {
                    log.PASSWORD = New;
                    if (entities.SaveChanges() >= 1)
                    {
                        return Json(new { msg = "Account Successfully Edited" });
                    }
                    else
                    {
                        return Json(new { msg = "An error occurred(Not Saved)" });
                    }
                }
                else
                {
                    return Json(new { msg = "An error occurred(Not Equal)" });
                }

            }
        }
    }
}