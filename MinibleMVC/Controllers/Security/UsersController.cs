using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels;
using System.Linq.Dynamic;
using Minible;

namespace Minible5.Controllers.Security
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        //Get datalist
        public List<SelectListItem> getGroups()
        {
            List<TableUsers_Groups> lst;
            using (var db = new db_pcsolutions_webEntities())
            {
                lst =
                    (from d in db.security_groups
                     select new TableUsers_Groups
                     {
                         Id = d.IdInternoSecurityGroup,
                         Name = d.name
                     }).ToList();
            }
            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getCompanies()
        {
            List<DDCompanies> lst;
            using (var db = new db_pcsolutions_webEntities())
            {
                lst =
                    (from d in db.security_companies
                     select new DDCompanies
                     {
                         CodigoEmpresa = d.codigo_empresa,
                         Name = d.nombre
                     }).ToList();
            }
            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.Name,
                    Value = d.CodigoEmpresa.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<DDCompanies> getUserCompanies(int id)
        {
            List<DDCompanies> lst;
            using (var db = new db_pcsolutions_webEntities())
            {
                lst =
                    (from d in db.security_company_users
                     join e in db.security_companies on d.idInternoSecurityCompany equals e.idInternoSecurityCompany
                     where d.IdInternoSecurityUser == id
                     select new DDCompanies
                     {
                         Id = d.IdInternoSecurityCompanyUser,
                         CodigoEmpresa = d.codigo_empresa,
                         Name = e.nombre
                     }).ToList();
            }
            return lst;
        }

        [HttpPost]
        public ActionResult GetUsers()
        {
            List<TableUsersViewModel> lst = new List<TableUsersViewModel>();

            //logistica datatable
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                IQueryable<TableUsersViewModel> query = 
                    (from d in db.security_users
                     join e in db.security_groups on d.IdInternoSecurityGroup equals e.IdInternoSecurityGroup
                        select new TableUsersViewModel
                        {
                            id = d.IdInternoSecurityUser,
                            username = d.username,
                            name = d.name,
                            email = d.email,
                            activo = d.activo,
                            grupo = e.name
                        });
                //Searching by name
                if(searchValue != "")
                {
                    query = query.Where(d => d.name.Contains(searchValue) || d.username.Contains(searchValue));
                }
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    query = query.OrderBy(sortColumn + " " + sortColumnDir);
                }
                recordsTotal = query.Count();
                lst = query.Skip(skip).Take(pageSize).ToList();
                return Json(new { 
                    draw = draw, 
                    recordsFiltered = recordsTotal, 
                    recordsTotal = recordsTotal, 
                    data = lst });
            }
        }

        // este es un comentario

        public ActionResult Add()
        {
            //Datalist
            var items = getGroups();
            ViewBag.items = items;
            var companies = getCompanies();
            ViewBag.companies = companies;
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            }
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                //GUARDAMOS EL USUARIO
                security_users oUser = new security_users();
                oUser.name = model.Name;
                oUser.username = model.UserName;
                oUser.email = model.Email;
                //group
                var oGroup = db.security_groups.Find(Convert.ToInt32(model.Group));
                oUser.group = oGroup.name;
                
                oUser.IdInternoSecurityGroup = oGroup.IdInternoSecurityGroup;
                //end group
                oUser.password = model.Password;
                oUser.created = DateTime.Now;
                oUser.modified = DateTime.Now;
                oUser.status = "A";
                oUser.activo = Convert.ToInt32(model.Active);
                db.security_users.Add(oUser);
                db.SaveChanges();
                //GUARDAMOS SUS EMPRESAS
                foreach(var codigoEmpresa in model.Empresas)
                {
                    //obtenemos el id de la empresa
                    var idEmpresa = (from d in db.security_companies
                                 where d.codigo_empresa == codigoEmpresa
                                 select d.idInternoSecurityCompany).FirstOrDefault();
                    //obtenemos el id del usuario
                    var idUser = (from d in db.security_users
                                 where d.username == model.UserName
                                 select d.IdInternoSecurityUser).FirstOrDefault();
                    var oRelacion = new security_company_users();
                    oRelacion.codigo_empresa = codigoEmpresa;
                    oRelacion.user_name = model.UserName;
                    oRelacion.application = Contants.APPLICATION;
                    oRelacion.IdInternoSecurityUser = idUser;
                    oRelacion.idInternoSecurityCompany = idEmpresa;
                    db.security_company_users.Add(oRelacion);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Users", new { success = "Se agregó correctamente!" });
        }

        public ActionResult Edit(int id)
        {
            //Datalist
            var items = getGroups();
            ViewBag.items = items;
            var companies = getCompanies();
            ViewBag.companies = companies;
            EditUserViewModel model = new EditUserViewModel();
            //Empresas del usuario
            var userCompanies = getUserCompanies(id);
            ViewBag.userCompanies = userCompanies;
            using (var db = new db_pcsolutions_webEntities())
            {
                var oUser = db.security_users.Find(id);
                model.Name = oUser.name;
                model.UserName = oUser.username;
                model.Email = oUser.email;
                model.Password = oUser.password;
                model.ConfirmPassword = oUser.password;
                model.Group = oUser.IdInternoSecurityGroup.ToString();
                model.Active = Convert.ToBoolean(oUser.activo);
                model.Id = id;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            }
            using (var db = new db_pcsolutions_webEntities())
            {
                var oUser = db.security_users.Find(model.Id);
                oUser.name = model.Name;
                oUser.username = model.UserName;
                oUser.email = model.Email;
                oUser.activo = Convert.ToInt32(model.Active);
                //group
                var oGroup = db.security_groups.Find(Convert.ToInt32(model.Group));
                oUser.group = oGroup.name;
                oUser.IdInternoSecurityGroup = oGroup.IdInternoSecurityGroup;
                //end group
                if (model.Password != null && model.Password.Trim() != "")
                {
                    oUser.password = model.Password;
                }
                db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Users", new { success = "Se editó correctamente!" });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                var oUser = db.security_users.Find(id);
                db.security_users.Remove(oUser);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult addCompany(int userId, string codigoEmpresa)
        {
            try
            {
                int userCompanyId;
                using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
                {
                    //obtenemos copmpanyId
                    var companyId = (from d in db.security_companies
                                     where d.codigo_empresa == codigoEmpresa
                                     select d.idInternoSecurityCompany).FirstOrDefault();
                    var oUser = db.security_users.Find(userId);
                    var oCompany = db.security_companies.Find(companyId);
                    var oUserCompany = new security_company_users();
                    oUserCompany.codigo_empresa = oCompany.codigo_empresa;
                    oUserCompany.user_name = oUser.username;
                    oUserCompany.application = Contants.APPLICATION;
                    oUserCompany.IdInternoSecurityUser = userId;
                    oUserCompany.idInternoSecurityCompany = companyId;
                    db.security_company_users.Add(oUserCompany);
                    db.SaveChanges();
                    userCompanyId = (from d in db.security_company_users
                                         where d.codigo_empresa == codigoEmpresa && d.IdInternoSecurityUser == userId
                                         select d.IdInternoSecurityCompanyUser).FirstOrDefault();
                }
                return Json(new { success = true, userCompanyId = userCompanyId });
            }
            catch(Exception e)
            {
                return Json(new { success = false, error = e.Message});
            }
        }

        [HttpPost]
        public ActionResult deleteCompany(int userCompanyId)
        {
            try
            {
                string codigoEmpresa;
                using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
                {
                    var oUserCompany = db.security_company_users.Find(userCompanyId);
                    codigoEmpresa = oUserCompany.codigo_empresa;
                    db.security_company_users.Remove(oUserCompany);
                    db.SaveChanges();
                }
                return Json(new { success = true, codigoEmpresa = codigoEmpresa });
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }

    }
}