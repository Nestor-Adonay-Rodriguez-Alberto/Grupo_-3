using System;


namespace Transferencia_Datos.Producto_DTO
{
    public class Obtener_Producto
    {
        // ATRIBUTOS:
        public int IdProducto { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public byte[]? Fotografia { get; set; }

    }
}
