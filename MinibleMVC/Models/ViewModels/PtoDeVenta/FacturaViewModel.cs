using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class FacturaViewModel
    {
        public string numdocFEL { get; set; }
        public string serieFEL { get; set; }
        public string numeroAutorizacion { get; set; }
        public string refInterna { get; set; }

        public string fecha { get; set; }
        public string numdocOrden { get; set; }


        public string nitEmpresa { get; set; }
        public string direccionEmpresa { get; set; }
        public string correoEmpresa { get; set; }
        public string telefonoEmpresa { get; set; }

        public string nombreCliente { get; set; }
        public string nitCliente { get; set; }
        public string correoCliente { get; set; }
        public string direccionCliente { get; set; }
        public string telefonoCliente { get; set; }

        public List<DetalleFactura> articulos { get; set; }

        public decimal? subtotal { get; set; }
        public decimal? descuento { get; set; }
        public decimal? total { get; set; }
        public decimal? vuelto { get; set; }

        public FacturaViewModel()
        {
            this.articulos = new List<DetalleFactura>();
        }
    }

    public class DetalleFactura
    {
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public decimal? precio { get; set; }
        public double? unidades { get; set; }
        public double? total { get; set; }

        public DetalleFactura(string descripcion, string codigo, decimal? precio, double? unidades)
        {
            this.descripcion = descripcion;
            this.codigo = codigo;
            this.precio = precio;
            this.unidades = unidades;
            this.total = (double)precio * unidades;
        }
    }
}