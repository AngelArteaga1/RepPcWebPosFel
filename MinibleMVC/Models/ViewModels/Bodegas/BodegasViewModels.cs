﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Minible5.Models.ViewModels
{
    public class BodegasViewModels
    {        

        [Required]
        [Display(Name = "Codigo bodega")]
        public string IdBodega { get; set; }

        [StringLength(70)]
        [Display(Name = "Descripcio de Bodega")]
        public string Descripcion { get; set; }

        [StringLength(70)]
        [Display(Name = "Encargado de Bodega")]
        public string Encargado { get; set; }

        [StringLength(70)]
        [Display(Name = "Direccion de Bodega")]
        public string Direccion { get; set; }

        [StringLength(30)]
        [Display(Name = "Telefono de Bodega")]
        public string Telefono { get; set; }

        [StringLength(6)]
        [Display(Name = "Centro de Costo")]
        public String idcentrocosto { get; set; }

        [StringLength(1)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [StringLength(15)]
        [Display(Name = "Cuenta Contable Inventario")]
        public string cta_bodega_inventario { get; set; }

    }


    public class EditBodegasViewModel
    {
        public int IdInternoBodegas { get; set; }

        [Required]
        [Display(Name = "Codigo bodega")]
        public string IdBodega { get; set; }

        [Required]
        [Display(Name = "Descripcio de Bodega")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Encargado de Bodega")]
        public string Encargado { get; set; }

        [Required]
        [Display(Name = "Direccion de Bodega")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Telefono de Bodega")]
        public string Telefono { get; set; }

        [Display(Name = "Centro de Costo")]
        public String idcentrocosto { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Display(Name = "Cuenta Contable Inventario")]
        public string cta_bodega_inventario { get; set; }

    }


}