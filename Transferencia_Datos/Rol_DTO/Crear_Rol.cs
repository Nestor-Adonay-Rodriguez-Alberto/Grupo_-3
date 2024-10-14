using System;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Rol_DTO
{
    public class Crear_Rol
    {
        // ATRIBUTOS:
        [Required(ErrorMessage = "Ingrese Un Nombre Al Rol.")]
        public string Nombre { get; set; }

    }
}
