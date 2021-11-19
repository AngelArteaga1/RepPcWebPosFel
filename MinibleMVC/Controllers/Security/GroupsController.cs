using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels;
using System.Linq.Dynamic;

namespace Minible5.Controllers.Security
{
    public class GroupsController : Controller
    {
        // GET: Groups
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

        [HttpPost]
        public ActionResult GetDatatable()
        {
            List<TableGroupsViewModel> lst = new List<TableGroupsViewModel>();

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
                IQueryable<TableGroupsViewModel> query = (from d in db.security_groups
                                                         select new TableGroupsViewModel
                                                         {
                                                             id = d.IdInternoSecurityGroup,
                                                             name = d.name,
                                                             created = d.created.ToString()
                                                         });
                //Searching by name
                if (searchValue != "")
                {
                    query = query.Where(d => d.name.Contains(searchValue));
                }
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    query = query.OrderBy(sortColumn + " " + sortColumnDir);
                }
                recordsTotal = query.Count();
                lst = query.Skip(skip).Take(pageSize).ToList();
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = lst
                });
            }
        }

        [HttpPost]
        public ActionResult GetDatatableDetail(int id)
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
                     where d.IdInternoSecurityGroup == id
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
                if (searchValue != "")
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
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = lst
                });
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(GroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                //GUARDAMOS EL USUARIO
                security_groups oGroup = new security_groups();
                oGroup.name = model.Name;
                oGroup.created = DateTime.Now;
                oGroup.modified = DateTime.Now;
                oGroup.status = "A";
                db.security_groups.Add(oGroup);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Groups", new { success = "Se agregó correctamente!" });
        }

        public ActionResult Edit(int id)
        {
            EditGroupViewModel model = new EditGroupViewModel();
            using (var db = new db_pcsolutions_webEntities())
            {
                var oGroup = db.security_groups.Find(id);
                model.Id = id;
                model.Name = oGroup.name;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                //GUARDAMOS EL USUARIO
                var oGroup = db.security_groups.Find(model.Id);
                oGroup.name = model.Name;
                oGroup.modified = DateTime.Now;
                db.Entry(oGroup).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Groups", new { success = "Se editó correctamente!" });
        }
    }
}