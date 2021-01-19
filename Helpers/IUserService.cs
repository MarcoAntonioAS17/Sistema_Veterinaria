using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema_Veterinaria.Models;

namespace Sistema_Veterinaria.Helpers
{
    public interface IUserService
    {
        Usuarios Authenticate(veterinariaContext context, string username, string password);
    }
}
