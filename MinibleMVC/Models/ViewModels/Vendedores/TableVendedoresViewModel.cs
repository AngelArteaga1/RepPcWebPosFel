using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Vendedores
{
    public class TableVendedoresViewModel
    {
        public int idInternoVendedores { get; set; }

        public string idVendedor { get; set; }

        public string descripcion { get; set; }

        public string ruta { get; set; }

        public decimal porcentajeComision { get; set; }

        public string emailVende { get; set; }

        public string status { get; set; }
    }

}