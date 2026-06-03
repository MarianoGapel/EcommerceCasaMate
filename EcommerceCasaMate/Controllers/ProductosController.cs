using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceCasaMate.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceCasaMate.Controllers
{
    public class ProductosController : Controller
    {
        private readonly EcommerceRegionalDbContext _context;

        public ProductosController(EcommerceRegionalDbContext context)
        {
            _context = context;
        }

        // INDEX 
        public async Task<IActionResult> Index()
        {
            var productosConCategoria = _context.Productos.Include(p => p.IdCategoriaNavigation);
            return View(await productosConCategoria.ToListAsync());
        }

        // 1. GET: Para mostrar el formulario vacío
        public IActionResult Create()
        {
            ViewBag.ListaCategorias = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria");
            return View();
        }

        // 2. POST: Para guardar el producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ListaCategorias = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            return View(producto);
        }

        // 3. EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            ViewBag.ListaCategorias = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            return View(producto);
        }

        // 3.1 EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.IdProducto) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ListaCategorias = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", producto.IdCategoria);
            return View(producto);
        }

        // 4. DETAILS 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var producto = await _context.Productos.FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // 5. DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var producto = await _context.Productos.FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // 5.1 DELETE POST 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null) _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}