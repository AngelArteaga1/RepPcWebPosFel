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
    
    public partial class traslados
    {
        public int IdInternotraslados { get; set; }
        public string Numdoc { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> IdBodega_In { get; set; }
        public Nullable<int> IdBodega_Out { get; set; }
        public string Descripcion { get; set; }
        public string Anulado { get; set; }
        public string Impreso { get; set; }
        public string Autorizado { get; set; }
        public string Entregado { get; set; }
        public string Codigo_Empresa { get; set; }
        public string IdMotorista { get; set; }
        public string porconsignacion { get; set; }
        public int IdInternoTIposMovimientosSeries { get; set; }
        public int IdInternoPedidosClientes { get; set; }
    
        public virtual pedidosclientesinv pedidosclientesinv { get; set; }
        public virtual tiposmovimientosseriesinv tiposmovimientosseriesinv { get; set; }
    }
}