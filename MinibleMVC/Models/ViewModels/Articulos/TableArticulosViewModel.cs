using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Articulos
{
    public class TableArticulosViewModel
    {
        public int idInternoArticulos { get; set; }

        public string idArticulo { get; set; }

        public string descripcion { get; set; }

        public string marca { get; set; }

        public string familia { get; set; }

        public string medida { get; set; }

        public string status { get; set; }
    }
}