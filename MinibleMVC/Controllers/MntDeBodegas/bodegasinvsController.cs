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

namespace Minible5.Controllers.MntDeBodegas
{
    public class bodegasinvsController : Controller
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

        // GET: bodegasinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }


        [HttpPost]
        public ActionResult GetBodegas()
        {
            List<TableBodegasViewModel> lst = new List<TableBodegasViewModel>();

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

            IQueryable<TableBodegasViewModel> query =
                (from d in db.bodegasinv                 
                 select new TableBodegasViewModel
                    {
                        IdInternoBodegas = d.IdInternoBodegas,
                        IdBodega = d.IdBodega,
                        Descripcion = d.Descripcion,
                        idcentrocosto = d.idcentrocosto,
                        status = d.status
                    });

            query = query.Where(d => d.status.Equals(vStatus));

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

        // GET: bodegasinvs/Create
        public ActionResult Create()
        {
            return View();
        }

     

        [HttpPost]
        public ActionResult Create(BodegasViewModels model)
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
                bodegasinv oBodegas = new bodegasinv();

                
                oBodegas.IdBodega = model.IdBodega;
                oBodegas.Descripcion = model.Descripcion;
                oBodegas.Encargado = model.Encargado;
                oBodegas.Direccion = model.Direccion;
                oBodegas.Telefono = model.Telefono;
                oBodegas.idcentrocosto = model.idcentrocosto;
                oBodegas.Tipo = model.Tipo;
                oBodegas.cta_bodega_inventario = model.cta_bodega_inventario;
                oBodegas.status = "A";
                oBodegas.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.bodegasinv.Add(oBodegas);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "bodegasinvs", new { success = "Se agregó correctamente!" });
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

            EditBodegasViewModel model = new EditBodegasViewModel();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;
           
            var oBodega = db.bodegasinv.Find(id);
            if (oBodega == null)
            {
                return HttpNotFound();
            }

            model.IdInternoBodegas = oBodega.IdInternoBodegas;
            model.IdBodega = oBodega.IdBodega;
            model.Descripcion = oBodega.Descripcion;
            model.Encargado = oBodega.Encargado;
            model.Direccion = oBodega.Direccion;
            model.Telefono = oBodega.Telefono;
            model.idcentrocosto = oBodega.idcentrocosto;
            model.Tipo = oBodega.Tipo;
            model.cta_bodega_inventario = oBodega.cta_bodega_inventario;
            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBodegasViewModel model)
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

                var oBodegas = db.bodegasinv.Find(model.IdInternoBodegas);
                
               // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oBodegas.IdBodega = model.IdBodega;
                oBodegas.Descripcion = model.Descripcion;
                oBodegas.Encargado = model.Encargado;
                oBodegas.Direccion = model.Direccion;
                oBodegas.Telefono = model.Telefono;
                oBodegas.idcentrocosto = model.idcentrocosto;
                oBodegas.Tipo = model.Tipo;
                oBodegas.cta_bodega_inventario = model.cta_bodega_inventario;

                db.Entry(oBodegas).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            
            
            return RedirectToAction("Index", "bodegasinvs", new { success = "Se editó correctamente!" });            
        }


        // POST : bodegasinvs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oBodegas = db.bodegasinv.Find(id);
            oBodegas.status = "B";
            oBodegas.Fecha_baja = DateTime.Now;

            db.Entry(oBodegas).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

       

       
    }
}
