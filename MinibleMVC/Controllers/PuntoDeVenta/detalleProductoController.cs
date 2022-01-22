using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.PtoDeVenta;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class detalleProductoController : Controller
    {
        // GET: detalleProducto
        public ActionResult Index(int id)
        {
            //Obtenemos el modelo por medio del id
            var oCompany = (security_companies)Session["Company"];
            var model = Session["Sale"] as PtoDeVentaViewModel;
            ListItemsViewModel item;
            using (var db = new db_pcsolutions_webEntities())
            {
                item = (from d in db.articulosinv
                    join e in db.articulosdetalleinv
                        on d.IdInternoArticulos equals e.IdInternoArticulos
                    join b in db.medidasinv
                        on d.IdInternoMedidas equals b.IdInternoMedidas
                    join a in db.marcasinv
                        on d.IdInternoMarcas equals a.IdInternoMarcas
                    join i in db.familiasinv
                        on d.IdInternoFamilias equals i.IdInternoFamilias
                    where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                    && d.IdInternoArticulos == id
                    select new ListItemsViewModel
                    {
                        id = id,
                        descripcion = d.NombreArticulo,
                        compDescripcion = d.compDescripcion,
                        foto = d.foto,
                        unidadMedida = b.Descripcion,
                        existencia = e.UnidadesIniciales + e.UnidadesEntrantes - e.UnidadesSalientes,
                        precio1 = d.precioventa_1_1,
                        precio2 = d.precioventa_2_2,
                        descuento1 = d.PorcentajeDescuento1_1,
                        descuento2 = d.PorcentajeDescuento2_2,
                        marca = a.Descripcion,
                        familia = i.Descripcion
                    }).FirstOrDefault();
                if(model.tipoPrecio == "G")
                {
                    item.precio = item.precio1;
                    item.descuento = item.descuento1;
                }
                else
                {
                    item.precio = item.precio2;
                    item.descuento = item.descuento2;
                }
                var articulo = new Articulo(item.id, item.precio, 1, item.descuento);
                item.descuentoPrecio = articulo.descuentoPrecio;
                item.subtotal = articulo.total;
            }
            var existencias = getExistencias(id);
            ViewBag.existencias = existencias;
            return View(item);
        }
        public List<ListBodegasViewModel> getExistencias(int id)
        {
            var oCompany = (security_companies)Session["Company"];
            List<ListBodegasViewModel> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.articulosdetalleinv
                     join a in db.bodegasinv
                        on d.IdInternoBodegas equals a.IdInternoBodegas 
                     where d.Codigo_Empresa == oCompany.codigo_empresa && d.IdInternoArticulos == id
                     select new ListBodegasViewModel {
                        bodega = a.Descripcion,
                        existencia = d.UnidadesIniciales + d.UnidadesEntrantes - d.UnidadesSalientes
                     }).ToList();
            }
            return items;
        }
    }
}