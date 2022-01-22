using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.Cajas;
using System.Linq.Dynamic;

namespace Minible5.Controllers.Transacciones.MntDeCajas
{
    public class cajasController : Controller
    {
        //VARIABLES GLOBALES PARA LA DATATABLE
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        // GET: cajas
        public ActionResult Index()
        {
            return View();
        }


        /***************************** PETICIONES DE DATOS *****************************/
        [HttpPost]
        public ActionResult GetCajasList()
        {
            List<TableCajasViewModel> lst = new List<TableCajasViewModel>();

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
                IQueryable<TableCajasViewModel> query =
                    (from d in db.movimientoscaja
                     select new TableCajasViewModel
                     {
                         idInternoCaja = d.IdInternoMovimientosCaja,
                         noCaja = d.IdCaja.ToString(),
                         usuario = d.Usuario,
                         inicio = d.Fecha_Inicio.ToString(),
                         fin = d.Fecha_Final.ToString(),
                         valorInicial = d.Valor_Inicial.ToString(),
                         operacion = d.Total_Operacion.ToString(),
                         total = d.Total_Dinero.ToString(),
                         cerrado = d.Cerrado
                     });
                //Searching by name
                if (searchValue != "")
                {
                    query = query.Where(d => d.noCaja.Contains(searchValue) || d.inicio.Contains(searchValue) || d.fin.Contains(searchValue));
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

        public ActionResult Create()
        {
            var model = new CajasViewModel(DateTime.Now.ToString("yyyy-MM-ddThh:mm"));
            return View(model);
        }
    }
}