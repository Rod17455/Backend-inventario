namespace API.Dtos;

public class ReporteDto
{
    public int ID { get; set; }
    public string TipoReporte { get; set; } = null!;
    public DateTime? FechaGeneracion { get; set; }
    public int? CveUsr { get; set; }
    public string? DatosReporte { get; set; }
}
