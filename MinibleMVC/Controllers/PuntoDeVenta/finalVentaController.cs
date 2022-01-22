using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Models.ViewModels.PtoDeVenta;
using Minible5.Models.ViewModels.TransmisionFEL;
using Minible5.Models;
using conectorfelv2;

namespace Minible5.Controllers.PuntoDeVenta
{
    public class finalVentaController : Controller
    {
        // GET: finalVenta
        public ActionResult Index(string error)
        {
            //Obtenemos variables globales
            var oCompany = Session["Company"] as security_companies;
            var model = Session["Sale"] as PtoDeVentaViewModel;

            //Verificamos si ya iniciaron la venta
            if (model == null)
                return RedirectToAction("Index", "inicioVenta", new { error = "Debe iniciar una transaccion para finalizarla" });

            //Verificamos si tiene mas de un articulo agregado
            if(model.articulos.Count() < 1)
                return RedirectToAction("Index", "carrito", new { error = "Debe de ingresar articulos para poder hacer el checkout" });


            //Ingresamos los detalles de pedidosclientes
            ingresarDetallesPed(model, oCompany);

            //Obtenemos las listas de bancos, tarjetas, articulos
            ViewBag.articulos = getArticulos(model);
            ViewBag.bancos = getBancos();
            ViewBag.tarjetas = getTarjetas();

            ViewBag.error = error;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            //Obtnemos las variables globales
            var model = Session["Sale"] as PtoDeVentaViewModel;
            var oUser = Session["User"] as security_users;
            var oCompany = Session["Company"] as security_companies;

            //Verificamos si ya iniciaron la venta
            if (model == null)
                return RedirectToAction("Index", "inicioVenta", new { error = "Debe iniciar una transaccion para finalizarla" });

            //Vamos a hacer las validaciones a pata porque esta dando error el modelstate
            if(model.total <= 0)
                return returnView(model, "El monto total no puede ser menor o igual a 0");
            if (model.pago.montoTotal < model.total)
                return returnView(model, "El monto total del pago debe ser mayor o igual al total de la factura");

            //Declaracion de identificadores del movimiento
            string numdoc;
            string successInfo;
            int idInternoMovimientosInv;

            using (var db = new db_pcsolutions_webEntities())
            {
                //Variables que usan varios metodos
                int idInternoCliente = getIdInternoCliente(model.nit);
                string idBodega = getIdBodega(model.bodega);
                string idTipoMovimiento = getIdTipoMovimiento(model.serie, oCompany.codigo_empresa);

                //Ingresamos el encabezado en la tabla y obtenemos el numdoc junto con el idInterno
                numdoc = ingresarMovimiento(model, oCompany, idInternoCliente, idBodega);
                idInternoMovimientosInv = getIdInternoMovimientosInv(numdoc, model.serie, oCompany.codigo_empresa);

                //Ahora realizamos fel solo si es tipo FA
                if (idTipoMovimiento == "FA")
                    successInfo = realizarTransmicionFEL(model, oCompany, numdoc, idInternoMovimientosInv, idInternoCliente);
                else
                    successInfo = "true";

                if (successInfo == "true")
                {
                    //Ahora metemos los detelles de la caja
                    successInfo = ingresarDetallesCaja(model, oUser, oCompany, numdoc);
                    if(successInfo != "true")
                        return returnView(model, successInfo);

                    //Ahora metemos los detalle del encabezado
                    successInfo = ingresarDetallesMov(model, oCompany, numdoc, idInternoMovimientosInv, idBodega);
                    if (successInfo != "true")
                        return returnView(model, successInfo);
                } else
                {
                    return returnView(model, successInfo);
                }
                Session["Sale"] = null;
            }
            return RedirectToAction("Index", "factura", new { id = idInternoMovimientosInv });
        }


        /***************** FUNCION PARA RETORNAR LA VISTA CON UN ERROR *****************/
        public ViewResult returnView(PtoDeVentaViewModel model, string error)
        {
            ViewBag.articulos = getArticulos(model);
            ViewBag.bancos = getBancos();
            ViewBag.tarjetas = getTarjetas();
            ViewBag.error = error;
            return View(model);
        }



