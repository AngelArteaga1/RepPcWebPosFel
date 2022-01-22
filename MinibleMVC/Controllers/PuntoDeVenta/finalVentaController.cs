using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models.ViewModels.PtoDeVenta;
using Minible5.Models;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class finalVentaController : Controller
    {
        // GET: finalVenta
        public ActionResult Index()
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            var articulos = getArticulos(model);
            var bancos = getBancos();
            var tarjetas = getTarjetas();
            ViewBag.articulos = articulos;
            ViewBag.bancos = bancos;
            ViewBag.tarjetas = tarjetas;
            return View(model);
        }

        public List<SelectListItem> getTarjetas()
        {
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.tarjetasbancos
                     where d.Codigo_empresa == oCompany.codigo_empresa && d.TarjetaBanco == "T"
                     select new SelectListItem
                     {
                         Value = d.IdInternoTarjetasBancos.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }

        public List<SelectListItem> getBancos()
        {
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.tarjetasbancos
                     where d.Codigo_empresa == oCompany.codigo_empresa && d.TarjetaBanco == "B"
                     select new SelectListItem
                     {
                         Value = d.IdInternoTarjetasBancos.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }

        public List<ListItemsViewModel> getArticulos(PtoDeVentaViewModel model)
        {
            var oCompany = (security_companies)Session["Company"];
            List<ListItemsViewModel> items = new List<ListItemsViewModel>();
            //Reseteamos cada vez el monto final
            using (var db = new db_pcsolutions_webEntities())
            {
                foreach (var i in model.articulos)
                {
                    var item = (from d in db.articulosinv
                                join e in db.articulosdetalleinv
                                on d.IdInternoArticulos equals e.IdInternoArticulos
                                join b in db.medidasinv
                                on d.IdInternoMedidas equals b.IdInternoMedidas
                                where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                                && d.IdInternoArticulos == i.Key
                                select new ListItemsViewModel
                                {
                                    id = i.Key,
                                    descripcion = d.NombreArticulo,
                                    foto = d.foto,
                                    unidadMedida = b.Descripcion,
                                    existencia = e.UnidadesIniciales + e.UnidadesEntrantes - e.UnidadesSalientes,
                                    unidades = i.Value.unidades,
                                    precio = i.Value.precio,
                                    descuento = i.Value.descuento,
                                    descuentoPrecio = i.Value.descuentoPrecio
                                }).FirstOrDefault();
                    //Calculamos el subtotal
                    item.subtotal = (item.precio * item.unidades) - item.descuentoPrecio;
                    items.Add(item);
                }
            }
            return items;
        }
    }
}