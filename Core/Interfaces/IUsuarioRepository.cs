using Core.Entities;
using Core.Entities.Personalizadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario> GetByUserNameAsync(string userName);
    Task<Usuario> GetByRefreshAsync(string refreshToken);
    Task<InformacionUsuario> DetalleUsuario(int idUsuario);
    Task<string> ObtenerEmail(int id);

}
