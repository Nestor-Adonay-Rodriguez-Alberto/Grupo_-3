using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Transferencia_Datos.Seguridad_DTO;

namespace UI_MVC.Controllers
{
    public class AutenticacionController : Controller
    {
        // PARA HACER SOLICITUDES A LA API:
        private readonly HttpClient _HttpClient;


        // CONSTRUCTOR:
        public AutenticacionController(IHttpClientFactory httpClientFactory)
        {
            _HttpClient = httpClientFactory.CreateClient("API_RESTful");
        }



        // NOS MANDA A LA VISTA DE INICIAR SESION:
        public async Task<IActionResult> Login()
        {
            // Cierra Toda Sesion Existente:
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            // Solicitud GET al Endpoint de la API:
            HttpResponseMessage Respuesta = await _HttpClient.PostAsJsonAsync("/api/Autenticacion/Login", login);

            // OBJETO:
            Autenticado? Objeto_Obtenido = new Autenticado();

            // True=200-299
            if (Respuesta.IsSuccessStatusCode)
            {
                // Deserializamos el Json:
                Objeto_Obtenido = await Respuesta.Content.ReadFromJsonAsync<Autenticado>();
            }

            // HACEMOS LA AUTENTICACION Y GUARDAMOS DATOS:
            if (Objeto_Obtenido.Token_Seguro != null)
            {
                // Identidad Del Autenticado: 
                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, Objeto_Obtenido.Nombre),
                    new Claim(ClaimTypes.Role, Objeto_Obtenido.Objeto_Rol.Nombre),
                    new Claim("Token_Obtenido", Objeto_Obtenido.Token_Seguro),
                    new Claim("IdEmpleado", Objeto_Obtenido.IdEmpleado.ToString())
                };

                // Para Autorizaciones:
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Inicia Sesion Del Usuario Actual:
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });


                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "¡Error!... Credenciales Incorrectas.";
            return View(login);

        }

    }
}
