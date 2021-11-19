using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels;

namespace Minible5.Controllers.Tables
{
    public class TablesDatatableController : Controller
    {
        // GET: TablesDatatable
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult GetList()
        {
            System.Diagnostics.Debug.WriteLine("My debug string here");
            using (db_testmvcEntities db = new db_testmvcEntities())
            {
                var empList = db.empleados.ToList<empleados>();
                var lst = (from d in db.empleados
                       select new TableEmpleadosViewModel
                       {
                           Name = d.Name,
                           Position = d.Position,
                           Office = d.Office,
                           Age = (int)d.Age,
                           StartDate = (d.StartDate).ToString(),
                           Salary = (int)d.Salary
                       }).ToList();
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
            }
        }

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        /*[HttpPost]
        public ActionResult  ()
        {
            List<TableEmpleadosViewModel> lst = new List<TableEmpleadosViewModel>();

            //Logistica datatable
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][Column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            using (db_testmvcEntities db = new db_testmvcEntities())
            {
                lst = (from d in db.empleados
                       select new TableEmpleadosViewModel
                       {
                           Name = d.Name,
                           Position = d.Position,
                           Office = d.Office,
                           Age = (int)d.Age,
                           StartDate = (d.StartDate).ToString(),
                           Salary = (int)d.Salary
                       }).ToList();
                recordsTotal = lst.Count();

                lst = lst.Skip(skip).Take(pageSize).ToList();
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = lst });
        }*/

    }
}