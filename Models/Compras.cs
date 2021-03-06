﻿using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Compras
    {
        public Compras()
        {
            DetalleCompras = new HashSet<DetalleCompras>();
        }

        public int IdCompras { get; set; }
        public int RProveedor { get; set; }
        public DateTime FechaHora { get; set; }
        public int RUsuario { get; set; }

        public virtual Proveedores RProveedorNavigation { get; set; }
        public virtual Usuarios RUsuarioNavigation { get; set; }
        public virtual ICollection<DetalleCompras> DetalleCompras { get; set; }
    }
}
