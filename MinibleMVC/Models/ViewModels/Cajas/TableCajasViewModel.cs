using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Cajas
{
    public class TableCajasViewModel
    {
        public int idInternoCaja { get; set; }
        public string noCaja { get; set; }
        public string usuario { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public string valorInicial { get; set; }
        public string operacion { get; set; }
        public string total { get; set; }
        public string cerrado { get; set; }
    }
}