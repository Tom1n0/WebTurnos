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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string apellido, string nombre, int? numeroDocumento)
        {
            // Obtener todos los clientes con sus relaciones de Localidad y Provincia
            IQueryable<Cliente> clientes = _context.Clientes
                .Include(c => c.Localidad)
                .Include(c => c.Provincia);

            // Filtrar por apellido si se proporciona
            if (!string.IsNullOrEmpty(apellido))
            {
                clientes = clientes.Where(c => c.Apellido.Contains(apellido));
            }

            // Filtrar por nombre si se proporciona
            if (!string.IsNullOrEmpty(nombre))
            {
                clientes = clientes.Where(c => c.Nombre.Contains(nombre));
            }

            // Filtrar por numero de documento si se proporciona
            if (numeroDocumento != null)
            {
                clientes = clientes.Where(c => c.NumeroDocumento == numeroDocumento);
            }

            // Pasar los valores de filtrado a la vista para su uso en la UI
            ViewBag.Nombre = nombre;
            ViewBag.Apellido = apellido;
            ViewBag.NumeroDocumento = numeroDocumento;

            // Retornar la lista de clientes filtrados a la vista
            return View(await clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Validar si el ID proporcionado es nulo o no existe en la base de datos
            if (id == null || !_context.Clientes.Any(c => c.ClienteId == id))
            {
                return NotFound();
            }

            // Obtener el cliente con sus relaciones de Localidad y Provincia
            var cliente = await _context.Clientes
                .Include(c => c.Localidad)
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.ClienteId == id);

            // Si no se encuentra el cliente, retornar un error 404
            if (cliente == null)
            {
                return NotFound();
            }

            // Retornar el cliente a la vista
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            // Poblar las listas desplegables para Localidad y Provincia
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion");
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");

            // Mostrar el formulario de creación
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Apellido,Nombre,Telefono,Celular,TipoDocumento,NumeroDocumento,FechaNacimiento,CorreoElectronico,Calle,Altura,Barrio,Partido,LocalidadId,ProvinciaId,CuitCuil,RazonSocial")] Cliente cliente)
        {
            // Validar el modelo del cliente proporcionado
            if (ModelState.IsValid)
            {
                // Agregar el cliente al contexto y guardar cambios
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                // Redirigir al Index después de crear exitosamente
                return RedirectToAction(nameof(Index));
            }
            // En caso de error en la validación, repoblar las listas desplegables y mostrar el formulario nuevamente
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", cliente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", cliente.ProvinciaId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Validar si el ID proporcionado es nulo o no existe en la base de datos
            if (id == null || !_context.Clientes.Any(c => c.ClienteId == id))
            {
                return NotFound();
            }

            // Obtener el cliente con el ID proporcionado
            var cliente = await _context.Clientes.FindAsync(id);

            // Si no se encuentra el cliente, retornar un error 404
            if (cliente == null)
            {
                return NotFound();
            }

            // Poblar las listas desplegables para Localidad y Provincia
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", cliente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", cliente.ProvinciaId);

            // Retornar el cliente a la vista de edición
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Apellido,Nombre,Telefono,Celular,TipoDocumento,NumeroDocumento,FechaNacimiento,CorreoElectronico,Calle,Altura,Barrio,Partido,LocalidadId,ProvinciaId,CuitCuil,RazonSocial")] Cliente cliente)
        {
            // Validar si el ID del cliente proporcionado coincide con el ID en la URL
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            // Validar el modelo del cliente proporcionado
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
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

            // En caso de error en la validación, repoblar las listas desplegables y mostrar el formulario de edición nuevamente
            ViewData["LocalidadId"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", cliente.LocalidadId);
            ViewData["ProvinciaId"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", cliente.ProvinciaId);

            // Retornar el cliente a la vista de edición
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Validar si el ID proporcionado es nulo o no existe en la base de datos
            if (id == null || !_context.Clientes.Any(c => c.ClienteId == id))
            {
                return NotFound();
            }

            // Obtener el cliente con el ID proporcionado y sus relaciones de Localidad y Provincia
            var cliente = await _context.Clientes
                .Include(c => c.Localidad)
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.ClienteId == id);

            // Si no se encuentra el cliente, retornar un error 404
            if (cliente == null)
            {
                return NotFound();
            }

            // Retornar el cliente a la vista de confirmación de eliminación
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();

            // Redirigir al Index después de eliminar exitosamente
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.ClienteId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLocalidadesPorProvincia(int provinciaId)
        {
            // Obtener las localidades que pertenecen a la provincia especificada
            var localidades = await _context.Localidades
                .Where(l => l.ProvinciaId == provinciaId)
                .Select(l => new { l.LocalidadId, l.Descripcion })
                .ToListAsync();

            // Retornar las localidades en formato JSON
            return Json(localidades);
        }

        // GET: Clientes/Registrar
        public IActionResult Registrar()
        {
            // Poblar las listas desplegables para Localidad y Provincia
            ViewData["SelectListLocalidad"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion");
            ViewData["SelectListProvincia"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion");

            // Mostrar el formulario de creación
            ViewBag.HideHeader = true;
            return View();
        }


        // POST: Clientes/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar([Bind("ClienteId,Apellido,Nombre,FechaNacimiento,TipoDocumento,NumeroDocumento,Calle,Altura,Barrio,Partido,ProvinciaId,LocalidadId,CodigoPostal,CuitCuil,RazonSocial,CorreoElectronico,Celular,Telefono")] Cliente cliente)
        {
            // Validar el modelo del cliente proporcionado
            if (ModelState.IsValid)
            {
                // Verificar si el número de documento o correo electrónico ya están registrados
                bool documentoExiste = (from c in _context.Clientes
                                        where c.NumeroDocumento == cliente.NumeroDocumento
                                        select c).Any();

                bool correoExiste = (from c in _context.Clientes
                                     where c.CorreoElectronico == cliente.CorreoElectronico
                                     select c).Any();


                if (documentoExiste)
                {
                    ModelState.AddModelError("NumeroDocumento", "El número de documento ya está registrado en la base de datos.");
                }

                if (correoExiste)
                {
                    ModelState.AddModelError("CorreoElectronico", "El correo electrónico ya está registrado en la base de datos.");
                }

                if (!documentoExiste && !correoExiste)
                {

                    // Agregar el cliente al contexto y guardar cambios
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();

                    // Redirigir al Index después de crear exitosamente
                    return RedirectToAction(nameof(RegistroExitoso));
                }
            }

            // En caso de error en la validación, repoblar las listas desplegables y mostrar el formulario nuevamente
            ViewData["SelectListLocalidad"] = new SelectList(_context.Localidades, "LocalidadId", "Descripcion", cliente.LocalidadId);
            ViewData["SelectListProvincia"] = new SelectList(_context.Provincias, "ProvinciaId", "Descripcion", cliente.ProvinciaId);

            ViewBag.HideHeader = true;
            return View(cliente);
        }

        public IActionResult RegistroExitoso()
        {
            ViewBag.HideHeader = true;
            ViewBag.Message = "El registro se ha completado con éxito.";
            return View();
        }
    }
}



