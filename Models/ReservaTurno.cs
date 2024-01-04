using WebAplicacionTurnos.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models
{
    public class ReservaTurno
    {
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; } = new Cliente();


        [Display(Name = "Tipo de Servicio")]
        public int TipoServicioId { get; set; }

        [Display(Name = "Tipo de Servicio")]
        public string TipoServicioDescripcion { get; set; } = string.Empty;


        [Display(Name = "Servicio")]
        public int? ServicioId { get; set; }

        [Display(Name = "Servicio")]
        public string? ServicioDescripcion { get; set; }


        [Display(Name = "Fecha turno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaTurno { get; set; }

        [Display(Name = "Hora turno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public TimeSpan HoraTurno { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string? Observacion { get; set; }
    }
}
