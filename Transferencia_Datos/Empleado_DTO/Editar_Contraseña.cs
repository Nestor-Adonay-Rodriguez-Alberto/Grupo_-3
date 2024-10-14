using System;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Empleado_DTO
{
    public class Editar_Contraseña
    {
        // ATRIBUTOS:
        [Required]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Ingrese Una Contraseña Segura.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
