using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class EscasesRepository : GenericRepository<InformacionEscasez>, IEscasezRepository
{
    public EscasesRepository(InventarioContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<InformacionEscasez>> GetAllAsync()
    {
        var result = (
            from p in _context.Escasezes
            join c in _context.Productos on p.ProductoId equals c.ID
            join u in _context.Usuarios on p.UsuarioId equals u.ID
            select new InformacionEscasez
            {
                ID = p.ID,
                Estatus = p.Estatus,
                Cant_Soli = p.Cant_Soli,
                Fecha_Registro = p.Fecha_Registro,
                Nom_Producto = c.NomProd,
                Nom_Usuario = u.Apellido,
                Precio = p.Precio
                
            }).ToListAsync();

        return await result;
    }

    public override async Task<(int totalRegistros, IEnumerable<InformacionEscasez> registros)> GetAllAsync(
        int pageIndex, int pageSize, string search)
    {

        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = (
           from p in _context.Escasezes
           join c in _context.Productos on p.ProductoId equals c.ID
           join u in _context.Usuarios on p.UsuarioId equals u.ID
           select new InformacionEscasez
           {
               ID = p.ID,
               Estatus = p.Estatus,
               Cant_Soli = p.Cant_Soli,
               Fecha_Registro = p.Fecha_Registro,
               Nom_Producto = c.NomProd,
               Nom_Usuario = u.Apellido,
               Precio = p.Precio
           });
        // Solo muestra las que son diferentes del estatus 4
        var _consulta = consulta as IQueryable<InformacionEscasez>;

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.Nom_Producto.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }


    public async Task<int> ActualizarEstatusProductoPendiente(int idProducto)
    {
        int update = _context.Database.ExecuteSqlRaw(@"UPDATE Producto
                                                                SET Estatus = {0}
                                                                WHERE ID = {1}", 4, idProducto);

        return await Task.FromResult(update);
    }

    public async Task<int> ObtenerCantidadProducto(int idProducto)
    {
        var cantidad = await _context.Productos.Where(p => p.ID == idProducto).Select(p => p.Stock).FirstAsync();

        return (int)cantidad;

    }

    public async Task<decimal> ObtenerPrecioProducto(int idProducto)
    {
        var precio = await _context.Productos.Where(p => p.ID == idProducto).Select(p => p.Precio).FirstAsync();

        return (decimal)precio;
    }

    public async Task<int> InsertarEscasez(EscasezProducto escasezProducto)
    {
        int insertEscasez = _context
                               .Database
                               .ExecuteSqlRaw(@"
                                    INSERT INTO Escasez_Producto
                                    (
                                    ProductoId,
                                    Cant_Soli,
                                    Fecha_registro,
                                    UsuarioId,
                                    Estatus,
                                    Precio)
                                    VALUES
                                    ({0},{1},{2},{3},{4},{5})
                                 ", escasezProducto.ProductoId, escasezProducto.Cant_Soli, escasezProducto.Fecha_Registro,
                                    escasezProducto.UsuarioId, escasezProducto.Estatus, escasezProducto.Precio);


        return await Task.FromResult(insertEscasez);
    }
}
