namespace API.Dtos;

public class ProductoPendienteDto
{
    public int ID { get; set; }
    public string? Nom_Producto { get; set; }
    public int Cant_Soli { get; set; }
    public DateTime Fecha_Registro { get; set; }
    public string? Nom_Usuario { get; set; }
    public string? Estatus { get; set; }
    public decimal? Precio { get; set; }
}
