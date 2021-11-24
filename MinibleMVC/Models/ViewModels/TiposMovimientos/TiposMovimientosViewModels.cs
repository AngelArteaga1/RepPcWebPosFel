using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.TiposMovimientos
{
    public class TiposMovimientosViewModels
    {
        [Required]
        [Display(Name ="Codigo")]
        public string IdTipoMovimiento { get; set; }

        [Required]
        [Display (Name ="Tipo de Movimiento")]
        public string Descripcion { get; set; }

        
        [Display(Name ="Afecta costo promedio")]
        public bool afectaCostoPromedio { get; set; }

        
        [Display (Name ="Afecta Costo Reposicion")]
        public bool afectaCostoRepocicion { get; set; }

        
        [Display (Name ="Afecta Costo Ultima Compra")]
        public bool afectaCostoUCompra { get; set; }

        
        [Display (Name ="Afecta Estadistica de Ventas")]
        public bool afectaestadisticaventa { get; set; }

        
        [Display(Name ="Afecta estadistica de Compra")]
        public bool afectaestadisticacompra { get; set; }

        
        [Display (Name ="Entrada/Salida")]
        public string entradaSalida { get; set; }

        
        [Display(Name ="Factura/Inventario")]
        public string facturacionInventario { get; set; }
        
        
        [Display (Name ="Cliente/Proveedor")]
        public string clienteProveedor { get; set; } 

        [Required]
        [Display (Name ="Poliza")]
        public string poliza { get; set; }

         public List<TiposMovimientosSeriesViewModels> conceptos { get; set; }
    }

    public class TiposMovimientosSeriesViewModels
    {        

        [Required]
        [Display(Name ="Serie")]
        public string idSerie { get; set; }

        [Required]
        [Display (Name ="Correlativo")]
        public string correlativo { get; set; }

        [Required]
        [Display (Name ="Usa Correlativo")]
        public string usaCorrelativo { get; set; }

        [Display (Name ="Formato de Impresion")]
        public string formatoImpresion { get; set; }

        [Display (Name ="Fecha de Autorizacion")]
        public DateTime fechaAutorizacion { get; set; }

        [Display (Name ="Del")]
        public string res_del { get; set; }

        [Display (Name ="Al")]
        public string res_al { get; set; }

        [Display (Name ="Numero de Resolucion")]
        public string resolucionNumero { get; set; }

        [Display (Name = "Secuencia")]
        public int secuencia { get; set; }

    }


    public class EditTiposMovimientosViewModels
    {
        [Required]
        public int idInternoTiposMovimientos { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        public string IdTipoMovimiento { get; set; }

        [Required]
        [Display(Name = "Tipo de Movimiento")]
        public string Descripcion { get; set; }


        [Display(Name = "Afecta costo promedio")]
        public bool afectaCostoPromedio { get; set; }


        [Display(Name = "Afecta Costo Reposicion")]
        public bool afectaCostoRepocicion { get; set; }


        [Display(Name = "Afecta Costo Ultima Compra")]
        public bool afectaCostoUCompra { get; set; }


        [Display(Name = "Afecta Estadistica de Ventas")]
        public bool afectaestadisticaventa { get; set; }


        [Display(Name = "Afecta estadistica de Compra")]
        public bool afectaestadisticacompra { get; set; }


        [Display(Name = "Entrada/Salida")]
        public string entradaSalida { get; set; }


        [Display(Name = "Factura/Inventario")]
        public string facturacionInventario { get; set; }


        [Display(Name = "Cliente/Proveedor")]
        public string clienteProveedor { get; set; }

        [Required]
        [Display(Name = "Poliza")]
        public string poliza { get; set; }

        public int contador { get; set; }

        public List<EditTiposMovimientosSeriesViewModels> conceptos { get; set; }
        public List<TiposMovimientosSeriesViewModels> conceptosAdd { get; set; }

        public List<DeleteTiposMovimientosSeriesViewModels> conceptosDelete { get; set; }

    }

    public class EditTiposMovimientosSeriesViewModels
    {
        [Required]
        public int idInternoTIposMovimientosSeries { get; set; }

        [Required]
        public int idInternoTiposMovimientos { get; set; }

        [Required]
        [Display(Name = "Serie")]
        public string idSerie { get; set; }

        [Required]
        [Display(Name = "Correlativo")]
        public string correlativo { get; set; }
        
        [Display(Name = "Usa Correlativo")]
        public string usaCorrelativo { get; set; }

        [Display(Name = "Formato de Impresion")]
        public string formatoImpresion { get; set; }

        [Display(Name = "Fecha de Autorizacion")]
        public DateTime fechaAutorizacion { get; set; }

        [Display(Name = "Del")]
        public string res_del { get; set; }

        [Display(Name = "Al")]
        public string res_al { get; set; }

        [Display(Name = "Numero de Resolucion")]
        public string resolucionNumero { get; set; }

        [Display(Name = "Secuencia")]
        public int secuencia { get; set; }   
        
        

    }


    public class DeleteTiposMovimientosSeriesViewModels
    {
        [Required]
        public int idInternoTIposMovimientosSeries { get; set; }    
      
    }

}