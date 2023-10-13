using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    internal class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto");

            builder.Property(e => e.ID).IsRequired();
            builder.Property(e => e.Categoria).HasMaxLength(50);
            builder.Property(e => e.CveProv)
               .HasColumnType("int")
                .IsRequired()
                .HasColumnName("Cve_prov");
            builder.Property(e => e.DescProd)
                .HasColumnType("text")
                .HasColumnName("Desc_prod");
            builder.Property(e => e.NomProd)
                .HasMaxLength(255)
                .HasColumnName("Nom_prod");
            builder.Property(e => e.Precio).HasPrecision(10, 2);
           /* builder.Property(e => e.Imagen)
                .HasMaxLength(255)
                .HasColumnName("Imagen");*/
        }
    }
}
