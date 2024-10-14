using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Empleado_DTO
{
    public class Crear_Empleado
    {
        // ATRIBUTOS:
        [Required(ErrorMessage = "Ingrese Un Nombre.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Ingrese Un Salario.")]
        [Column(TypeName = "decimal(18, 2)")]
        public double Salaraio { get; set; }


        [Required(ErrorMessage = "Ingrese Un Numero De Telefono.")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "Ingrese Un Email.")]
        [EmailAddress(ErrorMessage = "¡Error!... Email Invalido.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Ingrese Una Contraseña Segura.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        // Referencia Tabla Rol:  * RELACION *
        [Required]
        public int IdRolEnEmpleado { get; set; }

    }
}
