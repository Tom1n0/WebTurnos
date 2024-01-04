using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAplicacionTurnos.Data;
using WebAplicacionTurnos.Models;

namespace WebAplicacionTurnos.Controllers
{
    public class LocalidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Localidades
        public async Task<IActionResult> Index()
        {
            // Lista de objetos Localidad, y cada objeto Localidad tendrá su propiedad Provincia cargada. 
            var applicationDbContext = _context.Localidades.Include(l => l.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Localidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Validar si el ID proporcionado es nulo o no existe en la base de datos
            if (id == null || !_context.Localidades.Any(l => l.LocalidadId == id))
            {
                return NotFound();
            }

            // Obtener todas las localidades con sus relaciones de Provincia
            var localidad = await _context.Localidades
                .Include(l => l.Provincia)
                // Obtener la primera entidad Localidad que cumple con la condición dada.
                .FirstOrDefaultAsync(m => m.LocalidadId == id);

            // Si no se encuentra la localidad, retornar un error 404
            if (localidad == null)
            {
                return NotFound();
            }

            // Retornar la localidad a la vista
            return View(localidad);
        }

        // GET: Localidades/Create
        public IActionResult Create()
        {
            // Poblar la lista desplegable para Provincia
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");
            // Mostrar el formulario de creación
            return View();
        }

        // POST: Localidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalidadId,Descripcion,ProvinciaId")] Localidad localidad)
        {
            // Validar el modelo de la localidad proporcionado
            if (ModelState.IsValid)
            {
                // Agregar la localidad al contexto y guardar cambios
                _context.Add(localidad);
                await _context.SaveChangesAsync();

                // Redirigir al Index después de crear exitosamente
                return RedirectToAction(nameof(Index));
            }
            // En caso de error en la validación, repoblar la lista desplegable para Provincia
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", localidad.ProvinciaId);
            return View(localidad);
        }

        // GET: Localidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Validar si el ID proporcionado es nulo o no existe en la base de datos
            if (id == null || !_context.Localidades.Any(l => l.LocalidadId == id))
            {
                return NotFound();
            }

            // Si no se encuentra la localidad, retornar un error 404
            var localidad = await _context.Localidades.FindAsync(id);
            if (localidad == null)
            {
                return NotFound();
            }

            // Poblar la lista desplegable para Provincia
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", localidad.ProvinciaId);
            // Retornar el cliente a la vista de edición
            return View(localidad);
        }

        // POST: Localidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocalidadId,Descripcion,Descripcion")] Localidad localidad)
        {
            // Validar si el ID proporcionado de la localidad coincide con el ID en la URL
            if (id != localidad.LocalidadId)
            {
                return NotFound();
            }

            // Validar el modelo de la localidad proporcionado
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalidadeExists(localidad.LocalidadId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirigir al Index después de editar exitosamente
                return RedirectToAction(nameof(Index));
            }
            // En caso de error en la validación, repoblar la lista desplegable y mostrar el formulario de edición nuevamente
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", localidad.ProvinciaId);
            return View(localidad);
        }

        // GET: Localidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !_context.Localidades.Any(l => l.LocalidadId == id))
            {
                return NotFound();
            }

            // Obtener la localidad con el ID proporcionado y su relacion de Provincia
            var localidad = await _context.Localidades
                .Include(l => l.Provincia)
                .FirstOrDefaultAsync(m => m.LocalidadId == id);
            if (localidad == null)
            {
                return NotFound();
            }

            // Retornar la localidad a la vista de confirmación de eliminación
            return View(localidad);
        }

        // POST: Localidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Localidades == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Localidades'  is null.");
            }
            var localidad = await _context.Localidades.FindAsync(id);
            if (localidad != null)
            {
                _context.Localidades.Remove(localidad);
            }
            
            await _context.SaveChangesAsync();

            // Redirigir al Index después de eliminar exitosamente
            return RedirectToAction(nameof(Index));
        }

        private bool LocalidadeExists(int id)
        {
          return (_context.Localidades?.Any(e => e.LocalidadId == id)).GetValueOrDefault();
        }
    }
}
