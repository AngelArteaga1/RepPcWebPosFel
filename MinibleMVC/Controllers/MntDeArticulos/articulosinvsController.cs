using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Minible5.Models;
using System.Linq.Dynamic;
using Minible5.Models.ViewModels.Articulos;
using System.IO;

namespace Minible5.Controllers.MntDeArticulos
{
    public class articulosinvsController : Controller
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

        // GET: articulosinvs
        public ActionResult Index(string success)
        {
            ViewBag.Success = success;
            return View();
        }

        [HttpPost]
        public ActionResult GetArticulos()
        {
            List<TableArticulosViewModel> lst = new List<TableArticulosViewModel>();

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

            IQueryable<TableArticulosViewModel> query =
                (from d in db.articulosinv
                 join m in db.marcasinv on d.IdInternoMarcas equals m.IdInternoMarcas
                 join me in db.medidasinv on d.IdInternoMarcas equals me.IdInternoMedidas
                 join f in db.familiasinv on d.IdInternoFamilias equals f.IdInternoFamilias
                 select new TableArticulosViewModel
                 {
                     idInternoArticulos = d.IdInternoArticulos,
                     idArticulo = d.IdArticulo,
                     descripcion = d.NombreArticulo,
                     marca = m.Descripcion,
                     medida = me.Descripcion,
                     familia = f.Descripcion,
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


        [HttpPost]
        public ActionResult GetArticulosDetalle(int? id)
        {
            List<TableArticulosDetalleViewModel> lst = new List<TableArticulosDetalleViewModel>();

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

            IQueryable<TableArticulosDetalleViewModel> query =
                (from d in db.articulosdetalleinv
                 join da in db.articulosinv on d.IdInternoArticulos equals da.IdInternoArticulos
                 join b in db.bodegasinv on d.IdInternoBodegas equals b.IdInternoBodegas
                 where d.IdInternoArticulos == id
                 where d.status == "A"
                 select new TableArticulosDetalleViewModel
                 {
                     idInternoArticulos = d.IdInternoArticulos,
                     idBodega = d.IdBodega,
                     descripcionBodega = b.Descripcion,
                     ubicacion = d.Ubicacion,
                     ultimaFechaVenta = d.UltimaFechaVenta.ToString(),
                     ultimaFechaCompra = d.UltimaFechaCompra.ToString(),
                     fechaAlta = d.FechaAlta.ToString(),
                     unidadesIniciales = d.UnidadesIniciales.ToString(),
                     unidadesEntrantes = d.UnidadesEntrantes.ToString(),
                     unidadesSalientes = d.UnidadesSalientes.ToString(),
                     costoInicial_Q = d.CostoInicial_Q.ToString(),
                     costoEntradas_Q = d.CostoEntradas_Q.ToString(),
                     costoSalidas_Q = d.CostoSalidas_Q.ToString(),
                     docfechaultcomp1 = d.docfechaultcomp1.ToString(),
                     docfechaultcomp2 = d.docfechaultcomp2.ToString(),
                     docfechaultcomp3 = d.docfechaultcomp3.ToString(),
                     valorultcomp1 = d.valorultcomp1.ToString(),
                     valorultcomp2 = d.valorultcomp2.ToString(),
                     valorultcomp3 = d.valorultcomp3.ToString(),
                     maximo = d.Maximo.ToString(),
                     minimo = d.Minimo.ToString()
                 });





            //Searching by name
            if (searchValue != "")
            {
                query = query.Where(d => d.descripcionBodega.Contains(searchValue) || d.descripcionBodega.Contains(searchValue));
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


        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {

                    var originalDirectory = new DirectoryInfo(string.Format("{0}assets\\images\\", Server.MapPath(@"\")));

                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "products");

                    var fileName1 = Path.GetFileName(file.FileName);


                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, file.FileName);
                    file.SaveAs(path);

                }

            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        // GET: articulosinvs/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditArticulosViewModels model = new EditArticulosViewModels();

            var OarticulosInv = db.articulosinv.Find(id);
            if (OarticulosInv == null)
            {
                return HttpNotFound();
            }

            model.idInternoArticulos = OarticulosInv.IdInternoArticulos;
            model.idInternoFamilias = OarticulosInv.IdInternoFamilias;
            model.idInternoMarcas = OarticulosInv.IdInternoMarcas;
            model.idArticulo = OarticulosInv.IdArticulo;
            model.nombreArticulo = OarticulosInv.NombreArticulo;
            model.tipo = OarticulosInv.tipo;
            model.idInternoMedidas = OarticulosInv.IdInternoMedidas;
            model.codigoBarras = OarticulosInv.CodigoBarras;
            model.idInternoProveedores = OarticulosInv.IdInternoProveedores;            
            model.idinternoArticuloDetalleGenerico = OarticulosInv.IdinternoArticuloDetalleGenerico;
                        

            if (OarticulosInv.statusmenudeo == "S") { model.statusMenudeo = true; }
            else { model.statusMenudeo = false; }

            model.precioVenta_1_1 = (decimal)OarticulosInv.precioventa_1_1;
            model.precioVenta_2_2 = (decimal)OarticulosInv.precioventa_2_2;            
            model.porcentajeDescuento1_1 = (decimal)OarticulosInv.PorcentajeDescuento1_1;
            model.porcentajeDescuento2_2 = (decimal)OarticulosInv.PorcentajeDescuento2_2;
            model.porcentajeDescuento3_3 = (decimal)OarticulosInv.PorcentajeDescuento3_3;
            model.foto = OarticulosInv.foto;

            var itemsFamilias = getFamilias();
            ViewBag.itemsFamilias = itemsFamilias;

            var itemsMarcas = getMarcas();
            ViewBag.itemsMarcas = itemsMarcas;

            var itemsTipo = getTipo();
            ViewBag.itemsTipo = itemsTipo;

            var itemsBodega = getBodegas();
            ViewBag.itemsBodega = itemsBodega;

            var itemsMedidas = getMedidas();
            ViewBag.itemsMedidas = itemsMedidas;

            var itemsProveedores = getProveedores();
            ViewBag.itemsProveedores = itemsProveedores;

            var itemsArticulogenerico = getArticulogenericos();
            ViewBag.itemsArticulogenerico = itemsArticulogenerico;

            return View(model);

        }


        // GET: articulosinvs/Create
        public ActionResult Create()
        {
            ArticulosViewModels model = new ArticulosViewModels();
            

            var itemsFamilias = getFamilias();
            ViewBag.itemsFamilias = itemsFamilias;

            var itemsMarcas = getMarcas();
            ViewBag.itemsMarcas = itemsMarcas;

            var itemsTipo = getTipo();
            ViewBag.itemsTipo = itemsTipo;

            var itemsBodega = getBodegas();
            ViewBag.itemsBodega = itemsBodega;

            var itemsMedidas = getMedidas();
            ViewBag.itemsMedidas = itemsMedidas;

            var itemsProveedores = getProveedores();
            ViewBag.itemsProveedores = itemsProveedores;

            var itemsArticulogenerico = getArticulogenericos();
            ViewBag.itemsArticulogenerico = itemsArticulogenerico;


            return View(model);
        }

        // POST: articulosinvs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ArticulosViewModels model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    articulosinv Oarticulos = new articulosinv();
                    Oarticulos.IdInternoFamilias = model.idInternoFamilias;
                    Oarticulos.IdInternoMarcas = model.idInternoMarcas;
                    Oarticulos.IdArticulo = model.idArticulo;
                    Oarticulos.NombreArticulo = model.nombreArticulo;
                    Oarticulos.tipo = model.tipo;
                    Oarticulos.IdInternoMedidas = model.idInternoMedidas;
                    Oarticulos.CodigoBarras = model.codigoBarras;
                    Oarticulos.IdInternoProveedores = model.idInternoProveedores;                    
                    Oarticulos.IdinternoArticuloDetalleGenerico = model.idinternoArticuloDetalleGenerico;                 

                    if (model.statusMenudeo) { Oarticulos.statusmenudeo = "S"; }
                    else { Oarticulos.statusmenudeo = "N"; }

                    Oarticulos.precioventa_1_1 = model.precioVenta_1_1;
                    Oarticulos.precioventa_2_2 = model.precioVenta_2_2;                    
                    Oarticulos.PorcentajeDescuento1_1 = model.porcentajeDescuento1_1;
                    Oarticulos.PorcentajeDescuento2_2 = model.porcentajeDescuento2_2;
                    Oarticulos.PorcentajeDescuento3_3 = model.porcentajeDescuento3_3;
                    Oarticulos.foto = model.foto;

                    Oarticulos.status = "A";
                    Oarticulos.Codigo_Empresa = "001";

                    db.articulosinv.Add(Oarticulos);
                    db.SaveChanges();
                    return RedirectToAction("Details", "articulosinvs", new { id = Oarticulos.IdInternoArticulos });

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "articulosinvs", new { success = "Entro en la Excepcion" });

                }
            }

            return RedirectToAction("Index", "articulosinvs", new { success = "No hizo nada" });
        }


        // GET: articulosinvs/Edit/5
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

            EditArticulosViewModels model = new EditArticulosViewModels();
            List<EditArticulosdetalleViewModels> oLista = new List<EditArticulosdetalleViewModels>();

            //Empresas del usuario
            //var userCompanies = getUserCompanies(id);
            //ViewBag.userCompanies = userCompanies;

            var OarticulosInv = db.articulosinv.Find(id);
            if (OarticulosInv == null)
            {
                return HttpNotFound();
            }
            model.idInternoArticulos = OarticulosInv.IdInternoArticulos;
            model.idInternoFamilias = OarticulosInv.IdInternoFamilias;
            model.idInternoMarcas = OarticulosInv.IdInternoMarcas;
            model.idArticulo = OarticulosInv.IdArticulo;
            model.nombreArticulo = OarticulosInv.NombreArticulo;
            model.tipo = OarticulosInv.tipo;
            model.idInternoMedidas = OarticulosInv.IdInternoMedidas;
            model.codigoBarras = OarticulosInv.CodigoBarras;
            model.idInternoProveedores = OarticulosInv.IdInternoProveedores;            
            model.idinternoArticuloDetalleGenerico = OarticulosInv.IdinternoArticuloDetalleGenerico;          

            if (OarticulosInv.statusmenudeo == "S") { model.statusMenudeo = true; }
            else { model.statusMenudeo = false; }

            model.precioVenta_1_1 = (decimal)OarticulosInv.precioventa_1_1;
            model.precioVenta_2_2 = (decimal)OarticulosInv.precioventa_2_2;            
            model.porcentajeDescuento1_1 = (decimal)OarticulosInv.PorcentajeDescuento1_1;
            model.porcentajeDescuento2_2 = (decimal)OarticulosInv.PorcentajeDescuento2_2;
            model.porcentajeDescuento3_3 = (decimal)OarticulosInv.PorcentajeDescuento3_3;
            model.foto = OarticulosInv.foto;

            IQueryable<EditArticulosdetalle> query =
                (from d in db.articulosinv
                 join a in db.articulosdetalleinv on d.IdInternoArticulos equals a.IdInternoArticulos
                 where d.IdInternoArticulos == model.idInternoArticulos
                 where d.status == "A"

                 select new EditArticulosdetalle
                 {
                     ubicacion = a.Ubicacion,
                     maximo = (double)a.Maximo,
                     minimo = (double)a.Minimo,
                     idInternoArticulos = a.IdInternoArticulos,
                     idInternoBodegas = a.IdInternoBodegas,
                     idInternoArticulosDetalle = a.IdInternoArticulosDetalle
                 });


            if (query == null)
            {
                return HttpNotFound();
            }


            foreach (var oC in query)
            {
                EditArticulosdetalleViewModels Oedit = new EditArticulosdetalleViewModels();

                Oedit.ubicacion = oC.ubicacion;
                Oedit.maximo = oC.maximo;
                Oedit.minimo = oC.minimo;
                Oedit.idInternoArticulos = oC.idInternoArticulos;
                Oedit.idInternoBodegas = oC.idInternoBodegas;
                Oedit.idInternoArticulosDetalle = oC.idInternoArticulosDetalle;
                oLista.Add(Oedit);
                contador++;
            }

            model.conceptosEdit = oLista;

            var itemsFamilias = getFamilias();
            ViewBag.itemsFamilias = itemsFamilias;

            var itemsMarcas = getMarcas();
            ViewBag.itemsMarcas = itemsMarcas;

            var itemsTipo = getTipo();
            ViewBag.itemsTipo = itemsTipo;

            var itemsBodega = getBodegas();
            ViewBag.itemsBodega = itemsBodega;

            var itemsMedidas = getMedidas();
            ViewBag.itemsMedidas = itemsMedidas;

            var itemsProveedores = getProveedores();
            ViewBag.itemsProveedores = itemsProveedores;

            var itemsArticulogenerico = getArticulogenericos();
            ViewBag.itemsArticulogenerico = itemsArticulogenerico;

            model.contador = contador;

            return View(model);
        }



