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
using Minible5.Models.ViewModels.Paises;

namespace Minible5.Controllers.MntDePaises
{
    public class paisesController : Controller
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

        // GET: paises
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }


        [HttpPost]
        public ActionResult GetPaises()
        {
            List<TablePaisesViewModel> lst = new List<TablePaisesViewModel>();

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

            IQueryable<TablePaisesViewModel> query =
                (from d in db.paises
                 select new TablePaisesViewModel
                 {
                     idInternoPaises = d.IdInternoPaises,
                     idPais = d.IdPais,
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


        // GET: paises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: paises/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(PaisesViewModels model)
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
                paises oPais = new paises();

                oPais.IdPais = model.idPais;
                oPais.Descripcion = model.descripcion;
                oPais.status = "A";
                oPais.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.paises.Add(oPais);
                db.SaveChanges();

            }


            return RedirectToAction("Index", "paises", new { success = "Se agregó correctamente!" });
        }

        // GET: paises/Edit/5
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

            EditPaisesViewModels model = new EditPaisesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oPais = db.paises.Find(id);
            if (oPais == null)
            {
                return HttpNotFound();
            }

            model.idInternoPaises = oPais.IdInternoPaises;
            model.idPais = oPais.IdPais;
            model.descripcion = oPais.Descripcion;

            return View(model);
        }

        // POST: paises/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPaisesViewModels model)
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
                var oPais = db.paises.Find(model.idInternoPaises);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oPais.IdPais = model.idPais;
                oPais.Descripcion = model.descripcion;

                db.Entry(oPais).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return RedirectToAction("Index", "paises", new { success = "Se editó correctamente!" });
        }



        // POST: paises/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oPaises = db.paises.Find(id);
            oPaises.status = "B";
            oPaises.Fecha_baja = DateTime.Now;

            db.Entry(oPaises).State = System.Data.Entity.EntityState.Modified;
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
