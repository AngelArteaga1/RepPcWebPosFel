//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Minible5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class movimientosdetallecomboshistoricosinv
    {
        public int idInternoMovimientosDetalleCombos { get; set; }
        public string Codigo_Empresa { get; set; }
        public string Numdoc { get; set; }
        public string IdBodega { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<double> Unidades { get; set; }
        public Nullable<decimal> Precio_unitario { get; set; }
        public Nullable<decimal> Costo_Unitario { get; set; }
        public Nullable<decimal> PorcentajeDescuento { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> PorcentajeAranceles { get; set; }
        public Nullable<decimal> MontoAranceles { get; set; }
        public Nullable<decimal> MontoCargosDolares { get; set; }
        public Nullable<decimal> MontoCargosLocales { get; set; }
        public Nullable<decimal> MontoAjusteDolares { get; set; }
        public Nullable<decimal> MontoAjusteLocales { get; set; }
        public Nullable<decimal> Impuesto { get; set; }
        public Nullable<double> TasaCambio { get; set; }
        public Nullable<int> Secuencia { get; set; }
        public string Oferta { get; set; }
        public string Anulado { get; set; }
        public Nullable<decimal> Precio_Unitario_Q { get; set; }
        public Nullable<decimal> Descuento_Q { get; set; }
        public Nullable<decimal> Impuesto_Q { get; set; }
        public string Observaciones { get; set; }
        public string NombreArticulo { get; set; }
        public string FactCompra { get; set; }
        public string IdProveedor { get; set; }
        public Nullable<System.DateTime> FechaFactCompra { get; set; }
        public string NoLote { get; set; }
        public Nullable<System.DateTime> FechaVenc { get; set; }
        public Nullable<double> Peso { get; set; }
        public Nullable<decimal> Costo_peso { get; set; }
        public Nullable<decimal> Preciou_peso { get; set; }
        public string PesoUnidades { get; set; }
        public int IdInternoBodegas { get; set; }
        public int IdInternoTIposMovimientosSeries { get; set; }
        public int IdInternoArticulos { get; set; }
    
        public virtual articulosinv articulosinv { get; set; }
        public virtual bodegasinv bodegasinv { get; set; }
        public virtual tiposmovimientosseriesinv tiposmovimientosseriesinv { get; set; }
    }
}
