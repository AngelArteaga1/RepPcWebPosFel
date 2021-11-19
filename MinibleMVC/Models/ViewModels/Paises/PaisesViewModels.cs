using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Paises
{
    public class PaisesViewModels
    {
        [Required]
        [Display (Name ="Codigo Pais")]
        public string idPais { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }
    }

    public class EditPaisesViewModels
    {
        [Required]
        public int idInternoPaises {get; set;  }

        [Required]
        [Display(Name = "Codigo Pais")]
        public string idPais { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
    }

}