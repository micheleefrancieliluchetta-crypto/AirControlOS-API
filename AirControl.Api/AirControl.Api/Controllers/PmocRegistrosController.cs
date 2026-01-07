using System;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/PmocRegistros
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

            // ======== TRATANDO A DATA PARA NÃO DAR ERRO NO POSTGRES ========
            DateTime dataFinal;

            if (string.IsNullOrWhiteSpace(dto.Data))
            {
                // sem data -> agora em UTC (Kind = Utc)
                dataFinal = DateTime.UtcNow;
            }
            else
            {
                if (!DateTime.TryParse(dto.Data, out var parsed))
                    return BadRequest("Data inválida.");

                // a string "2026-01-07" vira DateTime com Kind Unspecified.
                // Aqui forçamos para UTC pra evitar o erro do Npgsql.
                dataFinal = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
            }
            // =================================================================

            var registro = new PmocRegistro
            {
                AparelhoHdvId       = dto.AparelhoHdvId,
                Data                = dataFinal,
                ChecklistJson       = dto.ChecklistJson ?? "[]",
                ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty
                // se depois quiser de novo TecnicoNome/Email é só adicionar aqui.
            };

            _context.PmocRegistros.Add(registro);
            await _context.SaveChangesAsync();

            // retorna 201 com o registro criado
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

        // OPTIONS pra pré-flight CORS
        [HttpOptions]
        public IActionResult Options() => Ok();
    }

    // DTO usado no POST
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId        { get; set; }
        public string? Data             { get; set; }
        public string? ChecklistJson    { get; set; }
        public string? ObservacoesTecnicas { get; set; }
    }
}
