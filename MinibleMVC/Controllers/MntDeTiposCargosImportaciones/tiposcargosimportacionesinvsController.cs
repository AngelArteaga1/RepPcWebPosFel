using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using System.Linq.Dynamic;
using Minible5.Models.ViewModels.TiposCargosImportaciones;

namespace Minible5.Controllers.MntDeTiposCargosImportaciones
{
    public class tiposcargosimportacionesinvsController : Controller
    {
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;
        public string vStatus = "A";
        public decimal valDefault = 0;

        private db_pcsolutions_webEntities db = new db_pcsolutions_webEntities();

        // GET: tiposcargosimportacionesinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetCargosImportaciones()
        {
            List<TableCargosImportacionesViewModel> lst = new List<TableCargosImportacionesViewModel>();

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

            IQueryable<TableCargosImportacionesViewModel> query =
                (from d in db.tiposcargosimportacionesinv
                 select new TableCargosImportacionesViewModel
                 {
                     idInternoTipCargImportaciones = d.IdInternoTipCargImportaciones,
                     idtipocargo = d.idtipocargo,
                     descripcion = d.Descripcion,
                     localDolares = d.LocalDolares,
                     status = d.status
                 });

            query = query.Where(d => d.status.Equals(vStatus));

            //Searching by name
            if (searchValue != "")
            {
                query = query.Where(d => d.descripcion.Contains(searchValue) || d.descripcion.Contains(searchValue));
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

        // GET: tiposcargosimportacionesinvs/Create
        public ActionResult Create()
        {
            var itemsLocalDolares = getLocalDolares();
            ViewBag.itemsLocalDolares = itemsLocalDolares;
            return View();
        }


        // POST: tiposcargosimportacionesinvs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(tiposcargosimportacionesinv model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS el SECTOR
            if (ModelState.IsValid)
            {
                tiposcargosimportacionesinv oTCargoImportacion = new tiposcargosimportacionesinv();

                oTCargoImportacion.idtipocargo = model.idtipocargo;
                oTCargoImportacion.Descripcion = model.Descripcion;
                oTCargoImportacion.LocalDolares = model.LocalDolares;
                oTCargoImportacion.status = "A";
                oTCargoImportacion.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.tiposcargosimportacionesinv.Add(oTCargoImportacion);
                db.SaveChanges();

            }

            return RedirectToAction("Index", "tiposcargosimportacionesinvs", new { success = "Se agregó correctamente!" });
        }


        // GET: tiposcargosimportacionesinvs/Edit/5
        public ActionResult Edit(int? id)
        {
            /*          
            ViewBag.items = items;
            var companies = getCompanies();
            ViewBag.companies = companies; */

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditCargosImportacionesViewModels model = new EditCargosImportacionesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oTCargoImportacion = db.tiposcargosimportacionesinv.Find(id);
            if (oTCargoImportacion == null)
            {
                return HttpNotFound();
            }

            model.idInternoTipCargImportaciones = oTCargoImportacion.IdInternoTipCargImportaciones;
            model.idtipocargo = oTCargoImportacion.idtipocargo;
            model.descripcion = oTCargoImportacion.Descripcion;
            model.localDolares = oTCargoImportacion.LocalDolares;

            var itemsLocalDolares = getLocalDolares();
            ViewBag.itemsLocalDolares = itemsLocalDolares;

            return View(model);
        }

        // POST: tiposcargosimportacionesinvs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCargosImportacionesViewModels model)
        {
            /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */

            if (ModelState.IsValid)
            {
                var oTCargoImportacion = db.tiposcargosimportacionesinv.Find(model.idInternoTipCargImportaciones);

                oTCargoImportacion.idtipocargo = model.idtipocargo;
                oTCargoImportacion.Descripcion = model.descripcion;
                oTCargoImportacion.LocalDolares = model.localDolares;

                db.Entry(oTCargoImportacion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return RedirectToAction("Index", "tiposcargosimportacionesinvs", new { success = "Se editó correctamente!" });
        }



        // POST: tiposcargosimportacionesinvs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oTCargoImportacion = db.tiposcargosimportacionesinv.Find(id);
            oTCargoImportacion.status = "B";
            oTCargoImportacion.Fecha_baja = DateTime.Now;

            db.Entry(oTCargoImportacion).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }


        public List<SelectListItem> getLocalDolares()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "Local", Value = "L" });
            lst.Add(new SelectListItem() { Text = "Dolares", Value = "D" });


            return lst;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
