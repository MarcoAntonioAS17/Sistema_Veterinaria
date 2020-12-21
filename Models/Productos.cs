using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Productos
    {
        public string IdProductos { get; set; }
        public string Nombre { get; set; }
        public float PrecioVenta { get; set; }
        public float? PrecioCompra { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? Caducidad { get; set; }
        public string Descripcion { get; set; }
        public int? RCategoria { get; set; }
        public int? RProveedor { get; set; }

        public virtual Categorias RCategoriaNavigation { get; set; }
        public virtual Proveedores RProveedorNavigation { get; set; }
    }
}
