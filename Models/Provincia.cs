using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models;

public partial class Provincia
{
    [Key]
    public int ProvinciaId { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "La descripción no debe exceder los 50 caracteres")]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Localidad> Localidades { get; set; } = new List<Localidad>();
}
