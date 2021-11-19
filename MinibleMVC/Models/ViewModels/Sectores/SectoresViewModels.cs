using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Sectores
{
    public class SectoresViewModels
    {
        
        [Required]
        [Display (Name ="Codigo Sector")]
        public string IdSector { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }

        [Required]
        [Display (Name ="Codigo Ruta")]
        public int IdInternoRutas { get; set; }

    }

    public class EditSectoresViewModels
    {
        [Required]
        public int idInternoSectores { get; set; }

        [Required]
        [Display(Name = "Codigo Sector")]
        public string IdSector { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Codigo Ruta")]
        public int IdInternoRutas { get; set; }

    }

}