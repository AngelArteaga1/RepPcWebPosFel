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
        // GET: puntoDeVenta
        public ActionResult Index()
        {
            var vendedores = getVendedores();
            var bodegas = getBodegas();
            var tipos = getTipo();
            ViewBag.vendedores = vendedores;
            ViewBag.bodegas = bodegas;
            ViewBag.tipos = tipos;
            //Obtenemos la venta globalmente
            var model = Session["Sale"] as PtoDeVentaViewModel;
            return View(model);
        }

        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        public List<SelectListItem> getVendedores()
        {
            var oCompany = (security_companies)Session["Company"];
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
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
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
            return items;
        }

        public List<SelectListItem> getTipo()
        {
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.tiposmovimientosinv
                     join e in db.tiposmovimientosseriesinv
                     on d.IdInternoTiposMovimientos equals e.IdInternoTiposMovimientos
                     where d.Codigo_Empresa == oCompany.codigo_empresa && d.IdTipoMovimiento == "FA"
                     select new SelectListItem
                     {
                         Value = e.IdInternoTIposMovimientosSeries.ToString(),
                         Text = "TIPO: " + d.IdTipoMovimiento + " | SERIE: " + e.IdSerie,
                         Selected = false
                     }).ToList();
            }
            return items;
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
        public ActionResult Index(PtoDeVentaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
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
                model.tipoPrecio = (from d in db.clientes
                               where d.nit == model.nit
                               select d.TipoPrecio).FirstOrDefault();
                if(model.tipoPrecio == null)
                {
                    model.tipoPrecio = "G";
                }
            }
            Session["Sale"] = model;
            return RedirectToAction("Index", "carrito");
        }
    }
}