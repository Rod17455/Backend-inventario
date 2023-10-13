namespace API.Dtos;

public class ProductoDto
{
    public int ID { get; set; }
    public string? NomProd { get; set; }
    public string? DescProd { get; set; }
    public string? Categoria { get; set; }
    public decimal? Precio { get; set; }
    public int? Stock { get; set; }
    //public string? NombEmpresa { get; set; }
    //public string? Imagen { get; set; }
}
