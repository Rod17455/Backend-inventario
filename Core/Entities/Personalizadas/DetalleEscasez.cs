using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Personalizadas;

public class DetalleEscasez : InformacionEscasez
{
    // DATOS DEL PROVEEDOR
    public string? Nomb_Prov { get; set; }

    public string? Telefono_Prov { get; set; }

    public string? Email_Prov { get; set; }
    public string? Nomb_Empresa { get; set; }

    // DATOS DEL USUARIO
    public string? Correo_Usuario { get; set; }

    // DATOS DEL PRODUCTO
    public int Id_Producto { get; set; }


}
