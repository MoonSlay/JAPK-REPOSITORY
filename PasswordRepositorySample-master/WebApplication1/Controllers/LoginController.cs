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
                return RedirectToAction("Index", "Dashboard");
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
                using (PassRepoDbEntities entities = new PassRepoDbEntities())
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
                    Session.Timeout = 1440;

                    Response.Cookies.Add(httpCookie);
                    return RedirectToAction("Index","Dashboard");

                }
            }
            ViewBag.ErrorMesage = "Invalid Email/Password";
            return View();
        }
    }
}