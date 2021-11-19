﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using System.Linq.Dynamic;
using Minible5.Models.ViewModels.Rutas;
namespace Minible5.Controllers.MntDeRutas
{
    public class rutasController : Controller
    {
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;
        public string vStatus = "A";
        

        private db_pcsolutions_webEntities db = new db_pcsolutions_webEntities();

        // GET: rutas
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetRutas()
        {
            List<TableRutasViewModel> lst = new List<TableRutasViewModel>();

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

            IQueryable<TableRutasViewModel> query =
                (from d in db.rutas
                 select new TableRutasViewModel
                 {
                     idInternoRutas = d.IdInternoRutas,
                     idRuta = d.IdRuta,                     
                     descripcion = d.Descripcion,
                     recorrido = d.Recorrido,
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



        // GET: rutas/Create
        public ActionResult Create()
        {            
            return View();
        }



        // POST: rutas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(RutasViewModels model)
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
                rutas oRutas = new rutas();

                oRutas.IdRuta = model.idRuta;
                oRutas.Descripcion = model.descripcion;
                oRutas.Recorrido = model.recorrido;
                oRutas.status = "A";
                oRutas.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.rutas.Add(oRutas);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "rutas", new { success = "Se agregó correctamente!" });
        }



        // GET: rutas/Edit/5        
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

            EditRutasViewModels model = new EditRutasViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oRutas = db.rutas.Find(id);
            if (oRutas == null)
            {
                return HttpNotFound();
            }

            model.idInternoRutas = oRutas.IdInternoRutas;
            model.idRuta = oRutas.IdRuta;
            model.descripcion = oRutas.Descripcion;
            model.recorrido = oRutas.Recorrido;

            return View(model);
        }


        // POST: rutas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRutasViewModels model)
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
                var oRutas = db.rutas.Find(model.idInternoRutas);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oRutas.IdRuta = model.idRuta;
                oRutas.Descripcion = model.descripcion;
                oRutas.Recorrido = model.recorrido;

                db.Entry(oRutas).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "rutas", new { success = "Se editó correctamente!" });
        }



        // POST: rutas/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oRutas = db.rutas.Find(id);
            oRutas.status = "B";
            oRutas.Fecha_baja = DateTime.Now;

            db.Entry(oRutas).State = System.Data.Entity.EntityState.Modified;
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
