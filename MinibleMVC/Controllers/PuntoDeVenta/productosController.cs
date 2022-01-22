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
    public class productosController : Controller
    {
        // GET: productos
        public ActionResult Index()
        {
            //Obtenemos la venta globalmente
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model == null)
            {
                model = new PtoDeVentaViewModel();
            }
            var marcas = getMarcas();
            ViewBag.marcas = marcas;
            var familias = getFamilias();
            ViewBag.familias = getFamilias();
            return View(model);
        }

        public List<SelectListItem> getMarcas()
        {
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.marcasinv
                     where d.Codigo_Empresa == oCompany.codigo_empresa
                     select new SelectListItem
                     {
                         Value = d.IdInternoMarcas.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }

        public List<SelectListItem> getFamilias()
        {
            var oCompany = (security_companies)Session["Company"];
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.familiasinv
                     where d.Codigo_Empresa == oCompany.codigo_empresa
                     select new SelectListItem
                     {
                         Value = d.IdInternoFamilias.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }


        [HttpPost]
        public ActionResult getProducts(int page, string searchValue, string column, string direction, int? idFamilia, int? idMarca)
        {
            //Declaracion de variables
            List<ListItemsViewModel> lst = new List<ListItemsViewModel>();
            var model = Session["Sale"] as PtoDeVentaViewModel;
            var oCompany = Session["Company"] as security_companies;
            //Ahora calculamos el numero de pagina y los registros que tenemos que skipear
            var skip = (page-1) * 6;

            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                //Obtenemos el precio y el descuento de los items
                IQueryable<ListItemsViewModel> query;
                if (model.tipoPrecio == "G")
                {
                    query =
                        (from d in db.articulosinv
                            join e in db.articulosdetalleinv
                                on d.IdInternoArticulos equals e.IdInternoArticulos
                         where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                         select new ListItemsViewModel
                         {
                             id = d.IdInternoArticulos,
                             descripcion = d.NombreArticulo,
                             foto = d.foto,
                             precio = d.precioventa_1_1,
                             descuento = d.PorcentajeDescuento1_1,
                             codigo = d.IdArticulo,
                             idFamilia = d.IdInternoFamilias,
                             idMarca = d.IdInternoMarcas
                         });
                } 
                else
                {
                    query =
                        (from d in db.articulosinv
                            join e in db.articulosdetalleinv
                                on d.IdInternoArticulos equals e.IdInternoArticulos
                         where d.Codigo_Empresa == oCompany.codigo_empresa && e.IdInternoBodegas == model.bodega
                         select new ListItemsViewModel
                         {
                             id = d.IdInternoArticulos,
                             descripcion = d.NombreArticulo,
                             foto = d.foto,
                             precio = d.precioventa_2_2,
                             descuento = d.PorcentajeDescuento2_2,
                             codigo = d.IdArticulo,
                             idFamilia = d.IdInternoFamilias,
                             idMarca = d.IdInternoMarcas
                         });
                }
                //Searching by name
                if (searchValue != "")
                {
                    query = query.Where(d => d.descripcion.Contains(searchValue) || d.codigo.Contains(searchValue));
                }
                if (idFamilia != null)
                {
                    query = query.Where(d => d.idFamilia == idFamilia);
                }
                if (idMarca != null)
                {
                    query = query.Where(d => d.idMarca == idMarca);
                }
                //Sorting
                query = query.OrderBy(column + " " + direction);


                // Obtenemos el numero de paginas
                var records = query.Count();
                var pagesTotal = records/6;
                if (records % 6 > 0)
                    pagesTotal++;

                //Obtenemos la lista de paginas a presentar, como maximo, 5 paginas
                var paginas = getPages(pagesTotal, page);

                //Booleanos para saber si habilitamos las banderas
                var left = true;
                var right = true;
                if (page == 1) left = false;
                if (page == pagesTotal) right = false;

                lst = query.Skip(skip).Take(6).ToList();
                //Ahora obtenemos el precio del descuento
                foreach(var item in lst)
                {
                    var articulo = new Articulo(item.id, item.precio, 1, item.descuento);
                    item.descuentoPrecio = articulo.descuentoPrecio;
                    item.subtotal = articulo.total;
                }
                return Json(new
                {
                    success = true,
                    pagesTotal = pagesTotal,
                    page = page,
                    list = lst,
                    pages = paginas,
                    left = left,
                    right = right
                });
            }
        }


        //Este metodo es el encargado de retornar la lista de paginas a presentar en la vista
        public List<GridPagesViewModel> getPages(int pagesTotal, int page)
        {
            var paginas = new List<GridPagesViewModel>();
            //Necesitamos situar la pagina actual en el medio si es posible, si no solo presentamos las posibles
            var contador = 0;
            //Metemos los valores menores a la pagina actual, como maximo son 2
            for (int i = page - 1; i > 0 && contador < 2; i--)
            {
                paginas.Insert(0, new GridPagesViewModel(i, false));
                contador++;
            }
            //Metemos la pagina actual
            paginas.Add(new GridPagesViewModel(page, true));
            contador++;
            //Metemos las paginas que sobran
            for (int i = page + 1; i <= pagesTotal && contador < 5; i++)
            {
                paginas.Add(new GridPagesViewModel(i, false));
                contador++;
            }
            //Ahora solo revisamos si se pueden ingresar mas paginas
            if (contador < 5)
            {
                for (int i = paginas[0].numero - 1; i >= 1 && contador < 5; i--)
                {
                    paginas.Insert(0, new GridPagesViewModel(i, false));
                    contador++;
                }
            }
            return paginas;
        }
    }
}