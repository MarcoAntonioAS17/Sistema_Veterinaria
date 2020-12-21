using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Mascotas
    {
        public Mascotas()
        {
            Citas = new HashSet<Citas>();
        }

        public int IdMascotas { get; set; }
        public string Nombre { get; set; }
        public DateTime Edad { get; set; }
        public string Tipo { get; set; }
        public string Raza { get; set; }
        public string Descripcion { get; set; }
        public int? RCliente { get; set; }

        public virtual Clientes RClienteNavigation { get; set; }
        public virtual ICollection<Citas> Citas { get; set; }
    }
}
