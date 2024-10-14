using API_RESTful.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferencia_Datos.Empleado_DTO;
using Transferencia_Datos.Rol_DTO;


namespace API_RESTful.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        // REPRESENTA LA DB:
        private readonly Servicios_Empleado _Servicios_Empleado;


        // CONSTRUCTOR:
        public EmpleadoController(Servicios_Empleado servicios_Empleado)
        {
            _Servicios_Empleado = servicios_Empleado;
        }



        // **************** ENDPOINTS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        [HttpGet]
        public async Task<IActionResult> Obtner_Todos()
        {
            // DTO a Retornar:
            Registrados_Empleado Empleados_Registrados = await _Servicios_Empleado.Obtner_Todos();

            return Ok(Empleados_Registrados);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener_PorId(int id)
        {
            Obtener_Empleado? Objeto_Obtenido = await _Servicios_Empleado.Obtener_PorId(id);

            if (Objeto_Obtenido == null)
            {
                return NotFound("Registro No Existente.");
            }

            return Ok(Objeto_Obtenido);
        }


        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        [HttpGet("Roles_Registrados")]
        public async Task<IActionResult> Roles_Registrados()
        {
            // DTO a Retornar:
            Registrados_Rol Roles_Registrados = await _Servicios_Empleado.Roles_Registrados();

            return Ok(Roles_Registrados);
        }





        // *******  ENPOINTS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        public async Task<IActionResult> Registrar_Empleado([FromBody] Crear_Empleado crear_Empleado)
        {
            int Respuesta = await _Servicios_Empleado.Registrar_Empleado(crear_Empleado);
            if (Respuesta > 0)
            {
                return Ok("Empleado Guardado Correctamente");
            }
            else
            {
                return NotFound("Error Al Guardar el Empleado.");
            }
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        [HttpPut]
        public async Task<IActionResult> Editar_Empleado([FromBody] Editar_Empleado editar_Empleado)
        {
            int Respuesta = await _Servicios_Empleado.Editar_Empleado(editar_Empleado);
            if (Respuesta > 0)
            {
                return Ok("Empleado Editado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar_Empleado(int id)
        {
            int Respuesta = await _Servicios_Empleado.Eliminar_Empleado(id);
            if (Respuesta > 0)
            {
                return Ok("Empleado Eliminado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        [HttpPut("Editar_Contraseña")]
        public async Task<IActionResult> Editar_Contraseña([FromBody] Editar_Contraseña editar_Contraseña)
        {
            int Respuesta = await _Servicios_Empleado.Editar_Contraseña(editar_Contraseña);
            if (Respuesta > 0)
            {
                return Ok("Contraseña Del Empleado Editada Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }


    }
}
