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
    
    public partial class pedidostransitoinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pedidostransitoinv()
        {
            this.movimientosinv = new HashSet<movimientosinv>();
            this.pedidostransitodetalleinv = new HashSet<pedidostransitodetalleinv>();
        }
    
        public int IdInternoPedidosTransitos { get; set; }
        public string Numdoc { get; set; }
        public Nullable<System.DateTime> FechaEmision { get; set; }
        public Nullable<System.DateTime> FechaEmbarque { get; set; }
        public Nullable<System.DateTime> FechaRecibido { get; set; }
        public string Transporte { get; set; }
        public Nullable<decimal> MontoFOB { get; set; }
        public Nullable<decimal> MontoCIF { get; set; }
        public Nullable<decimal> MontoLocal { get; set; }
        public Nullable<decimal> TasaCambio { get; set; }
        public string Cerrado { get; set; }
        public string descripcion { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoProveedores { get; set; }
        public int IdInternoBodegas { get; set; }
    
        public virtual bodegasinv bodegasinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosinv> movimientosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidostransitodetalleinv> pedidostransitodetalleinv { get; set; }
        public virtual proveedores proveedores { get; set; }
    }
}