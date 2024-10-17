using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using Transferencia_Datos.Empleado_DTO;
using Transferencia_Datos.Rol_DTO;

namespace UI_MVC.Controllers
{
    [Authorize]
    public class EmpleadoController : Controller
    {
        // Para Hacer Solicitudes Al Servidor:
        private readonly HttpClient _HttpClient;


        // Constructor:
        public EmpleadoController(IHttpClientFactory httpClientFactory)
        {
            _HttpClient = httpClientFactory.CreateClient("API_RESTful");
        }




        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<IActionResult> Empleados_Registrados()
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenidos = await _HttpClient.GetAsync("/api/Empleado");

            // OBJETO:
            Registrados_Empleado Lista_Empleados = new Registrados_Empleado();

            // True=200-299
            if (JSON_Obtenidos.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Lista_Empleados = await JSON_Obtenidos.Content.ReadFromJsonAsync<Registrados_Empleado>();
            }

            return View(Lista_Empleados);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<IActionResult> Detalle_Empleado(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Empleado/" + id);

            // OBJETO:
            Obtener_Empleado Objeto_Obtenido = new Obtener_Empleado();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Empleado>();
            }

            return View(Objeto_Obtenido);
        }


        // OBTIENE TODOS LOS REGISTROS Rol DE LA DB Para ViewData:
        private async Task<Registrados_Rol> Lista_Roles()
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenidos = await _HttpClient.GetAsync("/api/Empleado/Roles_Registrados");

            // OBJETO:
            Registrados_Rol Lista_Roles = new Registrados_Rol();

            // True=200-299
            if (JSON_Obtenidos.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Lista_Roles = await JSON_Obtenidos.Content.ReadFromJsonAsync<Registrados_Rol>();
            }


            return Lista_Roles;
        }





        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // OBTIENE LOS ROLES Y LOS MANDA EN UN VIEWDATA:
        public async Task<IActionResult> Registrar_Empleado()
        {
            Registrados_Rol Objeto_Obtenido = await Lista_Roles();
            ViewData["Lista_Roles"] = new SelectList(Objeto_Obtenido.Lista_Roles, "IdRol", "Nombre");

            return View();
        }


        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar_Empleado(Crear_Empleado crear_Empleado)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "POST" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PostAsJsonAsync("/api/Empleado", crear_Empleado);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Empleados_Registrados", "Empleado");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA
        public async Task<IActionResult> Editar_Empleado(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Empleado/" + id);

            // OBJETO:
            Obtener_Empleado Objeto_Obtenido = new Obtener_Empleado();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Empleado>();
            }

            Editar_Empleado Objeto_Editar = new Editar_Empleado
            {
                IdEmpleado = Objeto_Obtenido.IdEmpleado,
                Nombre = Objeto_Obtenido.Nombre,
                Salaraio = Objeto_Obtenido.Salaraio,
                Telefono = Objeto_Obtenido.Telefono,
                Email = Objeto_Obtenido.Email,
                IdRolEnEmpleado = Objeto_Obtenido.IdRolEnEmpleado
            };

            // Para Seleccionar Roles:
            Registrados_Rol Objeto_Rol = await Lista_Roles();
            ViewData["Lista_Roles"] = new SelectList(Objeto_Rol.Lista_Roles, "IdRol", "Nombre", Objeto_Editar.IdRolEnEmpleado);

            return View(Objeto_Editar);
        }


        // RECIBE EL OBJETO MODIFICADO Y LO MODIFICA EN DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar_Empleado(Editar_Empleado editar_Empleado)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "PUT" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PutAsJsonAsync("/api/Empleado", editar_Empleado);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Empleados_Registrados", "Empleado");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA:
        public async Task<IActionResult> Eliminar_Empleado(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Empleado/" + id);

            // OBJETO:
            Obtener_Empleado Objeto_Obtenido = new Obtener_Empleado();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Empleado>();
            }

            return View(Objeto_Obtenido);
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar_Empleado(Obtener_Empleado obtener_Empleado)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "DELETE" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.DeleteAsync("/api/Empleado/" + obtener_Empleado.IdEmpleado);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Empleados_Registrados", "Empleado");
            }

            return View();
        }





        // *******  METODOS PARA PERFIL  *******
        // *************************************

        // BUSA UN REGISTRO CON EL MISMO ID EN LA DB:
        public async Task<IActionResult> Perfil()
        {
            // Id Obtenido al Iniciar Sesion:
            int IdEmpleado = Convert.ToInt32(User.FindFirstValue("IdEmpleado"));

            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Empleado/" + IdEmpleado);

            // OBJETO:
            Obtener_Empleado Objeto_Obtenido = new Obtener_Empleado();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Empleado>();
            }

            return View(Objeto_Obtenido);
        }


        // NOS MANDA A LA VISTA:
        public async Task<IActionResult> Cambiar_Password()
        {
            // Id Obtenido al Iniciar Sesion:
            int IdEmpleado = Convert.ToInt32(User.FindFirstValue("IdEmpleado"));

            Editar_Contraseña Objeto_Editar = new Editar_Contraseña
            {
                IdEmpleado=IdEmpleado,
            };

            return View(Objeto_Editar);
        }


        // BUSCA UN REGISTRO EN LA DB Y MODIFICA SU CONTRASEÑA:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cambiar_Password(Editar_Contraseña editar_Contraseña)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "DELETE" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PutAsJsonAsync("/api/Empleado/Editar_Contraseña", editar_Contraseña);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Login", "Autenticacion");
            }

            return View();
        }

    }
}
