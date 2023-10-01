using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class ReporteConfiguration : IEntityTypeConfiguration<Reporte>
{
    public void Configure(EntityTypeBuilder<Reporte> builder)
    {

        builder.ToTable("Reporte");

        builder.Property(e => e.ID).IsRequired();
        builder.Property(e => e.CveUsr)
            .HasColumnType("int")
            .IsRequired()
            .HasColumnName("Cve_usr");
        builder.Property(e => e.DatosReporte).HasColumnType("text");
        builder.Property(e => e.FechaGeneracion)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp");
        builder.Property(e => e.TipoReporte).HasMaxLength(50);
    }
}
