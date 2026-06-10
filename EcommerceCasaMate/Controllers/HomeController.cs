using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceCasaMate.Models;

namespace EcommerceCasaMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceRegionalDbContext _context;

        // Inyectamos la base de datos
        public HomeController(EcommerceRegionalDbContext context)
        {
            _context = context;
        }

        // 1. HOME / CATÁLOGO
        public async Task<IActionResult> Index()
        {
            // Traemos todos los productos e incluimos la categoría para poder mostrar el nombre (Mates, Termos, etc.)
            var productos = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .ToListAsync();

            return View(productos);
        }

        // 2. AGREGAR AL CARRITO
        public async Task<IActionResult> AgregarAlCarrito(int id)
        {
            // 1. Buscamos el producto en la base de datos para saber su nombre y precio
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return RedirectToAction("Index"); // Si no existe, volvemos al inicio
            }

            // 2. Leemos el carrito actual de la memoria (Sesión)
            List<ItemCarrito> carrito = new List<ItemCarrito>();
            string carritoJson = HttpContext.Session.GetString("Carrito");

            if (!string.IsNullOrEmpty(carritoJson))
            {
                // Si ya había cosas guardadas, las volvemos a convertir en lista
                carrito = JsonSerializer.Deserialize<List<ItemCarrito>>(carritoJson);
            }

            // 3. Verificamos si el producto ya estaba en el carrito
            var itemExistente = carrito.FirstOrDefault(i => i.ProductoId == id);

            if (itemExistente != null)
            {
                // Si ya estaba, solo le sumamos 1 a la cantidad
                itemExistente.Cantidad++;
            }
            else
            {
                // Si es nuevo, armamos la tarjetita del item y lo agregamos
                carrito.Add(new ItemCarrito
                {
                    ProductoId = producto.IdProducto, // Ojo acá: asegurate de que en tu BD se llame IdProducto
                    NombreProducto = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }

            // 4. Volvemos a guardar la lista actualizada en la sesión (convertida a texto JSON)
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));

            // 5. Recargamos el catálogo para que el usuario pueda seguir comprando
            return RedirectToAction("VerCarrito");
        }
        // 3. VER CARRITO (Detalle de Compra)
        public IActionResult VerCarrito()
        {
            List<ItemCarrito> carrito = new List<ItemCarrito>();
            string carritoJson = HttpContext.Session.GetString("Carrito");

            // Si hay algo guardado, lo convertimos de nuevo a lista
            if (!string.IsNullOrEmpty(carritoJson))
            {
                carrito = JsonSerializer.Deserialize<List<ItemCarrito>>(carritoJson);
            }

            // Le pasamos la lista a la vista
            return View(carrito);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}