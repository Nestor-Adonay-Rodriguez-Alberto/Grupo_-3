using System;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Rol_DTO
{
    public class Editar_Rol
    {
        // ATRIBUTOS:
        [Required]
        public int IdRol { get; set; }


        [Required(ErrorMessage = "Ingrese Un Nombre Al Rol.")]
        public string Nombre { get; set; }

    }
}
