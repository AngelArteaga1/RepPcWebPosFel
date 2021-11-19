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
using Minible5.Models.ViewModels.Sectores;

namespace Minible5.Controllers.MntDeSectores
{
    public class sectoresController : Controller
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

        // GET: sectores
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetSectores()
        {
            List<TableSectoresViewModel> lst = new List<TableSectoresViewModel>();

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

            IQueryable<TableSectoresViewModel> query =
                (from d in db.sectores
                 join r in db.rutas on d.IdInternoRutas equals r.IdInternoRutas
                 select new TableSectoresViewModel
                 {
                     idInternoSectores = d.IdInternoSectores,
                     IdSector = d.IdSector,
                     descripcion = d.descripcion,
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


        // GET: sectores/Create
        public ActionResult Create()
        {            
            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View();
        }

        // POST: sectores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(SectoresViewModels model)
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
                sectores oSector = new sectores();

                oSector.IdSector = model.IdSector;
                oSector.descripcion = model.descripcion;                
                oSector.IdInternoRutas = model.IdInternoRutas;
                oSector.status = "A";
                oSector.codigo_empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.sectores.Add(oSector);
                db.SaveChanges();

            }

            return RedirectToAction("Index", "sectores", new { success = "Se agregó correctamente!" });
        }



        // GET: sectores/Edit/5
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

            EditSectoresViewModels model = new EditSectoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oSector = db.sectores.Find(id);
            if (oSector == null)
            {
                return HttpNotFound();
            }

            model.idInternoSectores = oSector.IdInternoSectores;
            model.IdSector = oSector.IdSector;
            model.descripcion = oSector.descripcion;
            model.IdInternoRutas = oSector.IdInternoRutas;

            var itemsRutas = getRutas();
            ViewBag.itemsRutas = itemsRutas;

            return View(model);
        }


        // POST: sectores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditSectoresViewModels model)
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
                var oSector = db.sectores.Find(model.idInternoSectores);                

                oSector.IdSector = model.IdSector;
                oSector.descripcion = model.descripcion;
                oSector.IdInternoRutas = model.IdInternoRutas;

                db.Entry(oSector).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            return RedirectToAction("Index", "sectores", new { success = "Se editó correctamente!" });
        }



        // POST: sectores/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oSector = db.sectores.Find(id);
            oSector.status = "B";
            oSector.Fecha_baja = DateTime.Now;

            db.Entry(oSector).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }



        public List<SelectListItem> getRutas()
        {
            List<TableSectores_Rutas> lst;

            lst =
                (from d in db.rutas
                 select new TableSectores_Rutas
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
