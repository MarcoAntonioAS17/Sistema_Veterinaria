using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Sistema_Veterinaria.Models;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        // GET: api/<CitasController>
        [HttpGet]
        public IEnumerable<Cita> Get()
        {
            var context = new veterinariaContext();
            var citas = from cit in context.Citas
                        join cli in context.Clientes on cit.RCliente equals cli.IdClientes
                        join mas in context.Mascotas on cit.RMascota equals mas.IdMascotas
                        orderby cit.IdCitas ascending
                        select new Cita
                        {
                            IdCitas = cit.IdCitas,
                            RCliente = cli.IdClientes,
                            NombreCliente = cli.Nombre,
                            FechaHora = cit.FechaHora,
                            Tipo = cit.Tipo,
                            RMascota = mas.IdMascotas,
                            NombreMascota = mas.Nombre,
                            Notas = cit.Notas
                        };

            return citas;
        }

        // GET: api/<CitasController>/Inicio
        [HttpGet("Inicio")]
        public IEnumerable<Cita> GetInicio()
        {
            var context = new veterinariaContext();
            var citas = from cit in context.Citas
                        join cli in context.Clientes on cit.RCliente equals cli.IdClientes
                        join mas in context.Mascotas on cit.RMascota equals mas.IdMascotas
                        where cit.FechaHora >= DateTime.Now && cit.FechaHora < DateTime.Now.AddMonths(1)
                        orderby cit.FechaHora ascending
                        select new Cita
                        {
                            IdCitas = cit.IdCitas,
                            RCliente = cli.IdClientes,
                            NombreCliente = cli.Nombre,
                            FechaHora = cit.FechaHora,
                            Tipo = cit.Tipo,
                            RMascota = mas.IdMascotas,
                            NombreMascota = mas.Nombre,
                            Notas = cit.Notas
                        };
            
            return citas;
        }

        // GET api/<CitasController>/5
        [HttpGet("{id}")]
        public Cita Get(int id)
        {
            var context = new veterinariaContext();
            var cita = (from cit in context.Citas
                        join cli in context.Clientes on cit.RCliente equals cli.IdClientes
                        join mas in context.Mascotas on cit.RMascota equals mas.IdMascotas
                        orderby cit.FechaHora ascending
                        where cit.IdCitas == id
                        select new Cita
                        {
                            IdCitas = cit.IdCitas,
                            RCliente = cli.IdClientes,
                            NombreCliente = cli.Nombre,
                            FechaHora = cit.FechaHora,
                            Tipo = cit.Tipo,
                            RMascota = mas.IdMascotas,
                            NombreMascota = mas.Nombre,
                            Notas = cit.Notas
                        }).FirstOrDefault<Cita>();

            return cita;
        }

        // POST api/<CitasController>
        [HttpPost]
        public IActionResult Post([FromBody] Citas value)
        {
            bool error = false;

            if (string.IsNullOrWhiteSpace(value.Tipo)
                || value.RCliente == null
                || value.RMascota == null                
                || value.FechaHora == null
                )
            {
                error = true;
            }
            else
            {
                value.Notas = WebUtility.HtmlEncode(value.Notas);
                value.Tipo = WebUtility.HtmlEncode(value.Tipo);
                try
                {
                    var context = new veterinariaContext();
                    context.Citas.Add(value);
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

        // PUT api/<CitasController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Citas value)
        {
            bool error = false;

            if (string.IsNullOrWhiteSpace(value.Tipo)
                || value.RCliente == null
                || value.RMascota == null
                || value.FechaHora == null
                )
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Citas cita = (from cit in context.Citas
                                        where cit.IdCitas== id
                                        select cit).FirstOrDefault<Citas>();
                    if (cita == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    cita.RCliente = value.RCliente;
                    cita.FechaHora = value.FechaHora;
                    cita.Tipo = WebUtility.HtmlEncode(value.Tipo);
                    cita.RMascota = value.RMascota;
                    cita.Notas = WebUtility.HtmlEncode(value.Notas);
                    
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

        // DELETE api/<CitasController>/5
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
                    Citas cita = (from cit in context.Citas
                                  where cit.IdCitas == id
                                  select cit).FirstOrDefault<Citas>();

                    context.Citas.Remove(cita);
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

    public class Cita
    {
        public int IdCitas { get; set; }
        public int RCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaHora { get; set; }
        public string Tipo { get; set; }
        public int RMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Notas { get; set; }
    }
}
