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
    
    public partial class combosdetalleinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public combosdetalleinv()
        {
            this.movimientosdetalleinv = new HashSet<movimientosdetalleinv>();
            this.movimientosdetallehistoricosinv = new HashSet<movimientosdetallehistoricosinv>();
        }
    
        public int IdInternoCombosDetalle { get; set; }
        public string idcombo { get; set; }
        public Nullable<decimal> unidades { get; set; }
        public Nullable<decimal> precio_unitario { get; set; }
        public Nullable<decimal> costo_unitario { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string codigo_empresa { get; set; }
        public int IdInternoArticulos { get; set; }
    
        public virtual articulosinv articulosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetalleinv> movimientosdetalleinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallehistoricosinv> movimientosdetallehistoricosinv { get; set; }
    }
}
