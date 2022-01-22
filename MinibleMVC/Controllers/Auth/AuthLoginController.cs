using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;

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
                using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
                {
                    System.Diagnostics.Debug.WriteLine(username);
                    var oUser = (from d in db.security_users
                                 where d.username == username.Trim() && d.password == userpassword.Trim() && d.activo == 1
                                 select d).FirstOrDefault();
                    if (oUser == null)
                    {
                        ViewBag.Error = "Usuario o contraseña invalida";
                        return View();
                    }
                    Session["User"] = oUser;
                    var sizeCompany = db.security_company_users.Where(d => d.IdInternoSecurityUser == oUser.IdInternoSecurityUser).Count();
                    //Si el tamaño es 1, solo tiene acceso a una empresa, entonces procedemos a obtener la empresa
                    if (sizeCompany == 1)
                    {
                        var idCompany = (from d in db.security_company_users
                                         where d.IdInternoSecurityUser == oUser.IdInternoSecurityUser
                                         select d.idInternoSecurityCompany).FirstOrDefault();
                        var oCompany = (from d in db.security_companies
                                     where d.idInternoSecurityCompany == idCompany
                                     select d).FirstOrDefault();
                        Session["Company"] = oCompany;
                    }
                    else if (sizeCompany > 1)
                    {
                        Session["Company"] = "En proceso";
                        return RedirectToAction("Index", "AuthCompany");
                    }
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