        // POST: articulosinvs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public ActionResult Edit(EditArticulosViewModels model)
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


            if (ModelState.IsValid)
            {

                var OarticulosInv = db.articulosinv.Find(model.idInternoArticulos);

                OarticulosInv.IdInternoArticulos = model.idInternoArticulos;
                OarticulosInv.IdInternoFamilias = model.idInternoFamilias;
                OarticulosInv.IdInternoMarcas = model.idInternoMarcas;
                OarticulosInv.IdArticulo = model.idArticulo;
                OarticulosInv.NombreArticulo = model.nombreArticulo;
                OarticulosInv.tipo = model.tipo;
                OarticulosInv.IdInternoMedidas = model.idInternoMedidas;
                OarticulosInv.CodigoBarras = model.codigoBarras;
                OarticulosInv.IdInternoProveedores = model.idInternoProveedores;                
                OarticulosInv.IdinternoArticuloDetalleGenerico = model.idinternoArticuloDetalleGenerico;
                              

                if (model.statusMenudeo) { OarticulosInv.statusmenudeo = "S"; }
                else { OarticulosInv.statusmenudeo = "N"; }

                OarticulosInv.precioventa_1_1 = model.precioVenta_1_1;
                OarticulosInv.precioventa_2_2 = model.precioVenta_2_2;                
                OarticulosInv.PorcentajeDescuento1_1 = model.porcentajeDescuento1_1;
                OarticulosInv.PorcentajeDescuento2_2 = model.porcentajeDescuento2_2;
                OarticulosInv.PorcentajeDescuento3_3 = model.porcentajeDescuento3_3;
                OarticulosInv.foto = model.foto;

                db.Entry(OarticulosInv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                if (model.conceptosEdit != null)
                {
                    foreach (var oC in model.conceptosEdit)
                    {
                        var Oedit = db.articulosdetalleinv.Find(oC.idInternoArticulosDetalle);

                        Oedit.Ubicacion = oC.ubicacion;
                        Oedit.Maximo = oC.maximo;
                        Oedit.Minimo = oC.minimo;
                        Oedit.IdInternoArticulos = oC.idInternoArticulos;
                        Oedit.IdInternoBodegas = oC.idInternoBodegas;

                        db.Entry(Oedit).State = System.Data.Entity.EntityState.Modified;

                    }

                    db.SaveChanges();
                }
                
                return RedirectToAction("Details", "articulosinvs", new { id = OarticulosInv.IdInternoArticulos });
            }


            return RedirectToAction("Index", "articulosinvs", new { success = "No hizo nada" });
        }


