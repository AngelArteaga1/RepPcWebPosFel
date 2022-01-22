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
using Minible5.Models.ViewModels.Clientes;

namespace Minible5.Controllers.MntDeClientes
{
    public class clientesController : Controller
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

        // GET: clientes
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetClientes()
        {
            List<TableClientesViewModel> lst = new List<TableClientesViewModel>();

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

            IQueryable<TableClientesViewModel> query =
                (from d in db.clientes
                 join v in db.vendedores on d.IdInternoVendedores equals v.IdInternoVendedores 
                 select new TableClientesViewModel
                 {
                     IdInternoClientes = d.IdInternoClientes,
                     idCliente = d.IdCliente,
                     nombreComercial = d.NombreComercial,
                     nit = d.nit,
                     vendedor = v.Descripcion,
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

        // GET: clientes/Create
        public ActionResult Create()
        {
            ClientesViewModels model = new ClientesViewModels();
            /*Inicializando por Default*/
            DateTime fechaHoy;
            fechaHoy = DateTime.Now;

            model.fechadealta = fechaHoy;
            model.diascredito = (int)valDefault;
            model.limitecredito = (decimal)valDefault;
            model.descprontopago = (decimal)valDefault;
            model.sexo = "M";
            model.diapago = "0";
            model.tipoprecio = "G";

            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsBodegas = getBodegas();
            ViewBag.itemsBodegas = itemsBodegas;

            var itemsClases = getClases();
            ViewBag.itemsClases = itemsClases;

            var itemsCobradores = getCobradores();
            ViewBag.itemsCobradores = itemsCobradores;

            var itemsVendedores = getVendedores();
            ViewBag.itemsVendedores = itemsVendedores;

            var itemsSectores = getSectores();
            ViewBag.itemsSectores = itemsSectores;

            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            var itemsTipoPrecio = getTipoPrecio();
            ViewBag.itemsTipoPrecio = itemsTipoPrecio;

            return View(model);
        }

        // POST: clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ClientesViewModels model)
        {  /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */


            //GUARDAMOS LOS Clients
            if (ModelState.IsValid)
            {
                clientes oClientes = new clientes();

                oClientes.IdCliente = model.idCliente;
                oClientes.NombreComercial = model.nombreComercial;
                oClientes.RazonSocial = model.razonSocial;
                oClientes.direccion = model.direccion;
                oClientes.Telefono = model.telefono;
                oClientes.Fax = model.fax;
                oClientes.ApartadoPostal = model.apartadopostal;
                oClientes.Cedula = model.cedula;
                oClientes.nit = model.nit;
                oClientes.FechadeAlta = model.fechadealta;
                oClientes.DiasCredito = model.diascredito;
                oClientes.LimiteCredito = model.limitecredito;
                oClientes.Observaciones = model.observaciones;
                oClientes.E_Mail = model.e_mail;
                oClientes.DiaPago = model.diapago;
                oClientes.PersonaContacto_1 = model.personacontacto_1;//Validar este campo Cuando se regenere denuevo la db, debido aque este campo no necesariamente tiene que tener valor
                oClientes.PersonaContacto_2 = model.personacontacto_2 ;                
                oClientes.DiasProntoPago = model.diasprontopago;
                oClientes.DescProntoPago = model.descprontopago;
                oClientes.TipoPrecio = model.tipoprecio;
                oClientes.CuentaContable = model.cuentacontable;
                oClientes.contador_luz = model.contador_luz;
                oClientes.sexo = model.sexo;
                oClientes.dias_max_credito = model.dias_max_credito;
                if (model.agenteretenedor)
                {
                    oClientes.agenteretenedor = "S";                     
                }
                else
                {
                    oClientes.agenteretenedor = "N";                    
                }

                oClientes.AltaBaja = model.altabaja;
                oClientes.IdInternoClases = model.idInternoClase;
                oClientes.IdInternoLocalidades = model.idInternoLocalidad;
                oClientes.IdInternoCobrador = model.idInternoCobrador;
                oClientes.IdInternoPaises = model.idInternoPais;
                oClientes.IdInternoVendedores = model.idInternoVendedor;
                //oClientes.IdInternoRutas = model.idInternoRuta;
                oClientes.IdInternoSectores = model.idInternoSectores;
                oClientes.IdInternoZonas = model.idInternoZona;
                oClientes.IdInternoBodegas = model.idInternoBodega;
                oClientes.AltaBaja = "A";
                oClientes.status = "A";
                oClientes.Codigo_Empresa = "001"; //Tomar en cuenta que este campo NO tiene que ser estatico y tiene que estar en los Modelos..
                db.clientes.Add(oClientes);
                db.SaveChanges();

                return RedirectToAction("Index", "clientes", new { success = "Se agregó correctamente!" });

            }
            else
            {
                return RedirectToAction("Index", "clientes", new { success = "Error Guardando en la Base de datos" });
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

            EditClientesViewModels model = new EditClientesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oClientes = db.clientes.Find(id);
            if (oClientes == null)
            {
                return HttpNotFound();
            }

            model.idInternoClientes = oClientes.IdInternoClientes;
            model.idCliente = oClientes.IdCliente;
            model.nombreComercial = oClientes.NombreComercial;
            model.razonSocial = oClientes.RazonSocial;
            model.direccion = oClientes.direccion;
            model.telefono = oClientes.Telefono;
            model.fax = oClientes.Fax;
            model.apartadopostal = oClientes.ApartadoPostal;
            model.cedula = oClientes.Cedula;
            model.nit = oClientes.nit;
            model.fechadealta = (DateTime)oClientes.FechadeAlta;
            model.diascredito = (int)oClientes.DiasCredito;
            model.limitecredito = (Decimal)oClientes.LimiteCredito;
            model.observaciones = oClientes.Observaciones;
            model.e_mail = oClientes.E_Mail;
            model.diapago = oClientes.DiaPago;
            model.personacontacto_1 = oClientes.PersonaContacto_1;
            model.personacontacto_2 = oClientes.PersonaContacto_2;
            //model.fechabaja = (DateTime)oClientes.FechaBaja;
            model.diasprontopago = (int)oClientes.DiasProntoPago;
            model.descprontopago = (decimal)oClientes.DescProntoPago;

            model.tipoprecio = oClientes.TipoPrecio;
            model.cuentacontable = oClientes.CuentaContable;
            model.contador_luz = oClientes.contador_luz;
            model.sexo = oClientes.sexo;
            model.dias_max_credito = oClientes.dias_max_credito;
            if (oClientes.agenteretenedor == "S")
            {
                model.agenteretenedor = true;
            }
            else
            {
                model.agenteretenedor = false;
            }
            
            model.altabaja = oClientes.AltaBaja;
            model.idInternoClase = oClientes.IdInternoClases;
            model.idInternoLocalidad = oClientes.IdInternoLocalidades;
            model.idInternoCobrador = oClientes.IdInternoCobrador;
            model.idInternoPais = oClientes.IdInternoPaises;
            model.idInternoVendedor = oClientes.IdInternoVendedores;
            //model.idInternoRuta = oClientes.IdInternoRutas;
            model.idInternoSectores = oClientes.IdInternoSectores;
            model.idInternoZona = oClientes.IdInternoZonas;
            model.idInternoBodega = oClientes.IdInternoBodegas;


            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsBodegas = getBodegas();
            ViewBag.itemsBodegas = itemsBodegas;

            var itemsClases = getClases();
            ViewBag.itemsClases = itemsClases;

            var itemsCobradores = getCobradores();
            ViewBag.itemsCobradores = itemsCobradores;

            var itemsVendedores = getVendedores();
            ViewBag.itemsVendedores = itemsVendedores;

            var itemsSectores = getSectores();
            ViewBag.itemsSectores = itemsSectores;
            
            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            var itemsTipoPrecio = getTipoPrecio();
            ViewBag.itemsTipoPrecio = itemsTipoPrecio;

            return View(model);
        }

        [HttpPost]        
        public ActionResult Edit(EditClientesViewModels model)
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

                var oClientes = db.clientes.Find(model.idInternoClientes);

                oClientes.IdCliente = model.idCliente;
                oClientes.NombreComercial = model.nombreComercial;
                oClientes.RazonSocial = model.razonSocial;
                oClientes.direccion = model.direccion;
                oClientes.Telefono = model.telefono;
                oClientes.Fax = model.fax;
                oClientes.ApartadoPostal = model.apartadopostal;
                oClientes.Cedula = model.cedula;
                oClientes.nit = model.nit;
                oClientes.FechadeAlta = model.fechadealta;
                oClientes.DiasCredito = model.diascredito;
                oClientes.LimiteCredito = model.limitecredito;
                oClientes.Observaciones = model.observaciones;
                oClientes.E_Mail = model.e_mail;
                oClientes.DiaPago = model.diapago;
                oClientes.PersonaContacto_1 = model.personacontacto_1;
                oClientes.PersonaContacto_2 = model.personacontacto_2;
                oClientes.DiasProntoPago = model.diasprontopago;
                oClientes.DescProntoPago = model.descprontopago;
                oClientes.TipoPrecio = model.tipoprecio;
                oClientes.CuentaContable = model.cuentacontable;
                oClientes.contador_luz = model.contador_luz;
                oClientes.sexo = model.sexo;
                oClientes.dias_max_credito = model.dias_max_credito;
                if (model.agenteretenedor)
                {
                    oClientes.agenteretenedor = "S";
                }
                else
                {
                    oClientes.agenteretenedor = "N";
                }

                oClientes.AltaBaja = model.altabaja;
                oClientes.IdInternoClases = model.idInternoClase;
                oClientes.IdInternoLocalidades = model.idInternoLocalidad;
                oClientes.IdInternoCobrador = model.idInternoCobrador;
                oClientes.IdInternoPaises = model.idInternoPais;
                oClientes.IdInternoVendedores = model.idInternoVendedor;
                //oClientes.IdInternoRutas = model.idInternoRuta;
                oClientes.IdInternoSectores = model.idInternoSectores;
                oClientes.IdInternoZonas = model.idInternoZona;
                oClientes.IdInternoBodegas = model.idInternoBodega;

                db.Entry(oClientes).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("Index", "clientes", new { success = "Se editó correctamente!" });
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

            EditClientesViewModels model = new EditClientesViewModels();
            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var oClientes = db.clientes.Find(id);
            if (oClientes == null)
            {
                return HttpNotFound();
            }

            model.idInternoClientes = oClientes.IdInternoClientes;
            model.idCliente = oClientes.IdCliente;
            model.nombreComercial = oClientes.NombreComercial;
            model.razonSocial = oClientes.RazonSocial;
            model.direccion = oClientes.direccion;
            model.telefono = oClientes.Telefono;
            model.fax = oClientes.Fax;
            model.apartadopostal = oClientes.ApartadoPostal;
            model.cedula = oClientes.Cedula;
            model.nit = oClientes.nit;
            model.fechadealta = (DateTime)oClientes.FechadeAlta;
            model.diascredito = (int)oClientes.DiasCredito;
            model.limitecredito = (Decimal)oClientes.LimiteCredito;
            model.observaciones = oClientes.Observaciones;
            model.e_mail = oClientes.E_Mail;
            model.diapago = oClientes.DiaPago;
            model.personacontacto_1 = oClientes.PersonaContacto_1;
            model.personacontacto_2 = oClientes.PersonaContacto_2;
            //model.fechabaja = (DateTime)oClientes.FechaBaja;
            model.diasprontopago = (int)oClientes.DiasProntoPago;
            model.descprontopago = (decimal)oClientes.DescProntoPago;

            model.tipoprecio = oClientes.TipoPrecio;
            model.cuentacontable = oClientes.CuentaContable;
            model.contador_luz = oClientes.contador_luz;
            model.sexo = oClientes.sexo;
            model.dias_max_credito = oClientes.dias_max_credito;
            if (oClientes.agenteretenedor == "S")
            {
                model.agenteretenedor = true;
            }
            else
            {
                model.agenteretenedor = false;
            }

            model.altabaja = oClientes.AltaBaja;
            model.idInternoClase = oClientes.IdInternoClases;
            model.idInternoLocalidad = oClientes.IdInternoLocalidades;
            model.idInternoCobrador = oClientes.IdInternoCobrador;
            model.idInternoPais = oClientes.IdInternoPaises;
            model.idInternoVendedor = oClientes.IdInternoVendedores;
            //model.idInternoRuta = oClientes.IdInternoRutas;
            model.idInternoSectores = oClientes.IdInternoSectores;
            model.idInternoZona = oClientes.IdInternoZonas;
            model.idInternoBodega = oClientes.IdInternoBodegas;


            var itemsPaises = getPaises();
            ViewBag.itemsPaises = itemsPaises;

            var itemsZonas = getZonas();
            ViewBag.itemsZonas = itemsZonas;

            var itemsLocalidades = getLocalidades();
            ViewBag.itemsLocalidades = itemsLocalidades;

            var itemsBodegas = getBodegas();
            ViewBag.itemsBodegas = itemsBodegas;

            var itemsClases = getClases();
            ViewBag.itemsClases = itemsClases;

            var itemsCobradores = getCobradores();
            ViewBag.itemsCobradores = itemsCobradores;

            var itemsVendedores = getVendedores();
            ViewBag.itemsVendedores = itemsVendedores;

            var itemsSectores = getSectores();
            ViewBag.itemsSectores = itemsSectores;

            var itemsDiaPago = getDiaPago();
            ViewBag.itemsDiaPago = itemsDiaPago;

            var itemsTipoPrecio = getTipoPrecio();
            ViewBag.itemsTipoPrecio = itemsTipoPrecio;

            return View(model);
        }



        // POST: clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var oClientes = db.clientes.Find(id);
            oClientes.status = "B";
            oClientes.Fecha_baja = DateTime.Now;

            db.Entry(oClientes).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        public List<SelectListItem> getPaises()
        {
            List<TableClientes_Paises> lst;

            lst =
                (from d in db.paises
                 select new TableClientes_Paises
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
            List<TableClientes_Zonas> lst;

            lst =
                (from d in db.zonas
                 select new TableClientes_Zonas
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
            List<TableClientes_Localidades> lst;

            lst =
                (from d in db.localidades
                 select new TableClientes_Localidades
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

        public List<SelectListItem> getBodegas()
        {
            List<TableClientes_Bodegas> lst;

            lst =
                (from d in db.bodegasinv
                 select new TableClientes_Bodegas
                 {
                     idInternoBodegas = d.IdInternoBodegas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoBodegas.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        

       public List<SelectListItem> getClases()
        {
            List<TableClientes_Clases> lst;

            lst =
                (from d in db.clases
                 select new TableClientes_Clases
                 {
                     idInternoClases = d.IdInternoClases,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoClases.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getCobradores()
        {
            List<TableClientes_Cobrador> lst;

            lst =
                (from d in db.cobradores
                 select new TableClientes_Cobrador
                 {
                     IdInternoCobrador = d.IdInternoCobrador,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.IdInternoCobrador.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getSectores()
        {
            List<TableClientes_Sectores> lst;

            lst =
                (from d in db.sectores
                 select new TableClientes_Sectores
                 {
                     idInternoSectores = d.IdInternoSectores,
                     descripcion = d.descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoSectores.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getVendedores()
        {
            List<TableClientes_Vendedores> lst;

            lst =
                (from d in db.vendedores
                 select new TableClientes_Vendedores
                 {
                     idInternoVendedores = d.IdInternoVendedores,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoVendedores.ToString(),
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

        public List<SelectListItem> getTipoPrecio()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "General", Value = "G" });
            lst.Add(new SelectListItem() { Text = "Drogeria", Value = "D" });
            lst.Add(new SelectListItem() { Text = "Farmacia", Value = "F" });
            lst.Add(new SelectListItem() { Text = "Menudeo", Value = "M" });          

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
