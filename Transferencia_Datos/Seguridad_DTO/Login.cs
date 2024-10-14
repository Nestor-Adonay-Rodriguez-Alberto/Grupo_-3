using System;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Seguridad_DTO
{
    public class Login
    {
        // ATRIBUTOS
        [Required(ErrorMessage = "Ingrese Su Email.")]
        [EmailAddress(ErrorMessage = "¡Error!... Email Invalido.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Ingrese Su Contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
