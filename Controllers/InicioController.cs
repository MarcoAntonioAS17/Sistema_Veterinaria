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
    public class InicioController : ControllerBase
    {
        // GET: api/<InicioController>
        [HttpGet]
        public IActionResult Get()
        {
            var context = new veterinariaContext();

            int contactos = 0;
            int productos = 0;
            int citas_vigentes = 0;
            float ventas_sem = 0;
            int prod_caducar = 0;
            int prod_agotarse = 0;

            contactos = (from clien in context.Clientes
                         select clien).Count();

            productos = (from pro in context.Productos
                         where pro.IdProductos.Length > 3
                         select pro).Count();

            citas_vigentes = (from cit in context.Citas
                              where cit.FechaHora > DateTime.Now
                              select cit).Count();

            ventas_sem = (float)(from det_ventas in context.DetalleVentas
                          join prod in context.Productos on det_ventas.RProducto equals prod.IdProductos
                          select prod.PrecioVenta * det_ventas.Cantidad).Sum();

            var config = (from conf in context.Configuracion
                          select conf).FirstOrDefault<Configuracion>();

            prod_caducar = (from prod in context.Productos
                            where prod.Caducidad < DateTime.Now.AddDays(config.DiasCaducidad)
                            select prod).Count();

            prod_agotarse = (from prod in context.Productos
                             where prod.Cantidad < config.CantidadInventario
                             select prod).Count();

            var Result = new
            {
                N_Contactos = contactos,
                N_Productos = productos,
                N_Cita = citas_vigentes,
                N_Prod_Cadu = prod_caducar,
                N_Prod_Ago = prod_agotarse,
                Ventas_Sem = ventas_sem
            };

            return new JsonResult(Result);
        }

        
    }
}
