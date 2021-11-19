using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Localidades
{
    public class TableLocalidadesViewModel
    {
       
        public int idInternoLocalidades { get; set; }

        
        public string idLocalidad { get; set; }

       
        public string descripcion { get; set; }

        public string status { get; set; }

    }
}