using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAplicacionTurnos.Data;
using WebAplicacionTurnos.Models;

namespace WebAplicacionTurnos.Controllers
{
	public class IngresoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public IngresoController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet] // Responde a solicitudes HTTP GET
		public IActionResult Index() // Acción
		{
			ViewBag.HideHeader = true; // ViewBag permite pasar datos desde el controlar a la vista en APS.NET  
			return View();
		}

		[HttpPost] // Mi método index esta marcado por el este atributo, significa que responde a solicitudes HTTP POST
		public IActionResult Index(string numeroDocumento, string email)
		{
			// Realizar una consolta en la base de datos a traves del contexto
			var cliente = (from c in _context.Clientes
						   where c.NumeroDocumento.ToString() == numeroDocumento || c.CorreoElectronico == email
						   select c).FirstOrDefault(); // Solicitud LINQ para buscar un cliente en la base de datos que coincida con el numero de documento o correo electronico proporcionados

			ViewBag.HideHeader = true;

			if (cliente == null)
			{
				ViewBag.ErrorMessage = "Cliente no encontrado";
				return View(); // Se establece un mensaje de error en ViewBag y se devuelve la vista actual.
			}
			return RedirectToAction("IniciarReservaTurno", "Turnos", new { clienteId = cliente.ClienteId }); // Si se encuentra el cliente, se redirige a la acción "IniciarReservaTurno" en el controlador "Turnos" y se pasa el clienteId como parámetro.
		}
	}
}
