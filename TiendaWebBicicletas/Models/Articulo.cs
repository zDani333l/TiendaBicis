using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaWebBicicletas.DAL;

namespace TiendaWebBicicletas.Models
{
    public class Articulo
    {
        public Tbl_Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public void ResultadoPrecio()
        {
            this.Precio = (double)this.Producto.Precio * this.Cantidad;
        }
    }
}