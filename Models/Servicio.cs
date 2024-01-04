using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models;

public partial class Servicio
{
    [Key]
    public int ServicioId { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "La descripción no debe exceder los 50 caracteres")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int Duracion { get; set; }

    [Display(Name = "Observación")]
    public string? Observacion { get; set; }

    [Display(Name = "Tipo de servicio")]
    public int? TipoServicioId { get; set; }

    public virtual TiposServicio? TipoServicio { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
