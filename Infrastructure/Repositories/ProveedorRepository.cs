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

public class ProveedorRepository : GenericRepository<CteProv>, IProveedorRepository
{
    public ProveedorRepository(InventarioContext context) : base(context)
    {
    }

    public async Task<int> GetTotalProveedores()
    {
        var cantidadProveedores = (from proveedor in _context.CteProvs
                                   select proveedor).CountAsync();
        return await cantidadProveedores;
    }

    public override async Task<CteProv> GetByIdAsync(int id)
    {
        return await _context.CteProvs.FirstOrDefaultAsync(p => p.ID == id);
    }

    public override async Task<IEnumerable<CteProv>> GetAllAsync()
    {
        return await _context.CteProvs.ToListAsync();   
    }

    public override async Task<(int totalRegistros, IEnumerable<CteProv> registros)> GetAllAsync(
        int pageIndex, int pageSize, string search)
    {

        var consulta = _context.CteProvs as IQueryable<CteProv>;    

        if(!String.IsNullOrEmpty(search))
        {
            consulta = consulta.Where(p => p.NombreEmpresa.ToLower().Contains(search));
        }

        var totalRegistros = await consulta.CountAsync();

        var registros = await consulta
                                    .Skip((pageIndex -1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros); 
    }

    public async Task<string> ObtenerEmail(int id)
    {
        var dato = (
           from pro in _context.Productos
           join cte in _context.CteProvs on pro.CveProv equals cte.ID
           where pro.ID == id
           select cte.CorreoElectronico).FirstOrDefaultAsync();


        return await dato ?? "";
    }
}
