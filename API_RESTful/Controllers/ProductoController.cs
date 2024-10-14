using API_RESTful.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transferencia_Datos.Producto_DTO;


namespace API_RESTful.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        // REPRESENTA LA DB:
        private readonly Servicios_Producto _Servicios_Producto;


        // CONSTRUCTOR:
        public ProductoController(Servicios_Producto servicios_Producto)
        {
            _Servicios_Producto = servicios_Producto;
        }



        // **************** ENDPOINTS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        [HttpGet]
        public async Task<IActionResult> Obtner_Todos()
        {
            Registrados_Producto Lista_Productos = await _Servicios_Producto.Obtner_Todos();

            return Ok(Lista_Productos);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener_PorId(int id)
        {
            Obtener_Producto? Objeto_Obtenido = await _Servicios_Producto.Obtener_PorId(id);

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
        public async Task<IActionResult> Registrar_Producto([FromBody] Crear_Producto crear_Producto)
        {
            int Respuesta = await _Servicios_Producto.Registrar_Producto(crear_Producto);
            if (Respuesta > 0)
            {
                return Ok("Producto Guardado Correctamente");
            }
            else
            {
                return NotFound("Error Al Guardar el Producto.");
            }
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        [HttpPut]
        public async Task<IActionResult> Editar_Producto([FromBody] Editar_Producto editar_Producto)
        {
            int Respuesta = await _Servicios_Producto.Editar_Producto(editar_Producto);
            if (Respuesta > 0)
            {
                return Ok("Producto Editado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar_Producto(int id)
        {
            int Respuesta = await _Servicios_Producto.Eliminar_Producto(id);
            if (Respuesta > 0)
            {
                return Ok("Producto Eliminado Correctamente");
            }
            else
            {
                return NotFound("Error El Registro No Existe.");
            }

        }
    }
}