        /***************** ESTAS FUNCIONES SON PROCESOS DE LA TRANSACCION *****************/
        public void ingresarDetallesPed(PtoDeVentaViewModel model, security_companies oCompany)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                foreach (var articulo in model.articulos)
                {
                    var articuloDesc = db.articulosinv.Find(articulo.Value.id);
                    var oPedidosClientesDetalle = new pedidosclientesinvdetalle();
                    oPedidosClientesDetalle.IdInternoArticulosDetalle = GetIdInternoArticulosDetalle(articulo.Value.id, model.bodega);
                    oPedidosClientesDetalle.IdInternoPedidosClientes = getIdInternoPedidosClientes(model.numdoc);
                    oPedidosClientesDetalle.IdBodega = getIdBodega(model.bodega);
                    oPedidosClientesDetalle.Numdoc = model.numdoc;
                    oPedidosClientesDetalle.Fecha = DateTime.Now;
                    oPedidosClientesDetalle.Precio_unitario = (double?)articulo.Value.precio;
                    oPedidosClientesDetalle.Costo_Unitario = (double?)articuloDesc.CostoPromedioActual_Q;
                    oPedidosClientesDetalle.Unidades = articulo.Value.unidades;
                    oPedidosClientesDetalle.PorcentajeDescuento = articulo.Value.descuento;
                    oPedidosClientesDetalle.Descuento = articulo.Value.descuento / articulo.Value.unidades;
                    oPedidosClientesDetalle.descripcionarticulo = articuloDesc.compDescripcion;
                    oPedidosClientesDetalle.NombreArticulo = articuloDesc.NombreArticulo;
                    oPedidosClientesDetalle.Codigo_Empresa = oCompany.codigo_empresa;
                    oPedidosClientesDetalle.es_exento = articuloDesc.genmar;
                    oPedidosClientesDetalle.Impuesto = (articulo.Value.unidades * articulo.Value.precio) - ((articulo.Value.unidades * articulo.Value.precio) / (1 + oCompany.porcentaje_impuesto));
                    db.pedidosclientesinvdetalle.Add(oPedidosClientesDetalle);
                    db.SaveChanges();
                }
            }
        }
        public string ingresarMovimiento(PtoDeVentaViewModel model, security_companies oCompany, int idInternoCliente, string idBodega)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                var idInternoPedidosClientes = getIdInternoPedidosClientes(model.numdoc, model.serie, oCompany.codigo_empresa);

                //PRIMERO INGRESAMOS EL ENCABEZADO
                var oMovimientoInv = new movimientosinv();
                oMovimientoInv.Fecha = DateTime.Now;
                oMovimientoInv.diascredito = db.clientes.Find(idInternoCliente).DiasCredito;
                oMovimientoInv.Nombre = model.nombre;
                oMovimientoInv.direccion = model.direccion;
                oMovimientoInv.nit = model.nit;
                oMovimientoInv.correoReceptor = model.email;
                
                oMovimientoInv.porcentajedescuento = 100 - ((model.total * 100) / model.subtotal);
                oMovimientoInv.ClienteProveedor = idInternoCliente.ToString();
                oMovimientoInv.TasaCambio = 1;
                oMovimientoInv.Bodega = idBodega;

                oMovimientoInv.observaciones = "Error por transmisión FEL"; // Luego quitar si pasa la transmision
                oMovimientoInv.Anulado = "S"; // Luego cambiar a N si pasa la transmision

                oMovimientoInv.Monto = model.total;
                oMovimientoInv.Impuesto = (model.total - (model.total / (1 + oCompany.porcentaje_impuesto)));
                oMovimientoInv.Descuento = model.descuento;

                oMovimientoInv.status = "A";
                oMovimientoInv.Codigo_Empresa = oCompany.codigo_empresa;
                oMovimientoInv.IdInternoTIposMovimientosSeries = model.serie;
                oMovimientoInv.IdInternoVendedores = model.vendedor;
                oMovimientoInv.IdInternoPedidosClientes = idInternoPedidosClientes;

                //Aqui seteamos guardamos y seteamos el numdoc
                var numdoc = saveMovimiento(oMovimientoInv, model);
                return numdoc;
            }

        }
        public string saveMovimiento(movimientosinv oMovimientoInv, PtoDeVentaViewModel model)
        {
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    oMovimientoInv.Numdoc = getNumdocMovimientos(model);
                    try
                    {
                        db.movimientosinv.Add(oMovimientoInv);
                        db.SaveChanges();
                        transaccion.Commit();
                        return oMovimientoInv.Numdoc;
                    }
                    catch
                    {
                        return saveMovimiento(oMovimientoInv, model);
                    }
                }
            }
        }
        public string realizarTransmicionFEL(PtoDeVentaViewModel model, security_companies oCompany, string numdoc, int idInternoMovimientosInv, int idInternoCliente)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                //Constantes
                string localExporta = "L";
                var moneda = "Q";
                string paisReceptor = localExporta == "L" ? "GT" : null;


                string correoReceptor = model.email ?? "";
                string nombreReceptor = model.nombre;
                string direccionReceptor = model.direccion ?? "";
                string nitReceptor = model.nit;


                string departamentoReceptor, municipioReceptor, xvendedor;
                bool bResultadoBool;

                //Variables
                int codigoEstablecimiento;
                string direccionEmisor;
                var oCliente = db.clientes.Find(idInternoCliente);
                var IdCliente = oCliente.IdCliente;
                var dcredito = oCliente.DiasCredito;
                var NumTelefono = oCliente.Telefono;
                var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                var afiliacionIva = oCompany.fel_emisor_afiliacion_iva ?? "";
                var codigoPostal = oCompany.fel_emisor_codigo_postal ?? "";
                var correoEmisor = oCompany.fel_emisor_correo_emisor ?? "";
                var paisEmisor = oCompany.fel_emisor_pais ?? "";
                var departamentoEmisor = oCompany.fel_emisor_departamento ?? "";
                var municipioEmisor = oCompany.fel_emisor_municipio ?? "";
                var nitEmisor = oCompany.fel_emisor_nit ?? "";
                var nombreEmisor = oCompany.fel_emisor_razon_social ?? "";
                var nombreComercial = oCompany.fel_emisor_nombre_comercial ?? "";
                var telefono1 = oCompany.telefono1 ?? "";
                var telefono2 = oCompany.telefono2 ?? "";

                if(oCompany.maneja_sucursales == "N")
                {
                    codigoEstablecimiento = oCompany.fel_emisor_codigo_establecimiento ?? default(int);
                    direccionEmisor = oCompany.fel_emisor_direccion ?? "";
                } else
                {
                    var oBodega = db.bodegasinv.Find(model.bodega);
                    codigoEstablecimiento = oBodega.fel_emisor_codigo_establecimiento ?? default(int);
                    direccionEmisor = oBodega.fel_emisor_direccion ?? "";
                }


                //Remplazamos caracteres de los parametros
                nombreReceptor = nombreReceptor.Replace("&", "&amp;");
                nombreReceptor = nombreReceptor.Replace("\"", "&quot;");
                nombreReceptor = nombreReceptor.Replace("#", "&#35;");
                nombreReceptor = nombreReceptor.Replace("/", "&#47;");

                direccionReceptor = direccionReceptor.Replace("&", "&amp;");
                direccionReceptor = direccionReceptor.Replace("\"", "&quot;");
                direccionReceptor = direccionReceptor.Replace("#", "&#35;");
                direccionReceptor = direccionReceptor.Replace("/", "&#47;");

                nitReceptor = nitReceptor.Replace("-", "");
                nitReceptor = nitReceptor.Replace("/", "");
                nitReceptor = nitReceptor.ToUpper();
                nitReceptor = nitReceptor.Trim();

                //Objeto de FEL
                var oFel = new TransmisionFel();
                oFel.Inicializar();

                if (localExporta == "L")
                {
                    if (moneda == "Q")
                    {
                        bResultadoBool = oFel.LlenaDatosGenerales("GTQ", fechaHoy, "FACT", "", "", ""); //FCAM
                    }
                    else
                    {
                        bResultadoBool = oFel.LlenaDatosGenerales("USD", fechaHoy, "FACT", "", "", ""); //FCAM
                    }
                }
                else
                {
                    bResultadoBool = oFel.LlenaDatosGenerales("USD", fechaHoy, "FCAM", "SI", "", "");
                }
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.LlebaDatosGenerales";

                bResultadoBool = oFel.LlenaDatosEmisor(afiliacionIva, codigoEstablecimiento, codigoPostal, correoEmisor, paisEmisor, departamentoEmisor, municipioEmisor, direccionEmisor, nitEmisor, nombreEmisor, nombreComercial);
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.LlenaDatosEmisor";

                if (localExporta != "L")
                {
                    departamentoReceptor = "GUATEMALA";
                    municipioReceptor = "GUATEMALA";
                }
                else
                {
                    departamentoReceptor = "GUATEMALA";
                    municipioReceptor = "GUATEMALA";
                }

                if (correoReceptor != "")
                    correoReceptor += ";" + correoEmisor;

                bResultadoBool = oFel.LlenaDatosReceptor(nitReceptor, nombreReceptor, "010020", correoReceptor, paisReceptor, departamentoReceptor, municipioReceptor, direccionReceptor, "");
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.LlenaDatosReceptor";

                bResultadoBool = oFel.LlenaFrases(1, 1, "", "");
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.LlenaFrases";

                if (localExporta != "L")
                {
                    bResultadoBool = oFel.LlenaFrases(4, 1, "", "");
                    if (!bResultadoBool)
                        //Error
                        return "Error en oFel.LlenaFrases";
                }


                string descripcionMedida, descripcionArticulo, tipoProducto;
                string tipoArticulo, unidaMedida;

                double total = 0, valorIVA, precioSinIva = 0;
                double valorTotalIva = 0, valorTotal = 0;
                double precioUnitario = 0, precioTotal = 0, descuento = 0;

                int codigoArticulo, cantidadUnidades, idMedida;
                int i = 1;

                articulosinv oArticulo;
                medidasinv oMedida;

                //Obtenemos los detalles a facturar
                foreach (var articulo in model.articulos)
                {
                    oArticulo = db.articulosinv.Find(articulo.Value.id);
                    codigoArticulo = articulo.Value.id;
                    cantidadUnidades = articulo.Value.unidades;
                    precioUnitario = (double)(articulo.Value.precio ?? 0);
                    precioTotal = (double)(articulo.Value.total ?? 0);
                    descuento = (double)(articulo.Value.descuentoPrecio ?? 0);
                    descripcionArticulo = oArticulo.NombreArticulo ?? "";
                    tipoProducto = oArticulo.ProductoServicio;
                    idMedida = oArticulo.IdInternoMedidas;
                    oMedida = db.medidasinv.Find(idMedida);
                    unidaMedida = oMedida.fel_medida;
                    descripcionMedida = oMedida.Descripcion ?? "";
                    if (string.IsNullOrEmpty(descripcionMedida))
                        descripcionMedida = "UND";


                    if (tipoProducto == "P")
                        tipoArticulo = "B";
                    else if (tipoProducto == "S")
                        tipoArticulo = "S";
                    else
                        tipoArticulo = "B";

                    //REPLACE
                    descripcionArticulo = descripcionArticulo.Replace("&", "&amp;");
                    descripcionArticulo = descripcionArticulo.Replace("\"", "&quot;");
                    descripcionArticulo = descripcionArticulo.Replace("#", "&#35;");
                    descripcionArticulo = descripcionArticulo.Replace("/", "&#47;");
                    descripcionArticulo += "|" + descripcionMedida;

                    if (descuento > 0)
                    {
                        precioTotal = precioTotal + descuento;
                        total = (double)(precioTotal - descuento);
                    }
                    else
                    {
                        total = (double)precioTotal;
                    }

                    valorTotal = valorTotal + total;

                    if (localExporta == "E")
                    {
                        precioSinIva = Math.Round(precioTotal, 4);
                        valorIVA = 0.00;
                        valorTotalIva = valorTotalIva + valorIVA;
                    }
                    else
                    {
                        if (descuento > 0)
                        {
                            precioSinIva = Math.Round(total / 1.12, 4);
                            valorIVA = Math.Round((total / 1.12) * 12 / 100, 4);
                            valorTotalIva = valorTotalIva + valorIVA;
                        }
                        else
                        {
                            precioSinIva = Math.Round(precioTotal / 1.12, 4);
                            valorIVA = Math.Round((precioTotal / 1.12) * 12 / 100, 4);
                            valorTotalIva = valorTotalIva + valorIVA;
                        }
                    }

                    if (localExporta == "E")
                        bResultadoBool = oFel.AgregarItemUnImpuesto(tipoArticulo, unidaMedida, cantidadUnidades.ToString(), descripcionArticulo, i, precioUnitario, precioTotal, descuento, total, "IVA", 2, precioSinIva, valorIVA);
                    else
                        bResultadoBool = oFel.AgregarItemUnImpuesto(tipoArticulo, unidaMedida, cantidadUnidades.ToString(), descripcionArticulo, i, precioUnitario, precioTotal, descuento, total, "IVA", 1, precioSinIva, valorIVA);
                    if (!bResultadoBool)
                    {
                        //Error
                        return "Error en oFel.AgregarItemUnImpuesto";
                    }
                    i++;
                }

                if (localExporta == "L")
                    bResultadoBool = oFel.AsignarTotalesImpuesto("IVA", valorTotalIva);
                else
                    bResultadoBool = oFel.AsignarTotalesImpuesto("IVA", 0.00);
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.AsignarTotalesImpuesto";

                bResultadoBool = oFel.AsignarTotales(valorTotal);
                if (!bResultadoBool)
                    //Error
                    return "Error en oFel.AsignarTotales";

                if (localExporta == "E")
                {
                    bResultadoBool = oFel.ComplementoExportacion("EXPORTACION", "EXPORTACION", "http://www.sat.gob.gt/face2/ComplementoExportaciones/0.1.0", "", "", "E16550", direccionReceptor.Substring(0, 70), direccionReceptor.Substring(0, 70), "EXW", nombreReceptor, nombreReceptor, oCompany.nombre, "");
                    if (!bResultadoBool)
                        //Error
                        return "Error en oFel.ComplementoExportacion";
                }


                var tipoFac = getIdTipoMovimiento(model.serie, oCompany.codigo_empresa);
                var serieFac = db.tiposmovimientosseriesinv.Find(model.serie).IdSerie;
                var numdocFac = numdoc;
                var xCreditoContado = "C";
                xvendedor = model.nombreVendedor;
                var xcorrelativointerno = tipoFac + "-" + serieFac + "-" + numdocFac;

                oFel.AsignarAdendas("Correlativo", xcorrelativointerno);
                oFel.AsignarAdendas("telefono1", telefono1);
                oFel.AsignarAdendas("telefono2", telefono2);
                oFel.AsignarAdendas("Vendedor", xvendedor);
                oFel.AsignarAdendas("telefonocliente", NumTelefono);
                oFel.AsignarAdendas("codigoCliente", IdCliente);

                if (xCreditoContado == "D")
                    oFel.AsignarAdendas("CreditoContado", "Crédito");
                else
                    oFel.AsignarAdendas("CreditoContado", "Contado");

                string correoCopia = correoEmisor;
                //bool almacenarLocal = oCompany.fel_almacenar_local == "S" ? true : false;
                bool almacenarLocal = false; // Siempre debe de ser false para servidor en hosting
                bool almacenarXML = oCompany.fel_almacenar_xml == "S" ? true : false;

                string UUID = oFel.ObtenerUUID();
                string Resultado = oFel.TransmitirFacturaFEL(oCompany.fel_usuario, oCompany.fel_llave_general, UUID, correoCopia, oCompany.fel_usuario, oCompany.fel_llave_pfx, almacenarLocal, almacenarXML);

                var (resResultado, resTemp, resSerie, resNumero, resUUID) = ("","","","","");
                var (resInformacionAdicional, resTransaccion, resSysId) = ("","","");
                var (SysID, Errores) = ("","");

                if (Resultado.Contains("|"))
                {
                    var resultados = Resultado.Split('|');
                    resResultado = resultados[0];
                    if(resResultado == "1")
                    {
                        resSerie = resultados[1];
                        resNumero = resultados[2];
                        resUUID = resultados[3];
                        resInformacionAdicional = resultados[4];
                        if (resInformacionAdicional.Contains("transaccion"))
                        {
                            resultados = resInformacionAdicional.Split(',');
                            resTransaccion = resultados[0].Trim().Substring(12);
                            resSysId = (resultados[1] + resultados[2] + resultados[3]).Trim().Substring(6);
                        }

                        //Ahora actualizamos el encabezado de movimientos
                        var oMovimientosInv = db.movimientosinv.Find(idInternoMovimientosInv);
                        oMovimientosInv.fel_resultado = resResultado;
                        oMovimientosInv.fel_intentos = 1;
                        oMovimientosInv.fel_serie = resSerie;
                        oMovimientosInv.fel_numero = resNumero;
                        oMovimientosInv.fel_uuid = resUUID;
                        oMovimientosInv.fel_transaccion = resTransaccion;
                        oMovimientosInv.fel_sysid = resSysId;
                        oMovimientosInv.fel_transmitido = "S";
                        oMovimientosInv.Impreso = "S";
                        oMovimientosInv.Anulado = "N";
                        oMovimientosInv.observaciones = "";
                        db.Entry(oMovimientosInv).State = System.Data.Entity.EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                        } catch (Exception e)
                        {
                            return e.Message;
                        }
                    } else
                    {
                        //Error
                        return "Transisión de facturación fallida";
                    }
                } else
                {
                    //Error "mensaje no procesado"
                    return "Transisión de facturación fallida, mensaje no procesado";
                }
                return "true";
            }

        }
        public string ingresarDetallesCaja(PtoDeVentaViewModel model, security_users oUser, security_companies oCompany, string numdoc)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                //Obtenemos variables a utilizar
                int idCaja = getIdInternoCaja();
                DateTime ahora = DateTime.Now;

                //AHORA LLENAMOS EL DETALLE DE LA CAJA ACTUAL
                //EFECTIVO
                if (model.pago.efectivo.monto > 0)
                {
                    var oDetalleCaja = new movimientosdetallecaja();
                    oDetalleCaja.Usuario = oUser.username;
                    oDetalleCaja.Fecha = ahora;
                    oDetalleCaja.IdInternoTIposMovimientosSeries = model.serie;
                    oDetalleCaja.Numdoc = numdoc;
                    oDetalleCaja.Tipo = "EF";
                    oDetalleCaja.Monto = model.pago.efectivo.monto;
                    oDetalleCaja.Descripcion = "Entrada por facturacion";
                    oDetalleCaja.EntradaSalida = "E";
                    oDetalleCaja.Cerrado = "N";
                    oDetalleCaja.Codigo_Empresa = oCompany.codigo_empresa;
                    oDetalleCaja.IdInternoMovimientosCaja = idCaja;
                    oDetalleCaja.BancoTarjeta = "";
                    oDetalleCaja.Documento = "";
                    oDetalleCaja.NoTarjeta = "";
                    db.movimientosdetallecaja.Add(oDetalleCaja);
                }
                //CHEQUES
                if (model.pago.getMontoCheques() > 0)
                {
                    foreach (var cheque in model.pago.cheques)
                    {
                        var oDetalleCaja = new movimientosdetallecaja();
                        oDetalleCaja.Usuario = oUser.username;
                        oDetalleCaja.Fecha = ahora;
                        oDetalleCaja.IdInternoTIposMovimientosSeries = model.serie;
                        oDetalleCaja.Numdoc = numdoc;
                        oDetalleCaja.Tipo = "CH";
                        oDetalleCaja.Monto = cheque.Value.monto;
                        oDetalleCaja.Descripcion = "Entrada por facturacion";
                        oDetalleCaja.EntradaSalida = "E";
                        oDetalleCaja.Cerrado = "N";
                        oDetalleCaja.Codigo_Empresa = oCompany.codigo_empresa;
                        oDetalleCaja.IdInternoMovimientosCaja = idCaja;
                        oDetalleCaja.BancoTarjeta = cheque.Value.idBanco.ToString();
                        oDetalleCaja.Documento = cheque.Value.cheque;
                        oDetalleCaja.NoTarjeta = "";
                        db.movimientosdetallecaja.Add(oDetalleCaja);
                    }
                }
                //TARJETAS
                if (model.pago.getMontoTarjetas() > 0)
                {
                    foreach (var tarjeta in model.pago.tarjetas)
                    {
                        var oDetalleCaja = new movimientosdetallecaja();
                        oDetalleCaja.Usuario = oUser.username;
                        oDetalleCaja.Fecha = ahora;
                        oDetalleCaja.IdInternoTIposMovimientosSeries = model.serie;
                        oDetalleCaja.Numdoc = numdoc;
                        oDetalleCaja.Tipo = "TJ";
                        oDetalleCaja.Monto = tarjeta.Value.monto;
                        oDetalleCaja.Descripcion = "Entrada por facturacion";
                        oDetalleCaja.EntradaSalida = "E";
                        oDetalleCaja.Cerrado = "N";
                        oDetalleCaja.Codigo_Empresa = oCompany.codigo_empresa;
                        oDetalleCaja.IdInternoMovimientosCaja = idCaja;
                        oDetalleCaja.BancoTarjeta = tarjeta.Value.idEmisor.ToString();
                        oDetalleCaja.Documento = tarjeta.Value.autorizacion;
                        oDetalleCaja.NoTarjeta = tarjeta.Value.tarjeta;
                        db.movimientosdetallecaja.Add(oDetalleCaja);
                    }
                }
                //DOLARES
                if (model.pago.dolares.monto > 0)
                {
                    var oDetalleCaja = new movimientosdetallecaja();
                    oDetalleCaja.Usuario = oUser.username;
                    oDetalleCaja.Fecha = ahora;
                    oDetalleCaja.IdInternoTIposMovimientosSeries = model.serie;
                    oDetalleCaja.Numdoc = numdoc;
                    oDetalleCaja.Tipo = "DO";
                    oDetalleCaja.Monto = model.pago.dolares.monto;
                    oDetalleCaja.MontoExtranjera = model.pago.dolares.montoDolares;
                    oDetalleCaja.TasaCambio = model.pago.dolares.tasaCambio;
                    oDetalleCaja.Descripcion = "Entrada por facturacion";
                    oDetalleCaja.EntradaSalida = "E";
                    oDetalleCaja.Cerrado = "N";
                    oDetalleCaja.Codigo_Empresa = oCompany.codigo_empresa;
                    oDetalleCaja.IdInternoMovimientosCaja = idCaja;
                    oDetalleCaja.BancoTarjeta = "";
                    oDetalleCaja.Documento = "";
                    oDetalleCaja.NoTarjeta = "";
                    db.movimientosdetallecaja.Add(oDetalleCaja);
                }
                //VUELTO
                if (model.vuelto > 0)
                {
                    var oDetalleCaja = new movimientosdetallecaja();
                    oDetalleCaja.Usuario = oUser.username;
                    oDetalleCaja.Fecha = ahora;
                    oDetalleCaja.IdInternoTIposMovimientosSeries = model.serie;
                    oDetalleCaja.Numdoc = numdoc;
                    oDetalleCaja.Tipo = "EF";
                    oDetalleCaja.Monto = model.vuelto;
                    oDetalleCaja.Descripcion = "Vuelto";
                    oDetalleCaja.EntradaSalida = "V";
                    oDetalleCaja.Cerrado = "N";
                    oDetalleCaja.Codigo_Empresa = oCompany.codigo_empresa;
                    oDetalleCaja.IdInternoMovimientosCaja = idCaja;
                    oDetalleCaja.BancoTarjeta = "";
                    oDetalleCaja.Documento = "";
                    oDetalleCaja.NoTarjeta = "";
                    db.movimientosdetallecaja.Add(oDetalleCaja);
                }
                try
                {
                    db.SaveChanges();
                    return "true";
                } catch (Exception e)
                {
                    return e.Message;
                }
                
            }
        }
        public string ingresarDetallesMov(PtoDeVentaViewModel model, security_companies oCompany, string numdoc, int idInternoMovimientosInv, string idBodega )
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                //ITERAMOS LOS ARTICULOS DE LA FACTURA Y INGRESAMOS EN DETALLE DE MOVIENTOSINV
                int idInternoVendedor = model.vendedor;
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                foreach (var articulo in model.articulos)
                {
                    var articuloDesc = db.articulosinv.Find(articulo.Value.id);
                    var oMovimientoDetalleInv = new movimientosdetalleinv();
                    oMovimientoDetalleInv.IdInternoMovimientos = idInternoMovimientosInv;
                    oMovimientoDetalleInv.IdInternoVendedores = idInternoVendedor;
                    oMovimientoDetalleInv.IdInternoArticulosDetalle = GetIdInternoArticulosDetalle(articulo.Value.id, model.bodega);
                    oMovimientoDetalleInv.IdBodega = idBodega;
                    oMovimientoDetalleInv.Numdoc = numdoc;
                    oMovimientoDetalleInv.Fecha = DateTime.Now;
                    oMovimientoDetalleInv.Precio_unitario = articulo.Value.precio;
                    oMovimientoDetalleInv.Costo_Unitario = articuloDesc.CostoPromedioActual_Q;
                    oMovimientoDetalleInv.Unidades = articulo.Value.unidades;
                    oMovimientoDetalleInv.PorcentajeDescuento = articulo.Value.descuento;
                    oMovimientoDetalleInv.Descuento = articulo.Value.descuentoPrecio;
                    oMovimientoDetalleInv.descripcionarticulo = articuloDesc.compDescripcion;
                    oMovimientoDetalleInv.NombreArticulo = articuloDesc.NombreArticulo;
                    oMovimientoDetalleInv.Codigo_Empresa = oCompany.codigo_empresa;
                    oMovimientoDetalleInv.es_exento = articuloDesc.genmar;
                    oMovimientoDetalleInv.Impuesto = (articulo.Value.unidades * articulo.Value.precio) - ((articulo.Value.unidades * articulo.Value.precio) / (1 + oCompany.porcentaje_impuesto));
                    db.movimientosdetalleinv.Add(oMovimientoDetalleInv);
                }
                try
                {
                    db.SaveChanges();
                    return "true";
                } catch (Exception e)
                {
                    return e.Message;
                }
                
            }

        }
        


        /***************** ESTAS FUNCIONES AGREGAN FORMAS DE PAGO AL MODELO *****************/
        [HttpPost]
        public ActionResult addEfectivo(decimal? efectivo)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                //Añadimos el efectivo
                model.addEfectivo(efectivo);
                Session["Sale"] = model;

                return
                    Json(new
                    {
                        success = true,
                        recibido = model.pago.montoTotal,
                        vuelto = model.vuelto
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult addCheque(int id, int idBanco, string cheque, decimal? monto)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                if (model.pago.cheques.ContainsKey(id))
                {
                    //Modificamos el cheque
                    model.updateCheque(id, idBanco, cheque, monto);
                } 
                else
                {
                    //Añadimos el cheque
                    model.addCheque(id, idBanco, cheque, monto);
                }
                Session["Sale"] = model;
                return Json(new{success = true,recibido = model.pago.montoTotal,vuelto = model.vuelto}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult addTarjeta(int id, int idEmisor, string tarjeta, string autorizacion, decimal? monto)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                if (model.pago.tarjetas.ContainsKey(id))
                {
                    //Modificamos el cheque
                    model.updateTarjeta(id, idEmisor, tarjeta, autorizacion, monto);
                }
                else
                {
                    //Añadimos el cheque
                    model.addTajeta(id, idEmisor, tarjeta, autorizacion, monto);
                }
                Session["Sale"] = model;
                return Json(new { success = true, recibido = model.pago.montoTotal, vuelto = model.vuelto }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult addDolares(decimal? tasa, decimal? dolares)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                //Añadimos el efectivo
                model.addDolares(tasa,dolares);
                Session["Sale"] = model;

                return
                    Json(new
                    {
                        success = true,
                        recibido = model.pago.montoTotal,
                        vuelto = model.vuelto,
                        monto = model.pago.dolares.monto
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        /***************** ESTAS FUNCIONES ELIMINAN FORMAS DE PAGO AL MODELO *****************/
        public ActionResult deleteCheque(int id)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                if (model.pago.cheques.ContainsKey(id))
                {
                    //Eliminamos el cheque
                    model.pago.cheques.Remove(id);
                    model.calcularPago();
                }
                Session["Sale"] = model;
                return Json(new { success = true, recibido = model.pago.montoTotal, vuelto = model.vuelto }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteTarjeta(int id)
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            if (model != null)
            {
                if (model.pago.tarjetas.ContainsKey(id))
                {
                    //Eliminamos la tarjeta
                    model.pago.tarjetas.Remove(id);
                    model.calcularPago();
                }
                Session["Sale"] = model;
                return Json(new { success = true, recibido = model.pago.montoTotal, vuelto = model.vuelto }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        /***************************** PETICIONES DE CONTADORES *****************************/
        [HttpPost]
        public ActionResult getChequesCont()
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            model.pago.chequesCont++;
            Session["Sale"] = model;
            if (model != null)
            {
                Session["Sale"] = model;
                return Json(new { success = true, chequesCont = model.pago.chequesCont }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult getTarjetasCont()
        {
            var model = Session["Sale"] as PtoDeVentaViewModel;
            model.pago.tarjetasCont++;
            Session["Sale"] = model;
            if (model != null)
            {
                Session["Sale"] = model;
                return Json(new { success = true, tarjetasCont = model.pago.tarjetasCont }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }   



        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELVEN LISTAS *****************/
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



        /***************** ESTAS FUNCIONES SON CONSULTAS QUE DEVUELDEN DATOS *****************/
        public int getIdInternoMovimientosInv(string numdoc, int serie, string codigoEmpresa)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return
                    (from d in db.movimientosinv
                     where d.Numdoc == numdoc &&
                     d.IdInternoTIposMovimientosSeries == serie &&
                     d.Codigo_Empresa == codigoEmpresa
                     select d.IdInternoMovimientos).FirstOrDefault();
            }
        }
        public int getIdInternoPedidosClientes(string numdoc, int serie, string codigoEmpresa)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return
                    (from d in db.pedidosclientesinv
                     where d.Numdoc == numdoc &&
                     d.IdInternoTIposMovimientosSeries == serie &&
                     d.Codigo_Empresa == codigoEmpresa
                     select d.IdInternoPedidosClientes).FirstOrDefault();
            }
        }
        public int getIdInternoCliente(string nit)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return
                    (from d in db.clientes
                     where d.nit == nit
                     select d.IdInternoClientes).DefaultIfEmpty(-1).FirstOrDefault();
            }
        }
        public int getIdInternoCaja()
        {
            var oCompany = (security_companies)Session["Company"];
            var oUser = (security_users)Session["User"];
            var hoy = DateTime.Now;
            int id;
            using (var db = new db_pcsolutions_webEntities())
            {
                id = (from d in db.movimientoscaja
                      where d.Codigo_Empresa == oCompany.codigo_empresa &&
                      hoy <= d.Fecha_Final && hoy >= d.Fecha_Inicio &&
                      d.Cerrado == "N" && oUser.IdInternoSecurityUser == d.IdInternoSecurityUser
                      select d.IdInternoMovimientosCaja).DefaultIfEmpty(-1).FirstOrDefault();
            }
            return id;
        }
        public int GetIdInternoArticulosDetalle(int idArticulo, int idBodega)
        {
            var oCompany = (security_companies)Session["Company"];
            int idArticuloDetalle;
            using (var db = new db_pcsolutions_webEntities())
            {
                idArticuloDetalle = (from d in db.articulosdetalleinv
                                     where d.Codigo_Empresa == oCompany.codigo_empresa &&
                                     d.IdInternoBodegas == idBodega &&
                                     d.IdInternoArticulos == idArticulo
                                     select d.IdInternoArticulosDetalle).FirstOrDefault();
            }
            return idArticuloDetalle;
        }
        public int getIdInternoPedidosClientes(string numdoc)
        {
            var oCompany = (security_companies)Session["Company"];
            int idPedidosClientes;
            using (var db = new db_pcsolutions_webEntities())
            {
                idPedidosClientes = (from d in db.pedidosclientesinv
                                     where d.Codigo_Empresa == oCompany.codigo_empresa &&
                                     d.Numdoc == numdoc
                                     select d.IdInternoPedidosClientes).FirstOrDefault();
            }
            return idPedidosClientes;
        }
        public int getIdInternoDetalle(int idCaja)
        {
            var oCompany = (security_companies)Session["Company"];
            int idInterno;
            using (var db = new db_pcsolutions_webEntities())
            {
                idInterno = (from d in db.movimientosdetallecaja
                             where d.Codigo_Empresa == oCompany.codigo_empresa &&
                                 d.IdInternoMovimientosCaja == idCaja
                             select d.IdInternoMovimientosCaja).DefaultIfEmpty(0).Max();
            }
            return idInterno;
        }
        public string getIdBodega(int id)
        {
            var oCompany = (security_companies)Session["Company"];
            string idBodega;
            using (var db = new db_pcsolutions_webEntities())
            {
                idBodega = (from d in db.bodegasinv
                            where d.Codigo_Empresa == oCompany.codigo_empresa &&
                            d.IdInternoBodegas == id
                            select d.IdBodega).FirstOrDefault();
            }
            return idBodega;
        }
        public string getIdTipoMovimiento(int idInternoTipoSerie, string codigoEmpresa)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                return
                    (from d in db.tiposmovimientosinv
                     join e in db.tiposmovimientosseriesinv
                     on d.IdInternoTiposMovimientos equals e.IdInternoTiposMovimientos
                     where e.IdInternoTIposMovimientosSeries == idInternoTipoSerie &&
                     d.Codigo_Empresa == codigoEmpresa
                     select d.IdTipoMovimiento).FirstOrDefault();
            }
        }
        public string getNumdocMovimientos(PtoDeVentaViewModel model)
        {
            var oCompany = (security_companies)Session["Company"];
            string correlativo;
            using (var db = new db_pcsolutions_webEntities())
            {
                correlativo = (from d in db.movimientosinv
                               where d.Codigo_Empresa == oCompany.codigo_empresa &&
                                   d.IdInternoTIposMovimientosSeries == model.serie
                               select d.Numdoc).DefaultIfEmpty("0").Max();
            }
            return (Int32.Parse(correlativo) + 1).ToString("D10");
        }
    }
}