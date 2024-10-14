using System;


namespace Transferencia_Datos.Rol_DTO
{
    public class Registrados_Rol
    {
        // CLASE:
        public class Rol
        {
            // ATRIBUTOS:
            public int IdRol { get; set; }

            public string Nombre { get; set; }

        }

        // ALMACENA TODOS LOS ROLES DE LA DB:
        public List<Rol> Lista_Roles { get; set; }


        // CONSTRUCTOR:
        public Registrados_Rol()
        {
            Lista_Roles = new List<Rol>();
        }

    }
}
