using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Articulos
{
    public class EditArticulosdetalle
    {

        public string ubicacion { get; set; }

        public double maximo { get; set; }

        public double minimo { get; set; }

        public int idInternoArticulos { get; set; }

        public int idInternoBodegas { get; set; }

        public int idInternoArticulosDetalle { get; set; }

    }
}