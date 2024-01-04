using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAplicacionTurnos.Models;

public partial class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Celular { get; set; } = null!;

    [Display(Name = "Tipo documento")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string TipoDocumento { get; set; } = null!;

    [Display(Name = "Número documento")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int NumeroDocumento { get; set; }

    [Display(Name = "Fecha de nacimiento")]
    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    [Display(Name = "Correo Electrónico")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string CorreoElectronico { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Calle { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int Altura { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Barrio { get; set; } = null!;

    public string? Partido { get; set; }

    [Display(Name = "Localidad")]
    public int? LocalidadId { get; set; }

    [Display(Name = "Provincia")]
    public int? ProvinciaId { get; set; }

    [Display(Name = "CUIT/CUIL")]
    public string? CuitCuil { get; set; }

    [Display(Name = "Razón social")]
    public string? RazonSocial { get; set; }

    public virtual Localidad? Localidad { get; set; }

    public virtual Provincia? Provincia { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
