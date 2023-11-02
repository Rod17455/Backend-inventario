using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Autorizacion : BaseEntity
{
    public int Id_Escasez { get; set; }
    public int Id_Usuario { get; set; }
    public DateTime Fecha_Autoriza { get; set; }
    public int Estatus { get; set; }
}
