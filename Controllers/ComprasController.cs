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
    public class ComprasController : ControllerBase
    {
        // GET: api/<ComprasController>
        [HttpGet]
        public IEnumerable<Compra> Get()
        {
            var context = new veterinariaContext();
            var compras = from comp in context.Compras
                         join prove in context.Proveedores on comp.RProveedor equals prove.IdProveedores
                         join user in context.Usuarios on comp.RUsuario equals user.IdUser
                         orderby comp.IdCompras ascending
                         select new Compra
                         {
                             IdCompras = comp.IdCompras,
                             FechaHora = comp.FechaHora,
                             RProveedor = prove.IdProveedores,
                             RProveedorNombre = prove.ProveedorNombre,
                             RUsuario = user.IdUser,
                             RUsuarioNombre = user.UserName
                         };
            List<Compra> List_compras = compras.ToList<Compra>();

            for (int i = 0; i < List_compras.Count; i++)
            {
                var detalle_compra = from dcompra in context.DetalleCompras
                                     join prod in context.Productos on dcompra.RProducto equals prod.IdProductos
                                     orderby prod.Nombre ascending
                                     where dcompra.RCompra == List_compras[i].IdCompras
                                     select new DetalleCompra
                                     {
                                         Cantidad = (int)dcompra.Cantidad,
                                         Precio = (float)prod.PrecioCompra,
                                         RProducto = prod.IdProductos,
                                         RProductoNombre = prod.Nombre
                                     };
                List_compras[i].Productos = detalle_compra.ToList<DetalleCompra>();
            }
            return List_compras;
        }

        // GET api/<ComprasController>/5
        [HttpGet("{id}")]
        public Compra Get(int id)
        {
            var context = new veterinariaContext();
            Compra compra = (from comp in context.Compras
                             join prove in context.Proveedores on comp.RProveedor equals prove.IdProveedores
                             join user in context.Usuarios on comp.RUsuario equals user.IdUser
                             orderby comp.IdCompras ascending
                             where comp.IdCompras == id
                             select new Compra
                             {
                                 IdCompras = comp.IdCompras,
                                 FechaHora = comp.FechaHora,
                                 RProveedor = prove.IdProveedores,
                                 RProveedorNombre = prove.ProveedorNombre,
                                 RUsuario = user.IdUser, 
                                 RUsuarioNombre = user.UserName 
                             }).FirstOrDefault<Compra>();

            if (compra == null)
                return null;

            var detalle_compra = from dcompra in context.DetalleCompras
                                 join prod in context.Productos on dcompra.RProducto equals prod.IdProductos
                                 orderby prod.Nombre ascending
                                 where dcompra.RCompra == compra.IdCompras
                                 select new DetalleCompra
                                 {
                                     Cantidad = (int)dcompra.Cantidad,
                                     Precio = (float)prod.PrecioCompra,
                                     RProducto = prod.IdProductos,
                                     RProductoNombre = prod.Nombre
                                 };

            if (detalle_compra != null)
                compra.Productos = detalle_compra.ToList<DetalleCompra>();

            return compra;
        }

        // POST api/<ComprasController>
        [HttpPost]
        public IActionResult Post([FromBody] Compra value)
        {
            bool error = false;

            if (value.RProveedor == 0 || value.RUsuario == 0 || value.Productos.Count < 1)
            {
                error = true;
            }
            else
            {
                var context = new veterinariaContext();
                var transaccion = context.Database.BeginTransaction();
                try
                {
                    Compras compra = new Compras
                    {
                        RProveedor = value.RProveedor,
                        RUsuario = value.RUsuario,
                        FechaHora = value.FechaHora
                    };

                    context.Compras.Add(compra);
                    context.SaveChanges();

                    Compras id_compra = (from comp in context.Compras
                                         orderby comp.IdCompras descending
                                         select comp).FirstOrDefault<Compras>();

                    foreach (DetalleCompra detalle in value.Productos)
                    {
                        if (!(detalle.RProducto.Length < 4 || detalle.Cantidad < 1))
                        {
                            Productos productos = (from pro in context.Productos
                                                   where pro.IdProductos == detalle.RProducto
                                                   select pro).FirstOrDefault<Productos>();
                            productos.Cantidad = productos.Cantidad + detalle.Cantidad;
                            context.SaveChanges();
                        }

                        DetalleCompras nueva_detalleCompras = new DetalleCompras
                        {
                            RCompra = id_compra.IdCompras,
                            RProducto = detalle.RProducto,
                            Cantidad = detalle.Cantidad
                        };
                        context.DetalleCompras.Add(nueva_detalleCompras);
                        context.SaveChanges();
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

        // DELETE api/<ComprasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool error = false;

            var context = new veterinariaContext();
            var transaccion = context.Database.BeginTransaction();
            try
            {
                Compras compras = (from comp in context.Compras
                                   where comp.IdCompras == id
                                   select comp).FirstOrDefault<Compras>();

                if (compras == null)
                {
                    return new JsonResult(new { Status = "Fail" });
                }
                List<DetalleCompras> dcompras = (from dcompra in context.DetalleCompras
                                                where dcompra.RCompra == compras.IdCompras
                                                select dcompra).ToList<DetalleCompras>();

                foreach(DetalleCompras elem in dcompras)
                {
                    Productos productos = (from pro in context.Productos
                                           where pro.IdProductos == elem.RProducto
                                           select pro).FirstOrDefault<Productos>();
                    productos.Cantidad = productos.Cantidad - elem.Cantidad;
                    context.SaveChanges();
                }
                context.Compras.Remove(compras);

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
}

public class Compra
{
    public int IdCompras { get; set; }
    public int RProveedor { get; set; }
    public string RProveedorNombre { get; set; }
    public DateTime FechaHora { get; set; }
    public int RUsuario { get; set; }
    public string RUsuarioNombre { get; set; }
    public List<DetalleCompra> Productos { get; set; }
}

public class DetalleCompra
{
    public int Cantidad { get; set; }
    public string RProducto { get; set; }
    public float Precio { get; set; }
    public string RProductoNombre { get; set; }
}