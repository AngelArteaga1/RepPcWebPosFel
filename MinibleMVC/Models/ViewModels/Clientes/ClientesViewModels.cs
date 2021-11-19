using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels.Clientes
{
    public class ClientesViewModels
    {
        [Required]
        [Display(Name = "Codigo Cliente")]
        public string idCliente { get; set; }
        [Required]
        [Display(Name = "Nombre Comercial")]
        public string nombreComercial { get; set; }

        [Display(Name = "Razon Social")]
        public string razonSocial { get; set; }

        [Display(Name = "Direccion")]
        public string direccion { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Fax")]
        public string fax { get; set; }

        [Display(Name = "Apartado Postal")]
        public string apartadopostal { get; set; }

        [Display(Name = "DPI")]
        public string cedula { get; set; }

        [Required]
        [Display(Name = "Nit")]
        public string nit { get; set; }

        [Display(Name = "Fecha de Alta")]
        public DateTime fechadealta { get; set; }

        [Display(Name = "Dias de Credito")]
        public int diascredito { get; set; }

        [Display(Name = "Limite de Credito")]
        public Decimal limitecredito { get; set; }

        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }

        [Display(Name = "Correo Electronico")]
        public string e_mail { get; set; }

        [Display(Name = "Dia de Pago")]
        public string diapago { get; set; }

        [Display(Name = "Persona Contacto 1")]
        public string personacontacto_1 { get; set; }


        [Display(Name = "Persona Contacto 2")]
        public string personacontacto_2 { get; set; }
       

        [Display(Name = "Dias Pronto Pago")]
        public int diasprontopago { get; set; }

        [Display(Name = "Descuento Proto Pago")]
        public decimal descprontopago { get; set; }

        [Display(Name = "TIpo Precio")]
        public string tipoprecio { get; set; }

        [Display(Name = "Cuenta Contable")]
        public string cuentacontable { get; set; }
        

        [Display(Name = "Contador Luz")]
        public string contador_luz { get; set; }

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Display(Name = "Dias Maximo de Credito")]
        public string dias_max_credito { get; set; }

        [Display(Name = "Agente Retenedor")]
        public bool agenteretenedor { get; set; }        

        [Display(Name = "Alta")]
        public string altabaja { get; set; }

        [Required]
        [Display(Name = "Clase")]
        public int idInternoClase { get; set; }

        [Required]
        [Display(Name = "Localidades")]
        public int idInternoLocalidad { get; set; }

        [Required]
        [Display(Name = "Cobrador")]
        public int idInternoCobrador { get; set; }

        [Required]
        [Display(Name = "Paises")]
        public int idInternoPais { get; set; }

        [Required]
        [Display(Name = "Vendedores")]
        public int idInternoVendedor { get; set; }

        [Required]
        [Display(Name = "Sectores")]
        public int idInternoSectores { get; set; }

        [Required]
        [Display(Name = "Zonas")]
        public int idInternoZona { get; set; }

        [Required]
        [Display(Name = "Bodegas")]
        public int idInternoBodega { get; set; }


    }

    public class EditClientesViewModels
    {   
        [Required]
        public int idInternoClientes { get; set; }

        [Required]
        [Display(Name = "Codigo Cliente")]
        public string idCliente { get; set; }
        [Required]
        [Display(Name = "Nombre Comercial")]
        public string nombreComercial { get; set; }

        [Display(Name = "Razon Social")]
        public string razonSocial { get; set; }

        [Display(Name = "Direccion")]
        public string direccion { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Fax")]
        public string fax { get; set; }

        [Display(Name = "Apartado Postal")]
        public string apartadopostal { get; set; }

        [Display(Name = "DPI")]
        public string cedula { get; set; }

        [Required]
        [Display(Name = "Nit")]
        public string nit { get; set; }

        [Display(Name = "Fecha de Alta")]
        public DateTime fechadealta { get; set; }

        [Display(Name = "Dias de Credito")]
        public int diascredito { get; set; }

        [Display(Name = "Limite de Credito")]
        public Decimal limitecredito { get; set; }

        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }

        [Display(Name = "Correo Electronico")]
        public string e_mail { get; set; }

        [Display(Name = "Dia de Pago")]
        public string diapago { get; set; }

        [Display(Name = "Persona Contacto 1")]
        public string personacontacto_1 { get; set; }


        [Display(Name = "Persona Contacto 2")]
        public string personacontacto_2 { get; set; }

        [Display(Name = "Dias Pronto Pago")]
        public int diasprontopago { get; set; }

        [Display(Name = "Descuento Proto Pago")]
        public decimal descprontopago { get; set; }

        [Display(Name = "TIpo Precio")]
        public string tipoprecio { get; set; }

        [Display(Name = "Cuenta Contable")]
        public string cuentacontable { get; set; }        

        [Display(Name = "Contador Luz")]
        public string contador_luz { get; set; }   

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Display(Name = "Dias Maximo de Credito")]
        public string dias_max_credito { get; set; }

        [Display(Name = "Agente Retenedor")]
        public bool agenteretenedor { get; set; }

        [Display(Name = "Estatus")]
        public string altabaja { get; set; }

        [Required]
        [Display(Name = "Clase")]
        public int idInternoClase { get; set; }

        [Required]
        [Display(Name = "Localidades")]
        public int idInternoLocalidad { get; set; }

        [Required]
        [Display(Name = "Cobrador")]
        public int idInternoCobrador { get; set; }

        [Required]
        [Display(Name = "Paises")]
        public int idInternoPais { get; set; }

        [Required]
        [Display(Name = "Vendedores")]
        public int idInternoVendedor { get; set; }

        [Required]
        [Display(Name = "Sectores")]
        public int idInternoSectores { get; set; }

        [Required]
        [Display(Name = "Zonas")]
        public int idInternoZona { get; set; }

        [Required]
        [Display(Name = "Bodegas")]
        public int idInternoBodega { get; set; }

    }

}