using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // POST api/PmocRegistros/salvar
        [HttpPost("salvar")]
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
                TecnicoNome         = dto.TecnicoNome ?? string.Empty,
                TecnicoEmail        = dto.TecnicoEmail ?? "pmoc@aircontrolos"
            };

            _context.PmocRegistros.Add(registro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = registro.Id }, registro);
        }

        // GET api/PmocRegistros/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PmocRegistro>> ObterPorId(int id)
        {
            var registro = await _context.PmocRegistros.FindAsync(id);
            if (registro == null) return NotFound();
            return registro;
        }

        // ajuda em pré-flight OPTIONS
        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }
    }

    public class CriarPmocRegistroDto
    {
        public int    AparelhoHdvId       { get; set; }
        public string? Data               { get; set; }
        public string ChecklistJson       { get; set; } = "[]";
        public string? ObservacoesTecnicas { get; set; }
        public string? TecnicoNome        { get; set; }
        public string? TecnicoEmail       { get; set; }
    }
}
