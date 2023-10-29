using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class Producto : BaseEntity
{
    public string? NomProd { get; set; }

    public string? DescProd { get; set; }

    public string? Categoria { get; set; }

    public decimal? Precio { get; set; }
    public int? Stock { get; set; }
    public int? CveProv { get; set; }
    public int? Estatus { get; set; }
    //public string? Imagen { get; set; }
    //public string? NombEmpresa { get; set; }
}
