using API_RESTful.Models;
using Microsoft.AspNetCore.Authorization;
using Transferencia_Datos.Producto_DTO;
using Microsoft.EntityFrameworkCore;


namespace API_RESTful.Servicios
{
    [Authorize]
    public class Servicios_Producto
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;


        // Constructor:
        public Servicios_Producto(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }



        // **************** METODOS QUE MANDARAN OBJETOS *****************
        // *****************************************************************

        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<Registrados_Producto> Obtner_Todos()
        {
            List<Producto> Lista_Productos = await _MyDBcontext.Productos.ToListAsync();

            // DTO a retornar:
            Registrados_Producto Productos_Registrados = new Registrados_Producto();


            foreach (Producto producto in Lista_Productos)
            {
                Productos_Registrados.Lista_Productos.Add(new Registrados_Producto.Producto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Fotografia = producto.Fotografia,
                });
            }

            return Productos_Registrados;
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<Obtener_Producto> Obtener_PorId(int id)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (Objeto_Obtenido == null)
            {
                return null;
            }

            Obtener_Producto producto = new Obtener_Producto
            {
                IdProducto = Objeto_Obtenido.IdProducto,
                Nombre = Objeto_Obtenido.Nombre,
                Precio = Objeto_Obtenido.Precio,
                Fotografia = Objeto_Obtenido.Fotografia
            };

            return producto;
        }





        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  *******
        // ********************************************************************

        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        public async Task<int> Registrar_Producto(Crear_Producto crear_Producto)
        {
            // Objeto a Mapear:
            Producto producto = new Producto
            {
                Nombre = crear_Producto.Nombre,
                Precio = crear_Producto.Precio,
                Fotografia = crear_Producto.Fotografia
            };

            _MyDBcontext.Add(producto);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        public async Task<int> Editar_Producto(Editar_Producto editar_Producto)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == editar_Producto.IdProducto);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            if (editar_Producto.Fotografia == null)
            {
                editar_Producto.Fotografia = Objeto_Obtenido.Fotografia;
            }

            // Modificamos:
            Objeto_Obtenido.Nombre = editar_Producto.Nombre;
            Objeto_Obtenido.Precio = editar_Producto.Precio;
            Objeto_Obtenido.Fotografia = editar_Producto.Fotografia;

            _MyDBcontext.Update(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        public async Task<int> Eliminar_Producto(int id)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            _MyDBcontext.Remove(Objeto_Obtenido);

            return await _MyDBcontext.SaveChangesAsync();
        }


    }
}
