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