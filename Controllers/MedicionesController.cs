using Microsoft.AspNetCore.Mvc;
using Parcial2DDA.Data;
using Parcial2DDA.Models;
using Parcial2DDA.Models.Dtos;
using Parcial2DDA.Controllers;
using Parcial2DDA.Models.Servicios;
namespace Parcial2DDA.Controllers
{
    [ApiController]
    [Route("medicion")]
    public class MedicionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CalculosService _calculoService;
        public MedicionesController(AppDbContext context, CalculosService calculoService)
        {
            _context = context;
            _calculoService = calculoService;
        }
        [HttpPost("entrada")]
        public async Task<ActionResult> RegistrarEntrada([FromBody] MedicionDto medicion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            {
                Medicion nuevaMedicion = new Medicion()
                {
                    Huella = medicion.huella,
                    Peso = medicion.peso,
                    Tipo = medicion.tipo,
                    TiempoEntrada = DateTime.Now,
                    
                };
                _context.Add(nuevaMedicion);
                await _context.SaveChangesAsync();

                return Ok(nuevaMedicion);
            }
            
        }

        [HttpPut("salida")]
        public async Task<ActionResult> RegistrarSalida([FromBody] MedicionSalidaDto medicionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Medicion medicionEntrada = _context.Mediciones.FirstOrDefault(m=>m.Huella == medicionDto.huella);
            medicionEntrada.TiempoSalida= DateTime.Now;
            _context.Update(medicionEntrada);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("/reportes/totales")]
        public async Task<ActionResult<int>> TotalReportes()
        {
            List<Medicion> medicionesCompletadas = _context.Mediciones
                .Where(medicion=>medicion.TiempoSalida != DateTime.MinValue).ToList();
            MedicionCompletaDto reporte = new MedicionCompletaDto()
            {
                total_mediciones_completas = medicionesCompletadas.Count
            };
            return Ok(reporte);
        }
        [HttpGet("tiempoTotal")]
        public async Task<ActionResult> TiempoTotalCliente(string huella)
        {
            Medicion medicion = _context.Mediciones.FirstOrDefault(medicion=>medicion.Huella== huella);
            
            int totalTiempo = _calculoService.CantidadTiempoEnLocal(medicion);
            
            return Ok($"El tiempo total que estubo el cliente son {totalTiempo} segundos");
        }
    }

}
