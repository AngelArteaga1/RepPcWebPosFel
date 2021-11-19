using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels
{
    public class FamiliasViewModels
    {
        [Required]
        [Display(Name = "Codigo Familia")]
        public string idFamilia { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        
        public string ctaVentas { get; set; }

        
        public string ctaCostos { get; set; }

        
        public string ctaInventario { get; set; }

        
        public string ctaImpuesto { get; set; }

        
        public string ctaRebaja { get; set; }

        
        public decimal desviacionBlanco { get; set; }

        
        public decimal desviacionAmarillo { get; set; }

        
        public decimal desviacionRojo { get; set; }

        
        public string ctaCostoExcento { get; set; }

        
        public string ctaProdProceso { get; set; }

        
        public decimal porcentajeComision { get; set; }

   
        public string ctaVentaExcento { get; set; }

    }


    public class EditFamiliasViewModels
    {
        public int IdInternoFamilias { get; set; }

        [Required]
        [Display(Name = "Codigo Familia")]
        public string IdFamilia { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Display(Name = "Cuenta Contable Ventas")]
        public string ctaVentas { get; set; }

        [Display(Name = "Cuenta Contable Costos")]
        public string ctaCostos { get; set; }

        [Display(Name = "Cuenta Contable Inventario")]
        public string ctaInventario { get; set; }

        [Display(Name = "Cuenta Contable Impuesto")]
        public string ctaImpuesto { get; set; }

        [Display(Name = "Cuenta Contable Rebaja")]
        public string ctaRebaja { get; set; }

        [Display(Name = "Desviacion Blanco")]
        public decimal desviacionBlanco { get; set; }

        [Display(Name = "Desviacion Amarillo")]
        public decimal desviacionAmarillo { get; set; }

        [Display(Name = "Desviacion Rojo")]
        public decimal desviacionRojo { get; set; }

        [Display(Name = "Cuenta Contable Costo Excento")]
        public string ctaCostoExcento { get; set; }

        [Display(Name = "Cuenta Contable Producto Proceso")]
        public string ctaProdProceso { get; set; }

        [Display(Name = "Porcentaje Comision")]
        public decimal porcentajeComision { get; set; }

        [Display(Name = "Cuenta Contable Ventas Excento")]
        public string ctaVentaExcento { get; set; }
    }
}