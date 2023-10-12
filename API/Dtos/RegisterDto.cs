using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class RegisterDto
{
    [Required]
    public string? NomUser { get; set; }
    [Required]
    public string? Apellido { get; set; }
    [Required]
    public string? CorreoElectronico { get; set; }
    [Required]
    public string? Password { get; set; }
}
