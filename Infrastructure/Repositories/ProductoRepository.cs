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
        return await _context.Productos.ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(
        int pageIndex, int pageSize, string search)
    {

        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Productos as IQueryable<Producto>;

        if (!String.IsNullOrEmpty(search))
        {
            consulta = consulta.Where(p => p.NomProd.ToLower().Contains(search));
        }

        var totalRegistros = await consulta.CountAsync();

        var registros = await consulta
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

    
}
