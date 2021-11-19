using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels
{
    public class TableMedidasViewModel
    {
        public int idInternoMedidas { get; set; }

        public string idMedidas { get; set; }
        
        public string descripcion { get; set; }

        public string status { get; set; }


    }
}