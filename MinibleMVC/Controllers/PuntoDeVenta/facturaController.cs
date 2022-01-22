using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Minible5.Models.ViewModels.PtoDeVenta;
using Minible5.Models;
using System.Web.Mvc;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class facturaController : Controller
    {
        // GET: factura
        public ActionResult Index(int id)
        {
            security_companies oCompany = Session["Company"] as security_companies;
            FacturaViewModel model = new FacturaViewModel();
            string uuid;
            using (var db = new db_pcsolutions_webEntities())
            {
                var oMovimientoInv = db.movimientosinv.Find(id);
                var oPedidoCliente = db.pedidosclientesinv.Find(oMovimientoInv.IdInternoPedidosClientes);

                var oSerie = db.tiposmovimientosseriesinv.Find(oMovimientoInv.IdInternoTIposMovimientosSeries);
                var oTipo = db.tiposmovimientosinv.Find(oSerie.IdInternoTiposMovimientos);
                uuid = oMovimientoInv.fel_uuid;

                model.numdocFEL = oMovimientoInv.fel_numero;
                if (String.IsNullOrEmpty(model.numdocFEL))
                    model.numdocFEL = oMovimientoInv.Numdoc;
                model.serieFEL = oMovimientoInv.fel_serie;
                if (String.IsNullOrEmpty(model.serieFEL))
                    model.serieFEL = oSerie.IdSerie;
                model.numeroAutorizacion = oMovimientoInv.fel_uuid;
                model.refInterna = oTipo.IdTipoMovimiento + " " + oSerie.IdSerie + " " + oMovimientoInv.Numdoc;
                model.fecha = oMovimientoInv.Fecha.ToString();
                model.numdocOrden = oPedidoCliente.Numdoc;

                model.nitEmpresa = oCompany.fel_emisor_nit;
                model.direccionEmpresa = oCompany.fel_emisor_direccion;
                model.correoEmpresa = oCompany.fel_emisor_correo_emisor;
                model.telefonoEmpresa = oCompany.telefono1;

                model.nombreCliente = oMovimientoInv.Nombre;
                model.nitCliente = oMovimientoInv.nit;
                model.correoCliente = oMovimientoInv.correoReceptor;
                model.direccionCliente = oMovimientoInv.direccion;

                //Ahora llenamos la lista de articulos
                var detalles = getMovimientosDetalleInv(id);
                model.descuento = oMovimientoInv.Descuento;
                model.total = oMovimientoInv.Monto;
                model.subtotal = 0;
                foreach(var detalle in detalles)
                {
                    var oArticuloDetalle = db.articulosdetalleinv.Find(detalle.IdInternoArticulosDetalle);
                    var oArticulo = db.articulosinv.Find(oArticuloDetalle.IdInternoArticulos);
                    var articulo = new DetalleFactura(oArticulo.NombreArticulo, oArticulo.IdArticulo, detalle.Precio_unitario, detalle.Unidades);
                    model.articulos.Add(articulo);
                    model.subtotal += detalle.Precio_unitario * (decimal)detalle.Unidades;
                }
                model.vuelto = getVuelto(oMovimientoInv.Numdoc, oMovimientoInv.IdInternoTIposMovimientosSeries, oCompany.codigo_empresa);
            }
            if(!String.IsNullOrEmpty(uuid))
                ViewBag.url = "https://report.feel.com.gt/ingfacereport/ingfacereport_documento?uuid=" + uuid;
            return View(model);
        }


        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELVEN LISTAS *****************/
        public List<movimientosdetalleinv> getMovimientosDetalleInv(int idInternoMovimientosInv)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return (from d in db.movimientosdetalleinv
                 where d.IdInternoMovimientos == idInternoMovimientosInv
                 select d).ToList();
            }
        }


        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELDEN DATOS *****************/
        public decimal? getVuelto(string numdoc, int idInternoTipoSerie, string codigoEmpresa)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return (from d in db.movimientosdetallecaja
                        where d.Numdoc == numdoc &&
                        d.IdInternoTIposMovimientosSeries == idInternoTipoSerie &&
                        d.Codigo_Empresa == codigoEmpresa &&
                        d.EntradaSalida == "V"
                        select d.Monto).DefaultIfEmpty(0).FirstOrDefault();
            }
        }
    }
}