using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels
{
    public class MedidasViewModels
    {
        [Required]
        [Display (Name ="Codigo Medidas")]
        public string idMedidas { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }

        public string felMedida { get; set; }


    }

    public class EditMedidasViewModel
    {
        public int idInternoMedidas { get; set; }

        [Required]
        [Display(Name = "Codigo Medidas")]
        public string idMedidas { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        public string felMedida { get; set; }
    }
}