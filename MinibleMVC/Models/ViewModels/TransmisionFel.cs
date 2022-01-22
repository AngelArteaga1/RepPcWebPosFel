using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using conectorfelv2;
using System.Web;

namespace Minible5.Models.ViewModels.TransmisionFEL
{

    [ComVisible(true)]
    public interface ITransmisionFel
    {
        void Inicializar();

        Boolean LlenaDatosGenerales(String codigoMoneda, String fechaHoraEmision, String tipo,
            String exportacion, String numeroAcceso, string tipopersona);

        Boolean LlenaDatosEmisor(String afiliacionIva, Int32 codigoEstablecimiento, String codigoPostal,
            String correoEmisor, String pais, string departamento, String municipio, String direccion, String nitEmisor,
            String nombreEmisor, String nombreComercial);

        Boolean LlenaDatosReceptor(String idReceptor, String nombreReceptor, String codigoPostalReceptor,
            String correoReceptor, String paisReceptor, String departamentoReceptor, String municipioReceptor,
            String direccionReceptor, String tipoEspecialReceptor);

        Boolean AgregarItemUnImpuesto(String bienServicio, String unidadMedida, String cantidadUnidades,
            String descripcion, Int32 numeroLinea, Double precioUnitario, Double precioTotal, Double descuento,
            Double total, String nombreCortoImpuesto, Int32 CodigoUnidadGravable, Double montoGravable, Double montoImpuesto);

        Boolean AgregarItemDosImpuestos(String bienServicio, String unidadMedida, String cantidadUnidades,
            String descripcion, Int32 numeroLinea, Double precioUnitario, Double precioTotal, Double descuento,
            Double total, String nombreCortoImpuesto, Int32 CodigoUnidadGravable, Double montoGravable, Double montoImpuesto,
            String nombreCortoImpuesto2, Int32 CodigoUnidadGravable2, Double montoGravable2, Double montoImpuesto2);

        Boolean AgregarItemSinImpuesto(String bienServicio, String unidadMedida, String cantidadUnidades,
            String descripcion, Int32 numeroLinea, Double precioTotal, Double precioUnitario, Double descuento,
            Double total);

        Boolean AsignarTotalesImpuesto(String nombreImpuesto, Double valorTotalImpuesto);

        Boolean AsignarTotales(Double valorTotalFactura);

        Boolean AsignarAdendas(string aLlave, string aValor);

        Boolean LlenaFrases(Int32 frase, Int32 escenario, string numero_resolucion, string fecha_resolucion);

        String TransmitirFacturaFEL(String prefijo, String llaveGeneral, String identificadorTransaccion,
           String correoCopia, String aliasPFX, String llavePFX, Boolean generarTransaccion, Boolean guardarXML);


        Boolean ComplementoExportacion(String idComplemento, String nombreComplemento, String uriComplemento,
            String codigoComprador, String codigoConsignatarioODestinatario, String codigoExportador,
            String direccionComprador, String direccionConsignatarioODestinatario, String incoterm,
            String nombreComprador, String nombreConsignatarioODestinatario, String nombreExportador,
            String otraReferencia);

        Boolean ComplementoNotas(string IDComplemento, string NombreComplemento, string URIComplemento,
            string FechaEmisionDocumentoOrigen, string MotivoAjuste, string NumeroAutorizacionDocumentoOrigen,
            string RegimenAntiguo, string SerieDocumentoOrigen, string NumeroDocumentoOrigen);

        Boolean ComplementoCambiaria(string idComplemento, string nombreCompleto, string uriCompleto);

        Boolean AbonoFacturasCambiarias(string fechaFin, int abono, string monto);

        String ObtenerUUID();

        String Descargar(String asUrl, String asUbicacionDestino, String asNombreArchivoDestino, Boolean abSobreEscribir, Boolean abEjecutar);

