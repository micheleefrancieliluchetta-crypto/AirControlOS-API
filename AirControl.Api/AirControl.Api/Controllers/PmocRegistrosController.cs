using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Models;
using AirControl.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // => /api/PmocRegistros
    public class PmocRegistrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PmocRegistrosController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/PmocRegistros
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPmocRegistroDto dto)
        {
            if (dto == null)
                return BadRequest("Dados do PMOC não enviados.");

            if (dto.AparelhoHdvId <= 0)
                return BadRequest("AparelhoHdvId inválido.");

            var registro = new PmocRegistro
            {
                AparelhoHdvId       = dto.AparelhoHdvId,
                Data                = string.IsNullOrWhiteSpace(dto.Data)
                                        ? DateTime.UtcNow
                                        : DateTime.Parse(dto.Data),
                ChecklistJson       = dto.ChecklistJson ?? "[]",
                ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty,
                TecnicoNome         = dto.TecnicoNome,
                TecnicoEmail        = dto.TecnicoEmail
            };

            _context.PmocRegistros.Add(registro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = registro.Id }, registro);
        }

        // GET /api/PmocRegistros/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PmocRegistro>> ObterPorId(int id)
        {
            var registro = await _context.PmocRegistros.FindAsync(id);
            if (registro == null) return NotFound();
            return registro;
        }

        // GET /api/PmocRegistros/por-aparelho/3
        [HttpGet("por-aparelho/{aparelhoHdvId:int}")]
        public async Task<ActionResult<IEnumerable<PmocRegistro>>> ObterPorAparelho(int aparelhoHdvId)
        {
            var registros = await _context.PmocRegistros
                .Where(r => r.AparelhoHdvId == aparelhoHdvId)
                .OrderByDescending(r => r.Data)
                .ToListAsync();

            return Ok(registros);
        }

        [HttpOptions]
        public IActionResult Options() => Ok();
    }
}
