using System.Text.Json.Serialization;

namespace API.Dtos;

public class DatosUsuarioDto
{
    public int Id { get; set; }
    public string? Mensaje { get; set; }
    public bool EstaAutenticado { get; set; }
    public string? NomUser { get; set; }
    public string? CorreoElectronico { get; set; }
    public List<string>? Roles { get; set; }
    public string? Token { get; set; }
    public int? Permiso { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
