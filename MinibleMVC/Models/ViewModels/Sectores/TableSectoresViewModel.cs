using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Sectores
{
    public class TableSectoresViewModel
    {
        public int idInternoSectores { get; set; }

        public string IdSector { get; set; }

        public string descripcion { get; set; }

        public string rutas { get; set; }

        public string status { get; set; }
    }
}