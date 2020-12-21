using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Proveedores
    {
        public Proveedores()
        {
            Compras = new HashSet<Compras>();
            Productos = new HashSet<Productos>();
        }

        public int IdProveedores { get; set; }
        public string ProveedorNombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public virtual ICollection<Compras> Compras { get; set; }
        public virtual ICollection<Productos> Productos { get; set; }
    }
}
