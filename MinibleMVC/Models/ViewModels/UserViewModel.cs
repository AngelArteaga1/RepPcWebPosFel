using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Minible5.Models.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Nombre Completo")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        [UserExist]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Las contraseñas no son iguales")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Grupo")]
        [Required]
        public string Group { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public List<string> Empresas { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no son iguales")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Grupo")]
        [Required]
        public string Group { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public List<EmpresaDetalle> Empresas { get; set; }
    }

    public class EmpresaDetalle
    {
        public string CodigoEmpresa { get; set; }
        //Eliminado o Agregado, depende de eso asi vamos a hacer el update
        public string Update { get; set; }
    }

    //CUSTOM VALIDATIONS
    public class UserExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (var db = new db_pcsolutions_webEntities())
            {
                string username = (string)value;
                if(db.security_users.Where(d => d.username == username).Count() > 0)
                {
                    return new ValidationResult("El nombre de usuario ya existe");
                }
            }
            return ValidationResult.Success;
        }
    }

}