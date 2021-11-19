using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Marcas
{
    public class TableMarcasViewModel
    {
        public int idInternoMarcas { get; set; }

        public string idMarca { get; set; }

        public string descripcion { get; set; }

        public string status { get; set; }
    }
}