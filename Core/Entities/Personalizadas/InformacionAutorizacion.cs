using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Personalizadas;

public class InformacionAutorizacion : BaseEntity
{
    public string? Nombre_Producto { get; set; }
    public string? Nombre_Empleado { get; set; }
    public DateTime? Fecha_Autorizacion { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
}
