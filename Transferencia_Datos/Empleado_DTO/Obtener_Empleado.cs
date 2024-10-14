using System;
using static Transferencia_Datos.Rol_DTO.Registrados_Rol;

namespace Transferencia_Datos.Empleado_DTO
{
    public class Obtener_Empleado
    {
        // ATRIBUTOS:
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }

        public double Salaraio { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }



        // Referencia Tabla Rol:  * RELACION *
        public int IdRolEnEmpleado { get; set; }
        public virtual Rol? Objeto_Rol { get; set; }


    }
}
