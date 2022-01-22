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
using Minible5.Models.ViewModels.Marcas;

namespace Minible5.Controllers.MntDeMarcas
{
    public class marcasinvsController : Controller
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

        // GET: marcasinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetMarcas()
        {
            List<TableMarcasViewModel> lst = new List<TableMarcasViewModel>();

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

            IQueryable<TableMarcasViewModel> query =
                (from d in db.marcasinv
                 select new TableMarcasViewModel
                 {
                     idInternoMarcas = d.IdInternoMarcas,
                     idMarca = d.IdMarca,
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


        // GET: marcasinvs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: marcasinvs/Create
        [HttpPost]
        public ActionResult Create(MarcasViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS LA BODEGA
            if (ModelState.IsValid)
            {
                marcasinv oMarcas = new marcasinv();

                oMarcas.IdMarca = model.idMarca;
                oMarcas.Descripcion = model.descripcion;                
                oMarcas.status = "A";
                oMarcas.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.marcasinv.Add(oMarcas);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "marcasinvs", new { success = "Se agregó correctamente!" });
        }



        // GET: marcasinvs/Edit/5       
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

            EditMarcasViewModel model = new EditMarcasViewModel();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oMarca = db.marcasinv.Find(id);
            if (oMarca == null)
            {
                return HttpNotFound();
            }

            model.idInternoMarcas = oMarca.IdInternoMarcas;
            model.idMarca = oMarca.IdMarca;
            model.descripcion = oMarca.Descripcion;                        

            return View(model);
        }



        // POST: marcasinvs/Edit/5
        [HttpPost]        
        public ActionResult Edit(EditMarcasViewModel model)
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
                var oMarcas = db.marcasinv.Find(model.idInternoMarcas);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oMarcas.IdMarca = model.idMarca;
                oMarcas.Descripcion = model.descripcion;
               
                db.Entry(oMarcas).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "marcasinvs", new { success = "Se editó correctamente!" });
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

            EditMarcasViewModel model = new EditMarcasViewModel();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oMarca = db.marcasinv.Find(id);
            if (oMarca == null)
            {
                return HttpNotFound();
            }

            model.idInternoMarcas = oMarca.IdInternoMarcas;
            model.idMarca = oMarca.IdMarca;
            model.descripcion = oMarca.Descripcion;

            return View(model);
        }

        // POST: marcasinvs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oMarcas = db.marcasinv.Find(id);
            oMarcas.status = "B";
            oMarcas.Fecha_baja = DateTime.Now;

            db.Entry(oMarcas).State = System.Data.Entity.EntityState.Modified;
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
