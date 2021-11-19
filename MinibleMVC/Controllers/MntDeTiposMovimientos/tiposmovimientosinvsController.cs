using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using Minible5.Models.ViewModels.TiposMovimientos;
using System.Linq.Dynamic;

namespace Minible5.Controllers.MntDeTiposMovimientos
{
    public class tiposmovimientosinvsController : Controller
    {
        //Atributos para la datatable
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;
        public string vStatus = "A";
        public decimal valDefault = 0;

        private db_pcsolutions_webEntities db = new db_pcsolutions_webEntities();

        // GET: tiposmovimientosinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }


        [HttpPost]
        public ActionResult GetTiposMovimientos()
        {
            List<TableTipodMovimientosViewModel> lst = new List<TableTipodMovimientosViewModel>();

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

            IQueryable<TableTipodMovimientosViewModel> query =
                (from d in db.tiposmovimientosinv
                 select new TableTipodMovimientosViewModel
                 {
                     idInternoTiposMovimientos = d.IdInternoTiposMovimientos,
                     idTipoMovimiento = d.IdTipoMovimiento,
                     descripcion = d.Descripcion,
                     entradaSalida = d.EntradaSalida,
                     status = d.status
                 });


            query = query.Where(d => d.status.Equals(vStatus));

            //Searching by name
            if (searchValue != "")
            {
                query = query.Where(d => d.descripcion.Contains(searchValue) || d.descripcion.Contains(searchValue));
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



        // GET: tiposmovimientosinvs/Create
        public ActionResult Create()
        {
            TiposMovimientosViewModels model = new TiposMovimientosViewModels();

            model.entradaSalida = "S";
            model.facturacionInventario = "F";
            model.afectaCostoPromedio = true;
            model.afectaCostoRepocicion = true;
            model.afectaCostoUCompra = true;
            model.afectaestadisticacompra = true;
            model.afectaestadisticaventa = true;
            model.clienteProveedor = "C";
            model.facturacionInventario = "F";

            var itemsTipocliente = getTipoPrecio();
            ViewBag.itemsTipocliente = itemsTipocliente;

            return View(model);
        }



        // POST: tiposmovimientosinvs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(TiposMovimientosViewModels model)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    tiposmovimientosinv OtiposMov = new tiposmovimientosinv();
                    OtiposMov.IdTipoMovimiento = model.IdTipoMovimiento;
                    OtiposMov.Descripcion = model.Descripcion;
                    OtiposMov.EntradaSalida = model.entradaSalida;
                    OtiposMov.FacturacionInventario = model.facturacionInventario;
                    OtiposMov.Poliza = model.poliza;
                    if (model.afectaCostoPromedio) { OtiposMov.AfectaCostoPromedio = "S"; }
                    else { OtiposMov.AfectaCostoPromedio = "N"; }

                    if (model.afectaCostoRepocicion) { OtiposMov.AfectaCostoRepocicion = "S"; }
                    else { OtiposMov.AfectaCostoRepocicion = "N"; }

                    if (model.afectaCostoUCompra) { OtiposMov.AfectaCostoUCompra = "S"; }
                    else { OtiposMov.AfectaCostoUCompra = "N"; }

                    if (model.afectaestadisticacompra) { OtiposMov.Afectaestadisticacompra = "S"; }
                    else { OtiposMov.Afectaestadisticacompra = "N"; }

                    if (model.afectaestadisticaventa) { OtiposMov.Afectaestadisticaventa = "S"; }
                    else { OtiposMov.Afectaestadisticaventa = "N"; }

                    OtiposMov.ClienteProveedor = model.clienteProveedor;
                    OtiposMov.status = "A";
                    OtiposMov.Codigo_Empresa = "001";

                    db.tiposmovimientosinv.Add(OtiposMov);
                    db.SaveChanges();


                    foreach (var oC in model.conceptos)
                    {
                        tiposmovimientosseriesinv Otiposmovserie = new tiposmovimientosseriesinv();

                        Otiposmovserie.IdSerie = oC.idSerie;
                        if (oC.correlativo == null)
                        {
                            Otiposmovserie.Secuencia = 1;
                        }
                        else
                        {
                            Otiposmovserie.Secuencia = oC.secuencia;
                        }                       
                        Otiposmovserie.correlativo = oC.correlativo;
                        Otiposmovserie.UsaCorrelativo = oC.usaCorrelativo;
                        Otiposmovserie.FormatoImpresion = oC.formatoImpresion;
                        Otiposmovserie.ResolucionNumero = oC.resolucionNumero;
                        Otiposmovserie.FechaAutorizacion = oC.fechaAutorizacion;
                        Otiposmovserie.res_del = oC.res_del;
                        Otiposmovserie.res_al = oC.res_al;
                        Otiposmovserie.Codigo_Empresa = "001";
                        Otiposmovserie.status = "A";
                        Otiposmovserie.IdInternoTiposMovimientos = OtiposMov.IdInternoTiposMovimientos;

                        db.tiposmovimientosseriesinv.Add(Otiposmovserie);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index", "tiposmovimientosinvs", new { success = "Se agregó correctamente!" });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "tiposmovimientosinvs", new { success = "Excepcion" });

                }
            }

            return RedirectToAction("Index", "tiposmovimientosinvs", new { success = "No hizo nada" });
        }


