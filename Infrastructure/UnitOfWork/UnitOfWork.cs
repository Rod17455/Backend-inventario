using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly InventarioContext _context;
    private IProductoRepository _productos;
    private IReporteRepository _reportes;
    private IProveedorRepository _cteprovs;
    private IUsuarioRepository _usuarios;
    private IRolRepository _roles;
    private IEscasezRepository _escasezes;

    public UnitOfWork(InventarioContext context)
    {
        _context = context;
    }

    public IEscasezRepository Escasezes
    {
        get
        {
            if (_escasezes == null)
            {
                _escasezes = new EscasesRepository(_context);
            }
            return _escasezes;
        }
    }

    public IProductoRepository Productos
    {
        get
        {
            if (_productos == null)
            {
                _productos = new ProductoRepository(_context);
            }
            return _productos;
        }
    }

    public IReporteRepository Reportes
    {
        get
        {
            if (_reportes == null)
            {
                _reportes = new ReporteRepository(_context);
            }
            return _reportes;
        }
    }

    public IProveedorRepository CteProvs
    {
        get
        {
            if (_cteprovs == null)
            {
                _cteprovs = new ProveedorRepository(_context);
            }
            return _cteprovs;
        }
    }

    public IUsuarioRepository Usuarios
    {
        get
        {
            if (_usuarios == null)
            {
                _usuarios = new UsuarioRepository(_context);
            }
            return _usuarios;
        }
    }

    public IRolRepository Roles
    {
        get
        {
            if (_roles == null)
            {
                _roles = new RolRepository(_context);
            }
            return _roles;
        }
    }



    /*public async Task<int> SaveAsyn()
    {
        return await _context.SaveChangesAsync();
    }*/

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
