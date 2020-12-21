using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Citas
    {
        public int IdCitas { get; set; }
        public int? RCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public string Tipo { get; set; }
        public int? RMascota { get; set; }
        public string Notas { get; set; }

        public virtual Clientes RClienteNavigation { get; set; }
        public virtual Mascotas RMascotaNavigation { get; set; }
    }
}
