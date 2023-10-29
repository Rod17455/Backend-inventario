namespace API.Dtos;

public class PendienteAutorizarDto
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int IdUsuario { get; set; }
}
