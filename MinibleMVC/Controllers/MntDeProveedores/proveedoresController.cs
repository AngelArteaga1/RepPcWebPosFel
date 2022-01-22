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
using Minible5.Models.ViewModels.Proveedores;

namespace Minible5.Controllers.MntDeProveedores
{
    public class proveedoresController : Controller
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

        // GET: proveedores
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetProveedores()
        {
            List<TableProveedoresViewModel> lst = new List<TableProveedoresViewModel>();

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

            IQueryable<TableProveedoresViewModel> query =
                (from d in db.proveedores
                 select new TableProveedoresViewModel
                 {
                     idInternoProveedores = d.IdInternoProveedores,
                     idProveedor = d.IdProveedor,
                     nombreComercial = d.NombreComercial,
                     nit = d.Nit,
                     status = d.status
                 });

            query = query.Where(d => d.status.Equals(vStatus));

            //Searching by name
            if (searchValue != "")
            {
                query = query.Where(d => d.nombreComercial.Contains(searchValue) || d.nombreComercial.Contains(searchValue));
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

        // GET: proveedores/Create
        public ActionResult Create()
        {
            ProveedoresViewModels model = new ProveedoresViewModels();
            /*Inicializando por Default*/
            DateTime fechaHoy;
            fechaHoy = DateTime.Now;

            model.fechaAlta = fechaHoy;
            model.diasCredito = (decimal)valDefault;
            model.limiteCredito = (decimal)valDefault;
            model.saldoAnterior = (decimal)valDefault;
            model.diasProntoPago = (decimal)valDefault;
            model.descProntoPago = (decimal)valDefault;
            model.montoPorAplicar = (decimal)valDefault;
            model.montoProvicional = (decimal)valDefault;
            model.diaPago = "0";

            var itemsClasesProveedores = getClasesProveedores();
            ViewBag.itemsClasesProveedores = itemsClasesProveedores;

            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            return View(model);
        }


        [HttpPost]
        public ActionResult Create(ProveedoresViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS LOS PROVEEDORES
            if (ModelState.IsValid)
            {
                proveedores oProveedores = new proveedores();

                oProveedores.IdProveedor = model.idProveedor;
                oProveedores.NombreComercial = model.nombreComercial;
                oProveedores.RazonSocial = model.razonSocial;
                oProveedores.Direccion =  model.direccion;
                oProveedores.Telefono  = model.telefono;
                oProveedores.Fax = model.fax;
                oProveedores.ApartadoPostal = model.apartadoPostal;
                oProveedores.Cedula = model.cedula;
                oProveedores.Nit = model.nit;
                oProveedores.FechadeAlta  = model.fechaAlta;
                oProveedores.DiasCredito  = model.diasCredito;
                oProveedores.LimiteCredito = model.limiteCredito;
                oProveedores.Observaciones = model.observaciones;
                oProveedores.E_Mail  = model.email;
                oProveedores.DiaPago = model.diaPago;
                oProveedores.PersonaContacto_1 = model.personaContacto1;
                oProveedores.PersonaContacto_2 =  model.personaContacto2;
                oProveedores.DiasProntoPago  = model.diasProntoPago;
                oProveedores.DescProntoPago = model.descProntoPago;
                oProveedores.Idcuenta = model.idCuenta;
                oProveedores.IdInternoClasesProveedores =  model.idInternoClasesProveedores;
                oProveedores.IdInternoPaises = model.idInternoPaises;
                oProveedores.IdInternoZonas =  model.idInternoZonas;
                oProveedores.IdInternoLocalidades = model.idInternoLocalidades;
                oProveedores.status = "A";
                oProveedores.Codigo_empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.proveedores.Add(oProveedores);
                db.SaveChanges();

                return RedirectToAction("Index", "proveedores", new { success = "Se agregó correctamente!" });

            }
            else
            {
                return RedirectToAction("Index", "proveedores", new { success = "Error Guardando en la Base de datos" });
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

            EditProveedoresViewModels model = new EditProveedoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oProveedor = db.proveedores.Find(id);
            if (oProveedor == null)
            {
                return HttpNotFound();
            }

            model.idInternoProveedores = oProveedor.IdInternoProveedores;
            model.idProveedor = oProveedor.IdProveedor;
            model.nombreComercial = oProveedor.NombreComercial;
            model.razonSocial = oProveedor.RazonSocial;
            model.direccion = oProveedor.Direccion;
            model.telefono = oProveedor.Telefono;
            model.fax = oProveedor.Fax;
            model.apartadoPostal = oProveedor.ApartadoPostal;
            model.cedula = oProveedor.Cedula;
            model.nit = oProveedor.Nit;
            model.fechaAlta = (DateTime)oProveedor.FechadeAlta;
            model.diasCredito = (decimal)oProveedor.DiasCredito;
            model.limiteCredito = (decimal)oProveedor.LimiteCredito;
            model.observaciones = oProveedor.Observaciones;
            model.email = oProveedor.E_Mail;
            model.diaPago = oProveedor.DiaPago;
            model.personaContacto1 = oProveedor.PersonaContacto_1;
            model.personaContacto2 = oProveedor.PersonaContacto_2;
            model.altaBaja = oProveedor.AltaBaja;
            model.diasProntoPago = (decimal)oProveedor.DiasProntoPago;
            model.descProntoPago = (decimal)oProveedor.DescProntoPago;
            model.idCuenta = oProveedor.Idcuenta;
            model.idInternoClasesProveedores = oProveedor.IdInternoClasesProveedores;
            model.idInternoPaises = oProveedor.IdInternoPaises;
            model.idInternoZonas = oProveedor.IdInternoZonas;
            model.idInternoLocalidades = oProveedor.IdInternoLocalidades;

            var itemsClasesProveedores = getClasesProveedores();
            ViewBag.itemsClasesProveedores = itemsClasesProveedores;

            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            return View(model);
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

            EditProveedoresViewModels model = new EditProveedoresViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oProveedor = db.proveedores.Find(id);
            if (oProveedor == null)
            {
                return HttpNotFound();
            }

            model.idInternoProveedores = oProveedor.IdInternoProveedores;
            model.idProveedor = oProveedor.IdProveedor;
            model.nombreComercial = oProveedor.NombreComercial;
            model.razonSocial = oProveedor.RazonSocial;
            model.direccion = oProveedor.Direccion;
            model.telefono = oProveedor.Telefono;
            model.fax = oProveedor.Fax;
            model.apartadoPostal = oProveedor.ApartadoPostal;
            model.cedula = oProveedor.Cedula;
            model.nit = oProveedor.Nit;
            model.fechaAlta = (DateTime)oProveedor.FechadeAlta;
            model.diasCredito = (decimal)oProveedor.DiasCredito;
            model.limiteCredito = (decimal)oProveedor.LimiteCredito;
            model.observaciones = oProveedor.Observaciones;
            model.email = oProveedor.E_Mail;
            model.diaPago = oProveedor.DiaPago;
            model.personaContacto1 = oProveedor.PersonaContacto_1;
            model.personaContacto2 = oProveedor.PersonaContacto_2;
            model.altaBaja = oProveedor.AltaBaja;
            model.diasProntoPago = (decimal)oProveedor.DiasProntoPago;
            model.descProntoPago = (decimal)oProveedor.DescProntoPago;
            model.idCuenta = oProveedor.Idcuenta;
            model.idInternoClasesProveedores = oProveedor.IdInternoClasesProveedores;
            model.idInternoPaises = oProveedor.IdInternoPaises;
            model.idInternoZonas = oProveedor.IdInternoZonas;
            model.idInternoLocalidades = oProveedor.IdInternoLocalidades;

            var itemsClasesProveedores = getClasesProveedores();
            ViewBag.itemsClasesProveedores = itemsClasesProveedores;

            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            return View(model);
        }


        [HttpPost]        
        public ActionResult Edit(EditProveedoresViewModels model)
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

                var oProveedores = db.proveedores.Find(model.idInternoProveedores);

                // oBodegas.IdInternoBodegas = model.IdInternoBodegas;
                oProveedores.IdProveedor = model.idProveedor;

                oProveedores.NombreComercial = model.nombreComercial;
                oProveedores.RazonSocial = model.razonSocial;
                oProveedores.Direccion = model.direccion;
                oProveedores.Telefono = model.telefono;
                oProveedores.Fax = model.fax;
                oProveedores.ApartadoPostal = model.apartadoPostal;
                oProveedores.Cedula = model.cedula;
                oProveedores.Nit = model.nit;
                oProveedores.FechadeAlta = model.fechaAlta;
                oProveedores.DiasCredito = model.diasCredito;
                oProveedores.LimiteCredito = model.limiteCredito;
                oProveedores.Observaciones = model.observaciones;
                oProveedores.E_Mail = model.email;
                oProveedores.DiaPago = model.diaPago;
                oProveedores.PersonaContacto_1 = model.personaContacto1;
                oProveedores.PersonaContacto_2 = model.personaContacto2;
                oProveedores.AltaBaja = model.altaBaja;
                oProveedores.DiasProntoPago = model.diasProntoPago;
                oProveedores.DescProntoPago = model.descProntoPago;
                oProveedores.Idcuenta = model.idCuenta;
                oProveedores.IdInternoClasesProveedores = model.idInternoClasesProveedores;
                oProveedores.IdInternoPaises = model.idInternoPaises;
                oProveedores.IdInternoZonas = model.idInternoZonas;
                oProveedores.IdInternoLocalidades = model.idInternoLocalidades;


                db.Entry(oProveedores).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "proveedores", new { success = "Se editó correctamente!" });
        }


      

        // POST: proveedores/Delete/5
        [HttpPost, ActionName("Delete")]        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oPRoveedores = db.proveedores.Find(id);
            oPRoveedores.status = "B";
            oPRoveedores.Fecha_baja = DateTime.Now;

            db.Entry(oPRoveedores).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }



