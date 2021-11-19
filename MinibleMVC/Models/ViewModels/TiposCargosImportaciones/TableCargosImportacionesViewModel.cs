using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.TiposCargosImportaciones
{
    public class TableCargosImportacionesViewModel
    {
        public int idInternoTipCargImportaciones { get; set; }

        public string idtipocargo { get; set; }

        public string descripcion { get; set; }

        public string localDolares { get; set; }

        public string status { get; set; }

    }
}