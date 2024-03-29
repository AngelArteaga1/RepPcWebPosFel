//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Minible5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class movimientosinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public movimientosinv()
        {
            this.movimientosdetallecombosinv = new HashSet<movimientosdetallecombosinv>();
            this.movimientosdetalleinv = new HashSet<movimientosdetalleinv>();
        }
    
        public int IdInternoMovimientos { get; set; }
        public string Numdoc { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Descripcion { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public Nullable<decimal> Impuesto { get; set; }
        public string Impreso { get; set; }
        public string Anulado { get; set; }
        public string ClienteProveedor { get; set; }
        public Nullable<double> TasaCambio { get; set; }
        public string Bodega { get; set; }
        public string IncluyePedido { get; set; }
        public string PolizaImportacion { get; set; }
        public string Autorizado { get; set; }
        public string Entregado { get; set; }
        public string CreditoContado { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public string NoFactura { get; set; }
        public string TipoFactura { get; set; }
        public string SerieFactura { get; set; }
        public Nullable<int> Secuencia { get; set; }
        public string nit { get; set; }
        public string Nombre { get; set; }
        public string direccion { get; set; }
        public Nullable<decimal> Monto_Q { get; set; }
        public Nullable<decimal> Impuesto_Q { get; set; }
        public Nullable<decimal> Descuento_Q { get; set; }
        public Nullable<decimal> MontoArancel { get; set; }
        public string Moneda { get; set; }
        public string LocalExporta { get; set; }
        public string FormaPago { get; set; }
        public string NumCheque { get; set; }
        public string CasaEmisora { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public string Domicilio { get; set; }
        public string Descripcion2 { get; set; }
        public string CorrelativoPartida { get; set; }
        public Nullable<System.DateTime> mesanio { get; set; }
        public string usuario { get; set; }
        public string PolizaImportacion2 { get; set; }
        public string TrasladaIVA { get; set; }
        public Nullable<System.DateTime> MesDeclaracionIVA { get; set; }
        public string PesoUnidades { get; set; }
        public string registro { get; set; }
        public string giro { get; set; }
        public string exento { get; set; }
        public Nullable<decimal> valorexento { get; set; }
        public string FacturaContribuyente { get; set; }
        public Nullable<decimal> porcentajeRetencion { get; set; }
        public Nullable<decimal> valorRetencion { get; set; }
        public Nullable<decimal> diascredito { get; set; }
        public string cotizacion { get; set; }
        public string observaciones { get; set; }
        public Nullable<decimal> porcentajedescuento { get; set; }
        public string numpartidaarancel { get; set; }
        public Nullable<System.DateTime> fecha_vencimiento { get; set; }
        public Nullable<System.DateTime> fechaoperacioniva { get; set; }
        public Nullable<int> tasacambiocontrol { get; set; }
        public string nfacgenfac { get; set; }
        public string ordencompra { get; set; }
        public string tipo_precio { get; set; }
        public string tipopago { get; set; }
        public Nullable<decimal> totalcuotas { get; set; }
        public Nullable<decimal> nopagos { get; set; }
        public Nullable<decimal> diapago { get; set; }
        public Nullable<decimal> diapago2 { get; set; }
        public Nullable<decimal> montoultimacuota { get; set; }
        public Nullable<System.DateTime> fechainiciopago { get; set; }
        public Nullable<decimal> montoenganche { get; set; }
        public Nullable<decimal> montopago { get; set; }
        public string telefono { get; set; }
        public string fel_transmitido { get; set; }
        public string fel_resultado { get; set; }
        public Nullable<int> fel_intentos { get; set; }
        public string fel_xml { get; set; }
        public string fel_peticion { get; set; }
        public string fel_serie { get; set; }
        public string fel_numero { get; set; }
        public string fel_uuid { get; set; }
        public string fel_transaccion { get; set; }
        public string fel_sysid { get; set; }
        public string fel_Anulado { get; set; }
        public Nullable<System.DateTime> fechafac_notas { get; set; }
        public string correoReceptor { get; set; }
        public string fel_localexporta { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoTIposMovimientosSeries { get; set; }
        public int IdInternoVendedores { get; set; }
        public Nullable<int> IdInternoPedidosTransitos { get; set; }
        public Nullable<int> IdInternoPedidosClientes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallecombosinv> movimientosdetallecombosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetalleinv> movimientosdetalleinv { get; set; }
        public virtual vendedores vendedores { get; set; }
        public virtual tiposmovimientosseriesinv tiposmovimientosseriesinv { get; set; }
    }
}
