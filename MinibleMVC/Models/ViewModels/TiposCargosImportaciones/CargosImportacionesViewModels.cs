using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Minible5.Models.ViewModels.TiposCargosImportaciones
{
    public class CargosImportacionesViewModels
    {        
        [Required]
        [Display (Name ="Codigo Tipo Cargo Importaciones")]
        public string idtipocargo { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }
        
        [Display (Name ="Moneda")]
        public string localDolares { get; set; }

        
    }

    public class EditCargosImportacionesViewModels
    {
        [Required]
        public int idInternoTipCargImportaciones { get; set; }

        [Required]
        [Display(Name = "Codigo Tipo Cargo Importaciones")]
        public string idtipocargo { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Display(Name = "Moneda")]
        public string localDolares { get; set; }

        
    }
}