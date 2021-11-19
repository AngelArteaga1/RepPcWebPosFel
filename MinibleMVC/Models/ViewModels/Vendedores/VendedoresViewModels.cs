using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Vendedores
{
    public class VendedoresViewModels
    {
        [Required]
        [Display (Name ="Codigo Vendedor")]
        public string idVendedor { get; set; }

        [Required]
        [Display (Name ="Descripcion")]
        public string descripcion { get; set; }

        
        [Display (Name ="Porcentaje Comision")]
        public decimal porcentajeComision { get; set; }

        
        [Display (Name ="Telefono")]
        public string telefono { get; set; }

        [Display (Name ="Celular")]
        public string celular { get; set; }

        [Display (Name ="Correo Electronico")]       
        public string emailVende { get; set; }        

        [Display (Name ="Codigo Rutas")]
        public int idInternoRutas { get; set; }
    }

    public class EditVendedoresViewModels
    {
        [Required]
        public int idInternoVendedores {get; set;}

        [Required]
        [Display(Name = "Codigo Vendedor")]
        public string idVendedor { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Display(Name = "Porcentaje Comision")]
        public decimal porcentajeComision { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Celular")]
        public string celular { get; set; }

        [Display(Name = "Correo Electronico")]
        public string emailVende { get; set; }

        [Display(Name = "Codigo Rutas")]
        public int idInternoRutas { get; set; }

    }
}