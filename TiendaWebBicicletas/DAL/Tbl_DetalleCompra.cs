//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TiendaWebBicicletas.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_DetalleCompra
    {
        public int DetalleCompraId { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string CodigoPostal { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public string TipoPago { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<int> IdCarrito { get; set; }
        public Nullable<int> IdOrdenStatus { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Tbl_Carro Tbl_Carro { get; set; }
        public virtual Tbl_OrdenStatus Tbl_OrdenStatus { get; set; }
    }
}
