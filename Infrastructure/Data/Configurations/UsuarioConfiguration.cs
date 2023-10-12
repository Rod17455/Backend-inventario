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
        builder
          .HasMany(p => p.Roles)
          .WithMany(p => p.Usuarios)
          .UsingEntity<UsuarioRoles>(
                j => j
                     .HasOne(pt => pt.Rol)
                     .WithMany(t => t.UsuarioRoles)
                     .HasForeignKey(pt => pt.RolId),
                j => j
                     .HasOne(pt => pt.Usuario)
                     .WithMany(p => p.UsuarioRoles)
                     .HasForeignKey(pt => pt.UsuarioId),
                j =>
                {
                    j.HasKey(t => new
                    {
                        t.UsuarioId,
                        t.RolId

                    });
                }
            );
        builder.HasMany(p => p.RefreshTokens)
           .WithOne(p => p.Usuario)
           .HasForeignKey(p => p.UsuarioId);
       
    }
}
