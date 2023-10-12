using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class InventarioContextSeed
{
    public static async Task SeedRolesAsync(InventarioContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Rol>()
                        {
                            new Rol{ID=1, Nombre="Administrador"},
                            new Rol{ID=2, Nombre="Gerente"},
                            new Rol{ID=3, Nombre="Empleado"},
                        };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<InventarioContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}
