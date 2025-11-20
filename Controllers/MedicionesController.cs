using Microsoft.AspNetCore.Mvc;
using Parcial2DDA.Data;
using Parcial2DDA.Models;
using Parcial2DDA.Models.Dtos;
using Parcial2DDA.Controllers;
namespace Parcial2DDA.Controllers
{
    [ApiController]
    [Route("medicion")]
    public class MedicionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicionesController(AppDbContext context)
        {
            _context = context;
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
            return Ok();
        }
    }

}
