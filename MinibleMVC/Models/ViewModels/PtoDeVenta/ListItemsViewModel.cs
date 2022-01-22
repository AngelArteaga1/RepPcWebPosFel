using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class ListItemsViewModel
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string compDescripcion { get; set; }

        public string foto { get; set; }

        public string unidadMedida { get; set; }

        public double? existencia { get; set; }

        public int unidades { get; set; }

        //Precio oficial
        public decimal? precio { get; set; }
        //Precios variantes al cliente
        public decimal? precio1 { get; set; }
        public decimal? precio2 { get; set; }
        public decimal? precio3 { get; set; }

        //Descuento oficial
        public decimal? descuento { get; set; }
        //Porcentajes variantes al cliente
        public decimal? descuento1 { get; set; }
        public decimal? descuento2 { get; set; }
        public decimal? descuento3 { get; set; }

        public decimal? descuentoPrecio { get; set; }

        public int idFamilia { get; set; }
        public string familia { get; set; }
        public int idMarca { get; set; }
        public string marca { get; set; }

        public decimal? subtotal { get; set; }
    }
}