using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class PtoDeVentaViewModel
    {
        [Display(Name = "NIT")]
        public string nit { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        public string tipoPrecio { get; set; }

        [Display(Name = "Vendedor")]
        public int vendedor { get; set; }
        public string nombreVendedor { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Display(Name = "Bodega")]
        public int bodega { get; set; }
        public string nombrebodega { get; set; }

        [Display(Name = "Serie")]
        public int serie { get; set; }
        public string nombreSerie { get; set; }

        public Dictionary<int, Articulo> articulos { get; set; }

        public decimal? subtotal { get; set; }
        public decimal? descuento { get; set; }
        public decimal? total { get; set; }

        public PtoDeVentaViewModel()
        {
            this.articulos = new Dictionary<int, Articulo>();
            this.subtotal = 0;
            this.descuento = 0;
            this.total = 0;
        }
    }

    public class Articulo
    {
        public int id { get; set; }
        public decimal? precio { get; set; }
        public int unidades { get; set; }
        public decimal? descuento { get; set; }
        public decimal? descuentoPrecio { get; set; }
        public decimal? total { get; set; }
        public Articulo(int id, decimal? precio, int unidades, decimal? descuento)
        {
            this.id = id;
            this.unidades = unidades;
            this.descuento = descuento;
            this.precio = precio;
            this.descuentoPrecio = 0;
            this.total = 0;
            calcular();
        }

        public void agregarUnidad ()
        {
            this.unidades++;
            this.calcular();
        }

        public void calcularDescuento()
        {
            this.descuentoPrecio = ((this.precio * this.descuento) / 100) * this.unidades;
        }

        public void calcularTotal()
        {
            this.total = (this.precio * this.unidades) - this.descuentoPrecio;
        }

        public void calcular()
        {
            this.calcularDescuento();
            this.calcularTotal();
        }
    }
}