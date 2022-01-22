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
    public class listaProductosController : Controller
    {
        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        // GET: listaProductos
        public ActionResult Index()
        {
            //Obtenemos la venta globalmente
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model == null)
                return RedirectToAction("Index", "inicioVenta", new { error = "Debe iniciar una transaccion para ver los productos" });
            
            return View(model);
        }


        /***************************** PETICION DE LISTA DE DATOS *****************************/
        [HttpPost]
        public ActionResult GetItems()
        {
            List<TableItemsViewModel> lst = new List<TableItemsViewModel>();
            var model = Session["Sale"] as PtoDeVentaViewModel;
            var oCompany = Session["Company"] as security_companies;

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
                IQueryable<TableItemsViewModel> query;
                //Vamos a mostrar la informacion del precio dependiendo del tipo de cliente
                if(model.tipoPrecio == "G")
                {
                    query =
                        (from d in db.articulosinv
                         join e in db.articulosdetalleinv
                         on d.IdInternoArticulos equals e.IdInternoArticulos
                         join b in db.medidasinv
                         on d.IdInternoMedidas equals b.IdInternoMedidas
                         where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                         select new TableItemsViewModel
                         {
                             id = d.IdInternoArticulos,
                             foto = d.foto,
                             descripcion = d.NombreArticulo,
                             precio = d.precioventa_1_1 - ((d.precioventa_1_1 * d.PorcentajeDescuento1_1)/100),
                             codigo = d.CodigoBarras,
                             existencia = e.UnidadesIniciales + e.UnidadesEntrantes - e.UnidadesSalientes,
                             unidadMedida = b.Descripcion
                         });
                }
                else
                {
                    query =
                        (from d in db.articulosinv
                         join e in db.articulosdetalleinv
                         on d.IdInternoArticulos equals e.IdInternoArticulos
                         join b in db.medidasinv
                         on d.IdInternoMedidas equals b.IdInternoMedidas
                         where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                         select new TableItemsViewModel
                         {
                             id = d.IdInternoArticulos,
                             foto = d.foto,
                             descripcion = d.NombreArticulo,
                             precio = d.precioventa_2_2 - ((d.precioventa_2_2 * d.PorcentajeDescuento2_2)/100),
                             codigo = d.CodigoBarras,
                             existencia = e.UnidadesIniciales + e.UnidadesEntrantes - e.UnidadesSalientes,
                             unidadMedida = b.Descripcion
                         });
                }

                //Searching by name
                if (searchValue != "")
                {
                    query = query.Where(d => d.descripcion.Contains(searchValue) || d.codigo.Contains(searchValue));
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

    }
}