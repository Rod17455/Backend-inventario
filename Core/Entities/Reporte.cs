using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Reporte : BaseEntity
{

    public string TipoReporte { get; set; } = null!;

    public DateTime? FechaGeneracion { get; set; }

    public int? CveUsr { get; set; }

    public string? DatosReporte { get; set; }
}
