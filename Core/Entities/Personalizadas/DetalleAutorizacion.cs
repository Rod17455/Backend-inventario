using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Personalizadas;

public class DetalleAutorizacion : InformacionAutorizacion
{
    //INFORMACION DEL PRODUCTO
    public string? DescProd { get; set; }

    public string? Categoria { get; set; }

    public decimal? Precio { get; set; }

    public int? Stock { get; set; }


    // INFORMACION DEL PROVEEDOS
    public string? NombreEmpresa { get; set; }

    public string? Contacto { get; set; }

    public string? Telefono { get; set; }

    // INFORMACION DEL USUARIO

    public string? EmailUsuario { get; set; }
}
