using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class GridPagesViewModel
    {
        public int numero { get; set; }
        public bool activo { get; set; }
        public GridPagesViewModel(int numero, bool activo)
        {
            this.numero = numero;
            this.activo = activo;
        }
    }
}