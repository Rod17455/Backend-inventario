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

    public virtual DbSet<CteProv> CteProvs { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
