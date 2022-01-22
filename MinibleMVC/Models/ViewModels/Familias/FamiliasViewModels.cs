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

        [Display(Name = "Cuenta contable ventas")]
        public string ctaVentas { get; set; }

        [Display(Name = "Cuenta contable costos")]
        public string ctaCostos { get; set; }

        [Display(Name = "Cuenta contable inventario")]
        public string ctaInventario { get; set; }

        [Display(Name = "Cuenta contable impuesto")]
        public string ctaImpuesto { get; set; }

        [Display(Name = "Cuenta contable rebajas")]
        public string ctaRebaja { get; set; }

        [Display(Name = "Cuenta contable producto procesados")]
        public string ctaProdProceso { get; set; }


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

        [Display(Name = "Cuenta Contable Costo Excento")]
        public string ctaCostoExcento { get; set; }

        [Display(Name = "Cuenta Contable Producto Proceso")]
        public string ctaProdProceso { get; set; }

        
    }
}