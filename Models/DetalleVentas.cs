using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class DetalleVentas
    {
        public int IdDetalleVentas { get; set; }
        public int RVenta { get; set; }
        public string RProducto { get; set; }
        public int? Cantidad { get; set; }

        public virtual Productos RProductoNavigation { get; set; }
        public virtual Ventas RVentaNavigation { get; set; }
    }
}