        public List<SelectListItem> getClasesProveedores()
        {
            List<TableProveedores_ClasesProveedores> lst;

            lst =
                (from d in db.clasesproveedores
                 select new TableProveedores_ClasesProveedores
                 {
                     idInternoClasesProveedores = d.IdInternoClasesProveedores,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoClasesProveedores.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getPaises()
        {
            List<TableProveedores_Paises> lst;

            lst =
                (from d in db.paises
                 select new TableProveedores_Paises
                 {
                     idInternoPaises = d.IdInternoPaises,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoPaises.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getZonas()
        {
            List<TableProveedores_Zonas> lst;

            lst =
                (from d in db.zonas
                 select new TableProveedores_Zonas
                 {
                     IdInternoZonas = d.IdInternoZonas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.IdInternoZonas.ToString(),
                    Selected = false
                };
            });
            return items;
        }


        public List<SelectListItem> getLocalidades()
        {
            List<TableProveedores_Localidades> lst;

            lst =
                (from d in db.localidades
                 select new TableProveedores_Localidades
                 {
                     IdInternoLocalidades = d.IdInternoLocalidades,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.IdInternoLocalidades.ToString(),
                    Selected = false
                };
            });
            return items;
        }


        public List<SelectListItem> getDiaPago()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();

            //De la siguiente manera llenamos manualmente,
            //Siendo el campo Text lo que ve el usuario y
            //el campo Value lo que en realidad vale nuestro valor
            lst.Add(new SelectListItem() { Text = "Sin dia", Value = "0" });
            lst.Add(new SelectListItem() { Text = "Lunes", Value = "1" });
            lst.Add(new SelectListItem() { Text = "Martes", Value = "2" });
            lst.Add(new SelectListItem() { Text = "Miercoles", Value = "3" });
            lst.Add(new SelectListItem() { Text = "Jueves", Value = "4" });
            lst.Add(new SelectListItem() { Text = "Viernes", Value = "5" });
            lst.Add(new SelectListItem() { Text = "Sabado", Value = "6" });
            lst.Add(new SelectListItem() { Text = "Domingo", Value = "7" });

            return lst;
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
