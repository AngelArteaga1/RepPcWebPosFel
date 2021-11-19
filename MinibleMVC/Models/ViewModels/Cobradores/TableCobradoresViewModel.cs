using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Cobradores
{
    public class TableCobradoresViewModel
    {
       public int idInternoCobrador { get; set; }

        public string idCobrador { get; set; }
               
        public string descripcion { get; set; }
      
        public decimal porcentajeComision { get; set; }
       
        public string rutas { get; set; }

        public string status { get; set; }

    }

    
}