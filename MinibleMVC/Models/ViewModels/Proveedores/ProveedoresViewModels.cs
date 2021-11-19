using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels
{
    public class ProveedoresViewModels
    {
        [Required]
        [Display(Name = "Codigo Proveedor")]
        public string idProveedor { get; set; }

        [Required]
        [Display(Name = "Nombre Comercial")]
        public string nombreComercial { get; set; }

        [Display(Name = "Razon social")]
        public string razonSocial { get; set; }

        [Display(Name = "Direccion")]
        public string direccion { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Fax")]
        public string fax { get; set; }

        [Display(Name = "Apartado Postal")]
        public string apartadoPostal { get; set; }

        [Display(Name = "DPI")]
        public string cedula { get; set; }

        [Required]
        [Display(Name = "Nit")]
        public string nit { get; set; }

        [Display(Name = "Fecha Alta")]
        public DateTime fechaAlta { get; set; }

        [Display(Name = "Dias de Credito")]
        public decimal diasCredito { get; set; }

        [Display(Name = "Limite de Credito")]
        public decimal limiteCredito { get; set; }

        [Display(Name = "Saldo Anterior")]
        public decimal saldoAnterior { get; set; }

        [Display(Name = "Debe")]
        public decimal debe { get; set; }

        [Display(Name = "Haber")]
        public decimal haber { get; set; }

        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }

        [Display(Name = "Correo")]
        public string email { get; set; }

        [Display(Name = "Dia de Pago")]
        public string diaPago { get; set; }

        [Display(Name = "Persona Contacto 1")]
        public string personaContacto1 { get; set; }

        [Display(Name = "Persona Contacto 2")]
        public string personaContacto2 { get; set; }

        [Display(Name = "Alta Baja")]
        public string altaBaja { get; set; }

        [Display(Name = "Fecha de baja")]
        public DateTime fechaBaja { get; set; }

        [Display(Name = "Dias Pronto Pago")]
        public decimal diasProntoPago { get; set; }

        [Display(Name = "Descuento Pronto Pago")]
        public decimal descProntoPago { get; set; }

        [Display(Name = "Monto por Aplicar")]
        public decimal montoPorAplicar { get; set; }

        [Display(Name = "Monto Provisional")]
        public decimal montoProvicional { get; set; }

        [Display(Name = "Cuenta contable")]
        public string idCuenta { get; set; }

        [Display(Name = "Clase Proveedor")]
        public int idInternoClasesProveedores { get; set; }

        [Display(Name = "Pais")]
        public int idInternoPaises { get; set; }

        [Display(Name = "Zona")]
        public int idInternoZonas { get; set; }

        [Display(Name = "Localidades")]
        public int idInternoLocalidades { get; set; }

    }

    public class EditProveedoresViewModels
    {
        public int idInternoProveedores { get; set; }

        [Required]
        [Display(Name = "Codigo Proveedor")]
        public string idProveedor { get; set; }

        [Required]
        [Display(Name = "Nombre Comercial")]
        public string nombreComercial { get; set; }

        [Display(Name = "Razon Social")]
        public string razonSocial { get; set; }

        [Display(Name = "Direccion")]
        public string direccion { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Fax")]
        public string fax { get; set; }

        [Display(Name = "Apartado Postal")]
        public string apartadoPostal { get; set; }

        [Display(Name = "DPI")]
        public string cedula { get; set; }

        [Required]
        [Display(Name = "Nit")]
        public string nit { get; set; }

        [Display(Name = "Fecha Alta")]
        public DateTime fechaAlta { get; set; }

        [Display(Name = "Dias de Credito")]
        public decimal diasCredito { get; set; }

        [Display(Name = "Limite de Credito")]
        public decimal limiteCredito { get; set; }

        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }

        [Display(Name = "Correo Electronico")]
        public string email { get; set; }

        [Display(Name = "Dia de Pago")]
        public string diaPago { get; set; }

        [Display(Name = "Persona Contacto 1")]
        public string personaContacto1 { get; set; }

        [Display(Name = "Persona Contacto 2")]
        public string personaContacto2 { get; set; }

        [Display(Name = "Alta Baja")]
        public string altaBaja { get; set; }

        [Display(Name = "Fecha de Baja")]
        public DateTime fechaBaja { get; set; }

        [Display(Name = "Dias pronto Pago")]
        public decimal diasProntoPago { get; set; }

        [Display(Name = "Descuento Pronto Pago")]
        public decimal descProntoPago { get; set; }

        [Display(Name = "Cuenta Contable")]
        public string idCuenta { get; set; }

        [Display(Name = "Clase Proveedor")]
        public int idInternoClasesProveedores { get; set; }

        [Display(Name = "Pais")]
        public int idInternoPaises { get; set; }

        [Display(Name = "Zona")]
        public int idInternoZonas { get; set; }

        [Display(Name = "Localidades")]
        public int idInternoLocalidades { get; set; }


    }
}