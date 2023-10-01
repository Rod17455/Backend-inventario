using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IUnitOfWork
{
    IProductoRepository Productos { get; }
    IUsuarioRepository Usuarios { get; }
    IReporteRepository Reportes { get; }
    IProveedorRepository CteProvs { get; }
    Task<int> SaveAsync();

}
