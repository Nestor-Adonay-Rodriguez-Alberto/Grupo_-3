using API_RESTful.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Transferencia_Datos.Rol_DTO;
using Transferencia_Datos.Seguridad_DTO;


namespace API_RESTful.Servicios
{
    public class Servicios_De_Autenticacion
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;

        // Para Acceder a la Key:
        private IConfiguration _Configuration;


        // Constructor:
        public Servicios_De_Autenticacion(MyDBcontext myDBcontext, IConfiguration configuration)
        {
            _MyDBcontext = myDBcontext;
            _Configuration = configuration;
        }



        // BUSCA UN REGISTRO CON LAS MISMAS CREDENCIALES:
        public async Task<Autenticado> Login(Login login)
        {
            // Registros con el mismo Email:
            List<Empleado> Empleado_Encontrados = await _MyDBcontext.Empleados
                                                 .Include(x => x.Objeto_Rol)
                                                 .Where(x => x.Email == login.Email)
                                                 .ToListAsync();


            // Verificamos la Contraseña:
            foreach (Empleado empleado in Empleado_Encontrados)
            {
                // True sin son iguales:
                bool Password_Iguales = BCrypt.Net.BCrypt.Verify(login.Password, empleado.Password);

                if (Password_Iguales)
                {
                    // Objeto a Mandar:
                    Autenticado Empleado_Autenticado = new Autenticado
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        Nombre = empleado.Nombre,
                        Salaraio = empleado.Salaraio,
                        Telefono = empleado.Telefono,
                        Email = empleado.Email,
                        Password = empleado.Password,
                        IdRolEnEmpleado = empleado.IdRolEnEmpleado,
                        Objeto_Rol = new Registrados_Rol.Rol
                        {
                            IdRol = empleado.Objeto_Rol.IdRol,
                            Nombre = empleado.Objeto_Rol.Nombre
                        },

                        //Creacion Del Token:
                        Token_Seguro = Generar_Token(empleado)
                    };

                    return Empleado_Autenticado;
                }
            }

            return null;
        }


        // METODO PARA CREAR UN TOKEN FIRMADO:
        private string Generar_Token(Empleado empleado)
        {
            // DATOS DEL EMPLEADO:
            Claim[] Claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, empleado.Nombre),
                new Claim(ClaimTypes.Email, empleado.Email),
                new Claim("Rol", empleado.Objeto_Rol.Nombre),
            };

            // OBTENGO EN BYTES LA KEY SECRETA:
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:Key"]));

            // ENCRIPTAMOS LA KEY:
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            // CREAMOS EL TOKEN:
            JwtSecurityToken Token_Seguro = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: Creds
                );

            // Formatemos el Token:
            string Token = new JwtSecurityTokenHandler().WriteToken(Token_Seguro);

            return Token;
        }

    }
}
