namespace EcommerceCasaMate.Models
{
    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        public decimal Subtotal => Precio * Cantidad;
    }
}