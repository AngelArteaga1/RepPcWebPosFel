using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Zonas
{
    public class ZonasViewModels
    {
        [Required]
        [Display (Name ="Codigo Zonas")]
        public string idZona { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }
        
    }

    public class EditZonasViewModels
    {
        [Required]
        public int idInternoZonas { get; set; }

        [Required]
        [Display(Name = "Codigo Zonas")]
        public string idZona { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

    }
}