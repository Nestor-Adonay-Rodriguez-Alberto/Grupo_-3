using System;


namespace Transferencia_Datos.Producto_DTO
{
    public class Registrados_Producto
    {
        // CLASE:
        public class Producto
        {
            // ATRIBUTOS:
            public int IdProducto { get; set; }

            public string Nombre { get; set; }

            public decimal Precio { get; set; }

            public byte[]? Fotografia { get; set; }

        }

        // ALMACENA TODOS LOS PRODUCTOS DE LA DB:
        public List<Producto> Lista_Productos { get; set; }


        // CONSTRUCTOR:
        public Registrados_Producto()
        {
            Lista_Productos = new List<Producto>();
        }

    }
}
