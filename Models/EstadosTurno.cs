using System;
using System.Collections.Generic;

namespace WebAplicacionTurnos.Models;

public partial class EstadosTurno
{
    public int EstadoTurnoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
