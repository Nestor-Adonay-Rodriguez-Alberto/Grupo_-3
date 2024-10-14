using API_RESTful.Servicios;
using Microsoft.AspNetCore.Mvc;
using Transferencia_Datos.Seguridad_DTO;


namespace API_RESTful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        // REPRESENTA LA DB:
        private readonly Servicios_De_Autenticacion _Autenticacion;


        // CONSTRUCTOR:
        public AutenticacionController(Servicios_De_Autenticacion autenticacion)
        {
            _Autenticacion = autenticacion;
        }



        // BUSCA UN REGISTRO CON LAS MISMAS CREDENCIALES:
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            Autenticado? Empleado_Autenticado = await _Autenticacion.Login(login);

            if (Empleado_Autenticado != null)
            {
                return Ok(Empleado_Autenticado);
            }

            return NotFound("Credenciales Incorrectas.");
        }


    }
}
