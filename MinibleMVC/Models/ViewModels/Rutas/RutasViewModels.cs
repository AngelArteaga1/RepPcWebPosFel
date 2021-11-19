using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Rutas
{
    public class RutasViewModels
    {
        [Required]
        [Display (Name ="Codigo Ruta")]
        public string idRuta { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }

        [Display (Name ="Recorrido")]
        public string recorrido { get; set; }
        
    }

    public class EditRutasViewModels
    {
        [Required]
        public int idInternoRutas { get; set; }

        [Required]
        [Display(Name = "Codigo Ruta")]
        public string idRuta { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Display(Name = "Recorrido")]
        public string recorrido { get; set; }

    }
}