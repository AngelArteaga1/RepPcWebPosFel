using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Clases
{
    public class ClasesViewModels
    {
        [Required]
        [Display (Name ="Codigo Clase")]
        public string idClase { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }
    }

    public class EditClasesViewModels
    {
        [Required]
        public int idInternoClases { get; set; }

        [Required]
        [Display(Name = "Codigo Clase")]
        public string idClase { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
    }


}