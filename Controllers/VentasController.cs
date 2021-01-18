using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sistema_Veterinaria.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        // GET: api/<VentasController>
        [HttpGet]
        public IEnumerable<Venta> Get()
        {
            var context = new veterinariaContext();
            var ventas = from vent in context.Ventas
                         join cli in context.Clientes on vent.RCliente equals cli.IdClientes
                         join user in context.Usuarios on vent.RUsuario equals user.IdUser
                         orderby vent.IdVentas ascending
                         select new Venta
                         {
                             IdVentas = vent.IdVentas,
                             FechaHora = vent.FechaHora,
                             RCliente = cli.IdClientes,
                             RClienteNombre = cli.Nombre,
                             RUsuario = user.IdUser,
                             RUsuarioNombre = user.UserName
                         };

            List<Venta> List_ventas = ventas.ToList<Venta>();

            for (int i = 0; i < List_ventas.Count; i++)
            {
                var detalle_venta = from dventa in context.DetalleVentas
                                    join prod in context.Productos on dventa.RProducto equals prod.IdProductos
                                    orderby prod.Nombre ascending
                                    where dventa.RVenta == List_ventas[i].IdVentas
                                    select new DetalleVenta
                                    {
                                        Cantidad = (int)dventa.Cantidad,
                                        RProducto = prod.IdProductos,
                                        RProductoNombre = prod.Nombre,
                                        Precio = prod.PrecioVenta
                                    };
                List_ventas[i].Productos = detalle_venta.ToList<DetalleVenta>();
            }

            return List_ventas;
        }

        // GET api/<VentasController>/5
        [HttpGet("{id}")]
        public Venta Get(int id)
        {
            var context = new veterinariaContext();
            Venta ventas = (from vent in context.Ventas
                            join cli in context.Clientes on vent.RCliente equals cli.IdClientes
                            join user in context.Usuarios on vent.RUsuario equals user.IdUser
                            orderby vent.IdVentas ascending
                            where vent.IdVentas == id
                            select new Venta
                            {
                                IdVentas = vent.IdVentas,
                                FechaHora = vent.FechaHora,
                                RCliente = cli.IdClientes,
                                RClienteNombre = cli.Nombre,
                                RUsuario = user.IdUser,
                                RUsuarioNombre = user.UserName
                            }).FirstOrDefault<Venta>();

            if (ventas == null)
                return null;

            var detalle_venta = from dventa in context.DetalleVentas
                                join prod in context.Productos on dventa.RProducto equals prod.IdProductos
                                orderby prod.Nombre ascending
                                where dventa.RVenta == ventas.IdVentas
                                select new DetalleVenta
                                {
                                    Cantidad = (int)dventa.Cantidad,
                                    RProducto = prod.IdProductos,
                                    RProductoNombre = prod.Nombre,
                                    Precio = prod.PrecioVenta
                                };

            if (detalle_venta != null)
                ventas.Productos = detalle_venta.ToList<DetalleVenta>();

            return ventas;
        }

        // POST api/<VentasController>
        [HttpPost]
        public IActionResult Post([FromBody] Venta value)
        {
            bool error = false;

            if (value.RCliente == 0 || value.RUsuario == 0 || value.Productos.Count < 1)
            {
                error = true;
            }
            else
            {
                var context = new veterinariaContext();
                var transaccion = context.Database.BeginTransaction();
                try
                {
                    Ventas venta = new Ventas
                    {
                        RCliente = value.RCliente,
                        RUsuario = value.RUsuario,
                        FechaHora = value.FechaHora
                    };
                    context.Ventas.Add(venta);

                    context.SaveChanges();

                    Ventas id_venta = (from vent in context.Ventas
                                       orderby vent.IdVentas descending
                                       select vent).FirstOrDefault<Ventas>();

                    foreach (DetalleVenta detalleVenta in value.Productos)
                    {
                        if (string.IsNullOrWhiteSpace(detalleVenta.RProducto))
                        {
                            Productos servicio = (from pro in context.Productos
                                                  orderby pro.IdProductos descending
                                                  where pro.Nombre == detalleVenta.RProductoNombre && pro.PrecioVenta == detalleVenta.Precio 
                                                  select pro).FirstOrDefault<Productos>();

                            if (servicio == null)
                            {
                                Productos producto = (from pro in context.Productos
                                                      orderby pro.IdProductos descending
                                                      where pro.IdProductos.Length < 4
                                                      select pro).FirstOrDefault<Productos>();
                                int id;
                                if (producto == null)
                                    id = 1;
                                else
                                    id = Int32.Parse(producto.IdProductos) + 1;
                                //Nuevo Servicio
                                servicio = new Productos
                                {
                                    IdProductos = id.ToString(),
                                    Nombre = WebUtility.HtmlEncode(detalleVenta.RProductoNombre),
                                    PrecioVenta = detalleVenta.Precio
                                };

                                context.Productos.Add(servicio);
                                context.SaveChanges();

                            }


                            DetalleVentas detalleVentas = new DetalleVentas()
                            {
                                RProducto = servicio.IdProductos,
                                RVenta = id_venta.IdVentas,
                                Cantidad = detalleVenta.Cantidad
                            };

                            context.DetalleVentas.Add(detalleVentas);
                            context.SaveChanges();

                        }
                        else
                        {
                            if (detalleVenta.RProducto.Length > 4)
                            {
                                Productos productos = (from pro in context.Productos
                                                       where pro.IdProductos == detalleVenta.RProducto
                                                       select pro).FirstOrDefault<Productos>();

                                int invt = (int)(productos.Cantidad - detalleVenta.Cantidad);
                                if (invt < 0)
                                {
                                    transaccion.Dispose();
                                    return new JsonResult(new { Status = "Fail" });
                                }
                                productos.Cantidad = invt;
                                context.SaveChanges();
                            }

                            DetalleVentas nueva_detalleVentas = new DetalleVentas
                            {
                                RProducto = detalleVenta.RProducto,
                                RVenta = id_venta.IdVentas,
                                Cantidad = detalleVenta.Cantidad
                            };

                            context.DetalleVentas.Add(nueva_detalleVentas);
                            context.SaveChanges();

                        }
                    }

                    context.SaveChanges();
                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error ocurrido.");
                    Console.WriteLine(ex.Message);
                    error = true;
                }
                transaccion.Dispose();


            }

            var Result = new { Status = !error ? "Success" : "Fail" };
            return new JsonResult(Result);

        }

        // DELETE api/<VentasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool error = false;

            var context = new veterinariaContext();
            var transaccion = context.Database.BeginTransaction();
            try
            {   
                Ventas ventas = (from vent in context.Ventas
                                 where vent.IdVentas == id
                                 select vent).FirstOrDefault<Ventas>();

                if (ventas == null)
                {
                    return new JsonResult(new { Status = "Fail" });
                }

                List<DetalleVentas> dventas = (from dventa in context.DetalleVentas
                              where dventa.RVenta == ventas.IdVentas
                              select dventa).ToList<DetalleVentas>();

                foreach(DetalleVentas elem in dventas)
                {
                    if (elem.RProducto.Length > 3)
                    {
                        Productos producto = (from pro in context.Productos
                                              where pro.IdProductos == elem.RProducto
                                              select pro).FirstOrDefault<Productos>();
                        producto.Cantidad = producto.Cantidad + elem.Cantidad;

                        context.SaveChanges();

                    }
                }
                context.Ventas.Remove(ventas);

                context.SaveChanges();
                transaccion.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ocurrido.");
                Console.WriteLine(ex.Message);
                error = true;
            }
            transaccion.Dispose();

            var Result = new { Status = !error ? "Success" : "Fail" };

            return new JsonResult(Result);

        }
    }

    public class Venta
    {
        public int IdVentas { get; set; }
        public int RCliente { get; set; }
        public string RClienteNombre { get; set; }
        public DateTime FechaHora { get; set; }
        public int RUsuario { get; set; }
        public string RUsuarioNombre { get; set; }
        public List<DetalleVenta> Productos { get; set; }

    }

    public class DetalleVenta
    {
        public int Cantidad { get; set; }
        public string RProducto { get; set; }
        public float Precio { get; set; }
        public string RProductoNombre { get; set; }
    }
}
