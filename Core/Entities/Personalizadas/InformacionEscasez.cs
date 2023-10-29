using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Personalizadas;

public class InformacionEscasez : BaseEntity
{
    public string? Nom_Producto { get; set; }
    public int Cant_Soli { get; set; }
    public DateTime Fecha_Registro { get; set; }
    public string? Nom_Usuario { get; set; }
    public string? Estatus { get; set; }
    public decimal? Precio { get; set; }
}
