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
    
    public partial class pedidostransitodetallehistoricoinv
    {
        public int id { get; set; }
        public string Numdoc { get; set; }
        public string IdArticulo { get; set; }
        public Nullable<decimal> CostoFOB { get; set; }
        public Nullable<decimal> CostoCIF { get; set; }
        public Nullable<decimal> GastoLocal { get; set; }
        public Nullable<decimal> UnidadesPedidas { get; set; }
        public Nullable<decimal> UnidadesRecibidas { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoPedidosTransitosHistoricos { get; set; }
    
        public virtual pedidostransitohistoricoinv pedidostransitohistoricoinv { get; set; }
    }
}
