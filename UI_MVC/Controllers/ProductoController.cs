using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using Transferencia_Datos.Producto_DTO;

namespace UI_MVC.Controllers
{

    [Authorize]
    public class ProductoController : Controller
    {
        // Para Hacer Solicitudes Al Servidor:
        private readonly HttpClient _HttpClient;


        // Constructor: 
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _HttpClient = httpClientFactory.CreateClient("API_RESTful");
        }




        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<IActionResult> Productos_Registrados()
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenidos = await _HttpClient.GetAsync("/api/Producto");

            // OBJETO:
            Registrados_Producto Lista_Productos = new Registrados_Producto();

            // True=200-299
            if (JSON_Obtenidos.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Lista_Productos = await JSON_Obtenidos.Content.ReadFromJsonAsync<Registrados_Producto>();
            }

            return View(Lista_Productos);
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<IActionResult> Detalle_Producto(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Producto/" + id);

            // OBJETO:
            Obtener_Producto Objeto_Obtenido = new Obtener_Producto();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Producto>();
            }

            return View(Objeto_Obtenido);
        }





        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // NOS MANDA A LA VISTA:
        public ActionResult Registrar_Producto()
        {
            return View();
        }


        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar_Producto(Crear_Producto crear_Producto, IFormFile Fotografia)
        {
            // Convirtiendo el archivo a Arreglo De Bytes:
            if (Fotografia != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Fotografia.CopyTo(memoryStream);

                    crear_Producto.Fotografia = memoryStream.ToArray();
                }
            }

            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "POST" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.PostAsJsonAsync("/api/Producto", crear_Producto);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Productos_Registrados", "Producto");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA
        public async Task<IActionResult> Editar_Producto(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Producto/" + id);

            // OBJETO:
            Obtener_Producto Objeto_Obtenido = new Obtener_Producto();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Producto>();
            }

            Editar_Producto Objeto_Editar = new Editar_Producto
            {
                IdProducto = Objeto_Obtenido.IdProducto,
                Nombre = Objeto_Obtenido.Nombre,
                Precio=Objeto_Obtenido.Precio,
                Fotografia=Objeto_Obtenido.Fotografia
            };

            return View(Objeto_Editar);
        }


        // RECIBE EL OBJETO MODIFICADO Y LO MODIFICA EN DB:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar_Producto(Editar_Producto editar_Producto, IFormFile Fotografia)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Producto/" + editar_Producto.IdProducto);

            // OBJETO:
            Obtener_Producto Objeto_Obtenido = new Obtener_Producto();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Producto>();
            }

            // Convirtiendo el archivo a Arreglo De Bytes:
            if (Fotografia != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Fotografia.CopyTo(memoryStream);

                    editar_Producto.Fotografia = memoryStream.ToArray();
                }
            }
            else
            {
                editar_Producto.Fotografia = Objeto_Obtenido.Fotografia;
            }

            // Solicitud "PUT" al Endpoint de la API:
            HttpResponseMessage Respuesta = await _HttpClient.PutAsJsonAsync("/api/Producto", editar_Producto);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Productos_Registrados", "Producto");
            }

            return View();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MANDA A VISTA:
        public async Task<IActionResult> Eliminar_Producto(int id)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "GET" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage JSON_Obtenido = await _HttpClient.GetAsync("/api/Producto/" + id);

            // OBJETO:
            Obtener_Producto Objeto_Obtenido = new Obtener_Producto();

            // True=200-299
            if (JSON_Obtenido.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await JSON_Obtenido.Content.ReadFromJsonAsync<Obtener_Producto>();
            }

            return View(Objeto_Obtenido);
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar_Producto(Obtener_Producto obtener_Producto)
        {
            // Token Obtenido al Iniciar Sesion:
            string Token_Obtenido = User.FindFirstValue("Token_Obtenido");

            // Solicitud "DELETE" al Endpoint de la API Con Su Token:
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token_Obtenido);
            HttpResponseMessage Respuesta = await _HttpClient.DeleteAsync("/api/Producto/" + obtener_Producto.IdProducto);

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Volemos a Vista Principal:
                return RedirectToAction("Productos_Registrados", "Producto");
            }

            return View();
        }


    }
}
