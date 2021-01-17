using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Compras = new HashSet<Compras>();
            Registro = new HashSet<Registro>();
            Ventas = new HashSet<Ventas>();
        }

        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Nivel { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Compras> Compras { get; set; }
        public virtual ICollection<Registro> Registro { get; set; }
        public virtual ICollection<Ventas> Ventas { get; set; }
    }
}
