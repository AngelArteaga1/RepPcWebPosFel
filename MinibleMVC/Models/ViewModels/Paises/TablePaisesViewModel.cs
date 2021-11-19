using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Paises
{
    public class TablePaisesViewModel
    {       
        public int idInternoPaises { get; set; }
        
        public string idPais { get; set; }
        
        public string descripcion { get; set; }

        public string status { get; set; }
    }
}