        // GET: tiposmovimientosinvs/Edit/5        
        public ActionResult Edit(int? id)
        {
            /*          
            ViewBag.items = items;
            var companies = getCompanies();
            ViewBag.companies = companies; */
            int contador = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditTiposMovimientosViewModels model = new EditTiposMovimientosViewModels();
            List<EditTiposMovimientosSeriesViewModels> oLista = new List<EditTiposMovimientosSeriesViewModels>();

            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var OtiposMov = db.tiposmovimientosinv.Find(id);
            if (OtiposMov == null)
            {
                return HttpNotFound();
            }

            model.idInternoTiposMovimientos = OtiposMov.IdInternoTiposMovimientos;
            model.IdTipoMovimiento = OtiposMov.IdTipoMovimiento;
            model.Descripcion = OtiposMov.Descripcion;
            model.entradaSalida = OtiposMov.EntradaSalida;
            model.facturacionInventario = OtiposMov.FacturacionInventario;
            model.poliza = OtiposMov.Poliza;
            if (OtiposMov.AfectaCostoPromedio == "S") { model.afectaCostoPromedio = true; }
            else { model.afectaCostoPromedio = false; }

            if (OtiposMov.AfectaCostoRepocicion == "S") { model.afectaCostoRepocicion = true; }
            else { model.afectaCostoRepocicion = false; }

            if (OtiposMov.AfectaCostoUCompra == "S") { model.afectaCostoUCompra = true; }
            else { model.afectaCostoUCompra = false; }

            if (OtiposMov.Afectaestadisticacompra == "S") { model.afectaestadisticacompra = true; }
            else { model.afectaestadisticacompra = false; }

            if (OtiposMov.Afectaestadisticaventa == "S") { model.afectaestadisticaventa = true; }
            else { model.afectaestadisticaventa = false; }

            model.clienteProveedor = OtiposMov.ClienteProveedor;


            var itemsTipoPrecio = getTipoPrecio();
            ViewBag.itemsTipoPrecio = itemsTipoPrecio;

            IQueryable<EditarTiposMovimientosInv> query =
                (from d in db.tiposmovimientosseriesinv
                 join v in db.tiposmovimientosinv on d.IdInternoTiposMovimientos equals v.IdInternoTiposMovimientos
                 where d.IdInternoTiposMovimientos == model.idInternoTiposMovimientos 
                 where d.status == "A"
                 
                 select new EditarTiposMovimientosInv
                 {
                     idInternoTiposMovimientos = d.IdInternoTiposMovimientos,
                     idInternoTIposMovimientosSeries = d.IdInternoTIposMovimientosSeries,
                     idSerie = d.IdSerie,
                     correlativo = d.correlativo,
                     usaCorrelativo = d.UsaCorrelativo,
                     formatoImpresion = d.FormatoImpresion,
                     fechaAutorizacion = (DateTime)d.FechaAutorizacion,
                     res_del = d.res_del,
                     res_al = d.res_al,
                     resolucionNumero = d.ResolucionNumero,
                     secuencia = (int)d.Secuencia
                 });


            if (query == null)
            {
                return HttpNotFound();
            }



            foreach (var oC in query)
            {
                EditTiposMovimientosSeriesViewModels Oedit = new EditTiposMovimientosSeriesViewModels();

                Oedit.idSerie = oC.idSerie;
                Oedit.idInternoTiposMovimientos = oC.idInternoTiposMovimientos;
                Oedit.idInternoTIposMovimientosSeries = oC.idInternoTIposMovimientosSeries;
                Oedit.idSerie = oC.idSerie;
                Oedit.correlativo = oC.correlativo;
                Oedit.usaCorrelativo = oC.usaCorrelativo;
                Oedit.formatoImpresion = oC.formatoImpresion;
                Oedit.fechaAutorizacion = oC.fechaAutorizacion;
                Oedit.res_del = oC.res_del;
                Oedit.res_al = oC.res_al;
                Oedit.resolucionNumero = oC.resolucionNumero;
                Oedit.secuencia = oC.secuencia;                
                oLista.Add(Oedit);
                contador++;
            }

            model.conceptos = oLista;

            var itemsTipocliente = getTipoPrecio();
            ViewBag.itemsTipocliente = itemsTipocliente;
            model.contador = contador;
            

            return View(model);
        }



