using WebAplicacionTurnos.Data;
using WebAplicacionTurnos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;

namespace WebAplicacionTurnos.Controllers
{
    public class TurnosController : Controller
    {
        // Constructor del controlador "TurnosController" que acepta una instancia de "ApplicationDbContext".
        private readonly ApplicationDbContext _context;

        public TurnosController(ApplicationDbContext context)
        {
            _context = context; // Permite acceder a la base de datos a traves de la instancia "_context"
        }
        // Cuando se llama a la acción Index de este controlador, la solicitud se redirige a la acción "Index" del controlador "Ingreso".
        // Esto es común en escenarios donde se tiene una ruta principal que redirige a otra parte del sistema. Basicamente esto me lleva al controlador Ingreso.
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Ingreso");
        }

        // La funcion de este código esta diseñada para recuperar una lista de objetos "TiposServicios" desde la base de datos, ordebados por la prioridad "Descripcion".
        private List<TiposServicio> ObtenerTiposDeServicios()
        {
            var tiposServicios =
                (
                from t in _context.TiposServicios // Fuente de la consulta.
                orderby t.Descripcion
                select t).ToList(); // selecciona todos los objetos y convierte en una lista.

            return tiposServicios;
        }

        // La funcion de ObtenerServiciosPorTipoServicio toma un parámeto y devuelve una lista de servicios que tienen el mismo tipoServicioId
        private List<Servicio> ObtenerServiciosPorTipoServicio(int tipoServicioId)
        {
            var servicios = (
                from s in _context.Servicios // Define una variable s que representa cada elemento en la tabla de servicios en _context.
                where s.TipoServicioId == tipoServicioId // Filtra los servicios para obtener solo aquellos cuyo TipoServicioId coincide con el valor proporcionado como parámetro.
                orderby s.Descripcion
                select s).ToList();
            return servicios;
        }

        [HttpGet]

        // Este método devuelve un JsonResult que contiene información sobre servicios filtrados por un tipo de servicio específico en formato JSON.
        public async Task<JsonResult> ObtenerJsonServiciosPorTipoServicio(int tipoServicioId) // async Task<JsonResult> Indica que este método es asincrónico y devuelve un resultado en formato JSON.
        {
            var servicios = (
                from s in ObtenerServiciosPorTipoServicio(tipoServicioId) // Llama al método que se define anteriormente para obtener la lista de servicios filtadros por el tipo de servicio.
                select new { s.ServicioId, s.Descripcion } // Proyecta la lista de servicios en un nuevo objeto anónimo que solo contiene las propiedades ServicioId y Descripcion.
                ).ToList();
            return Json(servicios);
        }

        // Este método recibe un parámetro clienteId y devuelve un objeto Cliente.
        private Cliente? ObtenerClientePorId(int clienteId)
        {
            var cliente = _context.Clientes
                .Include(c => c.Localidad) // Utiliza la carga explícita para incluir la propiedad de navegación "Localidad". Esto significa que cuando recuperas un cliente, también se cargarán sus propiedades de navegación "Localidad" y "Provincia". 
                .Include(c => c.Provincia)
                .FirstOrDefault(m => m.ClienteId == clienteId); // Devuelve el primer elemento que coincide con la condición especificada.
            // m => m.ClienteId == clienteId) Es una expresión lambda que define la condición que se debe cumplir para seleccionar un elemento.
            // En este caso, se selecciona el primer elemento cuyo ClienteId coincide con el valor proporcionado en el parámetro clienteId.
            return cliente;
        }

        // Obtener fechas disponibles de turnos proximos a 60 días

        public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(DateTime fecha)
        {
            var fechaDesde = fecha.Date;
            var fechaHasta = fecha.AddDays(60).Date;

            var fechas = (from t in _context.Turnos
                          where t.FechaTurno.Date >= fechaDesde &&  // Solo se seleccionan los turnos con fechas iguales o posteriores a fechaDesde
                                t.FechaTurno.Date < fechaHasta && // Solo se seleccionan los turnos con fechas anteriores a fechaHasta.
                                t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre // Solo se seleccionan los turnos con un estado específico (el estado "Libre").
                          group t by t.FechaTurno.Date into grouped
                          orderby grouped.Key
                          select grouped.Key).ToList();
            return fechas;
        }

