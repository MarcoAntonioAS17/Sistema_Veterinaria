using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Registro
    {
        public int IdAcciones { get; set; }
        public int? RUsuario { get; set; }
        public int Seccion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Detalles { get; set; }

        public virtual Usuarios RUsuarioNavigation { get; set; }
    }
}
