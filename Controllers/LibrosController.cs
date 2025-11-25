using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManejadorLibreria.Data;
using ManejadorLibreria.Models;


namespace ManejadorLibreria.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibreriaContext _context;

        public LibrosController(LibreriaContext context)
        {
            _context = context;
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetString("UsuarioId") != null;
        }

        public async Task<IActionResult> Index()
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para acceder a la biblioteca";
                return RedirectToAction("Login", "Account");
            }

            return View(await _context.Libros.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para ver detalles";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
                return NotFound();

            var libro = await _context.Libros.FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
                return NotFound();

            return View(libro);
        }

        public IActionResult Create()
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para agregar libros";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Autor,Year,ISBN")] Libro libro)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para agregar libros";
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Libro creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para editar libros";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
                return NotFound();

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Autor,Year,ISBN")] Libro libro)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para editar libros";
                return RedirectToAction("Login", "Account");
            }

            if (id != libro.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Libro actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para eliminar libros";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
                return NotFound();

            var libro = await _context.Libros.FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
                return NotFound();

            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UsuarioAutenticado())
            {
                TempData["Error"] = "Debes iniciar sesión para eliminar libros";
                return RedirectToAction("Login", "Account");
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Libro eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}