        // POST: articulosinvs/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var OarticulosInv = db.articulosinv.Find(id);
            OarticulosInv.status = "B";
            OarticulosInv.Fecha_baja = DateTime.Now;

            db.Entry(OarticulosInv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        public List<SelectListItem> getTipo()
        {
            //creamos una lista tipo SelectListItem
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "Normal", Value = "N" });
            lst.Add(new SelectListItem() { Text = "Servicio", Value = "S" });

            return lst;
        }

        public List<SelectListItem> getFamilias()
        {
            List<TableArticulos_Familias> lst;

            lst =
                (from d in db.familiasinv
                 select new TableArticulos_Familias
                 {
                     idInternoFamilias = d.IdInternoFamilias,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoFamilias.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getMarcas()
        {
            List<TableArticulos_Marcas> lst;

            lst =
                (from d in db.marcasinv
                 select new TableArticulos_Marcas
                 {
                     idInternoMarcas = d.IdInternoMarcas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoMarcas.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getBodegas()
        {
            List<TableArticulos_Bodegas> lst;

            lst =
                (from d in db.bodegasinv
                 select new TableArticulos_Bodegas
                 {
                     idInternoBodegas = d.IdInternoBodegas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoBodegas.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getMedidas()
        {
            List<TableArticulos_Medidas> lst;

            lst =
                (from d in db.medidasinv
                 select new TableArticulos_Medidas
                 {
                     idInternoMedidas = d.IdInternoMedidas,
                     descripcion = d.Descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoMedidas.ToString(),
                    Selected = false
                };
            });
            return items;
        }


        public List<SelectListItem> getProveedores()
        {
            List<TableArticulos_Proveedores> lst;

            lst =
                (from d in db.proveedores
                 select new TableArticulos_Proveedores
                 {
                     idInternoProveedor = d.IdInternoProveedores,
                     descripcion = d.NombreComercial
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoProveedor.ToString(),
                    Selected = false
                };
            });
            return items;
        }

        public List<SelectListItem> getArticulogenericos()
        {
            List<TableArticulos_ArticulosGenericos> lst;

            lst =
                (from d in db.articulosdetallegenericos
                 select new TableArticulos_ArticulosGenericos
                 {
                     idInternoArticulosGenericos = d.IdinternoArticuloDetalleGenerico,
                     descripcion = d.descripcion
                 }).ToList();

            List<SelectListItem> items = lst.ConvertAll(d => {
                return new SelectListItem()
                {
                    Text = d.descripcion,
                    Value = d.idInternoArticulosGenericos.ToString(),
                    Selected = false
                };
            });
            return items;
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
