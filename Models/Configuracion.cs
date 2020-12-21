using System;
using System.Collections.Generic;

namespace Sistema_Veterinaria.Models
{
    public partial class Configuracion
    {
        public int Clave { get; set; }
        public int DiasCaducidad { get; set; }
        public int CantidadInventario { get; set; }
    }
}
