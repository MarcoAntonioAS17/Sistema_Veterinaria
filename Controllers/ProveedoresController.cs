﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sistema_Veterinaria.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.DataProtection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private veterinariaContext context;
        public ProveedoresController(veterinariaContext ctx, IDataProtectionProvider provider)
        {
            this.context = ctx;
        }

        // GET: api/<ProveedoresController>
        [HttpGet]
        public IEnumerable<Proveedores> Get()
        {
            var proveedores = context.Proveedores.ToList();
            return proveedores;
        }

        // GET api/<ProveedoresController>/5
        [HttpGet("{id}")]
        public Proveedores Get(int id)
        {
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

            if (validando_campos(value.ProveedorNombre, value.Telefono, value.Correo))
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

            if (validando_campos(value.ProveedorNombre, value.Telefono, value.Correo))
            {
                error = true;
            }
            else
            {
                try
                {
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
