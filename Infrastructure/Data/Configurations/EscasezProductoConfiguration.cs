using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class EscasezProductoConfiguration : IEntityTypeConfiguration<EscasezProducto>
{
    public void Configure(EntityTypeBuilder<EscasezProducto> builder)
    {
        builder.ToTable("Escasez_Producto");
        builder.Property(e => e.ID).IsRequired();
        builder.Property(e => e.ProductoId).HasColumnType("int")
                .IsRequired()
                .HasColumnName("ProductoId");
        builder.Property(e => e.Cant_Soli).HasColumnType("int")
                .IsRequired()
                .HasColumnName("Cant_Soli");
        builder.Property(e => e.Fecha_Registro).HasColumnType("datetime")
                .IsRequired()
                .HasColumnName("Fecha_Registro");
        builder.Property(e => e.UsuarioId).HasColumnType("int")
                .IsRequired()
                .HasColumnName("UsuarioId");
        builder.Property(e => e.Estatus)
            .HasMaxLength(20)
            .HasColumnName("Estatus");
        builder.Property(e => e.Precio).HasPrecision(10, 2);



    }
}
