using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaWebBicicletas.Models
{
    public class CheckoutViewModels
    {
        [Required]
        [Display(Name = "Nombres*")]
        public string Nombres { get; set; }

        [Required]
        [Display(Name = "Apellidos*")]
        public string Apellidos { get; set; }

        [Required]
        [Display(Name = "Direccion envio*")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Ciudad*")]
        public string Ciudad { get; set; }

        [Required]
        [Display(Name = "Pais*")]
        public string Pais { get; set; }

        [Required]
        [Display(Name = "Correo electrónico*")]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [Display(Name = "Numero de contacto*")]
        public string NumeroContacto { get; set; }

        [Display(Name = "Informacion Adicional")]
        public string InformacionAdicional{ get; set; }

        public List<Articulo> Productos { get; set; }
        public double TotalOrden { get; set; }

        public void ResTotalOrden()
        {
            foreach (var item in Productos)
            {
                item.ResultadoPrecio();
                this.TotalOrden += item.Precio;
            }
        }
    }
}