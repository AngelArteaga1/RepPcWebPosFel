using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Minible5.Controllers.Auth
{
    public class AuthLoginController : Controller
    {
        // GET: AuthLogin
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string username, string userpassword)
        {
            try
            {
                using (Models.db_pcsolutions_webEntities db = new Models.db_pcsolutions_webEntities())
                {
                    System.Diagnostics.Debug.WriteLine(username);
                    var oUser = (from d in db.security_users
                                 where d.username == username.Trim() && d.password == userpassword.Trim() && d.activo == 1
                                 select d).FirstOrDefault();
                    if(oUser == null)
                    {
                        ViewBag.Error = "Usuario o contraseña invalida";
                        return View();
                    }
                    Session["User"] = oUser;
                }
                return RedirectToAction("Index", "index");
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
    }
}