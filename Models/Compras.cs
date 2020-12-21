using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Compras
    {
        public int IdCompras { get; set; }
        public int RProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int RUsuario { get; set; }

        public virtual Proveedores RProveedorNavigation { get; set; }
        public virtual Usuarios RUsuarioNavigation { get; set; }
    }
}
