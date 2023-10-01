using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Usuario : BaseEntity
{

    public string NomUser { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
