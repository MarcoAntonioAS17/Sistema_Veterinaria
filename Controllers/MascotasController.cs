using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema_Veterinaria.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasController : ControllerBase
    {
        // GET: api/<MascotasController>
        [HttpGet]
        public IEnumerable<Mascota> Get()
        {
            var context = new veterinariaContext();
            var mascota = from m in context.Mascotas
                          join c in context.Clientes on m.RCliente equals c.IdClientes
                          orderby m.IdMascotas ascending
                          select new Mascota
                          {
                              IdMascotas = m.IdMascotas,
                              Nombre = m.Nombre,
                              Edad = m.Edad,
                              Tipo = m.Tipo,
                              Raza = m.Raza,
                              Descripcion = m.Descripcion,
                              RCliente = c.IdClientes,
                              ClienteNombre = c.Nombre
                          };
            return mascota;
        }

        // GET api/<MascotasController>/5
        [HttpGet("{id}")]
        public Mascota Get(int id)
        {
            var context = new veterinariaContext();
            var mascota = (from m in context.Mascotas
                          join c in context.Clientes on m.RCliente equals c.IdClientes
                          orderby m.IdMascotas ascending
                          where m.IdMascotas == id
                          select new Mascota
                          {
                              IdMascotas = m.IdMascotas,
                              Nombre = m.Nombre,
                              Edad = m.Edad,
                              Tipo = m.Tipo,
                              Raza = m.Raza,
                              Descripcion = m.Descripcion,
                              RCliente = c.IdClientes,
                              ClienteNombre = c.Nombre
                          }).FirstOrDefault<Mascota> ();
            return mascota;
        }

        // GET api/<MascotasController>/5
        [HttpGet("Cliente/{id}")]
        public IEnumerable<Mascota> GetbyCliente(int id)
        {
            var context = new veterinariaContext();
            var mascota = from m in context.Mascotas
                          join c in context.Clientes on m.RCliente equals c.IdClientes
                          orderby m.IdMascotas ascending
                          where c.IdClientes == id
                          select new Mascota
                          {
                              IdMascotas = m.IdMascotas,
                              Nombre = m.Nombre,
                              Edad = m.Edad,
                              Tipo = m.Tipo,
                              Raza = m.Raza,
                              Descripcion = m.Descripcion,
                              RCliente = c.IdClientes,
                              ClienteNombre = c.Nombre
                          };
            return mascota;
        }

        // POST api/<MascotasController>
        [HttpPost]
        public IActionResult Post([FromBody] Mascotas value)
        {
            bool error = false;

            if (validar_campos(value.Nombre, value.Edad, value.Tipo, value.Raza, value.Descripcion))
            {
                error = true;
            }
            else{
                value.Nombre = WebUtility.HtmlEncode(value.Nombre);
                value.Tipo = WebUtility.HtmlEncode(value.Tipo);
                value.Raza = WebUtility.HtmlEncode(value.Raza);
                value.Descripcion = WebUtility.HtmlEncode(value.Descripcion);

                try
                {
                    var context = new veterinariaContext();
                    context.Mascotas.Add(value);
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

        // PUT api/<MascotasController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Mascotas value)
        {
            bool error = false;

            if (validar_campos(value.Nombre, value.Edad, value.Tipo, value.Raza, value.Descripcion))
            {
                error = true;
            }
            else
            {
                
                try
                {
                    var context = new veterinariaContext();
                    Mascotas mascota = (from m in context.Mascotas
                                        where m.IdMascotas == id
                                        select m).FirstOrDefault<Mascotas>();
                    if (mascota == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }

                    mascota.Nombre = WebUtility.HtmlEncode(value.Nombre);
                    mascota.Edad = value.Edad; 
                    mascota.Tipo = WebUtility.HtmlEncode(value.Tipo);
                    mascota.Raza = WebUtility.HtmlEncode(value.Raza);
                    mascota.Descripcion = WebUtility.HtmlEncode(value.Descripcion);
                    mascota.RCliente = value.RCliente;

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

        // DELETE api/<MascotasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool error = false;

            if (id == 0)
            {
                error = false;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Mascotas mascota = (from m in context.Mascotas
                                        where m.IdMascotas == id
                                        select m).FirstOrDefault<Mascotas>();
                    if (mascota == null) {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    context.Mascotas.Remove(mascota);
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

        bool validar_campos(string nombre, DateTime edad, string tipo, string raza, string descrip)
        {
            bool error = false;
            Regex solo_letras = new Regex("^[a-zA-Z ]*$");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (!solo_letras.IsMatch(nombre))
                    error = true;
            }
            else
            {
                error = true;
            }
            if (edad > DateTime.Now)
            {
                error = true;
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                if (!solo_letras.IsMatch(tipo))
                    error = true;
            }
            if (!string.IsNullOrWhiteSpace(raza))
            {
                if (!solo_letras.IsMatch(raza))
                    error = true;
            }
            if (!string.IsNullOrWhiteSpace(descrip))
            {
                if (!solo_letras.IsMatch(descrip))
                    error = true;
            }

            return error;
        }
    }


    public class Mascota
    {
        public int IdMascotas { get; set; }
        public string Nombre { get; set; }
        public DateTime Edad { get; set; }
        public string Tipo { get; set; }
        public string Raza { get; set; }
        public string Descripcion { get; set; }
        public int RCliente { get; set; }
        public string ClienteNombre { get; set; }
    }

}
