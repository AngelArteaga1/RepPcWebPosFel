using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Rutas
{
    public class TableRutasViewModel
    {
        public int idInternoRutas { get; set; }

        public string idRuta { get; set; }

        public string descripcion { get; set; }

        public string recorrido { get; set; }

        public string status { get; set; }
    }
}