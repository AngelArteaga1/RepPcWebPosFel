using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Clases
{
    public class TableClasesViewModel
    {
        
        public int idInternoClases { get; set; }
        
        public string idClase { get; set; }
        
        public string descripcion { get; set; }

        public string status { get; set; }

    }
}