        // POST: tiposmovimientosinvs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTiposMovimientosViewModels model)
        {
            /*
            if (!ModelState.IsValid)
            {
                var items = getGroups();
                ViewBag.items = items;
                var companies = getCompanies();
                ViewBag.companies = companies;
                return View(model);
            } */
            bool addMovSerie = false;

            if (ModelState.IsValid)
            {

                var OtiposMov = db.tiposmovimientosinv.Find(model.idInternoTiposMovimientos);

                OtiposMov.IdTipoMovimiento = model.IdTipoMovimiento;                
                OtiposMov.Descripcion = model.Descripcion;
                OtiposMov.EntradaSalida = model.entradaSalida;
                OtiposMov.FacturacionInventario = model.facturacionInventario;
                OtiposMov.Poliza = model.poliza;
                if (model.afectaCostoPromedio) { OtiposMov.AfectaCostoPromedio = "S"; }
                else { OtiposMov.AfectaCostoPromedio = "N"; }

                if (model.afectaCostoRepocicion) { OtiposMov.AfectaCostoRepocicion = "S"; }
                else { OtiposMov.AfectaCostoRepocicion = "N"; }

                if (model.afectaCostoUCompra) { OtiposMov.AfectaCostoUCompra = "S"; }
                else { OtiposMov.AfectaCostoUCompra = "N"; }

                if (model.afectaestadisticacompra) { OtiposMov.Afectaestadisticacompra = "S"; }
                else { OtiposMov.Afectaestadisticacompra = "N"; }

                if (model.afectaestadisticaventa) { OtiposMov.Afectaestadisticaventa = "S"; }
                else { OtiposMov.Afectaestadisticaventa = "N"; }

                OtiposMov.ClienteProveedor = model.clienteProveedor;
               
                db.Entry(OtiposMov).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                
                if (model.conceptosAdd != null)
                {
                    foreach (var oC in model.conceptosAdd)
                    {

                        tiposmovimientosseriesinv Otiposmovserie = new tiposmovimientosseriesinv();

                        Otiposmovserie.IdSerie = oC.idSerie;
                        Otiposmovserie.Secuencia = oC.secuencia;
                        Otiposmovserie.correlativo = oC.correlativo;
                        Otiposmovserie.UsaCorrelativo = oC.usaCorrelativo;
                        Otiposmovserie.FormatoImpresion = oC.formatoImpresion;
                        Otiposmovserie.ResolucionNumero = oC.resolucionNumero;
                        Otiposmovserie.FechaAutorizacion = oC.fechaAutorizacion;
                        Otiposmovserie.res_del = oC.res_del;
                        Otiposmovserie.res_al = oC.res_al;
                        Otiposmovserie.Codigo_Empresa = "001";
                        Otiposmovserie.status = "A";
                        Otiposmovserie.IdInternoTiposMovimientos = model.idInternoTiposMovimientos;
                        db.tiposmovimientosseriesinv.Add(Otiposmovserie);

                        addMovSerie = true;


                    }
                }
                if (addMovSerie)
                {
                    addMovSerie = false;
                    db.SaveChanges();                    
                }



                if (model.conceptosDelete != null)
                {
                    foreach (var oC in model.conceptosDelete)
                    {
                        var oTiposMovSerie = db.tiposmovimientosseriesinv.Find(oC.idInternoTIposMovimientosSeries);
                        oTiposMovSerie.status = "B";
                        oTiposMovSerie.Fecha_baja = DateTime.Now;
                        db.Entry(oTiposMovSerie).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();                      

                    }
                }
               

                if(model.conceptos != null)
                {
                    foreach (var oC in model.conceptos)
                    {                    
                            var OtiposmovserieAtualizar = db.tiposmovimientosseriesinv.Find(oC.idInternoTIposMovimientosSeries);

                            OtiposmovserieAtualizar.IdSerie = oC.idSerie;
                            OtiposmovserieAtualizar.Secuencia = oC.secuencia;
                            OtiposmovserieAtualizar.correlativo = oC.correlativo;
                            OtiposmovserieAtualizar.UsaCorrelativo = oC.usaCorrelativo;
                            OtiposmovserieAtualizar.FormatoImpresion = oC.formatoImpresion;
                            OtiposmovserieAtualizar.ResolucionNumero = oC.resolucionNumero;
                            OtiposmovserieAtualizar.FechaAutorizacion = oC.fechaAutorizacion;
                            OtiposmovserieAtualizar.res_del = oC.res_del;
                            OtiposmovserieAtualizar.res_al = oC.res_al;

                            db.Entry(OtiposmovserieAtualizar).State = System.Data.Entity.EntityState.Modified;
                   
                    }

                    db.SaveChanges();
                }
                



                return RedirectToAction("Index", "tiposmovimientosinvs", new { success = "Se agregó correctamente!" }); 
            }


            return RedirectToAction("Index", "tiposmovimientosinvs", new { success = "No hizo nada" });
        }


        // GET: tiposmovimientosinvs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tiposmovimientosinv tiposmovimientosinv = db.tiposmovimientosinv.Find(id);
            if (tiposmovimientosinv == null)
            {
                return HttpNotFound();
            }
            return View(tiposmovimientosinv);
        }

        // POST: tiposmovimientosinvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tiposmovimientosinv tiposmovimientosinv = db.tiposmovimientosinv.Find(id);
            db.tiposmovimientosinv.Remove(tiposmovimientosinv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> getTipoPrecio()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "Cliente", Value = "C" });
            lst.Add(new SelectListItem() { Text = "Proveedor", Value = "P" });
            lst.Add(new SelectListItem() { Text = "Interno", Value = "I" });

            return lst;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
