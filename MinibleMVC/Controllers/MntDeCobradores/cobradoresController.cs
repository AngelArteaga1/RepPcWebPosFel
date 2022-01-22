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
using Minible5.Models.ViewModels.Cobradores;

namespace Minible5.Controllers.MntDeCobradores
{
    
    public class cobradoresController : Controller
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
        public decimal valDefault = 0;

        private db_pcsolutions_webEntities db = new db_pcsolutions_webEntities();

        // GET: cobradores
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetCobradores()
        {
            List<TableCobradoresViewModel> lst = new List<TableCobradoresViewModel>();

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

            IQueryable<TableCobradoresViewModel> query =
                (from d in db.cobradores
                 join r in db.rutas on d.IdInternoRutas equals r.IdInternoRutas
                 select new TableCobradoresViewModel
                 {
                     idInternoCobrador = d.IdInternoCobrador,
                     idCobrador = d.IdCobrador,
                     descripcion = d.Descripcion,
                     porcentajeComision = (decimal)d.Porcentajecomision,
                     rutas = r.Descripcion,
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




        // GET: cobradores/Create
        public ActionResult Create()
        {
            CobradoresViewModels model = new CobradoresViewModels();

            model.porcentajeComision = (decimal)valDefault;
            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);

        }

        // POST: cobradores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(CobradoresViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS el COBRADOR
            if (ModelState.IsValid)
            {
                cobradores oCobrador = new cobradores();

                oCobrador.IdCobrador = model.idCobrador;
                oCobrador.Descripcion = model.descripcion;
                oCobrador.Porcentajecomision = model.porcentajeComision;
                oCobrador.IdInternoRutas = model.IdInternoRutas;
                oCobrador.status = "A";
                oCobrador.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.cobradores.Add(oCobrador);
                db.SaveChanges();

            }

            return RedirectToAction("Index", "cobradores", new { success = "Se agregó correctamente!" });
        }


        // GET: cobradores/Edit/5
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

            EditCobradoresViewModels model = new EditCobradoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oCobrador = db.cobradores.Find(id);
            if (oCobrador == null)
            {
                return HttpNotFound();
            }

            model.idInternoCobrador = oCobrador.IdInternoCobrador;
            model.idCobrador = oCobrador.IdCobrador;
            model.descripcion = oCobrador.Descripcion;
            model.porcentajeComision = (decimal)oCobrador.Porcentajecomision;
            model.IdInternoRutas = oCobrador.IdInternoRutas;

            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }
        

        // POST: cobradores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public ActionResult Edit(EditCobradoresViewModels model)
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
                var oCobrador = db.cobradores.Find(model.idInternoCobrador);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oCobrador.IdCobrador = model.idCobrador;
                oCobrador.Descripcion = model.descripcion;
                oCobrador.Porcentajecomision = model.porcentajeComision;
                oCobrador.IdInternoRutas = model.IdInternoRutas;

                db.Entry(oCobrador).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "cobradores", new { success = "Se editó correctamente!" });
        }


        // GET: cobradores/Edit/5
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

            EditCobradoresViewModels model = new EditCobradoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oCobrador = db.cobradores.Find(id);
            if (oCobrador == null)
            {
                return HttpNotFound();
            }

            model.idInternoCobrador = oCobrador.IdInternoCobrador;
            model.idCobrador = oCobrador.IdCobrador;
            model.descripcion = oCobrador.Descripcion;
            model.porcentajeComision = (decimal)oCobrador.Porcentajecomision;
            model.IdInternoRutas = oCobrador.IdInternoRutas;

            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }


        // POST: cobradores/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oCobradores = db.cobradores.Find(id);
            oCobradores.status = "B";
            oCobradores.Fecha_baja = DateTime.Now;

            db.Entry(oCobradores).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }


        public List<SelectListItem> getRutas()
        {
            List<TableCobradores_Rutas> lst;

            lst =
                (from d in db.rutas
                 select new TableCobradores_Rutas
                 {
                     idInternoRutas = d.IdInternoRutas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoRutas.ToString(),
                    Selected = false
                };
            });
            return items;
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
