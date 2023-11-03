using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
{
    public ProductoRepository(InventarioContext context) : base(context)
    {
    }



    public override async Task<Producto> GetByIdAsync(int id)
    {
        return await _context.Productos.FirstOrDefaultAsync(p => p.ID == id);
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos.Where(x => x.Estatus != 4).ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(
        int pageIndex, int pageSize, string search)
    {

        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Productos as IQueryable<Producto>;
        // Solo muestra las que son diferentes del estatus 4
        var _consulta = consulta.Where(x => x.Estatus != 4);

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.NomProd.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }

    public async Task<int> GetTotalProductos()
    {
        var cantidadProductos = (from producto in _context.Productos
                                 select producto).CountAsync();

        return await cantidadProductos;
    }

    public async Task<IEnumerable<Producto>> GetAllPendienteAsync()
    {
        return await _context.Productos.Where(x => x.Estatus == 4).ToListAsync();
    }

    public async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllPendienteAsync(int pageIndex, int pageSize, string search)
    {
        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Productos as IQueryable<Producto>;
        // Solo muestra las que son diferentes del estatus 4
        var _consulta = consulta.Where(x => x.Estatus == 4);

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.NomProd.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<Producto>> GetAllRechazoAsync()
    {
        return await _context.Productos.Where(x => x.Estatus == 5).ToListAsync();
    }

    public async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllRechazoAsync(int pageIndex, int pageSize, string search)
    {
        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Productos as IQueryable<Producto>;
        // Solo muestra las que son diferentes del estatus 4
        var _consulta = consulta.Where(x => x.Estatus == 5);

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.NomProd.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<Producto>> GetAllXEntregarAsync()
    {
        return await _context.Productos.Where(x => x.Estatus == 6).ToListAsync();
    }

    public async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllXEntregarAsync(int pageIndex, int pageSize, string search)
    {
        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Productos as IQueryable<Producto>;
        // Solo muestra las que son diferentes del estatus 4
        var _consulta = consulta.Where(x => x.Estatus == 6);

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.NomProd.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }
}
