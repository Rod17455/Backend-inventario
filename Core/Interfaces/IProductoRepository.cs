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
    Task<int> GetTotalProductos();
}
