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


namespace Minible5.Controllers.MntDeMedidas
{
    public class medidasinvsController : Controller
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

        // GET: medidasinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetMedidas()
        {
            List<TableMedidasViewModel> lst = new List<TableMedidasViewModel>();

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

            IQueryable<TableMedidasViewModel> query =
                (from d in db.medidasinv
                 select new TableMedidasViewModel
                 {
                     idInternoMedidas = d.IdInternoMedidas,
                     idMedidas = d.IdMedida,
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


        // GET: medidasinvs/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MedidasViewModels model)
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
                medidasinv oMedidas = new medidasinv();

                oMedidas.IdMedida = model.idMedidas;
                oMedidas.Descripcion = model.descripcion;
                oMedidas.fel_medida = model.felMedida;
                oMedidas.status = "A";
                oMedidas.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.medidasinv.Add(oMedidas);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "medidasinvs", new { success = "Se agregó correctamente!" });
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

            EditMedidasViewModel model = new EditMedidasViewModel();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oMedidas = db.medidasinv.Find(id);
            if (oMedidas == null)
            {
                return HttpNotFound();
            }

            model.idInternoMedidas = oMedidas.IdInternoMedidas;
            model.idMedidas = oMedidas.IdMedida;
            model.descripcion = oMedidas.Descripcion;
            model.felMedida = oMedidas.fel_medida;           
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMedidasViewModel model)
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

                var oMedidas = db.medidasinv.Find(model.idInternoMedidas);
                //oMedidas.IdInternoMedidas = model.idInternoMedidas
                oMedidas.IdMedida = model.idMedidas;
                oMedidas.Descripcion = model.descripcion;
                oMedidas.fel_medida = model.felMedida;
              
                db.Entry(oMedidas).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "medidasinvs", new { success = "Se editó correctamente!" });
        }

        // POST : bodegasinvs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oMedidas = db.medidasinv.Find(id);
            oMedidas.status = "B";
            oMedidas.Fecha_baja = DateTime.Now;

            db.Entry(oMedidas).State = System.Data.Entity.EntityState.Modified;
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
