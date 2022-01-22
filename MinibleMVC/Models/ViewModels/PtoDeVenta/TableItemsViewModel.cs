using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class TableItemsViewModel
    {
        public int id { get; set; }
        public string foto { get; set; }
        public string descripcion { get; set; }
        public decimal? precio { get; set; }

        public string codigo { get; set; }

        public double? existencia { get; set; }

        public string unidadMedida { get; set; }
    }
}