using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Ventas
    {
        public int IdVentas { get; set; }
        public int RCliente { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int? RUsuario { get; set; }

        public virtual Clientes RClienteNavigation { get; set; }
        public virtual Usuarios RUsuarioNavigation { get; set; }
    }
}
