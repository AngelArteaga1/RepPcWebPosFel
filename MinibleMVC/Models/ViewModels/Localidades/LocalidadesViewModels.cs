using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Localidades
{
    public class LocalidadesViewModels
    {
        [Required]
        [Display (Name ="Codigo Localidad")]
        public string idLocalidad { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }

    }

    public class EditLocalidadesViewModels
    {
        [Required]
        public int idInternoLocalidades { get; set; }

        [Required]
        [Display(Name = "Codigo Localidad")]
        public string idLocalidad { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

    }
}