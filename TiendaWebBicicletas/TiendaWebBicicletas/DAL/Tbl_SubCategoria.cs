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
    
    public partial class Tbl_SubCategoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_SubCategoria()
        {
            this.Tbl_Producto = new HashSet<Tbl_Producto>();
        }
    
        public int IdSubCategoria { get; set; }
        public string NombreSubCategoria { get; set; }
        public string DescripcionSubCategoria { get; set; }
        public Nullable<int> CategoriaId { get; set; }
    
        public virtual Tbl_Categoria Tbl_Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Producto> Tbl_Producto { get; set; }
    }
}
