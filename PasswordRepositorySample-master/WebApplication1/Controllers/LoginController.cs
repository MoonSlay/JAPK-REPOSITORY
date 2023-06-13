using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("Homepage", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TBL_LOGIN login)
        {
            if (ModelState.IsValid)
            {
                var ePassword = Helpers.Encryption.Encrypt(login.PASSWORD);
                using (JAPKDBEntities entities = new JAPKDBEntities())
                {
                    var data = entities.TBL_LOGIN.Where(x => x.USERNAME == login.USERNAME && x.PASSWORD == ePassword).FirstOrDefault();
                    if (data == null)
                    {
                        ViewBag.ErrorMessage = "Invalid Email/Password";
                        return View();
                    }


                    FormsAuthentication.SetAuthCookie(data.ID.ToString(), true);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(data.ID, data.USERNAME, DateTime.Now, DateTime.Now.AddDays(1), true, data.ID.ToString());
                    string eCookie = FormsAuthentication.Encrypt(ticket);
                    HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, eCookie)
                    {
                        Path = FormsAuthentication.FormsCookiePath
                    };

                    Session["user_id"] = data.ID;

                    Response.Cookies.Add(httpCookie);
                    return RedirectToAction("Homepage","Dashboard");

                }
            }
            ViewBag.ErrorMesage = "Invalid Email/Password";
            return View();
        }

        //LogOut Code
        public ActionResult LogOut()
        {
            //clears all Session variables
            Session["ID"] = null;


            //Clears whole session altogether
            Session.Abandon();

            //Clears cookie session
            FormsAuthentication.SignOut();

            //Brings back to home
            return RedirectToAction("Index", "Login");
        }
    }
}