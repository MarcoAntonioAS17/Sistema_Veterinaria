using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sistema_Veterinaria.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private veterinariaContext context;
        public CategoriasController(veterinariaContext ctx, IDataProtectionProvider provider)
        {
            this.context = ctx;
        }
        // GET: api/<CategoriasController>
        [HttpGet]
        public IEnumerable<Categorias> Get()
        {
            var categorias = context.Categorias.ToList();
            return categorias;
        }

        
        // POST api/<CategoriasController>
        [HttpPost]
        public IActionResult Post([FromBody] Categorias value)
        {
            bool error = false;

            if ( string.IsNullOrWhiteSpace(value.Nombre)) {
                error = true;
            }
            else {
                value.Nombre = WebUtility.HtmlEncode(value.Nombre);

                try {
                    context.Categorias.Add(value);
                    context.SaveChanges();
                }catch( Exception ex) {
                    error = true;
                    Console.WriteLine(ex);
                }
            }
            var Result = new { Status = !error ? "Success" : "Fail" };

            return new JsonResult(Result);

        }
        

        // DELETE api/<CategoriasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool error = false;

            if (id <= 0)
            {
                error = true;
            }
            else
            {
                try
                {
                    Categorias cate = (from cat in context.Categorias
                                        where cat.IdCategorias == id
                                        select cat).FirstOrDefault<Categorias>();

                    if (cate == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    context.Categorias.Remove(cate);

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
