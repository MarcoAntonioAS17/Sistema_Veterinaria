using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sistema_Veterinaria.Models;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            if (value.IdClientes != 0 || string.IsNullOrWhiteSpace(value.Nombre)
                || (string.IsNullOrWhiteSpace(value.Telefono) && string.IsNullOrWhiteSpace(value.Correo))
                ){
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

            if (id == 0 || string.IsNullOrWhiteSpace(value.Nombre)
               || (string.IsNullOrWhiteSpace(value.Telefono) && string.IsNullOrWhiteSpace(value.Correo))
               )
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
                    Console.WriteLine(ex.InnerException.Message);

                }
            }
            var Result = new { Status = !error ? "Success" : "Fail" };

            return new JsonResult(Result);
        }
    }
}
