using System;
using System.ComponentModel.DataAnnotations;


namespace Transferencia_Datos.Producto_DTO
{
    public class Editar_Producto
    {
        // ATRIBUTOS:
        [Required]
        public int IdProducto { get; set; }


        [Required(ErrorMessage = "Ingrese El Nombre Del Producto.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Ingrese El Precio Del Producto.")]
        public decimal Precio { get; set; }


        public byte[]? Fotografia { get; set; }

    }
}