        public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(int año, int mes, int dia)
        {
            // Crear una instancia de DateTime con los parámetros proporcionados
            var fecha = new DateTime(año, mes, dia);
            return ObtenerFechasDisponiblesDeTurnosProximos60Dias(fecha);
        }

        public List<DateTime> ObtenerFechasDisponiblesDeTurnosProximos60Dias(string fecha)
        {
            // Convertir la cadena de fecha al formato DateTime
            var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return ObtenerFechasDisponiblesDeTurnosProximos60Dias(fechaConvertida);
        }

        [HttpGet]

        public JsonResult ObtenerJsonDeFechasDisponiblesFormateadasYYYYMMDDProximos60Dias(int año, int mes, int dia)
        {
            // Crear una instancia de DateTime con los parámetros proporcionados
            var fecha = new DateTime(año, mes, dia);

            // Obtener las fechas disponibles en los próximos 60 días
            var fechas = ObtenerFechasDisponiblesDeTurnosProximos60Dias(fecha);

            // Formatear las fechas en el formato "YYYYMMDD"
            var fechasFormateadas = FormatearFechasAStringYYYYMMDD(fechas);

            // Devolver el resultado como un objeto JsonResult
            return Json(fechasFormateadas);
        }

        // Obtener horas diponibles de turnos por fechas 
        public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(DateTime fecha)
        {
            var horas = (from t in _context.Turnos
                         where t.FechaTurno.Date == fecha.Date && t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre
                         orderby t.HoraTurno
                         select t.HoraTurno).ToList();
            return horas;
        }

        public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(int año, int mes, int dia)
        {
            var fecha = new DateTime(año, mes, dia); // Crea una instancia de DateTime con estos parámetros
            return ObtenerHorasDisponiblesDeTurnosPorFecha(fecha); // Devuelve una lista de TimeSpan que representan las horas de los turnos disponibles para la fecha proporcionada.
        }

