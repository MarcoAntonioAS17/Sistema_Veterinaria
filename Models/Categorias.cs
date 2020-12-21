using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Categorias
    {
        public Categorias()
        {
            Productos = new HashSet<Productos>();
        }

        public int IdCategorias { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}
