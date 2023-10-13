using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly InventarioContext _context;

    public GenericRepository(InventarioContext context)
    {
        _context = context;
    }



    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public async Task<InformacionProducto> DetalleProducto(int idProducto)
    {
        var detalle = (
               from p in _context.Productos
               join c in _context.CteProvs on p.CveProv equals c.ID
               where p.ID == idProducto
               select new InformacionProducto
               {
                   NomProd = p.NomProd,
                   Categoria = p.Categoria,
                   DescProd = p.DescProd,
                   NombEmpresa = p.CveProv + " - " + c.NombreEmpresa,
                   Precio = p.Precio,
                   Stock = p.Stock
               }
           ).FirstOrDefaultAsync();

        return await detalle;
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, 
        int pageSize, string search)
    {
        var totalRegistros = await _context.Set<T>()
                           .CountAsync();

        var registros = await _context.Set<T>()
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return (totalRegistros, registros);
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>()
            .Update(entity);
    }
}