        public List<TimeSpan> ObtenerHorasDisponiblesDeTurnosPorFecha(string fecha)
        {
            var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture); // Convierte la cadena en un objeto DateTime utilizando DateTime.ParseExact.
            return ObtenerHorasDisponiblesDeTurnosPorFecha(fechaConvertida); // Devuelve una lista de TimeSpan que representan las horas de los turnos disponibles para la fecha proporcionada.
        }

        public List<string> ObtenerHorasDisponiblesDeTurnosFormateadasPorFecha(string fecha)
        {
            var fechaConvertida = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var horas = ObtenerHorasDisponiblesDeTurnosPorFecha(fechaConvertida);

            var horasFormateadas = FormatearHorasAString(horas); // Formatea las horas como cadenas utilizando el método FormatearHorasAString.
            return horasFormateadas; // Devuelve una lista de cadenas que representan las horas formateadas de los turnos disponibles para la fecha proporcionada.
        }

        // Formatear las fechas y horas a string

        private static List<string> FormatearFechasAStringDDMMYYYY(List<DateTime> listaFechas)
        {
            var fechasFormateadas = (
                from f in listaFechas
                select f.ToString("dd/MM/yyyy") // Para cada fecha f, convierte la fecha en una cadena en el formato "dd/MM/yyyy" utilizando el método ToString.
                ).ToList();

            return fechasFormateadas;
        }

        private static List<string> FormatearFechasAStringYYYYMMDD(List<DateTime> listaFechas)
        {
            var fechaFormateadas = (
                from f in listaFechas
                select f.ToString("yyyy-MM-dd") // Para cada fecha f, convierte la fecha en una cadena en el formato "yyyy-MM-dd" utilizando el método ToString.
                ).ToList();

            return fechaFormateadas;
        }

        private static List<string> FormatearHorasAString(List<TimeSpan> listaHoras)
        {
            var horasFormateadas = (
                from h in listaHoras
                select h.ToString(@"hh\:mm")
                ).ToList();

            return horasFormateadas;
        }

		//ReservarTurno está decorado con atributos [HttpPost] y [ValidateAntiForgeryToken],
		//indicando que se trata de un método de acción para manejar solicitudes POST y protegido contra ataques de falsificación de solicitudes entre sitios (CSRF).

		[HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ReservarTurno(ReservaTurno model)
        {
            // Se realiza una consulta para verificar si existe un turno libre en la fecha y hora especificadas.
            var turnoExistente = await (from t in _context.Turnos
                                        where t.FechaTurno == model.FechaTurno
                                        && t.HoraTurno == model.HoraTurno
                                        && t.EstadoTurnoId == (int)EstadoTurnoEnum.Libre // Está buscando un turno donde la fecha y la hora coincidan con las proporcionadas en el modelo (model.FechaTurno y model.HoraTurno) y el estado del turno sea "Libre" (según EstadoTurnoEnum).
                                        select t).FirstOrDefaultAsync();

            if (turnoExistente != null) // Si se encuentra un turno libre, se actualiza con la información de la reserva y se marca como asignado.
            {
                turnoExistente.ClienteId = model.ClienteId;
                turnoExistente.FechaTurno = model.FechaTurno;
                turnoExistente.HoraTurno = model.HoraTurno;
                turnoExistente.ServicioId = model.ServicioId;
                turnoExistente.Observacion = model.Observacion;
                turnoExistente.EstadoTurnoId = (int)EstadoTurnoEnum.Asignado;
                _context.Update(turnoExistente);
                await _context.SaveChangesAsync();

                ViewBag.HideHeader = true;
                return View("TurnoReservado", model); // Se redirige a la vista "TurnoReservado" con la propiedad ViewBag.HideHeader configurada como true para ocultar el encabezado.
            }
            return View(model);
        }
        // Acción de controlador para reservar turno
        public IActionResult IniciarReservaTurno(int clienteId) // int clienteId: La acción toma un parámetro clienteId que representa el identificador del cliente.
        {
            Cliente? cliente = ObtenerClientePorId(clienteId); // Llama al método ObtenerClientePorId para obtener la información del cliente correspondiente al clienteId proporcionado.
                                                               // El tipo de Cliente? indica que la variable cliente puede ser nula.
            var tiposServicios = ObtenerTiposDeServicios(); // Llama al método para obtener una lista de tipos de servicios.
            var tipoServicioId = tiposServicios.FirstOrDefault()?.TipoServicioId ?? 0; // Obtiene el TipoServicioId del primer tipo de servicio en la lista (FirstOrDefault()) o establece el valor en 0 si la lista está vacía.

            var servicios = ObtenerServiciosPorTipoServicio(tipoServicioId); // Llama al método para obtener una lista de servicios por id especifico.

            // Obtener las fechas de los turnos libres
            var fechaHoy = DateTime.Today; // Obtiene la fecha actual
            var listaFechas = ObtenerFechasDisponiblesDeTurnosProximos60Dias(fechaHoy); // Llama al método con la fecha actual para obtener una lista de fechas disponibles para los próximos 60 días.
            var fecha = listaFechas.FirstOrDefault(); // Obtiene la primera fecha de la lista de fechas disponibles, o null si la lista está vacía.
            var listaFechasFormateadas = FormatearFechasAStringDDMMYYYY(listaFechas); // Llama al método para formatear la lista de fechas en formato de cadena con el formato "DD/MM/YYYY"

            // Obtener las horas de los turnos libres
            var listaHoras = ObtenerHorasDisponiblesDeTurnosPorFecha(fecha); // Llama al método con la fecha obtenida anteriormente (fecha). Este método parece estar diseñado para devolver una lista de horas disponibles para la fecha dada.
            var hora = listaHoras.FirstOrDefault(); // Obtiene la primera hora de la lista de horas disponibles, o null si la lista está vacía.
            var listaHorasFormateadas = FormatearHorasAString(listaHoras); // Llama al método FormatearHorasAString para formatear la lista de horas en un formato específico.


            var reservaTurno = new ReservaTurno //  Crea una nueva instancia de la clase ReservaTurno.
            {
                FechaTurno = fecha, // Establece la propiedad FechaTurno de la instancia reservaTurno con el valor de la variable fecha.
                HoraTurno = hora, // Establece la propiedad HoraTurno de la instancia reservaTurno con el valor de la variable hora.
                Cliente = cliente, // Establece la propiedad Cliente de la instancia reservaTurno con el valor de la variable cliente. Esto parece estar asignando un objeto de tipo Cliente al objeto reservaTurno.
                ClienteId = clienteId, // Establece la propiedad ClienteId de la instancia reservaTurno con el valor de la variable clienteId.
                // Este código está creando un objeto ReservaTurno con propiedades inicializadas específicamente para representar una reserva de turno. 
            };
            // Se crea una selectlist para tipos de servicios utilizando la lista tiposServicios. El SelectList se configura con valores de TipoServicioId como el valor de cada elemento y Descripcion.
            ViewData["SelectListTiposServicios"] = new SelectList(tiposServicios, "TipoServicioId", "Descripcion");
            ViewData["SelectListServicios"] = new SelectList(servicios, "ServicioId", "Descripcion");
            ViewData["SelectListFechas"] = new SelectList(listaFechasFormateadas);
            ViewData["SelectListHoras"] = new SelectList(listaHorasFormateadas);

            ViewBag.HideHeader = true;
            return View(reservaTurno);
        }

        [HttpGet]

        public ActionResult GenerarTurnosLibres() // Este tipo de retorno indica que el método de acción devuelve un objeto de tipo ActionResult.
        {
            return View(); // Este método simplemente devuelve una vista. 
        }

        [HttpPost]

        public async Task<ActionResult> GenerarTurnosLibres(GenerarTurnosLibresModel model) // Este método toma un modelo GenerarTurnosLibresModel como parámetro.
        {
            if (ModelState.IsValid) // Se verifica si el modelo pasado es válido según las reglas de validación definidas en el modelo. 
            {
                try
                {
                    var parametros = new List<SqlParameter>
                    {
                        // Se configuran los parámetros necesarios para ejecutar un procedimiento almacenado en la base de datos. 
                new SqlParameter("@FechaTurnoDesde", model.FechaTurnoDesde),
                new SqlParameter("@FechaTurnoHasta", model.FechaTurnoHasta),
                new SqlParameter("@HoraTurnoDesde", model.HoraTurnoDesde),
                new SqlParameter("@HoraTurnoHasta", model.HoraTurnoHasta),
                new SqlParameter("@Intervalo", model.Intervalo),
                new SqlParameter("@Lunes", model.Lunes),
                new SqlParameter("@Martes", model.Martes),
                new SqlParameter("@Miercoles", model.Miercoles),
                new SqlParameter("@Jueves", model.Jueves),
                new SqlParameter("@Viernes", model.Viernes),
                new SqlParameter("@Sabado", model.Sabado),
                new SqlParameter("@Domingo", model.Domingo)
                    };

                    // Se utiliza _context.Database.ExecuteSqlRawAsync para ejecutar el procedimiento almacenado. El resultado de la ejecución se almacena en la variable result.
                    var result = await _context.Database.ExecuteSqlRawAsync("EXEC GenerarTurnosLibres @FechaTurnoDesde, @FechaTurnoHasta, @HoraTurnoDesde, @HoraTurnoHasta, @Intervalo, @Lunes, @Martes, @Miercoles, @Jueves, @Viernes, @Sabado, @Domingo", parametros.ToArray());

                    // Mostrar mensaje de éxito
                    ViewBag.Message = "Turnos generados exitosamente.";
                }
                catch (Exception ex)
                {
                    // Manejar excepción si ocurre un error al ejecutar el procedimiento almacenado
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al generar los turnos: " + ex.Message);
                }
            }
            return View(model); // Independientemente de si la ejecución fue exitosa o no, se devuelve la vista original (GenerarTurnosLibres) con el modelo original.
        }
    }
}
