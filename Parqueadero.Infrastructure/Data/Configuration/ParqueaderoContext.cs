using Microsoft.EntityFrameworkCore;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace Parqueadero.Infrastructure.Data
{
    public class ParqueaderoContext : DbContext
    {

        public ParqueaderoContext(DbContextOptions<ParqueaderoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            for (int i = 1; i <= 20; i++)
            {
                modelBuilder.Entity<Espacio>().HasData(
                    new Espacio
                    {
                        Id = Guid.NewGuid(),
                        NumeroEspacio = i,
                        Estado = EstadosEspacios.Disponible,
                        TipoVehiculo = TiposVehiculos.Carro
                    });
            }
            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<Espacio>().HasData(
                    new Espacio
                    {
                        Id = Guid.NewGuid(),
                        NumeroEspacio = i,
                        Estado = EstadosEspacios.Disponible,
                        TipoVehiculo = TiposVehiculos.Moto
                    });
            }

            base.OnModelCreating(modelBuilder);

        }
    }
}
