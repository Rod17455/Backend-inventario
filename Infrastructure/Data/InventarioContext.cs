using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class InventarioContext : DbContext
{
    public InventarioContext(DbContextOptions<InventarioContext> options) : base(options)
    {
    }

    public  DbSet<CteProv> CteProvs { get; set; }

    public  DbSet<Producto> Productos { get; set; }

    public  DbSet<Reporte> Reportes { get; set; }

    public  DbSet<Usuario> Usuarios { get; set; }

    public  DbSet<Rol> Roles { get; set; }

    public DbSet<EscasezProducto> Escasezes { get; set; }

    public DbSet<Estatus> Estatuses { get; set; }
    public DbSet<Autorizacion> Autorizaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
