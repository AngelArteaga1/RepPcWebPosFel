using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.TiposMovimientos
{
    public class TableTipodMovimientosViewModel
    {
        public int idInternoTiposMovimientos { get; set; }

        public string idTipoMovimiento { get; set; }

        public string descripcion { get; set; }

        public string entradaSalida { get; set; }

        public string status { get; set; }

    }
}