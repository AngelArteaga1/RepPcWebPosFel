using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Minible5.Models.ViewModels
{
    public class GroupViewModel
    {
        [Required]
        [Display(Name = "Nombre del grupo")]
        public string Name { get; set; }
    }

    public class EditGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre del grupo")]
        public string Name { get; set; }
    }
}