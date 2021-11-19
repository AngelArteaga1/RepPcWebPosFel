using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Clientes
{
    public class TableClientesViewModel
    {
        public int IdInternoClientes { get; set; }

        public string idCliente { get; set; }

        public string nombreComercial { get; set; }

        public string nit { get; set; }

        public string vendedor { get; set; } 

        public string status { get; set; }

        public int pp { get; set; }
    }
}