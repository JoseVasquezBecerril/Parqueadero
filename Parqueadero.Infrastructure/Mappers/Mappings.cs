using AutoMapper;
using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using System;

namespace Parqueadero.Infrastructure.Mappers
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Espacio, EspacioDto>();
            CreateMap<EspacioDto, Espacio>();

            CreateMap<Espacio, EspacioIngreso>();
            CreateMap<EspacioIngreso, Espacio>();

            CreateMap<Espacio, EspacioPlaca>();
            CreateMap<EspacioPlaca, Espacio>();

            CreateMap<Espacio, EspacioSalida>();
            CreateMap<EspacioSalida, Espacio>();

            CreateMap<Espacio, EspacioConsultaSaldo>();
            CreateMap<EspacioConsultaSaldo, Espacio>();

            CreateMap<Espacio, EspacioTipoVehiculo>();
            CreateMap<EspacioTipoVehiculo, Espacio>();
        }
    }
}
