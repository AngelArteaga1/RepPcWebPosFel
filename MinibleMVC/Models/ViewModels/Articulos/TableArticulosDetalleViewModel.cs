using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.Articulos
{
    public class TableArticulosDetalleViewModel
    {
        public int idInternoArticulos { get; set; }

        public string idBodega { get; set; }

        public string descripcionBodega { get; set; }

        public string ubicacion { get; set; }

        public string ultimaFechaVenta { get; set; }

        public string ultimaFechaCompra { get; set; }

        public string fechaAlta { get; set; }

        public string unidadesIniciales { get; set; }

        public string unidadesEntrantes { get; set; }

        public string unidadesSalientes { get; set; }

        public string costoInicial_Q { get; set; }

        public string costoEntradas_Q { get; set; }

        public string costoSalidas_Q { get; set; }

        public string docfechaultcomp1 { get; set; }

        public string docfechaultcomp2 { get; set; }

        public string docfechaultcomp3 { get; set; }

        public string valorultcomp1 { get; set; }

        public string valorultcomp2 { get; set; }

        public string valorultcomp3 { get; set; }

        public string maximo { get; set; }

        public string minimo { get; set; }               

        
    }
}