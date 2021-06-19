using Microsoft.Extensions.Options;
using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using Parqueadero.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parqueadero.Core.Services
{
    public class EspacioService : IEspacioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PicoyPlaca _picoYPlaca;
        private readonly Tarifas _tarifas;
        public EspacioService(IUnitOfWork unitOfWork, IOptions<PicoyPlaca> picoYPlaca, IOptions<Tarifas> tarifas)
        {
            _unitOfWork = unitOfWork;
            _picoYPlaca = picoYPlaca.Value;
            _tarifas = tarifas.Value;
        }

        public async Task<IEnumerable<Espacio>> ObtenerEspaciosbyTipoVehiculo(Espacio espacio)
        {
            var tipoVehiculo = espacio.TipoVehiculo;
            var espacios = await _unitOfWork.EspacioRepository.ObtenerEspaciosbyTipoVehiculo(tipoVehiculo);
            return espacios;
        }

        public async Task<Espacio> ConsultaSaldo(Espacio espacio)
        {
            var placa = espacio.Placa;
            var espaciobyPlaca = await _unitOfWork.EspacioRepository.ObtenerEspaciobyPlaca(placa);
            if (espaciobyPlaca == null)
                throw new Exception("Vehiculo no esta en el parqueadero");
            var ValorSalida = CalcularValorServicio(espaciobyPlaca.TipoVehiculo, espaciobyPlaca.Cilindraje.Value, espaciobyPlaca.FechaHoraIngreso.Value, DateTime.UtcNow);
            espaciobyPlaca.ValorServicio = ValorSalida;
            return espaciobyPlaca;
        }

        public async Task<Espacio> IngresoVehiculo(Espacio espacio)
        {
            var picoyPlaca = _picoYPlaca;

            var respuesta = ValidarVehiculoPicoyPlaca(espacio.Placa, espacio.TipoVehiculo, DateTime.UtcNow, picoyPlaca);

            if (respuesta == TiposRespuestas.PicoyPlaca)
            {
                throw new Exception("El vehiculo tiene pico y placa");
            }
            else if (respuesta == TiposRespuestas.FormatoInvalido)
            {
                throw new Exception("Formato Invalido");
            }

            var espaciobyPlaca = await _unitOfWork.EspacioRepository.ObtenerEspaciobyPlaca(espacio.Placa);

            if (espaciobyPlaca != null)
                throw new Exception("Este vehiculo ya esta en el parqueadero");

            var espaciosDisponibles = await _unitOfWork.EspacioRepository.ObtnenerDisponibilidad(espacio);

            if (espaciosDisponibles.Count() == 0)
            {
                throw new Exception("No hay Espacio disponible");
            }

            var espacioActual = await _unitOfWork.EspacioRepository.GetById(espaciosDisponibles.First().Id);
            espacioActual.Estado = EstadosEspacios.Ocupado;
            espacioActual.Placa = espacio.Placa;
            espacioActual.Cilindraje = espacio.Cilindraje;
            espacioActual.FechaHoraIngreso = DateTime.UtcNow;
            espacioActual.FechaHoraSalida = null;
            espacioActual.ValorServicio = 0;
            _unitOfWork.EspacioRepository.Update(espacioActual);
            await _unitOfWork.SaveChangesAsync();
            return espacioActual;
        }

        public async Task<Espacio> SalidaVehiculo(Espacio espacio)
        {
            var espacioActual = await _unitOfWork.EspacioRepository.ObtenerEspaciobyPlaca(espacio.Placa);
            if (espacioActual == null)
                throw new Exception("Vehiculo no esta en el parqueadero");
            var valorServicio = CalcularValorServicio(espacioActual.TipoVehiculo, espacioActual.Cilindraje.Value, espacioActual.FechaHoraIngreso.Value, DateTime.UtcNow);
            espacioActual.FechaHoraSalida = DateTime.UtcNow;
            espacioActual.Estado = EstadosEspacios.Disponible;
            espacioActual.Cilindraje = 0;
            espacioActual.ValorServicio = valorServicio;
            espacioActual.FechaHoraIngreso = null;
            _unitOfWork.EspacioRepository.Update(espacioActual);
            await _unitOfWork.SaveChangesAsync();
            return espacioActual;
        }
        public TiposRespuestas ValidarVehiculoPicoyPlaca(string placa, TiposVehiculos? tipoVehiculo, DateTime fecha, PicoyPlaca picoyPlaca)
        {
            var ultimoDigito = 0;
            if (placa.Length != 6)
                return TiposRespuestas.FormatoInvalido;

            var posicion = tipoVehiculo == TiposVehiculos.Carro ? 1 : 2;
            var ultimoDigitoPlaca = placa.Substring(placa.Length - posicion, 1);
            if (!Int32.TryParse(ultimoDigitoPlaca, out ultimoDigito))
                return TiposRespuestas.FormatoInvalido;

            var placas = picoyPlaca.GetType().GetProperties();
            var dia = placas.Where(a =>
                        a.Name == fecha.DayOfWeek.ToString())
                        .FirstOrDefault();

            if (dia != null)
            {
                var listaPlacas = dia.GetValue(picoyPlaca) as IEnumerable;
                foreach (var valorPlaca in listaPlacas)
                {
                    if (valorPlaca.Equals(ultimoDigito))
                    {
                        return TiposRespuestas.PicoyPlaca;
                    }
                }
            }

            return TiposRespuestas.Ok;
        }

        public decimal CalcularValorServicio(TiposVehiculos tipoVehiculo, decimal cilindraje, DateTime fechaIngreso, DateTime fechaSalida)
        {
            decimal valorSalida = 0;
            var nroHoras = (fechaSalida - fechaIngreso).Hours + (fechaSalida - fechaIngreso).Days * 24;

            int minutos = (fechaSalida - fechaIngreso).Minutes;
            int dias = nroHoras / 24;
            int horas = nroHoras % 24;

            if (minutos > 0)
                horas += 1;

            if (horas >= 9)
            {
                dias += 1;
                horas = 0;
            }


            if (tipoVehiculo == TiposVehiculos.Carro)
            {
                valorSalida = _tarifas.CarroHora * horas + _tarifas.CarroDia * dias;
            }
            else if (cilindraje > 500)
            {
                valorSalida = _tarifas.MotoDia * dias + _tarifas.MotoHora * horas + _tarifas.SobreCargo;
            }
            else
            {
                valorSalida = _tarifas.MotoDia * dias + _tarifas.MotoHora * horas;
            }
            return valorSalida;
        }
    }
}
