using Core.Entities;
using Core.Entities.Personalizadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IEscasezRepository : IGenericRepository<InformacionEscasez>
{

    Task<int> ObtenerCantidadProducto(int idProducto);
    Task<decimal> ObtenerPrecioProducto(int idProducto);
    Task<int> ActualizarEstatusProductoPendiente(int idProducto, string color);
    Task<int> InsertarEscasez(EscasezProducto escasezProducto);
    Task<int> ActualizarEstatusProducto(int idProducto, int estatus, int stock, string color);
    Task<int> ActualizarEstatusEscasez(int idEscasez, int estatus);
    Task<int> ActualizarObservacion(int idEscasez, string observacion);
    Task<int> InsertarEscasez(Autorizacion autorizacion);

    Task<int> ActualizarEstatusProductoRechazado(int idProducto, string color, string obs);


}