        string AnulacionFacturaFEL(string prefijo, string llave, string identificador, string correoCopia,
        string aliasPfx, string llavepfx, bool GeneraTransaccion, bool guardarXML);

        Boolean DatosAnulacion(string FechaHoraAnulacion, string FechaEmisionDocumentoAnular, string IDReceptor,
            string NITEmisor, string MotivoAnulacion, string NumeroDocumentoAAnular);

        Boolean ComplementoEspecial(string idComplemento, string nombreComplemento, string URIComplemento,
            string retensionISR, string retensionIVA, string totalMenosRetenciones);

    }

    [ComVisible(true)]
    [ComDefaultInterface(typeof(ITransmisionFel))]
    [ClassInterface(ClassInterfaceType.None)]
    public class TransmisionFel : ITransmisionFel
    {

        // Objeto de conexión a dll de traducción Fel



        conectorfelv2.RequestCertificacionFel _RequestCertificacionFel;
        conectorfelv2.RequestAnulacionFel _RequestAnulacionFel;

        public void Inicializar()
        {
            this._RequestCertificacionFel = new conectorfelv2.RequestCertificacionFel();
            this._RequestAnulacionFel = new conectorfelv2.RequestAnulacionFel();
        }
        public bool LlenaDatosGenerales(string codigoMoneda, string fechaHoraEmision, string tipo,
            string exportacion, string numeroAcceso, string tipopersona)
        {
            return this._RequestCertificacionFel.Datos_generales(codigoMoneda, fechaHoraEmision, tipo, exportacion, numeroAcceso, tipopersona);


        }

        public bool LlenaDatosEmisor(string afiliacionIva, int codigoEstablecimiento, string codigoPostal,
            string correoEmisor, string pais, string departamento, string municipio, string direccion, string nitEmisor,
            string nombreEmisor, string nombreComercial)
        {
            return this._RequestCertificacionFel.Datos_emisor(afiliacionIva, codigoEstablecimiento,
                codigoPostal, correoEmisor, pais, departamento,
                municipio, direccion, nitEmisor, nombreEmisor,
                nombreComercial);
        }

        public bool LlenaDatosReceptor(string idReceptor, string nombreReceptor, string codigoPostalReceptor,
            string correoReceptor, string paisReceptor, string departamentoReceptor, string municipioReceptor,
            string direccionReceptor, string tipoEspecialReceptor)
        {
            return this._RequestCertificacionFel.Datos_receptor(idReceptor, nombreReceptor,
                codigoPostalReceptor, correoReceptor, paisReceptor, departamentoReceptor,
                municipioReceptor, direccionReceptor, tipoEspecialReceptor);
        }

        public bool AgregarItemUnImpuesto(string bienServicio, string unidadMedida, string cantidadUnidades,
            string descripcion, int numeroLinea, double precioUnitario, double precioTotal, double descuento,
            double total, string nombreCortoImpuesto, int codigoUnidadGrabable, double montoGravable, double montoImpuesto)
        {
            return this._RequestCertificacionFel.Item_un_impuesto(bienServicio, unidadMedida,
                cantidadUnidades.ToString(), descripcion, numeroLinea,
               precioUnitario.ToString(), precioTotal.ToString(), descuento.ToString(),
               total.ToString(), nombreCortoImpuesto, codigoUnidadGrabable, "",
               montoGravable.ToString(), montoImpuesto.ToString());
        }

        public bool AgregarItemDosImpuestos(String bienServicio, String unidadMedida, String cantidadUnidades,
            String descripcion, Int32 numeroLinea, Double precioUnitario, Double precioTotal, Double descuento,
            Double total, String nombreCortoImpuesto, Int32 CodigoUnidadGravable, Double montoGravable, Double montoImpuesto,
            String nombreCortoImpuesto2, Int32 CodigoUnidadGravable2, Double montoGravable2, Double montoImpuesto2)
        {
            return this._RequestCertificacionFel.Item_dos_impuestos(bienServicio, unidadMedida,
                cantidadUnidades.ToString(), descripcion, numeroLinea,
               precioUnitario.ToString(), precioTotal.ToString(), descuento.ToString(),
               total.ToString(), nombreCortoImpuesto, CodigoUnidadGravable, "",
               montoGravable.ToString(), montoImpuesto.ToString(), nombreCortoImpuesto2, CodigoUnidadGravable2, "",
               montoGravable2.ToString(), montoImpuesto2.ToString());
        }

