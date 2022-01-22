using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.PtoDeVenta
{
    public class PtoDeVentaViewModel
    {
        public string numdoc { get; set; }

        [Required]
        [Display(Name = "NIT")]
        public string nit { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Required]
        [Display(Name = "Vendedor")]
        public int vendedor { get; set; }
        public string nombreVendedor { get; set; }


        [Required]
        [Display(Name = "Bodega")]
        public int bodega { get; set; }
        public string nombrebodega { get; set; }

        [Required]
        [Display(Name = "Serie")]
        public int serie { get; set; }
        public string nombreSerie { get; set; }

        public string tipoPrecio { get; set; }

        public Dictionary<int, Articulo> articulos { get; set; }

        public decimal? subtotal { get; set; }
        public decimal? descuento { get; set; }
        public decimal? total { get; set; }

        [GreaterThanTotal]
        public Pago pago { get; set; }

        public decimal? vuelto { get; set; }

        public PtoDeVentaViewModel()
        {
            this.articulos = new Dictionary<int, Articulo>();
            this.subtotal = 0;
            this.descuento = 0;
            this.total = 0;
            this.pago = new Pago();
        }
        public void updateTotales()
        {
            this.updateSubTotal();
            this.updateDescuento();
            this.updateTotal();
            this.calcularVuelto();
        }
        public void updateSubTotal()
        {
            decimal? subtotal = 0;
            foreach (var articulo in this.articulos)
            {
                subtotal = subtotal + (articulo.Value.precio * articulo.Value.unidades);
            }
            this.subtotal = subtotal;
        }
        public void updateDescuento()
        {
            decimal? descuentoTotal = 0;
            foreach (var articulo in this.articulos)
            {
                descuentoTotal = descuentoTotal + articulo.Value.descuentoPrecio;
            }
            this.descuento = descuentoTotal;
        }
        public void updateTotal()
        {
            decimal? total = 0;
            foreach (var articulo in this.articulos)
            {
                total = total + articulo.Value.total;
            }
            this.total = total;
        }
        
        public void calcularVuelto()
        {
            this.vuelto = this.pago.montoTotal - this.total;
        }
        
        public void addEfectivo(decimal? efectivo)
        {
            this.pago.efectivo = new Efectivo(efectivo);
            this.calcularPago();
        }
        public void addCheque(int id, int idBanco, string cheque, decimal? monto)
        {
            this.pago.cheques.Add(id, new Cheque(idBanco, cheque, monto));
            this.calcularPago();
        }
        public void updateCheque(int id, int idBanco, string cheque, decimal? monto)
        {
            this.pago.cheques[id].idBanco = idBanco;
            this.pago.cheques[id].cheque = cheque;
            this.pago.cheques[id].monto = monto;
            this.calcularPago();
        }
        public void addTajeta(int id, int idEmisor, string tarjeta, string autorizacion, decimal? monto)
        {
            this.pago.tarjetas.Add(id, new Tarjeta(idEmisor, tarjeta, autorizacion, monto));
            this.calcularPago();
        }
        public void updateTarjeta(int id, int idEmisor, string tarjeta, string autorizacion, decimal? monto)
        {
            this.pago.tarjetas[id].idEmisor = idEmisor;
            this.pago.tarjetas[id].tarjeta = tarjeta;
            this.pago.tarjetas[id].autorizacion = autorizacion;
            this.pago.tarjetas[id].monto = monto;
            this.calcularPago();
        }
        public void addDolares(decimal? tasa, decimal? dolares)
        {
            this.pago.dolares = new Dolares(tasa, dolares);
            this.calcularPago();
        } 

        public void calcularPago()
        {
            this.pago.calcularMontoTotal();
            this.calcularVuelto();
        }
    }

    public class Pago
    {
        public Efectivo efectivo { get; set; }
        public Dictionary<int, Cheque> cheques { get; set; }
        
        public Dictionary<int, Tarjeta> tarjetas { get; set; }
        public Dolares dolares { get; set; }
        public decimal? montoTotal { get; set; }

        public int chequesCont { get; set; }
        public int tarjetasCont { get; set; }

        public Pago()
        {
            this.efectivo = new Efectivo(null);
            this.cheques = new Dictionary<int, Cheque>();
            this.tarjetas = new Dictionary<int, Tarjeta>();
            this.dolares = new Dolares(null, null);
            this.montoTotal = 0;
            this.chequesCont = 0;
            this.tarjetasCont = 0;
        }
        public void calcularMontoTotal()
        {
            this.montoTotal = 0;
            if(this.efectivo.monto != null)
                this.montoTotal += this.efectivo.monto;
            this.montoTotal += this.getMontoCheques();
            this.montoTotal += this.getMontoTarjetas();
            if (this.dolares.monto != null)
                this.montoTotal += this.dolares.monto;
        }

        public decimal? getMontoCheques()
        {
            decimal? monto = 0;
            foreach(var cheque in this.cheques)
            {
                monto += cheque.Value.monto;
            }
            return monto;
        }
        public decimal? getMontoTarjetas()
        {
            decimal? monto = 0;
            foreach (var tarjeta in this.tarjetas)
            {
                monto += tarjeta.Value.monto;
            }
            return monto;
        }
    }

    public class Efectivo
    {
        [Display(Name = "Efectivo:")]
        public decimal? monto { get; set; }
        public Efectivo(decimal? monto)
        {
            this.monto = monto;
        }
    }

    public class Cheque
    {
        public int idBanco { get; set; }
        public string cheque { get; set; }
        public decimal? monto { get; set; }

        public Cheque(int idBanco, string cheque, decimal? monto)
        {
            this.idBanco = idBanco;
            this.cheque = cheque;
            this.monto = monto;
        }

    }

    public class Tarjeta
    {
        public int idEmisor { get; set; }
        public string tarjeta { get; set; }
        public string autorizacion { get; set; }
        public decimal? monto { get; set; }
        public Tarjeta(int idEmisor, string tarjeta, string autorizacion, decimal? monto)
        {
            this.idEmisor = idEmisor;
            this.tarjeta = tarjeta;
            this.autorizacion = autorizacion;
            this.monto = monto;
        }
    }

    public class Dolares
    {
        [Display(Name = "Tasa de Cambio:")]
        public decimal? tasaCambio { get; set; }
        [Display(Name = "Monto Dólares:")]
        public decimal? montoDolares { get; set; }
        [Display(Name = "Monto Quetzales:")]
        public decimal? monto { get; set; }
        public Dolares(decimal? tasaCambio, decimal? montoDolares)
        {
            this.tasaCambio = tasaCambio;
            this.montoDolares = montoDolares;
            this.monto = tasaCambio * montoDolares;
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

    public class GreaterThanTotalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pago = value as Pago;
            var model = validationContext.ObjectInstance as PtoDeVentaViewModel;
            if (pago.montoTotal < model.total)
            {
                return new ValidationResult("El monto total del pago debe ser mayor o igual al total de la factura");
            }
            return ValidationResult.Success;
        }
    }
}