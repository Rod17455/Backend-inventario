using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class AutorizacionConfiguration : IEntityTypeConfiguration<Autorizacion>
{
    public void Configure(EntityTypeBuilder<Autorizacion> builder)
    {
        builder.ToTable("Autorizacion");
        builder.Property(e => e.ID).IsRequired();
        builder.Property(e => e.Id_Escasez).HasColumnType("int")
                .IsRequired()
                .HasColumnName("Escasez_prod_id");
        builder.Property(e => e.Id_Usuario).HasColumnType("int")
                .IsRequired()
                .HasColumnName("Usuario_id");
        builder.Property(e => e.Fecha_Autoriza).HasColumnType("datetime")
                .IsRequired()
                .HasColumnName("Fecha_autoriza");
        builder.Property(e => e.Estatus).HasColumnType("int")
                .IsRequired()
                .HasColumnName("Estado_autoriza");



    }
}
