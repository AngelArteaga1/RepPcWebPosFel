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
    
    public partial class familiasinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public familiasinv()
        {
            this.articulosinv = new HashSet<articulosinv>();
        }
    
        public int IdInternoFamilias { get; set; }
        public string IdFamilia { get; set; }
        public string Descripcion { get; set; }
        public string Cta_Ventas { get; set; }
        public string Cta_Costo { get; set; }
        public string Cta_Inventario { get; set; }
        public string Cta_Impuesto { get; set; }
        public string Cta_Rebaja { get; set; }
        public Nullable<decimal> DesviacionBlanco { get; set; }
        public Nullable<decimal> DesviacionAmarillo { get; set; }
        public Nullable<decimal> DesviacionRojo { get; set; }
        public string cta_costos_exento { get; set; }
        public string cta_prodproceso { get; set; }
        public Nullable<decimal> porcentaje_comision { get; set; }
        public string Cta_Ventas_exento { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<articulosinv> articulosinv { get; set; }
    }
}
