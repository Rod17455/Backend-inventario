using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class CteProvConfiguration : IEntityTypeConfiguration<CteProv>
{
    public void Configure(EntityTypeBuilder<CteProv> builder)
    {
        builder.ToTable("Cte_prov");

        builder.Property(e => e.ID).IsRequired();
        builder.Property(e => e.Contacto).HasMaxLength(255);
        builder.Property(e => e.CorreoElectronico).HasMaxLength(255);
        builder.Property(e => e.Direccion).HasMaxLength(255);
        builder.Property(e => e.NombreEmpresa).HasMaxLength(255);
        builder.Property(e => e.Telefono).HasMaxLength(20);
    }
}
