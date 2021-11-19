using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels
{
    public class TableProveedoresViewModel
    {
        public int idInternoProveedores { get; set; }
        public string idProveedor { get; set; }

        public string nombreComercial { get; set; }

        public string nit { get; set; }

        public string status { get; set; }
    }
}