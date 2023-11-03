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

public class AutorizacionRepository : GenericRepository<InformacionAutorizacion>, IAutorizacionRepository
{
    public AutorizacionRepository(InventarioContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<InformacionAutorizacion>> GetAllAsync()
    {
        var result = (
           from a in _context.Autorizaciones
           join e in _context.Escasezes on a.Id_Escasez equals e.ID
           join u in _context.Usuarios on a.Id_Usuario equals u.ID
           join s in _context.Estatuses on a.Estatus equals s.ID
           join p in _context.Productos on e.ProductoId equals p.ID
           select new InformacionAutorizacion
           {
               ID = a.ID,
               Cantidad = e.Cant_Soli,
               Fecha_Autorizacion = a.Fecha_Autoriza,
               Nombre_Empleado = u.Apellido,
               Nombre_Producto = p.NomProd,
               Precio = e.Precio

           }).ToListAsync();

        return await result;
    }

    public override async Task<(int totalRegistros, IEnumerable<InformacionAutorizacion> registros)> GetAllAsync(
       int pageIndex, int pageSize, string search)
    {

        var consulta = (
           from a in _context.Autorizaciones
           join e in _context.Escasezes on a.Id_Escasez equals e.ID
           join u in _context.Usuarios on a.Id_Usuario equals u.ID
           join s in _context.Estatuses on a.Estatus equals s.ID
           join p in _context.Productos on e.ProductoId equals p.ID
           select new InformacionAutorizacion
           {
               ID = a.ID,
               Cantidad = e.Cant_Soli,
               Fecha_Autorizacion = a.Fecha_Autoriza,
               Nombre_Empleado = u.Apellido,
               Nombre_Producto = p.NomProd,
               Precio = e.Precio

           });

        var _consulta = consulta as IQueryable<InformacionAutorizacion>;

        if (!String.IsNullOrEmpty(search))
        {
            _consulta = _consulta.Where(p => p.Nombre_Producto.ToLower().Contains(search));
        }

        var totalRegistros = await _consulta.CountAsync();

        var registros = await _consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);

    }


}
