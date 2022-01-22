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
using Minible5.Models.ViewModels;


namespace Minible5.Controllers.MntDeFamilias
{
    public class familiasinvsController : Controller
    {
        private const double valDefault = 0.0000;

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

        // GET: familiasinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetFamilias()
        {
            List<TableFamiliasViewModel> lst = new List<TableFamiliasViewModel>();

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

            IQueryable<TableFamiliasViewModel> query =
                (from d in db.familiasinv
                 select new TableFamiliasViewModel
                 {
                     IdInternoFamilias = d.IdInternoFamilias,
                     IdFamilia = d.IdFamilia,
                     Descripcion = d.Descripcion,
                     Status = d.status
                 }); 

            query = query.Where(d => d.Status.Equals(vStatus));

            //Searching by name
            if (searchValue != "")
            {
                query = query.Where(d => d.Descripcion.Contains(searchValue) || d.Descripcion.Contains(searchValue));
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


        // GET: familiasinvs/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(FamiliasViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS LA FAMILIA
            if (ModelState.IsValid)
            {
                familiasinv oFamilias = new familiasinv();


                oFamilias.IdFamilia = model.idFamilia;
                oFamilias.Descripcion = model.descripcion;
                oFamilias.Cta_Ventas = model.ctaVentas;
                oFamilias.Cta_Costo = model.ctaCostos;
                oFamilias.Cta_Inventario = model.ctaInventario;
                oFamilias.Cta_Impuesto = model.ctaImpuesto;
                oFamilias.Cta_Rebaja = model.ctaRebaja;                
                oFamilias.cta_prodproceso = model.ctaProdProceso;                
                oFamilias.status = "A";
                oFamilias.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.familiasinv.Add(oFamilias);
                db.SaveChanges();

                return RedirectToAction("Index", "familiasinvs", new { success = "Se agregó correctamente!" });
                
            }
            else
            {
                return RedirectToAction("Index", "familiasinvs", new { success = "Error Guardando en la Base de datos" });
            }
            
        }



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

            EditFamiliasViewModels model = new EditFamiliasViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oFamilia = db.familiasinv.Find(id);
            if (oFamilia == null)
            {
                return HttpNotFound();
            }
            model.IdInternoFamilias = oFamilia.IdInternoFamilias;
            model.IdFamilia = oFamilia.IdFamilia;
            model.descripcion = oFamilia.Descripcion;
            model.ctaVentas = oFamilia.Cta_Ventas;
            model.ctaCostos = oFamilia.Cta_Costo;
            model.ctaInventario = oFamilia.Cta_Inventario;
            model.ctaImpuesto = oFamilia.Cta_Impuesto;
            model.ctaRebaja = oFamilia.Cta_Rebaja;            
            model.ctaCostoExcento  = oFamilia.cta_costos_exento;
            model.ctaProdProceso = oFamilia.cta_prodproceso;            


            return View(model);
        }


        [HttpPost]        
        public ActionResult Edit(EditFamiliasViewModels model)
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

                var oFamilia = db.familiasinv.Find(model.IdInternoFamilias);

                oFamilia.IdInternoFamilias = model.IdInternoFamilias;
                oFamilia.IdFamilia = model.IdFamilia;
                oFamilia.Descripcion = model.descripcion;
                oFamilia.Cta_Ventas = model.ctaVentas;
                oFamilia.Cta_Costo = model.ctaCostos;
                oFamilia.Cta_Inventario = model.ctaInventario;
                oFamilia.Cta_Impuesto = model.ctaImpuesto;
                oFamilia.Cta_Rebaja = model.ctaRebaja;                
                oFamilia.cta_costos_exento = model.ctaCostoExcento;
                oFamilia.cta_prodproceso = model.ctaProdProceso;                


                db.Entry(oFamilia).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "familiasinvs", new { success = "Se editó correctamente!" });
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

            EditFamiliasViewModels model = new EditFamiliasViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oFamilia = db.familiasinv.Find(id);
            if (oFamilia == null)
            {
                return HttpNotFound();
            }
            model.IdInternoFamilias = oFamilia.IdInternoFamilias;
            model.IdFamilia = oFamilia.IdFamilia;
            model.descripcion = oFamilia.Descripcion;
            model.ctaVentas = oFamilia.Cta_Ventas;
            model.ctaCostos = oFamilia.Cta_Costo;
            model.ctaInventario = oFamilia.Cta_Inventario;
            model.ctaImpuesto = oFamilia.Cta_Impuesto;
            model.ctaRebaja = oFamilia.Cta_Rebaja;            
            model.ctaCostoExcento = oFamilia.cta_costos_exento;
            model.ctaProdProceso = oFamilia.cta_prodproceso;            


            return View(model);
        }

        // POST : bodegasinvs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oFamilias = db.familiasinv.Find(id);
            oFamilias.status = "B";
            oFamilias.Fecha_baja = DateTime.Now;

            db.Entry(oFamilias).State = System.Data.Entity.EntityState.Modified;
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
