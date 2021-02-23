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
    
    public partial class Tbl_Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Producto()
        {
            this.Tbl_CarroProducto = new HashSet<Tbl_CarroProducto>();
            this.Tbl_imagen = new HashSet<Tbl_imagen>();
        }
    
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public Nullable<int> ProveedorId { get; set; }
        public Nullable<int> IdSubCategoria { get; set; }
        public Nullable<int> IdCategoria { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CarroProducto> Tbl_CarroProducto { get; set; }
        public virtual Tbl_Categoria Tbl_Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_imagen> Tbl_imagen { get; set; }
        public virtual Tbl_SubCategoria Tbl_SubCategoria { get; set; }
        public virtual Tbl_Proveedor Tbl_Proveedor { get; set; }
    }
}