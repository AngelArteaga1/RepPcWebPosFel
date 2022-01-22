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
using Minible5.Models.ViewModels.Localidades;

namespace Minible5.Controllers.MntDeLocalidades
{
    public class localidadesController : Controller
    {
        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;
        public string vStatus = "A";
        private db_pcsolutions_webEntities db = new db_pcsolutions_webEntities();

        // GET: localidades
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }


        [HttpPost]
        public ActionResult GetLocalidades()
        {
            List<TableLocalidadesViewModel> lst = new List<TableLocalidadesViewModel>();

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

            IQueryable<TableLocalidadesViewModel> query =
                (from d in db.localidades
                 select new TableLocalidadesViewModel
                 {
                     idInternoLocalidades = d.IdInternoLocalidades,
                     idLocalidad = d.IdLocalidad,
                     descripcion = d.Descripcion,
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


        // GET: localidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: localidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(LocalidadesViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS LA LOCALIDAD
            if (ModelState.IsValid)
            {
                localidades oLocalidad = new localidades();

                oLocalidad.IdLocalidad = model.idLocalidad;
                oLocalidad.Descripcion = model.descripcion;
                oLocalidad.status = "A";
                oLocalidad.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.localidades.Add(oLocalidad);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "localidades", new { success = "Se agregó correctamente!" });
        }


        // GET: localidades/Edit/5
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

            EditLocalidadesViewModels model = new EditLocalidadesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oLocalidad = db.localidades.Find(id);
            if (oLocalidad == null)
            {
                return HttpNotFound();
            }

            model.idInternoLocalidades = oLocalidad.IdInternoLocalidades;
            model.idLocalidad = oLocalidad.IdLocalidad;
            model.descripcion = oLocalidad.Descripcion;

            return View(model);
        }


        // POST: localidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public ActionResult Edit(EditLocalidadesViewModels model)
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
                var oLocalidad = db.localidades.Find(model.idInternoLocalidades);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oLocalidad.IdLocalidad = model.idLocalidad;
                oLocalidad.Descripcion = model.descripcion;

                db.Entry(oLocalidad).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "localidades", new { success = "Se editó correctamente!" });
        }

        public ActionResult Details(int? id)
        {
            /*          
            ViewBag.items = items;
            var companies = getCompanies();
            ViewBag.companies = companies; */

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditLocalidadesViewModels model = new EditLocalidadesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oLocalidad = db.localidades.Find(id);
            if (oLocalidad == null)
            {
                return HttpNotFound();
            }

            model.idInternoLocalidades = oLocalidad.IdInternoLocalidades;
            model.idLocalidad = oLocalidad.IdLocalidad;
            model.descripcion = oLocalidad.Descripcion;

            return View(model);
        }


        // POST: localidades/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oLocalidades = db.localidades.Find(id);
            oLocalidades.status = "B";
            oLocalidades.Fecha_baja = DateTime.Now;

            db.Entry(oLocalidades).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

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
