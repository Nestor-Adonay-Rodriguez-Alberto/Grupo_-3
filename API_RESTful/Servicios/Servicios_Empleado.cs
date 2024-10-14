using API_RESTful.Models;
using Microsoft.AspNetCore.Authorization;
using Transferencia_Datos.Empleado_DTO;
using Transferencia_Datos.Rol_DTO;
using Microsoft.EntityFrameworkCore;


namespace API_RESTful.Servicios
{
    [Authorize]
    public class Servicios_Empleado
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;


        // Constructor:
        public Servicios_Empleado(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }



        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<Registrados_Empleado> Obtner_Todos()
        {
            List<Empleado> Lista_Empleados = await _MyDBcontext.Empleados
                .Include(x => x.Objeto_Rol)
                .ToListAsync();

            // DTO a retornar:
            Registrados_Empleado Empleados_Registrados = new Registrados_Empleado();


            foreach (Empleado empleado in Lista_Empleados)
            {
                Empleados_Registrados.Lista_Empleados.Add(new Registrados_Empleado.Empleado
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
                    }
                });
            }
            return Empleados_Registrados;
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<Obtener_Empleado> Obtener_PorId(int id)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados
                .Include(x => x.Objeto_Rol)
                .FirstOrDefaultAsync(x => x.IdEmpleado == id);

            if (Objeto_Obtenido == null)
            {
                return null;
            }

            Obtener_Empleado Empleado = new Obtener_Empleado
            {
                IdEmpleado = Objeto_Obtenido.IdEmpleado,
                Nombre = Objeto_Obtenido.Nombre,
                Salaraio = Objeto_Obtenido.Salaraio,
                Telefono = Objeto_Obtenido.Telefono,
                Email = Objeto_Obtenido.Email,
                Password = Objeto_Obtenido.Password,
                IdRolEnEmpleado = Objeto_Obtenido.IdRolEnEmpleado,
                Objeto_Rol = new Registrados_Rol.Rol
                {
                    IdRol = Objeto_Obtenido.Objeto_Rol.IdRol,
                    Nombre = Objeto_Obtenido.Objeto_Rol.Nombre
                }
            };

            return Empleado;
        }


        // OBTIENE TODOS LOS REGISTROS DE Roles DE LA DB:
        public async Task<Registrados_Rol> Roles_Registrados()
        {
            List<Rol> Lista_Roles = await _MyDBcontext.Roles.ToListAsync();

            // DTO a Retornar:
            Registrados_Rol Roles_Registrados = new Registrados_Rol();

            foreach (Rol rol in Lista_Roles)
            {
                Roles_Registrados.Lista_Roles.Add(new Registrados_Rol.Rol
                {
                    IdRol = rol.IdRol,
                    Nombre = rol.Nombre
                });
            }

            return Roles_Registrados;
        }





        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        public async Task<int> Registrar_Empleado(Crear_Empleado crear_Empleado)
        {
            // Encriptamos Password:
            crear_Empleado.Password = BCrypt.Net.BCrypt.HashPassword(crear_Empleado.Password);

            // Objeto a Mapear:
            Empleado empleado = new Empleado
            {
                Nombre = crear_Empleado.Nombre,
                Salaraio = crear_Empleado.Salaraio,
                Telefono = crear_Empleado.Telefono,
                Email = crear_Empleado.Email,
                Password = crear_Empleado.Password,
                IdRolEnEmpleado = crear_Empleado.IdRolEnEmpleado
            };


            _MyDBcontext.Add(empleado);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        public async Task<int> Editar_Empleado(Editar_Empleado editar_Empleado)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == editar_Empleado.IdEmpleado);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            // Modificamos:
            Objeto_Obtenido.Nombre = editar_Empleado.Nombre;
            Objeto_Obtenido.Salaraio = editar_Empleado.Salaraio;
            Objeto_Obtenido.Telefono = editar_Empleado.Telefono;
            Objeto_Obtenido.Email = editar_Empleado.Email;
            Objeto_Obtenido.IdRolEnEmpleado = editar_Empleado.IdRolEnEmpleado;

            _MyDBcontext.Update(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        public async Task<int> Eliminar_Empleado(int id)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            _MyDBcontext.Remove(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y MODIFICA CONTRASEÑA
        public async Task<int> Editar_Contraseña(Editar_Contraseña editar_Contraseña)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == editar_Contraseña.IdEmpleado);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            //Encriptamos Password:
            editar_Contraseña.Password = BCrypt.Net.BCrypt.HashPassword(editar_Contraseña.Password);

            // Modificamos:
            Objeto_Obtenido.Password = editar_Contraseña.Password;

            _MyDBcontext.Update(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }

    }
}
