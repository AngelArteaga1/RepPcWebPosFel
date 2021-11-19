using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels.TiposMovimientos
{
    public class EditarTiposMovimientosInv
    {
        
        public int idInternoTIposMovimientosSeries { get; set; }
        
        public int idInternoTiposMovimientos { get; set; }
               
        public string idSerie { get; set; }

        public string correlativo { get; set; }

        public string usaCorrelativo { get; set; }
       
        public string formatoImpresion { get; set; }
        
        public DateTime fechaAutorizacion { get; set; }
        
        public string res_del { get; set; }
        
        public string res_al { get; set; }
        
        public string resolucionNumero { get; set; }
        
        public int secuencia { get; set; }
    }
}