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
    
    public partial class tiposmovimientosseriesinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tiposmovimientosseriesinv()
        {
            this.importacionescargosinv = new HashSet<importacionescargosinv>();
            this.movimientosdetallecaja = new HashSet<movimientosdetallecaja>();
            this.movimientosdetallecomboshistoricosinv = new HashSet<movimientosdetallecomboshistoricosinv>();
            this.movimientoshistoricosinv = new HashSet<movimientoshistoricosinv>();
            this.movimientosinv = new HashSet<movimientosinv>();
            this.pedidosclientesinv = new HashSet<pedidosclientesinv>();
            this.traslados = new HashSet<traslados>();
        }
    
        public int IdInternoTIposMovimientosSeries { get; set; }
        public string IdSerie { get; set; }
        public string correlativo { get; set; }
        public string UsaCorrelativo { get; set; }
        public string FormatoImpresion { get; set; }
        public Nullable<System.DateTime> FechaAutorizacion { get; set; }
        public string res_del { get; set; }
        public string res_al { get; set; }
        public string ResolucionNumero { get; set; }
        public Nullable<int> Secuencia { get; set; }
        public string NombreComputadora { get; set; }
        public string GrabadaExenta { get; set; }
        public Nullable<System.DateTime> FechaVencimiento { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoTiposMovimientos { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importacionescargosinv> importacionescargosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallecaja> movimientosdetallecaja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallecomboshistoricosinv> movimientosdetallecomboshistoricosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientoshistoricosinv> movimientoshistoricosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosinv> movimientosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidosclientesinv> pedidosclientesinv { get; set; }
        public virtual tiposmovimientosinv tiposmovimientosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<traslados> traslados { get; set; }
    }
}
