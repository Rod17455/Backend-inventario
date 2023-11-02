using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class EstatusConfiguration : IEntityTypeConfiguration<Estatus>
{
    public void Configure(EntityTypeBuilder<Estatus> builder)
    {
        builder.ToTable("Estatus");

        builder.Property(e => e.ID).IsRequired();
        builder.Property(e => e.NombEstatus).HasColumnName("Estatus");
    }
}
