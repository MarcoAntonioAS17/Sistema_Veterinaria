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
    public class ProveedoresController : ControllerBase
    {
        // GET: api/<ProveedoresController>
        [HttpGet]
        public IEnumerable<Proveedores> Get()
        {
            var context = new veterinariaContext();
            var proveedores = context.Proveedores.ToList();
            return proveedores;
        }

        // GET api/<ProveedoresController>/5
        [HttpGet("{id}")]
        public Proveedores Get(int id)
        {
            var context = new veterinariaContext();
            Proveedores proveedores = (from pro in context.Proveedores
                                       where pro.IdProveedores == id
                                 select pro).FirstOrDefault<Proveedores>();
            return proveedores;
        }

        // POST api/<ProveedoresController>
        [HttpPost]
        public IActionResult Post([FromBody] Proveedores value)
        {
            bool error = false;

            if (value.IdProveedores != 0 || string.IsNullOrWhiteSpace(value.ProveedorNombre)
                || (string.IsNullOrWhiteSpace(value.Telefono) && string.IsNullOrWhiteSpace(value.Correo))
                )
            {
                error = true;
            }
            else
            {
                value.ProveedorNombre = WebUtility.HtmlEncode(value.ProveedorNombre);
                value.Telefono = WebUtility.HtmlEncode(value.Telefono);
                value.Correo = WebUtility.HtmlEncode(value.Correo);

                try
                {
                    var context = new veterinariaContext();
                    context.Proveedores.Add(value);
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

        // PUT api/<ProveedoresController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Proveedores value)
        {
            bool error = false;

            if (id == 0 || string.IsNullOrWhiteSpace(value.ProveedorNombre)
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
                    Proveedores proveedores = (from pro in context.Proveedores
                                               where pro.IdProveedores == id
                                        select pro).FirstOrDefault<Proveedores>();

                    if (proveedores == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    proveedores.ProveedorNombre = WebUtility.HtmlEncode(value.ProveedorNombre);
                    proveedores.Telefono = WebUtility.HtmlEncode(value.Telefono);
                    proveedores.Correo = WebUtility.HtmlEncode(value.Correo);

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

        // DELETE api/<ProveedoresController>/5
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
                    Proveedores proveedor = (from pro in context.Proveedores
                                        where pro.IdProveedores == id
                                        select pro).FirstOrDefault<Proveedores>();

                    if (proveedor == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    context.Proveedores.Remove(proveedor);

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
}
