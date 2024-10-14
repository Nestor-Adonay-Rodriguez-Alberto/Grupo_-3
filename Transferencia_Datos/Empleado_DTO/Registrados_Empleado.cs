using System;
using static Transferencia_Datos.Rol_DTO.Registrados_Rol;


namespace Transferencia_Datos.Empleado_DTO
{
    public class Registrados_Empleado
    {
        // CLASE:
        public class Empleado
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

        // ALMACENA TODOS LOS EMPLEADOS DE LA DB:
        public List<Empleado> Lista_Empleados { get; set; }


        // CONSTRUCTOR:
        public Registrados_Empleado()
        {
            Lista_Empleados = new List<Empleado>();
        }

    }
}
