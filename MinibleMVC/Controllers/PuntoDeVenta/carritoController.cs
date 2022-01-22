using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.PtoDeVenta;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class carritoController : Controller
    {
        // GET: carrito
        public ActionResult Index()
        {
            //Obtenemos la venta globalmente
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model == null)
            {
                model = new PtoDeVentaViewModel();
            }
            //Ahora hacemos la lista de articulos
            var articulos = getArticulos(model);
            ViewBag.articulos = articulos;
            return View(model);
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
                    item.subtotal = (item.precio * item.unidades)-item.descuentoPrecio;
                    items.Add(item);
                }
            }
            return items;
        }

        //Esta accion es la encargada de recibir un articulo por id y sumarlo al carrito
        [HttpPost]
        public ActionResult AddProduct(int id)
        {
            var oCompany = Session["Company"] as security_companies;
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                //Ahora revisamos si ya existe en nuestro diccionario el item
                if (model.articulos.ContainsKey(id))
                {
                    model.articulos[id].agregarUnidad();
                }
                else
                {
                    //Si no existe, tenemos que obtener el precio del producto
                    using (var db = new db_pcsolutions_webEntities())
                    {
                        var item = (from d in db.articulosinv
                                    join e in db.articulosdetalleinv
                                    on d.IdInternoArticulos equals e.IdInternoArticulos
                                    join b in db.medidasinv
                                    on d.IdInternoMedidas equals b.IdInternoMedidas
                                    where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                                    && d.IdInternoArticulos == id
                                    select new ListItemsViewModel
                                    {
                                        unidades = 1,
                                        precio1 = d.precioventa_1_1,
                                        precio2 = d.precioventa_2_2,
                                        precio3 = d.preciomenudeo,
                                        descuento1 = d.PorcentajeDescuento1_1,
                                        descuento2 = d.PorcentajeDescuento2_2,
                                        descuento3 = d.PorcentajeDescuento3_3,
                                    }).FirstOrDefault();
                        //Calculos de campos no constantes
                        if (model.tipoPrecio == "G")
                        {
                            item.precio = item.precio1;
                            item.descuento = item.descuento1;
                        }
                        else
                        {
                            item.precio = item.precio2;
                            item.descuento = item.descuento2;
                        }
                        model.articulos.Add(id, new Articulo(id, item.precio, item.unidades, item.descuento));
                    }
                }

                //Actualizamos el total
                model.subtotal = getSubTotal(model.articulos);
                model.descuento = getDescuentoTotal(model.articulos);
                model.total = getTotal(model.articulos);
                
                //Actualizamos la variable global
                Session["Sale"] = model;

                return Json(new{success = true}, JsonRequestBehavior.AllowGet);
            } 
            else
            {
                return Json(new{success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddProductByCode(string codigoBarras)
        {
            var oCompany = Session["Company"] as security_companies;
            var model = Session["Sale"] as PtoDeVentaViewModel;
            ListItemsViewModel item = new ListItemsViewModel();
            if (model != null)
            {
                using (var db = new db_pcsolutions_webEntities())
                {
                    //Obtenemos el id del producto mediante si codigo de barras
                    var id = (from d in db.articulosinv
                                join e in db.articulosdetalleinv
                                on d.IdInternoArticulos equals e.IdInternoArticulos
                                join b in db.medidasinv
                                on d.IdInternoMedidas equals b.IdInternoMedidas
                                where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                                && d.CodigoBarras == codigoBarras
                                select d.IdInternoArticulos).FirstOrDefault();
                    //Obtenemos el objeto item de la base de datos que presentaremos en la pagina
                    item = (from d in db.articulosinv
                            join e in db.articulosdetalleinv
                            on d.IdInternoArticulos equals e.IdInternoArticulos
                            join b in db.medidasinv
                            on d.IdInternoMedidas equals b.IdInternoMedidas
                            where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                            && d.IdInternoArticulos == id
                            select new ListItemsViewModel
                            {
                                id = d.IdInternoArticulos,
                                descripcion = d.NombreArticulo,
                                foto = d.foto,
                                unidadMedida = b.Descripcion,
                                existencia = e.UnidadesIniciales + e.UnidadesEntrantes - e.UnidadesSalientes,
                                unidades = 1,
                                precio1 = d.precioventa_1_1,
                                precio2 = d.precioventa_2_2,
                                precio3 = d.preciomenudeo,
                                descuento1 = d.PorcentajeDescuento1_1,
                                descuento2 = d.PorcentajeDescuento2_2,
                                descuento3 = d.PorcentajeDescuento3_3,
                            }).FirstOrDefault();
                    //Ahora revisamos si ya existe en nuestro diccionario el item
                    if (model.articulos.ContainsKey(id))
                    {
                        model.articulos[id].agregarUnidad();
                        //Actualizamos el total
                        model.subtotal = getSubTotal(model.articulos);
                        model.descuento = getDescuentoTotal(model.articulos);
                        model.total = getTotal(model.articulos);
                        //Seteamos el precio y el descuento del articulo existente
                        item.precio = model.articulos[id].precio;
                        item.descuento = model.articulos[id].descuento;
                        item.descuentoPrecio = model.articulos[id].descuentoPrecio;
                        item.unidades = model.articulos[id].unidades;
                        item.subtotal = model.articulos[id].total;
                        //Actualizamos la variable global
                        Session["Sale"] = model;
                        //Retornamos el valor
                        return 
                            Json(new 
                            { 
                                success = true, 
                                item = item, 
                                subtotal = model.subtotal, 
                                descuento = model.descuento, 
                                total = model.total, 
                                newItem = false 
                            }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Calculos de campos no constantes
                        if (model.tipoPrecio == "G")
                        {
                            item.precio = item.precio1;
                            item.descuento = item.descuento1;
                        }
                        else
                        {
                            item.precio = item.precio2;
                            item.descuento = item.descuento2;
                        }
                        model.articulos.Add(id, new Articulo(id, item.precio, 1, item.descuento));
                        //Actualizamos el precio y todo
                        item.descuento = model.articulos[id].descuento;
                        item.precio = model.articulos[id].precio;
                        item.descuentoPrecio = model.articulos[id].descuentoPrecio;
                        item.subtotal = model.articulos[id].total;
                        //Actualizamos el total
                        model.subtotal = getSubTotal(model.articulos);
                        model.descuento = getDescuentoTotal(model.articulos);
                        model.total = getTotal(model.articulos);
                        //Actualizamos la variable global
                        Session["Sale"] = model;
                        //Retornamos el valor
                        return
                            Json(new
                            {
                                success = true,
                                item = item,
                                subtotal = model.subtotal,
                                descuento = model.descuento,
                                total = model.total,
                                newItem = true
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //Esta accion es la encargada de recibir un articulo por id y sumarlo al carrito
        [HttpPost]
        public ActionResult UpdateProduct(int id, int unidades, int descuento)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                //Actualizamos y calculamos
                model.articulos[id].unidades = unidades;
                model.articulos[id].descuento = descuento;
                model.articulos[id].calcular();

                //Actualizamos el total
                model.subtotal = getSubTotal(model.articulos);
                model.descuento = getDescuentoTotal(model.articulos);
                model.total = getTotal(model.articulos);

                //Actualizamos la variable global
                Session["Sale"] = model;

                return 
                    Json(new { 
                        success = true, 
                        item = model.articulos[id], 
                        subtotal = model.subtotal, 
                        descuento = model.descuento, 
                        total = model.total 
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                model.articulos.Remove(id);
                //Actualizamos el total
                model.subtotal = getSubTotal(model.articulos);
                model.descuento = getDescuentoTotal(model.articulos);
                model.total = getTotal(model.articulos);
                //Obtenemos el tamaño de la lista de articulos
                var len = model.articulos.Count();
                return
                    Json(new
                    {
                        success = true,
                        subtotal = model.subtotal,
                        descuento = model.descuento,
                        total = model.total,
                        len = len
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public decimal? getSubTotal(Dictionary<int, Articulo> articulos)
        {
            decimal? subtotal = 0;
            foreach (var articulo in articulos)
            {
                subtotal = subtotal + (articulo.Value.precio * articulo.Value.unidades);
            }
            return subtotal;
        }
        public decimal? getDescuentoTotal(Dictionary<int, Articulo> articulos)
        {
            decimal? descuentoTotal = 0;
            foreach (var articulo in articulos)
            {
                descuentoTotal = descuentoTotal + articulo.Value.descuentoPrecio;
            }
            return descuentoTotal;
        }

        public decimal? getTotal(Dictionary<int, Articulo> articulos)
        {
            decimal? total = 0;
            foreach (var articulo in articulos)
            {
                total = total + articulo.Value.total;
            }
            return total;
        }

    }
}