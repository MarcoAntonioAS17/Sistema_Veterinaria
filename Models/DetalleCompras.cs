using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class DetalleCompras
    {
        public int IdDetalleCompra { get; set; }
        public int RCompra { get; set; }
        public string RProducto { get; set; }
        public int? Cantidad { get; set; }

        public virtual Compras RCompraNavigation { get; set; }
        public virtual Productos RProductoNavigation { get; set; }
    }
}
