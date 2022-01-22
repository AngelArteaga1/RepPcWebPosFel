using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.Empresas;

namespace Minible5.Controllers.Auth
{
    public class AuthCompanyController : Controller
    {
        // GET: AuthCompany
        public ActionResult Index()
        {
            //Tenemos que obtener la lista de empresas del usuario
            using (db_pcsolutions_webEntities db = new Models.db_pcsolutions_webEntities())
            {
                var oUser = (security_users)Session["User"];
                var oCompanies = (from d in db.security_company_users
                                  join e in db.security_companies
                                  on d.idInternoSecurityCompany equals e.idInternoSecurityCompany
                                  where d.IdInternoSecurityUser == oUser.IdInternoSecurityUser
                                  select new TableCompaniesViewModel
                                  { 
                                      IdInternoSecurityCompany = e.idInternoSecurityCompany,
                                      nombre = e.nombre,
                                      direccion = e.direccion
                                  }).ToList();
                ViewBag.companies = oCompanies;
            }
            return View();
        }

        public ActionResult Select(int id)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                var oCompany = db.security_companies.Find(id);
                Session["Company"] = oCompany;
            }
            return RedirectToAction("Index", "index");
        }
    }
}
