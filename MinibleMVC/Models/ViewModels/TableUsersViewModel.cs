using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Minible5.Models.ViewModels
{
    public class TableUsersViewModel
    {

        public int id { get; set; }

        public string username { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string grupo { get; set; }

        public int? activo { get; set; }

    }
}