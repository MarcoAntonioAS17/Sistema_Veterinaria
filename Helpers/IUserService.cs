using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema_Veterinaria.Models;

namespace Sistema_Veterinaria.Helpers
{
    public interface IUserService
    {
        Usuarios Authenticate(string username, string password);
        IEnumerable<Usuarios> GetAll();
    }
}
