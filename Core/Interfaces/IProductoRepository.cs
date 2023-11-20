using Core.Entities;
using Core.Entities.Personalizadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IProductoRepository : IGenericRepository<Producto>
{
    //Task<T> GetByIdAsync(int id);
    Task<IEnumerable<Producto>> GetAllPendienteAsync();
    Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllPendienteAsync(int pageIndex, int pageSize, string search);

    Task<IEnumerable<Producto>> GetAllRechazoAsync();
    Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllRechazoAsync(int pageIndex, int pageSize, string search);

    Task<IEnumerable<Producto>> GetAllXEntregarAsync();
    Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllXEntregarAsync(int pageIndex, int pageSize, string search);

    Task<IEnumerable<Producto>> GetAllSinStock();
    Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllSinStock(int pageIndex, int pageSize, string search);

    Task<int> GetTotalProductos();
}
