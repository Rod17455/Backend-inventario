namespace API.Dtos;

public class ProductoDto
{
    public int ID { get; set; }
    public string NomProd { get; set; } = null!;
    public string? DescProd { get; set; }
    public string? Categoria { get; set; }
    public decimal? Precio { get; set; }
    public int? Stock { get; set; }
    public int? CveProv { get; set; }
    public string? Imagen { get; set; }
}
