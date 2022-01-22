using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Minible5.Models.ViewModels.Articulos
{
    public class ArticulosViewModels
    {
        [Required]
        //[ValidarDuplicidad]
        [StringLength(15)]
        [Display(Name = "Codigo del articulos")]
        public string idArticulo { get; set; }

        [StringLength(25)]
        [Display(Name = "Codigo de barras")]
        public string codigoBarras { get; set; }       

        [Required]
        //[ValidarDuplicidad]
        [StringLength(70)]
        [Display(Name = "Nombre Articulo")]
        public string nombreArticulo { get; set; }

        //[Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento1_1 { get; set; }

       // [Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento2_2 { get; set; }

        //[Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento3_3 { get; set; }

        //[Range(1,19, ErrorMessage ="El valor ingresado pasa el rango aceptado")]
        [Display(Name = "Precio")]
        public decimal precioVenta_1_1 { get; set; }

       // [Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "Precio Dos")]
        public decimal precioVenta_2_2 { get; set; }

        //Incluir en vista y controlador Para Version en farmacia 
        [Display(Name = "Tiene fecha vencimiento")]
        public bool statusFechaVenc { get; set; }

        [Display(Name = "Precio Menudeo")]
        public bool statusMenudeo { get; set; }

        [Required]        
        [Display(Name = "Tipo de Articulo")]
        public string tipo { get; set; }

        [Required]
        [Display(Name = "Codigo Generico")]
        public int idinternoArticuloDetalleGenerico { get; set; }

        [Required]
        [Display(Name = "Codigo Familia")]
        public int idInternoFamilias { get; set; }

        [Required]
        [Display(Name = "Codigo Marcas")]
        public int idInternoMarcas { get; set; }

        [Required]        
        [Display(Name = "Codigo Medidas")]
        public int idInternoMedidas { get; set; }

        [Required]
        [Display(Name = "Codigo Proveedores")]
        public int idInternoProveedores { get; set; }
        
        public string foto { get; set; }

    }

    public class EditArticulosViewModels
    {
        [Required]
        public int idInternoArticulos { get; set; }

        [Required]
        //[ValidarDuplicidad]
        [StringLength(15)]
        [Display(Name = "Codigo del articulos")]
        public string idArticulo { get; set; }

        [StringLength(25)]
        [Display(Name = "Codigo de barras")]
        public string codigoBarras { get; set; }

        [Required]
        //[ValidarDuplicidad]
        [StringLength(70)]
        [Display(Name = "Nombre Articulo")]
        public string nombreArticulo { get; set; }

        //[Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento1_1 { get; set; }

        // [Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento2_2 { get; set; }

        //[Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "% Descuento")]
        public decimal porcentajeDescuento3_3 { get; set; }

        //[Range(1,19, ErrorMessage ="El valor ingresado pasa el rango aceptado")]
        [Display(Name = "Precio")]
        public decimal precioVenta_1_1 { get; set; }

        // [Range(1, 19, ErrorMessage = "El valor ingresado pasa el rango aceptado")]
        [Display(Name = "Precio Dos")]
        public decimal precioVenta_2_2 { get; set; }

        //Incluir en vista y controlador Para Version en farmacia 
        [Display(Name = "Tiene fecha vencimiento")]
        public bool statusFechaVenc { get; set; }

        [Display(Name = "Precio Menudeo")]
        public bool statusMenudeo { get; set; }

        [Required]
        //[StringLength(1)]
        [Display(Name = "Tipo de Articulo")]
        public string tipo { get; set; }

        [Required]
        [Display(Name = "Codigo Generico")]
        public int idinternoArticuloDetalleGenerico { get; set; }

        [Required]
        [Display(Name = "Codigo Familia")]
        public int idInternoFamilias { get; set; }

        [Required]
        [Display(Name = "Codigo Marcas")]
        public int idInternoMarcas { get; set; }

        [Required]
        [Display(Name = "Codigo Medidas")]
        public int idInternoMedidas { get; set; }

        [Required]
        [Display(Name = "Codigo Proveedores")]
        public int idInternoProveedores { get; set; }

        public string foto { get; set; }

        public int contador { get; set; }

        public List<EditArticulosdetalleViewModels> conceptosEdit { get; set; }

        //public List<Articulosdetalle> conceptosAdd { get; set; }
        //public List<DeleteArticulosdetalle> conceptosDelete { get; set; }

    }

    public class EditArticulosdetalleViewModels
    {
        [Display(Name = "Ubicacion")]
        public string ubicacion { get; set; }

        [Display(Name = "Maximo")]
        public double maximo { get; set; }

        [Display(Name = "Minimo")]
        public double minimo { get; set; }

        [Required]
        [Display(Name = "Codigo Articulo")]
        public int idInternoArticulos { get; set; }

        [Required]
        [Display(Name = "Codigo Bodega")]
        public int idInternoBodegas { get; set; }

        [Required]
        public int idInternoArticulosDetalle { get; set; }

    }

    public class DeleteArticulosdetalle
    {
        [Required]
        public int idInternoArticulosDetalle { get; set; }
    }

    /************Validar si el articulo ya existe en la base de datos**********/

    public class ValidarDuplicidadAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            using (db_pcsolutions_webEntities db = new db_pcsolutions_webEntities())
            {
                var valorDb = (string)value;
                
                if (db.articulosinv.Where(d => d.IdArticulo == valorDb).Count() > 0)
                {
                    return new ValidationResult("if");
                }
                else
                {
                    //return ValidationResult.Success;
                    return new ValidationResult("else");
                }
                
            }  

            
        }
    }

}