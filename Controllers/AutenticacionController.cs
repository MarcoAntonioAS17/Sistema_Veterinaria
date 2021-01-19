using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinaria.Helpers;
using Sistema_Veterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : Controller
    {
        private IUserService _userService;
        private IDataProtector protector;

        public AutenticacionController(IUserService userService, IDataProtectionProvider provider)
        {
            _userService = userService;
            this.protector = provider.CreateProtector("ProtectorID");
        }

        // POST api/<UsuariosController>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] Usuarios value)
        {
            var user = _userService.Authenticate(value.UserName, value.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect!" });
            }

            var foo = new Usuarios();

            //var bar = this.protector.Protect(user.IdUser.ToString());
            //foo.Id = bar;

            //var bar = this.protector.Protect(System.Text.Encoding.UTF8.GetBytes(user.UserName));
            //foo.UserName = System.Text.Encoding.UTF8.GetString(bar);

            foo.UserName = user.UserName;
            foo.Nivel = user.Nivel;
            foo.Token = user.Token;

            return Ok(foo);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("getuser/{cipherID}")]
        public Usuarios GetUser(string cipherID)
        {
            var id = this.protector.Unprotect(cipherID);
            return null;
        }
    }
}
