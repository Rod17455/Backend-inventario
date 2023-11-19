using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(InventarioContext context) : base(context)
    {
    }

    public async Task<InformacionUsuario> DetalleUsuario(int idUsuario)
    {
        var detalle = (
               from p in _context.Usuarios
               where p.ID == idUsuario
               select new InformacionUsuario
               {
                   ID = p.ID,
                   Apellido = p.Apellido,
                   CorreoElectronico = p.CorreoElectronico,
                   NomUser = p.NomUser
               }
           ).FirstOrDefaultAsync();

        return await detalle;
    }

    public override async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public override async Task<(int totalRegistros, IEnumerable<Usuario> registros)> GetAllAsync(
        int pageIndex, int pageSize, string search)
    {

        // var consulta = _context.Productos as IQueryable<Producto>;
        var consulta = _context.Usuarios as IQueryable<Usuario>;

        if (!String.IsNullOrEmpty(search))
        {
            consulta = consulta.Where(p => p.Apellido.ToLower().Contains(search));
        }

        var totalRegistros = await consulta.CountAsync();

        var registros = await consulta
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        return (totalRegistros, registros);
    }

    public async Task<Usuario> GetByRefreshAsync(string refreshToken)
    {
        return await _context.Usuarios
                        .Include(u => u.Roles)
                        .Include(u => u.RefreshTokens)
                        .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t=>t.Token==refreshToken));
    }

    public async Task<Usuario> GetByUserNameAsync(string userName)
    {
        return await _context.Usuarios
                             .Include(u => u.Roles)
                             .Include(u => u.RefreshTokens)
                             .FirstOrDefaultAsync(u => u.NomUser.ToLower() == userName.ToLower());
    }

    public async Task<string> ObtenerEmail(int id)
    {
        string email = _context.Usuarios
                        .Where(x => x.ID == 1)
                        .Select(x => x.CorreoElectronico)
                        .FirstOrDefault() ?? "";


        return await Task.FromResult(email);
    }
}
