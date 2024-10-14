using API_RESTful.Models;
using API_RESTful.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferencia_Datos.Rol_DTO;


namespace API_RESTful.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        // REPRESENTA LA DB:
        private readonly Servicios_Rol _Servicios_Rol;


        // CONSTRUCTOR:
        public RolController(Servicios_Rol servicios_Rol)
        {
            _Servicios_Rol = servicios_Rol;
        }




        // **************** ENDPOINTS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        [HttpGet]
        public async Task<IActionResult> Obtner_Todos()
        {
            List<Rol> Lista_Roles = await _Servicios_Rol.Obtner_Todos();

            // DTO a retornar:
            Registrados_Rol Roles_Registrados = new Registrados_Rol();

            foreach (Rol rol in Lista_Roles)
            {
                Roles_Registrados.Lista_Roles.Add(new Registrados_Rol.Rol
                {
                    IdRol = rol.IdRol,
                    Nombre = rol.Nombre
                });
            }

            return Ok(Roles_Registrados);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener_PorId(int id)
        {
            Obtener_Rol? Objeto_Obtenido = await _Servicios_Rol.Obtener_PorId(id);

            if (Objeto_Obtenido == null)
            {
                return NotFound("Registro No Existente.");
            }

            return Ok(Objeto_Obtenido);
        }





        // *******  ENPOINTS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        public async Task<IActionResult> Registrar_Rol([FromBody] Crear_Rol crear_Rol)
        {
            int Respuesta = await _Servicios_Rol.Registrar_Rol(crear_Rol);
            if (Respuesta > 0)
            {
                return Ok("Rol Guardado Correctamente");
            }
            else
            {
                return NotFound("Error Al Guardar el Rol.");
            }
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        [HttpPut]
        public async Task<IActionResult> Editar_Rol([FromBody] Editar_Rol editar_Rol)
        {
            int Respuesta = await _Servicios_Rol.Editar_Rol(editar_Rol);
            if (Respuesta > 0)
            {
                return Ok("Rol Editado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar_Rol(int id)
        {
            int Respuesta = await _Servicios_Rol.Eliminar_Rol(id);
            if (Respuesta > 0)
            {
                return Ok("Rol Eliminado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }

    }
}
