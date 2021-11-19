using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Cobradores
{
    public class CobradoresViewModels
    {
        [Required]
        [Display(Name = "Codigo Cobrador")]
        public string idCobrador { get; set; }

        [Required]
        [Display(Name = "Descripcions")]
        public string descripcion { get; set; }

        [Display(Name = "Porcentaje comision")]
        public decimal porcentajeComision { get; set; }

        [Required]
        [Display(Name = "Codigo Ruta")]
        public int IdInternoRutas { get; set; }
    }

    public class EditCobradoresViewModels
    {
        [Required]
        public int idInternoCobrador { get; set; }

        [Required]
        [Display(Name = "Codigo Cobrador")]
        public string idCobrador { get; set; }

        [Required]
        [Display(Name = "Descripcions")]
        public string descripcion { get; set; }

        [Display(Name = "Porcentaje comision")]
        public decimal porcentajeComision { get; set; }

        [Required]
        [Display(Name = "Codigo Ruta")]
        public int IdInternoRutas { get; set; }
    }
}