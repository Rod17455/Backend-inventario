using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class EscasezProducto : BaseEntity
{
    public int ProductoId { get; set; }
    public int Cant_Soli { get; set; }
    public DateTime Fecha_Registro { get; set; }
    public int UsuarioId { get; set; }
    public int? Estatus { get; set; }
    public decimal? Precio { get; set; }
    /*
     ProductoId int not null,
    Cant_Soli int not null,
    Fecha_Registro DATETIME not null,
    UsuarioId int not null,
    Estatus varchar(20) not null
     */
}
