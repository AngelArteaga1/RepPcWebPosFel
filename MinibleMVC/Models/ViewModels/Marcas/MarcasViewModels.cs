using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Marcas
{
    public class MarcasViewModels
    {
        [Required]
        [Display(Name ="Codigo Marcas")]
        public string idMarca { get; set; }

        [Required]
        [Display(Name = "Codigo Descripcion")]
        public string descripcion { get; set; }



    }

    public class EditMarcasViewModel
    {
        public int idInternoMarcas { get; set; }

        [Required]
        [Display(Name = "Codigo Marcas")]
        public string idMarca { get; set; }

        [Required]
        [Display(Name = "Codigo Descripcion")]
        public string descripcion { get; set; }

    }
  }