        public bool AgregarItemSinImpuesto(string bienServicio, string unidadMedida, string cantidadUnidades,
            string descripcion, int numeroLinea, double precioTotal, double precioUnitario, double descuento,
            double total)
        {
            return this._RequestCertificacionFel.Item_sin_impuesto(bienServicio, unidadMedida,
                cantidadUnidades.ToString(), descripcion, numeroLinea,
               precioUnitario.ToString(), precioTotal.ToString(), descuento.ToString(),
               total.ToString()
            );
        }

        public bool AsignarTotalesImpuesto(string nombreImpuesto, double valorTotalImpuesto)
        {
            return this._RequestCertificacionFel.total_impuestos(nombreImpuesto, valorTotalImpuesto.ToString());
        }

        public bool AsignarTotales(double valorTotalFactura)
        {
            return this._RequestCertificacionFel.Totales(valorTotalFactura.ToString());

        }



        public string TransmitirFacturaFEL(string prefijo, string llaveGeneral, string identificadorTransaccion,
            string correoCopia, string aliasPFX, string llavePFX, bool generarTransaccion, bool guardarXML)
        {
            this._RequestCertificacionFel.Agregar_adendas();
            string resultado = this._RequestCertificacionFel.enviar_peticion_fel(
                prefijo,
                llaveGeneral,
                identificadorTransaccion,
                correoCopia,
                aliasPFX,
                llavePFX,
                generarTransaccion);

            //System.Windows.Forms.MessageBox.Show(resultado, "resultado dll");
            string retorno = procesarRespuesta(resultado, guardarXML);
            //System.Windows.Forms.MessageBox.Show(retorno, "procesarRespuesta");

            this._RequestCertificacionFel = null;
            return retorno;
        }

        public bool LlenaFrases(int frase, int escenario, string numero_resolucion, string fecha_resolucion)
        {
            return this._RequestCertificacionFel.Frases(frase, escenario, numero_resolucion, fecha_resolucion);
        }

        public bool ComplementoExportacion(string idComplemento, string nombreComplemento, string uriComplemento,
            string codigoComprador, string codigoConsignatarioODestinatario, string codigoExportador,
            string direccionComprador, string direccionConsignatarioODestinatario, string incoterm,
            string nombreComprador, string nombreConsignatarioODestinatario, string nombreExportador,
            string otraReferencia)
        {
            return this._RequestCertificacionFel.Complemento_exportacion(idComplemento, nombreComplemento, uriComplemento,
                codigoComprador, codigoConsignatarioODestinatario, codigoExportador, direccionComprador,
                direccionConsignatarioODestinatario, incoterm, nombreComprador, nombreConsignatarioODestinatario,
                nombreExportador, otraReferencia);

        }



