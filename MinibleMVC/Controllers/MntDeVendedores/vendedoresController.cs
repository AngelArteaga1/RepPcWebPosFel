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
using Minible5.Models.ViewModels.Vendedores;

namespace Minible5.Controllers.MntDeVendedores
{
    public class vendedoresController : Controller
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

        // GET: vendedores
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetVendedores()
        {
            List<TableVendedoresViewModel> lst = new List<TableVendedoresViewModel>();

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

            IQueryable<TableVendedoresViewModel> query =
                (from d in db.vendedores
                 join r in db.rutas on d.IdInternoRutas equals r.IdInternoRutas
                 select new TableVendedoresViewModel
                 {
                     idInternoVendedores = d.IdInternoVendedores,
                     idVendedor = d.IdVendedor,
                     descripcion = d.Descripcion,
                     ruta = r.Descripcion,
                     porcentajeComision = (decimal)d.Porcentajecomision,                     
                     emailVende = d.emailvende,
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



        // GET: vendedores/Create
        public ActionResult Create()
        {
            VendedoresViewModels model = new VendedoresViewModels();

            model.porcentajeComision = (decimal)valDefault;
            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }


        // POST: vendedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(VendedoresViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS el VENDEDOR         
            if (ModelState.IsValid)
            {
                vendedores oVendedor = new vendedores();

                oVendedor.IdVendedor = model.idVendedor;
                oVendedor.Descripcion = model.descripcion;
                oVendedor.Porcentajecomision = model.porcentajeComision;
                oVendedor.Telefono = model.telefono;
                oVendedor.Celular = model.celular;
                oVendedor.emailvende = model.emailVende;
                oVendedor.IdInternoRutas = model.idInternoRutas;
                oVendedor.status = "A";
                oVendedor.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.vendedores.Add(oVendedor);
                db.SaveChanges();

            }

            return RedirectToAction("Index", "vendedores", new { success = "Se agregó correctamente!" });
        }



        // GET: vendedores/Edit/5
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

            EditVendedoresViewModels model = new EditVendedoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oVendedor = db.vendedores.Find(id);
            if (oVendedor == null)
            {
                return HttpNotFound();
            }

            model.idInternoVendedores = oVendedor.IdInternoVendedores;
            model.idVendedor = oVendedor.IdVendedor;
            model.descripcion = oVendedor.Descripcion;
            model.porcentajeComision = (decimal)oVendedor.Porcentajecomision;
            model.telefono = oVendedor.Telefono;
            model.celular = oVendedor.Celular;
            model.emailVende = oVendedor.emailvende;
            model.idInternoRutas = oVendedor.IdInternoRutas;

            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }



        // POST: vendedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public ActionResult Edit(EditVendedoresViewModels model)
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
                var oVendedor = db.vendedores.Find(model.idInternoVendedores);

                oVendedor.IdVendedor= model.idVendedor;                
                oVendedor.Descripcion = model.descripcion;
                oVendedor.Porcentajecomision = model.porcentajeComision;
                oVendedor.Telefono = model.telefono;
                oVendedor.Celular = model.celular;
                oVendedor.emailvende = model.emailVende;
                oVendedor.IdInternoRutas = model.idInternoRutas;

                db.Entry(oVendedor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "vendedores", new { success = "Se editó correctamente!" });
        }


        // GET: vendedores/Edit/5
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

            EditVendedoresViewModels model = new EditVendedoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oVendedor = db.vendedores.Find(id);
            if (oVendedor == null)
            {
                return HttpNotFound();
            }

            model.idInternoVendedores = oVendedor.IdInternoVendedores;
            model.idVendedor = oVendedor.IdVendedor;
            model.descripcion = oVendedor.Descripcion;
            model.porcentajeComision = (decimal)oVendedor.Porcentajecomision;
            model.telefono = oVendedor.Telefono;
            model.celular = oVendedor.Celular;
            model.emailVende = oVendedor.emailvende;
            model.idInternoRutas = oVendedor.IdInternoRutas;

            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }


        // POST: vendedores/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oVendedor = db.vendedores.Find(id);
            oVendedor.status = "B";
            oVendedor.Fecha_baja = DateTime.Now;

            db.Entry(oVendedor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }




        public List<SelectListItem> getRutas()
        {
            List<TableVendedores_Rutas> lst;

            lst =
                (from d in db.rutas
                 select new TableVendedores_Rutas
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
