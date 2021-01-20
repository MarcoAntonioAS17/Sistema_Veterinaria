using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema_Veterinaria.Models;

namespace Sistema_Veterinaria.Helpers
{
    public interface IUserService
    {
        IEnumerable<Usuarios> GetAll(veterinariaContext context);
        Usuarios Authenticate(veterinariaContext context, string username, string password);
    }
}