        private string procesarRespuesta(string jsonStr, bool guardarXML)
        {

            string uuid;
            string serie;
            string numero;
            string transaccion;
            string sysId;
            string[] arrResultado;
            string resultado;
            char separador = ',';
            string detalleJson;
            string informacionAdicional;
            string xmlStrB64;
            string xmlStr;
            string nombreArchivo;
            string retorno;
            arrResultado = jsonStr.Split(separador);
            detalleJson = jsonStr.Substring(jsonStr.IndexOf("detalle"));

            try
            {
                resultado = arrResultado[0];
                resultado = resultado.Substring(resultado.IndexOf(":") + 1);

                detalleJson = detalleJson.Substring(detalleJson.IndexOf(":") + 2);
                detalleJson = detalleJson.Substring(0, detalleJson.Length - 1).Trim();

                if (resultado == "true")
                {

                    JObject json = JObject.Parse(detalleJson);

                    serie = json.GetValue("serie").ToString();
                    numero = json.GetValue("numero").ToString();
                    uuid = json.GetValue("uuid").ToString();
                    informacionAdicional = json.GetValue("informacion_adicional").ToString();
                    xmlStrB64 = json.GetValue("xml_certificado").ToString();

                    xmlStr = Base64Decode(xmlStrB64);
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(xmlStr);

                    serie = serie.Replace("*", "");
                    serie = serie.Replace("\\", "");
                    serie = serie.Replace("/", "");

                    numero = numero.Replace("*", "");
                    numero = numero.Replace("\\", "");
                    numero = numero.Replace("/", "");

                    nombreArchivo = "~/FEL/" + serie + "-" + numero + ".XML";
                    if (guardarXML)
                        xml.Save(HttpContext.Current.Server.MapPath("~/FEL/" + serie + "-" + numero + ".XML"));

                    retorno = "1|" + serie + "|" + numero + "|" + uuid + "|" + informacionAdicional;
                }
                else
                {
                    if (detalleJson.IndexOf("{") >= 0)
                    {
                        JObject json = JObject.Parse(detalleJson);
                        JArray jarray = new JArray(json.GetValue("descripcion_errores"));
                        if (jarray.Count > 0)
                        {
                            retorno = extraerErroresJsonArray(jarray);

                        }
                        else
                        {
                            retorno = "-1|ERROR DESCONOCIDO";
                        }
                    }
                    else
                    {
                        retorno = "-1|ERROR - " + detalleJson;
                    }
                }
            }
            catch (Exception e)
            {
                retorno = "-1|ERROR DESCONOCIDO : " + e.Message;
                //throw;
            }

            return retorno;
        }

        private String extraerErroresJsonArray(JArray jarray)
        {
            String resultado = "";
            List<String> lista = new List<string>();

            for (int i = 0; i < jarray.Children().Count(); i++)
            {

                for (int j = 0; j < jarray.ElementAt(i).Children().Count(); j++)
                {
                    JObject jobj = JObject.Parse(jarray.ElementAt(i).ElementAt(j).ToString());
                    if (jobj.ContainsKey("mensaje_error"))
                    {
                        lista.Add(jobj.GetValue(("mensaje_error")).ToString());
                    }
                }
            }

            int k = 0;
            foreach (var item in lista)
            {
                if (k == 0)
                    resultado = item;
                else
                    resultado = resultado + ", " + item;
                k++;
            }
            return resultado;
        }


        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string ObtenerUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Descargar Documento en pdf
        /// </summary>
        /// <param name="asUrl">Direccion web</param>
        /// <param name="asUbicacionDestino">Directorio de ubicación del archivo de destino</param>
        /// <param name="asNombreArchivoDestino">Ubicación y nombre del directorio</param>
        /// <param name="abSobreEscribir">Bandiera para habilitar la sobreescritura del documento</param>
        /// <param name="abEjectuar">Bandera para habilitar la opción de abrir el archivo al finalizar</param>
        public string Descargar(string asUrl, string asUbicacionDestino, string asNombreArchivoDestino, bool abSobreEscribir, bool abEjectuar)
        {

            String archivo = asUbicacionDestino + "\\" + asNombreArchivoDestino;
            Boolean ejecutarDescarga = false;
            Boolean archivoExiste = false;
            string resultado = "";
            //throw new NotImplementedException();
            WebClient webClient = new WebClient();

            // Verifico si existe el directorio, sino existe intenta crearlo
            if (Directory.Exists(asUbicacionDestino))
            {
                // Si esta habilitada la sobreescritura, habilitamos la ejecución de la descarga
                if (File.Exists(archivo))
                {
                    archivoExiste = true;
                    if (abSobreEscribir)
                    {
                        ejecutarDescarga = true;
                    }
                    else
                    {
                        ejecutarDescarga = false;
                        resultado = "-2|Archivo ya existe";
                    }
                }
                else
                    ejecutarDescarga = true;
            }
            else
            {
                // No existe el directorio, revocar la descarga
                resultado = "-1|No existe el directorio especificado";
            }

            try
            {
                if (ejecutarDescarga)
                {
                    File.Delete(archivo);
                    webClient.DownloadFile(new Uri(asUrl), archivo);
                    if (abEjectuar)
                    {
                        System.Diagnostics.Process.Start(archivo);
                    }
                    resultado = "1|Archivo " + asNombreArchivoDestino + " creado de forma exitosa en: " + asUbicacionDestino;
                }
            }
            catch (Exception e)
            {
                resultado = "-3|Excepcion: " + e.Message.ToString();
                //throw;
            }
            return resultado;
        }

