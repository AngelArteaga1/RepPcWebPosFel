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
    
    public partial class articulosinv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public articulosinv()
        {
            this.articulosdetalleinv = new HashSet<articulosdetalleinv>();
            this.combosdetalleinv = new HashSet<combosdetalleinv>();
            this.esperadetalle = new HashSet<esperadetalle>();
            this.pedidostransitodetalleinv = new HashSet<pedidostransitodetalleinv>();
            this.ofertasdetalle = new HashSet<ofertasdetalle>();
            this.pedidosclientesinvdetalle = new HashSet<pedidosclientesinvdetalle>();
            this.movimientosdetallecombosinv = new HashSet<movimientosdetallecombosinv>();
            this.movimientosdetallehistoricosinv = new HashSet<movimientosdetallehistoricosinv>();
            this.movimientosdetallecomboshistoricosinv = new HashSet<movimientosdetallecomboshistoricosinv>();
        }
    
        public int IdInternoArticulos { get; set; }
        public string IdArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public string Modelo { get; set; }
        public Nullable<decimal> VentaMinima_1 { get; set; }
        public Nullable<decimal> VentaMinima_2 { get; set; }
        public Nullable<decimal> VentaMinima_3 { get; set; }
        public Nullable<decimal> PrecioVenta_1 { get; set; }
        public Nullable<decimal> PrecioVenta_2 { get; set; }
        public Nullable<decimal> PrecioVenta_3 { get; set; }
        public Nullable<decimal> PrecioCompraActual_Q { get; set; }
        public Nullable<decimal> PrecioCompraAnterior_Q { get; set; }
        public Nullable<decimal> PrecioCompraAnterior_D { get; set; }
        public Nullable<decimal> CostoPromedioActual_Q { get; set; }
        public Nullable<double> CostoPromedioAnterior_Q { get; set; }
        public Nullable<decimal> CostoReposicionActual_Q { get; set; }
        public Nullable<decimal> CostoReposicionAnterior_Q { get; set; }
        public Nullable<double> ExistenciaInicial { get; set; }
        public Nullable<double> EntradasActuales { get; set; }
        public string idbodega { get; set; }
        public Nullable<double> SalidasActuales { get; set; }
        public Nullable<double> ExistenciaActual { get; set; }
        public Nullable<double> UnidadesPedidas { get; set; }
        public Nullable<System.DateTime> FechaPedido { get; set; }
        public string proveedorlocal { get; set; }
        public string ProveedorExtrangero { get; set; }
        public Nullable<double> UnidadesPedidasClientes { get; set; }
        public string No_Meses_Garantia { get; set; }
        public Nullable<decimal> PrecioVenta_4 { get; set; }
        public Nullable<decimal> VentaMinima_4 { get; set; }
        public string CodigoBarras { get; set; }
        public Nullable<decimal> PrecioVenta_Q { get; set; }
        public Nullable<decimal> PrecioFOB { get; set; }
        public Nullable<decimal> DescuentoMax { get; set; }
        public string ProductoServicio { get; set; }
        public Nullable<decimal> ValorRef1 { get; set; }
        public Nullable<decimal> ValorRef2 { get; set; }
        public Nullable<decimal> ValorRef3 { get; set; }
        public Nullable<decimal> ValorRef4 { get; set; }
        public Nullable<decimal> ValorRef5 { get; set; }
        public Nullable<decimal> CostoFOB { get; set; }
        public Nullable<decimal> CostoProximoAnterior { get; set; }
        public Nullable<decimal> CostoFOBProximoAnteriord { get; set; }
        public Nullable<decimal> CostoDDPActual { get; set; }
        public Nullable<decimal> CostoDDPAnterior { get; set; }
        public string ProductoRelacionado { get; set; }
        public string OrdenEspecial { get; set; }
        public string usalote { get; set; }
        public string foto { get; set; }
        public string idcolor { get; set; }
        public string compDescripcion { get; set; }
        public Nullable<int> correlativoarticulo { get; set; }
        public Nullable<decimal> descuentocombo { get; set; }
        public Nullable<decimal> porcentajeDescuentoCombo { get; set; }
        public Nullable<decimal> porcenutilidad { get; set; }
        public Nullable<decimal> existenciaminima { get; set; }
        public Nullable<decimal> comisioncontado { get; set; }
        public Nullable<decimal> comisioncredito { get; set; }
        public Nullable<decimal> precioventa_5 { get; set; }
        public Nullable<decimal> precioventa_6 { get; set; }
        public Nullable<decimal> precioventa_7 { get; set; }
        public string usaseries { get; set; }
        public string tipo { get; set; }
        public string segunda { get; set; }
        public string statusfechavenc { get; set; }
        public string statusactualiza { get; set; }
        public string statusactualiza2 { get; set; }
        public string statusmenudeo { get; set; }
        public Nullable<decimal> porcentajeganancia { get; set; }
        public Nullable<decimal> porcentajeganancia1 { get; set; }
        public Nullable<decimal> porcentajeganancia2 { get; set; }
        public Nullable<decimal> porcentajeganancia22 { get; set; }
        public Nullable<decimal> porcentajeganancia3 { get; set; }
        public Nullable<decimal> porcentajeganancia4 { get; set; }
        public Nullable<decimal> preciomenudeo { get; set; }
        public Nullable<decimal> precioventa1 { get; set; }
        public Nullable<decimal> precioventa2 { get; set; }
        public Nullable<decimal> precioventa3 { get; set; }
        public Nullable<decimal> precioventa4 { get; set; }
        public string estatusactualiza1 { get; set; }
        public string estatusactualiza2 { get; set; }
        public string GrabadaExenta { get; set; }
        public string codigoarticulo { get; set; }
        public Nullable<decimal> descuento { get; set; }
        public string idpresentacion { get; set; }
        public string idtalla { get; set; }
        public string StatusActualiza1 { get; set; }
        public Nullable<decimal> precioventa_1_1 { get; set; }
        public Nullable<decimal> precioventa_2_2 { get; set; }
        public Nullable<decimal> PorcentajeGanancia2_2 { get; set; }
        public string genmar { get; set; }
        public Nullable<decimal> PorcentajeGanancia3_3 { get; set; }
        public Nullable<decimal> PorcentajeDescuento1_1 { get; set; }
        public Nullable<decimal> PorcentajeDescuento2_2 { get; set; }
        public Nullable<decimal> PorcentajeDescuento3_3 { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoFamilias { get; set; }
        public int IdInternoProveedores { get; set; }
        public int IdInternoMedidas { get; set; }
        public int IdInternoMarcas { get; set; }
        public int IdinternoArticuloDetalleGenerico { get; set; }
    
        public virtual articulosdetallegenericos articulosdetallegenericos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<articulosdetalleinv> articulosdetalleinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<combosdetalleinv> combosdetalleinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<esperadetalle> esperadetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidostransitodetalleinv> pedidostransitodetalleinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ofertasdetalle> ofertasdetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidosclientesinvdetalle> pedidosclientesinvdetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallecombosinv> movimientosdetallecombosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallehistoricosinv> movimientosdetallehistoricosinv { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movimientosdetallecomboshistoricosinv> movimientosdetallecomboshistoricosinv { get; set; }
        public virtual familiasinv familiasinv { get; set; }
        public virtual marcasinv marcasinv { get; set; }
        public virtual medidasinv medidasinv { get; set; }
        public virtual proveedores proveedores { get; set; }
    }
}