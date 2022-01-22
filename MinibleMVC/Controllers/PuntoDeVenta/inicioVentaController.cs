using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.PtoDeVenta;
using System.Linq.Dynamic;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class inicioVentaController : Controller
    {
        //VARIABLES GLOBALES PARA LA DATATABLE
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        // GET: puntoDeVenta
        public ActionResult Index(string error)
        {
            //Obtenemos las listas de datos
            ViewBag.vendedores = getVendedores();
            ViewBag.bodegas = getBodegas();
            ViewBag.tipos = new List<SelectListItem>();
            //Seteamos el posible error
            ViewBag.error = error;
            //Obtenemos la venta globalmente
            var model = Session["Sale"] as PtoDeVentaViewModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PtoDeVentaViewModel model)
        {
            List<string> PropertyNames = new List<string>() {"nit","nombre","email","direccion","vendedor","bodega","serie"};
            if (PropertyNames.Any(p => !ModelState.IsValidField(p)))
            {
                //Obtenemos las listas de datos
                ViewBag.vendedores = getVendedores();
                ViewBag.bodegas = getBodegas();
                ViewBag.tipos = new List<SelectListItem>();
                return View(model);
            }
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                var oCompany = (security_companies)Session["Company"];
                //Buscamos el nombre del vendedor
                var oVendedor = db.vendedores.Find(model.vendedor);
                model.nombreVendedor = oVendedor.Descripcion;
                //Buscamos el nombre de la serie
                var oSerie = db.tiposmovimientosseriesinv.Find(model.serie);
                model.nombreSerie = oSerie.IdSerie;
                //Buscamos el nombre del vendedor
                var oBodega = db.bodegasinv.Find(model.bodega);
                model.nombrebodega = oBodega.Descripcion;
                //Buscamos si existe el cliente, si si, seteamos el tipoPrecio
                var oCliente = getCliente(model.nit);
                if (oCliente == null)
                {
                    oCliente.IdCliente = model.nit;
                    oCliente.NombreComercial = model.nombre;
                    oCliente.RazonSocial = model.nombre;
                    oCliente.direccion = model.direccion;
                    oCliente.nit = model.nit;
                    oCliente.E_Mail = model.email;
                    oCliente.FechadeAlta = DateTime.Now;
                    oCliente.AltaBaja = "A";
                    oCliente.TipoPrecio = "G";
                    oCliente.IdInternoBodegas = model.bodega;
                    oCliente.Codigo_Empresa = oCompany.codigo_empresa;
                    db.clientes.Add(oCliente);
                    db.SaveChanges();
                }
                else
                {
                    bool actualizar = false;
                    if ((oCliente.NombreComercial != model.nombre ||
                        oCliente.nit != model.nit ||
                        oCliente.E_Mail != model.email ||
                        oCliente.direccion != model.direccion) &&
                        oCliente.nit.ToUpper() != "CF")
                        actualizar = true;

                    if (actualizar)
                    {
                        oCliente.IdCliente = model.nit;
                        oCliente.NombreComercial = model.nombre;
                        oCliente.direccion = model.direccion;
                        oCliente.nit = model.nit;
                        oCliente.E_Mail = model.email;
                        db.Entry(oCliente).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                model.tipoPrecio = oCliente.TipoPrecio;

                //Metemos el encabezado del pedido
                var oPedidosClientes = new pedidosclientesinv();
                oPedidosClientes.Fecha = DateTime.Now;
                oPedidosClientes.NombreCliente = model.nombre;
                oPedidosClientes.direccion = model.direccion;
                oPedidosClientes.nit = model.nit;
                oPedidosClientes.Anulado = "N";
                oPedidosClientes.ClienteProveedor = oCliente.IdInternoClientes.ToString();
                oPedidosClientes.TasaCambio = 1;
                oPedidosClientes.status = "A";

                oPedidosClientes.Codigo_Empresa = oCompany.codigo_empresa;
                oPedidosClientes.IdInternoTIposMovimientosSeries = model.serie;
                oPedidosClientes.IdInternoVendedores = model.vendedor;
                oPedidosClientes.IdInternoBodegas = model.bodega;
                oPedidosClientes.facturado = "N";
                savePedido(oPedidosClientes, model);
            }
            Session["Sale"] = model;
            return RedirectToAction("Index", "carrito");
        }


        /***************************** PETICIONES DE DATOS *****************************/
        [HttpPost]
        public ActionResult getTiposSeries(int? idInternoBodega)
        {
            if (idInternoBodega == null)
                return Json(new List<SelectListItem>());

            var oCompany = Session["Company"] as security_companies;
            var model = Session["Sale"] as PtoDeVentaViewModel;
            List<SelectListItem> items;

            using (var db = new db_pcsolutions_webEntities())
            {
                var oBodega = db.bodegasinv.Find(idInternoBodega);
                if (oBodega.Usa_multiseries == "S")
                {
                    items =
                        (from d in db.tiposmovimientosinv
                         join e in db.tiposmovimientosseriesinv
                         on d.IdInternoTiposMovimientos equals e.IdInternoTiposMovimientos
                         where d.Codigo_Empresa == oCompany.codigo_empresa &&
                         e.IdInternoBodegas == (int)idInternoBodega &&
                         (d.IdTipoMovimiento == "FA" || d.IdTipoMovimiento == "EN")
                         select new SelectListItem
                         {
                             Value = e.IdInternoTIposMovimientosSeries.ToString(),
                             Text = "TIPO: " + d.IdTipoMovimiento + " | SERIE: " + e.IdSerie,
                             Selected = false
                         }).ToList();
                } else
                {
                    items =
                        (from d in db.tiposmovimientosinv
                         join e in db.tiposmovimientosseriesinv
                         on d.IdInternoTiposMovimientos equals e.IdInternoTiposMovimientos
                         where d.Codigo_Empresa == oCompany.codigo_empresa &&
                         e.IdInternoBodegas == (int)idInternoBodega &&
                         e.IdSerie == oBodega.Serie_fac_env &&
                         (d.IdTipoMovimiento == "FA" || d.IdTipoMovimiento == "EN")
                         select new SelectListItem
                         {
                             Value = e.IdInternoTIposMovimientosSeries.ToString(),
                             Text = "TIPO: " + d.IdTipoMovimiento + " | SERIE: " + e.IdSerie,
                             Selected = false
                         }).ToList();
                }
                if(items.Count == 1)
                    items[0].Selected = true;
                if(model != null)
                {
                    for(var i = 0; i < items.Count; i++)
                    {
                        if (items[i].Value == model.serie.ToString())
                            items[i].Selected = true;
                    }
                }
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetClients()
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

            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                IQueryable<TableClientesViewModel> query =
                    (from d in db.clientes
                     select new TableClientesViewModel
                     {
                         nit = d.nit,
                         nit2 = d.nit,
                         nombre = d.NombreComercial
                     });
                //Searching by name
                if (searchValue != "")
                {
                    query = query.Where(d => d.nombre.Contains(searchValue) || d.nit.Contains(searchValue));
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
        }
        [HttpPost]
        public ActionResult GetClient(string nit)
        {
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                //Obtenemos el cliente por medio del nit
                var oClient = (from d in db.clientes
                               where d.nit == nit
                               select d).FirstOrDefault();
                //Si el cliente no existe solo mandamos un falso en la respuesta
                if (oClient == null)
                {
                    return Json(new
                    {
                        success = false
                    }, JsonRequestBehavior.AllowGet);
                }
                //Si existe enviamos sus datos
                else
                {
                    return Json(new
                    {
                        success = true,
                        nombre = oClient.NombreComercial,
                        email = oClient.E_Mail,
                        direccion = oClient.direccion
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        /***************** ESTAS FUNCIONES SON PROCESOS DE LA TRANSACCION *****************/
        public void savePedido(pedidosclientesinv oPedidosClientes, PtoDeVentaViewModel model)
        {
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                using(var transaccion = db.Database.BeginTransaction())
                {
                    oPedidosClientes.Numdoc = getNumdocPedidos(model);
                    try
                    {
                        db.pedidosclientesinv.Add(oPedidosClientes);
                        db.SaveChanges();
                        transaccion.Commit();
                        model.numdoc = oPedidosClientes.Numdoc;

                    }
                    catch
                    {
                        savePedido(oPedidosClientes, model);
                    }
                }
            }
        }


        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELVEN LISTAS *****************/
        public List<SelectListItem> getVendedores()
        {
            var oCompany = Session["Company"] as security_companies;
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.vendedores
                     where d.Codigo_Empresa == oCompany.codigo_empresa
                     select new SelectListItem
                     {
                         Value = d.IdInternoVendedores.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }
        public List<SelectListItem> getBodegas()
        {
            var oCompany = Session["Company"] as security_companies;
            var oUser = Session["User"] as security_users;
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                if (oUser.Acceso_multibodegas == "N")
                {
                    items =
                        (from d in db.bodegasinv
                         where d.Codigo_Empresa == oCompany.codigo_empresa &&
                         d.IdInternoBodegas == oUser.IdInternoBodegas
                         select new SelectListItem
                         {
                             Value = d.IdInternoBodegas.ToString(),
                             Text = d.Descripcion,
                             Selected = true
                         }).ToList();
                }
                else
                {
                    items =
                        (from d in db.bodegasinv
                         where d.Codigo_Empresa == oCompany.codigo_empresa
                         select new SelectListItem
                         {
                             Value = d.IdInternoBodegas.ToString(),
                             Text = d.Descripcion,
                             Selected = false
                         }).ToList();
                }
                if (items.Count == 1)
                    items[0].Selected = true;
            }
            return items;
        }


        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELDEN DATOS *****************/
        public clientes getCliente(string nit)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return
                    (from d in db.clientes
                     where d.nit == nit
                     select d).FirstOrDefault();
            }
        }
        public string getNumdocPedidos(PtoDeVentaViewModel model)
        {
            var oCompany = (security_companies)Session["Company"];
            string correlativo;
            using (var db = new db_pcsolutions_webEntities())
            {
                correlativo = (from d in db.pedidosclientesinv
                               where d.Codigo_Empresa == oCompany.codigo_empresa &&
                                   d.IdInternoTIposMovimientosSeries == model.serie
                               select d.Numdoc).DefaultIfEmpty("0").Max();
            }
            return (Int32.Parse(correlativo) + 1).ToString("D10");
        }
    }
}