        public bool AsignarAdendas(string aLlave, string aValor)
        {
            //this._RequestCertificacionFel.Adendas(aLlave, aValor);
            // return _RequestCertificacionFel.Agregar_adendas();
            return this._RequestCertificacionFel.Adendas(aLlave, aValor);
        }

        public bool ComplementoCambiaria(string idComplemento, string nombreCompleto, string uriCompleto)
        {
            return this._RequestCertificacionFel.Complemento_cambiaria(idComplemento, nombreCompleto, uriCompleto);


        }

        public bool AbonoFacturasCambiarias(string fechaFin, int abono, string monto)
        {
            return this._RequestCertificacionFel.Abonos_factura_cambiaria(fechaFin, abono, monto);
        }

        public string AnulacionFacturaFEL(string prefijo, string llave, string identificador, string correoCopia, string aliasPfx, string llavepfx, bool GeneraTransaccion, bool guardarXML)
        {
            string resultado = this._RequestAnulacionFel.enviar_anulacion_fel(prefijo, llave, identificador, correoCopia, aliasPfx, llavepfx, GeneraTransaccion);
            string retorno = procesarRespuesta(resultado, guardarXML);

            this._RequestCertificacionFel = null;
            return retorno;

        }

        public bool DatosAnulacion(string FechaHoraAnulacion, string FechaEmisionDocumentoAnular, string IDReceptor, string NITEmisor, string MotivoAnulacion, string NumeroDocumentoAAnular)
        {
            return this._RequestAnulacionFel.Datos_anulacion(FechaEmisionDocumentoAnular, FechaEmisionDocumentoAnular, IDReceptor, NITEmisor, MotivoAnulacion, NumeroDocumentoAAnular);
        }

        public bool ComplementoNotas(string IDComplemento, string NombreComplemento, string URIComplemento, string FechaEmisionDocumentoOrigen, string MotivoAjuste, string NumeroAutorizacionDocumentoOrigen, string RegimenAntiguo, string SerieDocumentoOrigen, string NumeroDocumentoOrigen)
        {
            return this._RequestCertificacionFel.Complemento_notas(IDComplemento, NombreComplemento, URIComplemento, FechaEmisionDocumentoOrigen, MotivoAjuste, NumeroAutorizacionDocumentoOrigen, RegimenAntiguo, SerieDocumentoOrigen, NumeroAutorizacionDocumentoOrigen);
        }

        public bool ComplementoEspecial(string idComplemento, string nombreComplemento, string uriComplemento,
                                        string retensionISR, string retensionIVA, string totalMenosRetenciones)
        {
            return this._RequestCertificacionFel.Complemento_especial(idComplemento, nombreComplemento, uriComplemento,
                                                                      retensionISR, retensionIVA, totalMenosRetenciones);
        }
    }

}
