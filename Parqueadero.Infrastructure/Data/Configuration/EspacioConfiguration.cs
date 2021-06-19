using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using System;

namespace Parqueadero.Infrastructure.Data.Configuration
{
    public class EspacioConfiguration : IEntityTypeConfiguration<Espacio>
    {
        public void Configure(EntityTypeBuilder<Espacio> builder)
        {
            builder.ToTable("Espacio");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Estado)
                    .IsRequired()
                    .HasConversion(x => x.ToString(),
                    x => (EstadosEspacios)Enum.Parse(typeof(EstadosEspacios), x));

            builder.Property(e => e.TipoVehiculo)
                    .HasConversion(x => x.ToString(),
                    x => (TiposVehiculos)Enum.Parse(typeof(TiposVehiculos), x));

            builder.Property(e => e.Placa)
                .HasMaxLength(10);

            builder.Property(e => e.FechaHoraIngreso)
                .HasColumnType("datetime");

            builder.Property(e => e.FechaHoraSalida)
                .HasColumnType("datetime");

        }
    }
}
