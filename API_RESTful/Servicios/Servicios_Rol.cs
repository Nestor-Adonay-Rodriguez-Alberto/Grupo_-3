using API_RESTful.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Transferencia_Datos.Rol_DTO;


namespace API_RESTful.Servicios
{
    [Authorize]
    public class Servicios_Rol
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;


        // Constructor:
        public Servicios_Rol(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }



        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<List<Rol>> Obtner_Todos()
        {
            List<Rol> Lista_Roles = await _MyDBcontext.Roles.ToListAsync();

            return Lista_Roles;
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<Obtener_Rol> Obtener_PorId(int id)
        {
            Rol? Objeto_Obtenido = await _MyDBcontext.Roles.FirstOrDefaultAsync(x => x.IdRol == id);

            if (Objeto_Obtenido == null)
            {
                return null;
            }

            Obtener_Rol rol = new Obtener_Rol
            {
                IdRol = Objeto_Obtenido.IdRol,
                Nombre = Objeto_Obtenido.Nombre
            };
            return rol;
        }





        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        public async Task<int> Registrar_Rol(Crear_Rol crear_Rol)
        {
            // Objeto a Mapear:
            Rol rol = new Rol
            {
                Nombre = crear_Rol.Nombre
            };

            _MyDBcontext.Add(rol);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        public async Task<int> Editar_Rol(Editar_Rol editar_Rol)
        {
            Rol? Objeto_Obtenido = await _MyDBcontext.Roles.FirstOrDefaultAsync(x => x.IdRol == editar_Rol.IdRol);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            // Modificamos:
            Objeto_Obtenido.Nombre = editar_Rol.Nombre;

            _MyDBcontext.Update(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        public async Task<int> Eliminar_Rol(int id)
        {
            Rol? Objeto_Obtenido = await _MyDBcontext.Roles.FirstOrDefaultAsync(x => x.IdRol == id);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            _MyDBcontext.Remove(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }

    }
}
