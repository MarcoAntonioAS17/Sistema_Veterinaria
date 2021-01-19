using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Sistema_Veterinaria.Models;
using System.Net;
using System.Text.RegularExpressions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]    
    //[EnableCors (Startup.MY_CORS)] 
    public class ClientesController : ControllerBase
    {
        // GET: api/<ClientesController>
        [HttpGet]
        public IEnumerable<Clientes> Get()
        {
            var context = new veterinariaContext();
            var clientes = context.Clientes.ToList();
            return clientes;
        }

        // GET api/<ClientesController>/5
        [HttpGet("{id}")]
        public Clientes Get(int id)
        {
            var context = new veterinariaContext();
            Clientes clientes = (from cli in context.Clientes
                                 where cli.IdClientes == id select cli).FirstOrDefault<Clientes> ();
            return clientes;
        }

        // POST api/<ClientesController>
        [HttpPost]
        public IActionResult Post([FromBody] Clientes value)
        {
            bool error = false;

            if (validando_campos(value.Nombre, value.Telefono, value.Correo))
            {
                error = true;
            }
            else{
                value.Nombre = WebUtility.HtmlEncode(value.Nombre);
                value.Telefono = WebUtility.HtmlEncode(value.Telefono);
                value.Correo = WebUtility.HtmlEncode(value.Correo);

                try{
                    var context = new veterinariaContext();
                    context.Clientes.Add(value);
                    context.SaveChanges();
                }catch( Exception ex){
                    error = true;
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
            var Result = new { Status = !error ? "Success" : "Fail" };

            return new JsonResult (Result);
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Clientes value)
        {
            bool error = false;

            if (validando_campos(value.Nombre, value.Telefono, value.Correo))
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Clientes cliente = (from cli in context.Clientes
                                         where cli.IdClientes == id
                                         select cli).FirstOrDefault<Clientes>();

                    if (cliente == null){
                        return new JsonResult(new { Status = "Fail" });
                    }
                    cliente.Nombre = WebUtility.HtmlEncode(value.Nombre);
                    cliente.Telefono = WebUtility.HtmlEncode(value.Telefono);
                    cliente.Correo = WebUtility.HtmlEncode(value.Correo);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = true;
                    Console.WriteLine(ex.InnerException.Message);

                }
            }
            var Result = new { Status = !error ? "Success" : "Fail" };

            return new JsonResult(Result);

        }

        // DELETE api/<ClientesController>/5
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
                    Clientes cliente = (from cli in context.Clientes
                                        where cli.IdClientes == id
                                        select cli).FirstOrDefault<Clientes>();

                    if (cliente == null){
                        return new JsonResult(new { Status = "Fail" });
                    }
                    context.Clientes.Remove(cliente);

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

        bool validando_campos(string nombre, string telefono, string correo)
        {
            bool error = false;
            Regex alfanumerico = new Regex("^[a-zA-Z0-9 ]*$");
            Regex num_tel = new Regex("^[0-9-]*$");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (!alfanumerico.IsMatch(nombre))
                    error = true;
            }
            if (!string.IsNullOrWhiteSpace(telefono))
                if (!num_tel.IsMatch(telefono))
                    error = true;

            if (!string.IsNullOrWhiteSpace(correo))
                if (!IsValidEmail(correo))
                    error = true;

            if (string.IsNullOrWhiteSpace(telefono) && string.IsNullOrWhiteSpace(correo))
                error = true;

            return error;
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}
