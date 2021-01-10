using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Ventas
    {
        public Ventas()
        {
            DetalleVentas = new HashSet<DetalleVentas>();
        }

        public int IdVentas { get; set; }
        public int RCliente { get; set; }
        public DateTime FechaHora { get; set; }
        public int? RUsuario { get; set; }

        public virtual Clientes RClienteNavigation { get; set; }
        public virtual Usuarios RUsuarioNavigation { get; set; }
        public virtual ICollection<DetalleVentas> DetalleVentas { get; set; }
    }
}
