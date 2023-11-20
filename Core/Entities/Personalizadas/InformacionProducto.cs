using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Personalizadas;

public class InformacionProducto : BaseEntity
{
    public string? NomProd { get; set; }

    public string? DescProd { get; set; }

    public string? Categoria { get; set; }

    public decimal? Precio { get; set; }

    public int? Stock { get; set; }

    public string? NombEmpresa { get; set; }

    public string? Imagen { get; set; }

    public string? Estatus { get; set; }

    public string? NombProv { get; set; }

    public string? TelefonoProv { get; set; }

    public string? EmailProv { get; set; }

    public string? Observacion { get; set;}

    public string? Color { get; set; }


}
