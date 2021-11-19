//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Minible5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class clientes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clientes()
        {
            this.bitacora = new HashSet<bitacora>();
            this.cotizacionesinv = new HashSet<cotizacionesinv>();
        }
    
        public int IdInternoClientes { get; set; }
        public string IdCliente { get; set; }
        public string NombreComercial { get; set; }
        public string RazonSocial { get; set; }
        public string direccion { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string ApartadoPostal { get; set; }
        public string Cedula { get; set; }
        public string nit { get; set; }
        public Nullable<System.DateTime> FechadeAlta { get; set; }
        public Nullable<int> DiasCredito { get; set; }
        public Nullable<decimal> LimiteCredito { get; set; }
        public Nullable<decimal> SaldoAnterior { get; set; }
        public Nullable<decimal> Debe { get; set; }
        public Nullable<decimal> Haber { get; set; }
        public string Observaciones { get; set; }
        public string E_Mail { get; set; }
        public string DiaPago { get; set; }
        public string PersonaContacto_1 { get; set; }
        public string PersonaContacto_2 { get; set; }
        public string AltaBaja { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public Nullable<int> DiasProntoPago { get; set; }
        public Nullable<decimal> DescProntoPago { get; set; }
        public Nullable<decimal> MontoPorAplicar { get; set; }
        public Nullable<decimal> MontoProvicional { get; set; }
        public Nullable<decimal> SaldoInicioPeriodo { get; set; }
        public Nullable<decimal> MontoPorAplicarPeriodo { get; set; }
        public Nullable<decimal> MontoProvicionalPeriodo { get; set; }
        public string TipoPrecio { get; set; }
        public string CuentaContable { get; set; }
        public string CreditoContado { get; set; }
        public string MedidaFacturacion { get; set; }
        public Nullable<decimal> ValorTarifaU { get; set; }
        public Nullable<decimal> DiasFacturacion { get; set; }
        public Nullable<System.DateTime> FechaUltimaFacturacion { get; set; }
        public string registro { get; set; }
        public string giro { get; set; }
        public string codigocliente { get; set; }
        public string direcciontrabajo { get; set; }
        public string direccionfiador { get; set; }
        public string puesto { get; set; }
        public string telefonotrabajo { get; set; }
        public string telefonofiador { get; set; }
        public string nombrefiador { get; set; }
        public string lugartrabajofiador { get; set; }
        public string idsector { get; set; }
        public string contador_luz { get; set; }
        public string ref_comercial { get; set; }
        public Nullable<System.DateTime> fechanacimiento { get; set; }
        public string tipocasa { get; set; }
        public string sexo { get; set; }
        public string verificadov { get; set; }
        public string dias_max_credito { get; set; }
        public string nombrecontacto1 { get; set; }
        public string direccioncontacto1 { get; set; }
        public string telefonocontacto1 { get; set; }
        public string nombrecontacto2 { get; set; }
        public string direccioncontacto2 { get; set; }
        public string telefonocontacto2 { get; set; }
        public string agenteretenedor { get; set; }
        public Nullable<System.DateTime> Fecha_baja { get; set; }
        public string status { get; set; }
        public string Codigo_Empresa { get; set; }
        public int IdInternoClases { get; set; }
        public int IdInternoLocalidades { get; set; }
        public int IdInternoCobrador { get; set; }
        public int IdInternoPaises { get; set; }
        public int IdInternoVendedores { get; set; }
        public int IdInternoZonas { get; set; }
        public int IdInternoBodegas { get; set; }
        public int IdInternoSectores { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bitacora> bitacora { get; set; }
        public virtual bodegasinv bodegasinv { get; set; }
        public virtual clases clases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cotizacionesinv> cotizacionesinv { get; set; }
        public virtual cobradores cobradores { get; set; }
        public virtual localidades localidades { get; set; }
        public virtual paises paises { get; set; }
        public virtual sectores sectores { get; set; }
        public virtual vendedores vendedores { get; set; }
        public virtual zonas zonas { get; set; }
    }
}