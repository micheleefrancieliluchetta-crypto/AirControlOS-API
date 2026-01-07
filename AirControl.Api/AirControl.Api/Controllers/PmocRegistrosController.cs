using System;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;

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

        // DTO que a API recebe
        public class CriarPmocRegistroDto
        {
            public int AparelhoHdvId { get; set; }
            public string? Data { get; set; }                 // "2026-01-07"
            public string? ChecklistJson { get; set; } = "[]";
            public string? ObservacoesTecnicas { get; set; }
            public string? TecnicoNome { get; set; }
            public string? TecnicoEmail { get; set; }
        }

        // POST /api/PmocRegistros
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPmocRegistroDto dto)
        {
            if (dto == null)
                return BadRequest("Dados do PMOC não enviados.");

            if (dto.AparelhoHdvId <= 0)
                return BadRequest("AparelhoHdvId inválido.");

            // === CONCERTO DO DateTime ===
            DateTime dataUtc;
            if (string.IsNullOrWhiteSpace(dto.Data))
            {
                dataUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            }
            else
            {
                if (!DateTime.TryParse(dto.Data, out var dataParsed))
                    return BadRequest("Data inválida.");

                dataUtc = DateTime.SpecifyKind(dataParsed, DateTimeKind.Utc);
            }
            // =============================

            var registro = new PmocRegistro
            {
                AparelhoHdvId       = dto.AparelhoHdvId,
                Data                = dataUtc,
                ChecklistJson       = dto.ChecklistJson ?? "[]",
                ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty,
                TecnicoNome         = dto.TecnicoNome ?? string.Empty,
                TecnicoEmail        = dto.TecnicoEmail ?? string.Empty
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

        // OPTIONS pra ajudar em preflight CORS
        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }
    }
}

