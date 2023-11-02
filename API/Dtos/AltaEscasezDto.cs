using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API.Dtos;

public class AltaEscasezDto
{
    public int IdEscasez { get; set; }
    public int IdUsuario { get; set; }
    public int IdProducto { get; set; }
    public string? FechaAutorizacion { get; set; }
    public string? Observacion { get; set; }
}
