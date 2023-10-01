namespace API.Dtos;

public class UsuarioDto
{
    public int ID { get; set; }
    public string NomUser { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string CorreoElectronico { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Rol { get; set; } = null!;
}
