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
    
    public partial class pedidostransitodetalleinv
    {
        public int IdInternoPedidosTransitosDetalle { get; set; }
        public string Numdoc { get; set; }
        public Nullable<decimal> CostoFOB { get; set; }
        public Nullable<decimal> CostoCIF { get; set; }
        public Nullable<decimal> GastoLocal { get; set; }
        public Nullable<decimal> UnidadesPedidas { get; set; }
        public Nullable<decimal> UnidadesRecibidas { get; set; }
        public string Codigo_Empresa { get; set; }
        public string idbodega { get; set; }
        public int IdInternoPedidosTransitos { get; set; }
        public int IdInternoArticulos { get; set; }
    
        public virtual articulosinv articulosinv { get; set; }
        public virtual pedidostransitoinv pedidostransitoinv { get; set; }
    }
}