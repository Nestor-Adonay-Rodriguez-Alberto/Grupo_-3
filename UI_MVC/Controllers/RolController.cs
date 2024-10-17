using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using Transferencia_Datos.Rol_DTO;

namespace UI_MVC.Controllers
{
    [Authorize]
    public class RolController : Controller
    {
        // Para Hacer Solicitudes Al Servidor:
        private readonly HttpClient _HttpClient;


        // Constructor: 
        public RolController(IHttpClientFactory httpClientFactory)
        {
            _HttpClient = httpClientFactory.CreateClient("API_RESTful");
        }




        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<IActionResult> Roles_Registrados()
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenidos = await _HttpClient.GetAsync("/api/Rol");

            // OBJETO:
            Registrados_Rol Lista_Roles = new Registrados_Rol();

            // True=200-299
            if (JSON_Obtenidos.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Lista_Roles = await JSON_Obtenidos.Content.ReadFromJsonAsync<Registrados_Rol>();
            }

            return View(Lista_Roles);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<IActionResult> Detalle_Rol(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Rol/" + id);

            // OBJETO:
            Obtener_Rol Objeto_Obtenido = new Obtener_Rol();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Rol>();
            }

            return View(Objeto_Obtenido);
        }





        // *******  METOFOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // NOS MANDA A LA VISTA:
        public ActionResult Registrar_Rol()
        {
            return View();
        }


        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar_Rol(Crear_Rol crear_Rol)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "POST" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PostAsJsonAsync("/api/Rol", crear_Rol);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Roles_Registrados", "Rol");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA
        public async Task<IActionResult> Editar_Rol(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Rol/" + id);

            // OBJETO:
            Obtener_Rol Objeto_Obtenido = new Obtener_Rol();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Rol>();
            }

            Editar_Rol Objeto_Editar = new Editar_Rol
            {
                IdRol = Objeto_Obtenido.IdRol,
                Nombre = Objeto_Obtenido.Nombre,
            };

            return View(Objeto_Editar);
        }


        // RECIBE EL OBJETO MODIFICADO Y LO MODIFICA EN DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar_Rol(Editar_Rol editar_Rol)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "PUT" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PutAsJsonAsync("/api/Rol", editar_Rol);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Roles_Registrados", "Rol");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA:
        public async Task<IActionResult> Eliminar_Rol(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Rol/" + id);

            // OBJETO:
            Obtener_Rol Objeto_Obtenido = new Obtener_Rol();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Rol>();
            }

            return View(Objeto_Obtenido);
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar_Rol(Obtener_Rol obtener_Rol)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "DELETE" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.DeleteAsync("/api/Rol/" + obtener_Rol.IdRol);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Roles_Registrados", "Rol");
            }

            return View();
        }


    }
}
