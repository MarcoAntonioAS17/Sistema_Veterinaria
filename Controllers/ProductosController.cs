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
    public class ProductosController : ControllerBase
    {
        // GET: api/<ProductosController>
        [HttpGet]
        public IEnumerable<Producto> Get()
        {
            var context = new veterinariaContext();
            var producto = from pro in context.Productos
                           join prove in context.Proveedores on pro.RProveedor equals prove.IdProveedores
                           join cate in context.Categorias on pro.RCategoria equals cate.IdCategorias
                           where pro.IdProductos.Length > 4
                           orderby pro.IdProductos ascending
                           select new Producto
                           {
                               IdProductos = pro.IdProductos,
                               Nombre = pro.Nombre,
                               PrecioVenta = pro.PrecioVenta,
                               PrecioCompra = pro.PrecioCompra,
                               Caducidad = pro.Caducidad,
                               Cantidad = pro.Cantidad,
                               Descripcion = pro.Descripcion,
                               RCategoria = cate.IdCategorias,
                               RCategoriaNombre = cate.Nombre,
                               RProveedor = prove.IdProveedores,
                               RProveedorNombre = prove.ProveedorNombre

                           };

            return producto;
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public Producto Get(String id)
        {
            var context = new veterinariaContext();
            var producto = (from pro in context.Productos
                            join prove in context.Proveedores on pro.RProveedor equals prove.IdProveedores
                            join cate in context.Categorias on pro.RCategoria equals cate.IdCategorias
                            orderby pro.IdProductos ascending
                            where pro.IdProductos == id
                            select new Producto
                            {
                                IdProductos = pro.IdProductos,
                                Nombre = pro.Nombre,
                                PrecioVenta = pro.PrecioVenta,
                                PrecioCompra = pro.PrecioCompra,
                                Caducidad = pro.Caducidad,
                                Cantidad = pro.Cantidad,
                                Descripcion = pro.Descripcion,
                                RCategoria = cate.IdCategorias,
                                RCategoriaNombre = cate.Nombre,
                                RProveedor = prove.IdProveedores,
                                RProveedorNombre = prove.ProveedorNombre

                            }).FirstOrDefault<Producto>();

            return producto;
        }

        // POST api/<ProductosController>
        [HttpPost]
        public IActionResult Post([FromBody] Productos value)
        {
            bool error = false;

            if (validar_campos(value))
            {
                error = true;
            }
            else{
                value.IdProductos = WebUtility.HtmlEncode(value.IdProductos);
                value.Nombre = WebUtility.HtmlEncode(value.Nombre);
                value.Descripcion = WebUtility.HtmlEncode(value.Descripcion);

                try
                {
                    var context = new veterinariaContext();
                    context.Productos.Add(value);
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

        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(String id, [FromBody] Productos value)
        {
            bool error = false;
            if (validar_campos(value))
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Productos producto = (from pro in context.Productos
                                    join prove in context.Proveedores on pro.RProveedor equals prove.IdProveedores
                                    join cate in context.Categorias on pro.RCategoria equals cate.IdCategorias
                                    orderby pro.IdProductos ascending
                                    where pro.IdProductos == id
                                    select pro).FirstOrDefault<Productos>();

                    if (producto == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    producto.Nombre = WebUtility.HtmlEncode(value.Nombre);
                    producto.PrecioVenta = value.PrecioVenta;
                    producto.PrecioCompra = value.PrecioCompra;
                    producto.Cantidad = value.Cantidad;
                    producto.Caducidad = value.Caducidad;
                    producto.Descripcion = WebUtility.HtmlEncode(value.Descripcion);
                    producto.RCategoria = value.RCategoria;
                    producto.RProveedor = value.RProveedor;

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

        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            bool error = false;
            if (string.IsNullOrWhiteSpace(id))
            {
                error = true;
            }
            else
            {
                try
                {
                    var context = new veterinariaContext();
                    Productos producto = (from pro in context.Productos
                                    join prove in context.Proveedores on pro.RProveedor equals prove.IdProveedores
                                    join cate in context.Categorias on pro.RCategoria equals cate.IdCategorias
                                    orderby pro.IdProductos ascending
                                    where pro.IdProductos == id
                                    select pro).FirstOrDefault<Productos>();
                    if (producto == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    context.Productos.Remove(producto);
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

        bool validar_campos(Productos value)
        {
            bool error = false;
            Regex alfanumerico = new Regex("^[a-zA-Z0-9 ]*$");
            Regex nombre = new Regex("^[a-zA-Z0-9,. ]*$");

            if (!alfanumerico.IsMatch(value.IdProductos) || value.IdProductos.Length > 15 || string.IsNullOrWhiteSpace(value.IdProductos))
            {
                error = true;                
            }
            if (!alfanumerico.IsMatch(value.Nombre) || value.Nombre.Length > 45 || string.IsNullOrWhiteSpace(value.Nombre))
            {
                error = true;
            }
            if (value.PrecioVenta < 0 )
            {
                error = true;
            }
            if (value.PrecioCompra < 0)
            {
                error = true;
            }
            if (value.Cantidad < 0 )
            {
                error = true;
            }
            if (value.RCategoria < 1)
            {
                error = true;
            }
            if (value.RProveedor < 1)
            {
                error = true;
            }
            if (value.Descripcion.Length > 0)
                if (!nombre.IsMatch(value.Descripcion) || value.Descripcion.Length > 60)
                {
                    error = true;
                }
            return error;
        }
    }

    public class Producto
    {
        public string IdProductos { get; set; }
        public string Nombre { get; set; }
        public float PrecioVenta { get; set; }
        public float? PrecioCompra { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? Caducidad { get; set; }
        public string Descripcion { get; set; }
        public int? RCategoria { get; set; }
        public String RCategoriaNombre { get; set; }
        public int? RProveedor { get; set; }
        public String RProveedorNombre { get; set; }
    }
}
