using AirControl.Api.Data;
using AirControl.Api.Dtos;
using AirControl.Api.Models;         // onde está PmocRegistro
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // /api/PmocRegistros
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

            // monta entidade para salvar
            var registro = new PmocRegistro
            {
                AparelhoHdvId = dto.AparelhoHdvId,
                Data = string.IsNullOrWhiteSpace(dto.Data)
                            ? DateTime.UtcNow
                            : DateTime.Parse(dto.Data),
                ChecklistJson       = dto.ChecklistJson ?? "[]",
                ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty
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

        // GET /api/PmocRegistros/por-aparelho/{aparelhoId}
        [HttpGet("por-aparelho/{aparelhoId:int}")]
        public async Task<ActionResult<IEnumerable<PmocRegistro>>> ObterPorAparelho(int aparelhoId)
        {
            var registros = await _context.PmocRegistros
                .Where(r => r.AparelhoHdvId == aparelhoId)
                .OrderByDescending(r => r.Data)
                .ToListAsync();

            return Ok(registros);
        }

        // OPTIONS (ajuda navegadores com CORS preflight)
        [HttpOptions]
        public IActionResult Options() => Ok();
    }
}
