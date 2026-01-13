using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Dtos;
using AirControl.Api.Models; // onde está a entidade PmocRegistro
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // /api/PmocRegistros
    [EnableCors("AllowAll")]      // garante CORS nesse controller
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

            // Data: SEMPRE agora, horário do servidor (UTC)
            // (no front você já usa new Date(...).toLocaleString("pt-BR"))
            var data = DateTime.UtcNow;

            var registro = new PmocRegistro
            {
                AparelhoHdvId       = dto.AparelhoHdvId,
                Data                = data,
                ChecklistJson       = string.IsNullOrWhiteSpace(dto.ChecklistJson)
                                        ? "[]"
                                        : dto.ChecklistJson,
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
            if (registro == null)
                return NotFound();

            return registro;
        }

        // GET /api/PmocRegistros/por-aparelho/{aparelhoId}
        [HttpGet("por-aparelho/{aparelhoId:int}")]
        public async Task<ActionResult<IEnumerable<PmocRegistro>>> ObterPorAparelho(int aparelhoId)
        {
            var regs = await _context.PmocRegistros
                .Where(r => r.AparelhoHdvId == aparelhoId)
                .OrderByDescending(r => r.Data)
                .ToListAsync();

            return regs;
        }

        // OPTIONS /api/PmocRegistros  (pré-flight CORS)
        [HttpOptions]
        public IActionResult Preflight()
        {
            // libera geral só pra essa rota
            Response.Headers["Access-Control-Allow-Origin"]  = "*";
            Response.Headers["Access-Control-Allow-Methods"] = "GET,POST,OPTIONS";
            Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";

            return Ok();
        }
    }
}
