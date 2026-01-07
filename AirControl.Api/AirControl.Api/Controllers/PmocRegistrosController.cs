using System;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Models;
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
                ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty
                // se algum dia quiser voltar com Técnico, pode completar aqui:
                // TecnicoNome  = dto.TecnicoNome,
                // TecnicoEmail = dto.TecnicoEmail
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
            if (registro == null)
                return NotFound();

            return registro;
        }

        // ajuda em pré-flight OPTIONS (CORS)
        [HttpOptions]
        public IActionResult Options() => Ok();
    }

    // DTO usado no POST
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }
        public string? Data { get; set; }
        public string? ChecklistJson { get; set; }
        public string? ObservacoesTecnicas { get; set; }

        // se quiser voltar com técnico depois:
        // public string? TecnicoNome  { get; set; }
        // public string? TecnicoEmail { get; set; }
    }
}
