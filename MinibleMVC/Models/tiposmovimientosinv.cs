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
    
    public partial class tiposmovimientosinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tiposmovimientosinv()
        {
            this.tiposmovimientosseriesinv = new HashSet<tiposmovimientosseriesinv>();
        }
    
        public int IdInternoTiposMovimientos { get; set; }
        public string IdTipoMovimiento { get; set; }
        public string Descripcion { get; set; }
        public string AfectaCostoPromedio { get; set; }
        public string AfectaCostoRepocicion { get; set; }
        public string AfectaCostoUCompra { get; set; }
        public string Afectaestadisticaventa { get; set; }
        public string Afectaestadisticacompra { get; set; }
        public string EntradaSalida { get; set; }
        public string FacturacionInventario { get; set; }
        public string ClienteProveedor { get; set; }
        public string Poliza { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tiposmovimientosseriesinv> tiposmovimientosseriesinv { get; set; }
    }
}
