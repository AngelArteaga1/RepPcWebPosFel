using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Cajas
{
    public class CajasViewModel
    {
        public int id { get; set; }

        [Display(Name = "Fecha Inicio")]
        public string fechaInicio { get; set; }
        [Display(Name = "Fecha Final")]
        public string fechaFinal { get; set; }
        [Display(Name = "Valor Inicial")]
        public decimal valorInicial { get; set; }
        public CajasViewModel(string fechaInicio)
        {
            this.fechaInicio = fechaInicio;
            this.valorInicial = default(decimal);
        }
    }
}