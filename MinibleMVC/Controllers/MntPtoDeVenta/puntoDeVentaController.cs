using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.PtoDeVenta;
using System.Linq.Dynamic;

namespace Minible5.Controllers.MntPtoDeVenta
{
    public class puntoDeVentaController : Controller
    {
        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        public string draw_item = "";
        public string start_item = "";
        public string length_item = "";
        public string sortColumn_item = "";
        public string sortColumnDir_item = "";
        public string searchValue_item = "";
        public int pageSize_item, skip_item, recordsTotal_item;

        public List<SelectListItem> getVendedores()
        {
            List<SelectListItem> items;
            using (var db = new db_pcsolutions_webEntities())
            {
                items =
                    (from d in db.vendedores
                     select new SelectListItem
                     {
                         Value = d.IdInternoVendedores.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     }).ToList();
            }
            return items;
        }

        // GET: puntoDeVenta
        public ActionResult Index()
        {
            var vendedores = getVendedores();
            ViewBag.vendedores = vendedores;
            return View();
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
        public ActionResult GetItems()
        {
            List<TableItemsViewModel> lst = new List<TableItemsViewModel>();

            //logistica datatable
            var draw_item = Request.Form.GetValues("draw").FirstOrDefault();
            var start_item = Request.Form.GetValues("start").FirstOrDefault();
            var length_item = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn_item = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir_item = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue_item = Request.Form.GetValues("search[value]").FirstOrDefault();
            pageSize_item = length_item != null ? Convert.ToInt32(length_item) : 0;
            skip_item = start_item != null ? Convert.ToInt32(start_item) : 0;
            recordsTotal_item = 0;

            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                IQueryable<TableItemsViewModel> query =
                    (from d in db.articulosinv
                     select new TableItemsViewModel
                     {
                         id = d.IdInternoArticulos,
                         descripcion = d.NombreArticulo,
                         codigo = d.codigoarticulo,
                         existencia = d.ExistenciaActual,
                         //unidadMedida = d.UnidadesPedidas
                     });
                //Searching by name
                if (searchValue_item != "")
                {
                    query = query.Where(d => d.descripcion.Contains(searchValue_item) || d.codigo.Contains(searchValue_item));
                }
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn_item) && string.IsNullOrEmpty(sortColumnDir_item)))
                {
                    query = query.OrderBy(sortColumn_item + " " + sortColumnDir_item);
                }
                recordsTotal_item = query.Count();
                lst = query.Skip(skip_item).Take(pageSize_item).ToList();
                return Json(new
                {
                    draw = draw_item,
                    recordsFiltered = recordsTotal_item,
                    recordsTotal = recordsTotal_item,
                    data = lst
                });
            }
        }
    }
}