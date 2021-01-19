using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sistema_Veterinaria.Models;
using Microsoft.AspNetCore.DataProtection;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Veterinaria.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private veterinariaContext context;
        public ConfigController(veterinariaContext ctx, IDataProtectionProvider provider)
        {
            this.context = ctx;
        }

        // GET api/<ConfigController>/5
        [HttpGet]
        public Configuracion Get()
        {
         
            Configuracion configuracion = (from config in context.Configuracion
                                           where config.Clave == 1
                                           select config).FirstOrDefault<Configuracion>();
            if (configuracion == null)
            {
                Configuracion config_ini = new Configuracion
                {
                    CantidadInventario = 5,
                    DiasCaducidad = 60
                };

                context.Configuracion.Add(config_ini);
                context.SaveChanges();

                config_ini.Clave = 1;

                return config_ini;
            }
            
            return configuracion;
        }

        // PUT api/<ConfigController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Configuracion value)
        {
            bool error = false;

            if (value.CantidadInventario < 1 || value.DiasCaducidad < 1)
            {
                error = true;
            }
            else
            {
                try
                {
                   
                    Configuracion configuracion = (from config in context.Configuracion
                                                   where config.Clave == 1
                                                   select config).FirstOrDefault<Configuracion>();
                    if (configuracion == null)
                    {
                        return new JsonResult(new { Status = "Fail" });
                    }
                    configuracion.CantidadInventario = value.CantidadInventario;
                    configuracion.DiasCaducidad = value.DiasCaducidad;
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
