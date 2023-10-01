using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {

        builder.ToTable("Usuario");
        builder.Property(e => e.ID).IsRequired();
        builder.HasIndex(e => e.CorreoElectronico, "CorreoElectronico").IsUnique();
        builder.Property(e => e.Apellido).HasMaxLength(255);
        builder.Property(e => e.NomUser)
            .HasMaxLength(255)
            .HasColumnName("Nom_user");
        builder.Property(e => e.Password).HasMaxLength(255);
        builder.Property(e => e.Rol).HasMaxLength(20);
    }
}
