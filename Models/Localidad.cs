using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models;

public partial class Localidad
{
    [Key]
    public int LocalidadId { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "La descripción no debe exceder los 50 caracteres")]
    public string Descripcion { get; set; } = null!;

    [Display(Name = "Provincia")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int? ProvinciaId { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Provincia? Provincia { get; set; }
}
