using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sistema_Veterinaria.Models;
using System.Net;
using Sistema_Veterinaria.Encryption;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // GET: api/<UsuariosController>
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            var context = new veterinariaContext();
            var usuarios = from usu in context.Usuarios
                           orderby usu.IdUser
                           select new Usuario
                           {
                               IdUser = usu.IdUser,
                               UserName = usu.UserName,
                               Nivel = usu.Nivel
                           };

            return usuarios;
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public Usuario Get(int id)
        {
            var context = new veterinariaContext();
            Usuario usuario = (from usu in context.Usuarios
                           orderby usu.IdUser
                           where usu.IdUser == id
                                select new Usuario
                                {
                                    IdUser = usu.IdUser,
                                    UserName = usu.UserName,
                                    Nivel = usu.Nivel
                                }).FirstOrDefault<Usuario>();
            return usuario;
        }

        // POST api/<UsuariosController>
        [HttpPost]
        public IActionResult Post([FromBody] Usuarios value)
        {
            bool error = false;

            if (value.Password.Length < 6 
                || value.UserName.Length < 8)
            {
                error = true;
            }
            else 
            {
                value.Password = WebUtility.HtmlDecode(value.Password);
                value.Password = Encrypt.getSHA256(value.Password);
                value.UserName = WebUtility.HtmlDecode(value.UserName);
                try
                {
                    var context = new veterinariaContext();
                    context.Usuarios.Add(value);
                    context.SaveChanges();
                }catch (Exception ex)
                {
                    error = true;
                    Console.WriteLine(ex);
                }

            }
            var Result = new { Status = !error ? "Success" : "Fail" };
            return new JsonResult(Result);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UsuarioModi value)
        {
            bool error = false;

            if (string.IsNullOrWhiteSpace(value.UserName)
                || string.IsNullOrWhiteSpace(value.Password)
                || value.PasswordN.Length < 6
                || value.Password.Length < 6
                )
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Usuarios usuario = (from usu in context.Usuarios
                                         where usu.IdUser == id && usu.UserName == value.UserName 
                                         && usu.Password == Encrypt.getSHA256(value.Password)
                                        select usu).SingleOrDefault<Usuarios>();
                    if (usuario == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    usuario.Password = WebUtility.HtmlDecode(value.PasswordN);
                    usuario.Password = Encrypt.getSHA256(usuario.Password);
                    usuario.UserName = WebUtility.HtmlDecode(value.UserName);
                    usuario.Nivel = value.Nivel;
                    
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = true;
                    Console.WriteLine(ex);
                }

            }
            var Result = new { Status = !error ? "Success" : "Fail" };
            return new JsonResult(Result);
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool error = false;

            if (id == 0)
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Usuarios usuario = (from usu in context.Usuarios
                                        where usu.IdUser == id
                                        select usu).SingleOrDefault<Usuarios>();

                    context.Usuarios.Remove(usuario);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = true;
                    Console.WriteLine(ex);
                }

            }
            var Result = new { Status = !error ? "Success" : "Fail" };
            return new JsonResult(Result);
        }
    }

    public class Usuario
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int Nivel { get; set; }
    }

    public class UsuarioModi
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int Nivel { get; set; }
        public string Password { get; set; }
        public string PasswordN { get; set; }
    }
}
