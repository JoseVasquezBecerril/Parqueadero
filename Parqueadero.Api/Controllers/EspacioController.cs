using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using Parqueadero.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Parqueadero.Api.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class EspacioController : ControllerBase
    {
        private readonly IEspacioService _espacioService;
        private readonly IMapper _mapper;
        public EspacioController(IEspacioService espacioService, IMapper mapper)
        {
            _espacioService = espacioService;
            _mapper = mapper;
        }

        [HttpGet("tipoVehiculo/{tipoVehiculo}")]
        public async Task<ActionResult> ObtenerEspacios(TiposVehiculos tipoVehiculo)
        {
            try
            {
                var espacioTipoVehiculo = new EspacioTipoVehiculo { TipoVehiculo = tipoVehiculo };
                var espacio = _mapper.Map<Espacio>(espacioTipoVehiculo);
                var result = await _espacioService.ObtenerEspaciosbyTipoVehiculo(espacio);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensaje = e.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> IngresarVehiculo(EspacioDto espacioDto)
        {
            try
            {
                var espacio = _mapper.Map<Espacio>(espacioDto);
                var result = await _espacioService.IngresoVehiculo(espacio);
                return Ok(_mapper.Map<EspacioIngreso>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensaje = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> RetirarVehiculo(EspacioPlaca espacioPlaca)
        {
            try
            {
                var espacio = _mapper.Map<Espacio>(espacioPlaca);
                var result = await _espacioService.SalidaVehiculo(espacio);
                return Ok(_mapper.Map<EspacioSalida>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensaje = e.Message });
            }

        }

        [HttpGet("placa/{placa}")]
        public async Task<ActionResult> ConsultarSaldo(string placa)
        {
            try
            {
                var espacioPlaca = new EspacioPlaca { Placa = placa };
                var espacio = _mapper.Map<Espacio>(espacioPlaca);
                var result = await _espacioService.ConsultaSaldo(espacio);
                return Ok(_mapper.Map<EspacioConsultaSaldo>(result));
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensaje = e.Message });
            }

        }

    }
}

