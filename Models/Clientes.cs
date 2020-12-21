using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Clientes
    {
        public Clientes()
        {
            Citas = new HashSet<Citas>();
            Mascotas = new HashSet<Mascotas>();
            Ventas = new HashSet<Ventas>();
        }

        public int IdClientes { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public virtual ICollection<Citas> Citas { get; set; }
        public virtual ICollection<Mascotas> Mascotas { get; set; }
        public virtual ICollection<Ventas> Ventas { get; set; }
    }
}
