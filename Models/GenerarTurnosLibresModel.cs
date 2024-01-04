using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models
{
    public class GenerarTurnosLibresModel
    {
        [Display(Name = "Fecha desde")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaTurnoDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaTurnoHasta { get; set; }

        [Display(Name = "Hora desde")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraTurnoDesde { get; set; }

        [Display(Name = "Hora hasta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraTurnoHasta { get; set; }

        [Display(Name = "Intervalo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Time)]
        public TimeSpan Intervalo { get; set; }

        [Display(Name = "Lunes")]
        public bool Lunes { get; set; }
        [Display(Name = "Martes")]
        public bool Martes { get; set; }
        [Display(Name = "Miércoles")]
        public bool Miercoles { get; set; }
        [Display(Name = "Jueves")]
        public bool Jueves { get; set; }
        [Display(Name = "Viernes")]
        public bool Viernes { get; set; }
        [Display(Name = "Sábado")]
        public bool Sabado { get; set; }
        [Display(Name = "Domingo")]
        public bool Domingo { get; set; }

    }
}
