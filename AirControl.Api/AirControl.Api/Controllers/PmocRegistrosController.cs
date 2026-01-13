using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Dtos;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

            // Data: se vier vazia, usa agora (UTC)
              DateTime data;
             if (string.IsNullOrWhiteSpace(dto.Data)) &&
             data = DateTime.UtcNow;
                  else if (!DateTime.TryParse(dto.Data, out data))
                  return BadRequest("Data em formato inválido.");

             var registro = new PmocRegistro
           {
                  AparelhoHdvId       = dto.AparelhoHdvId,
                  Data                = data,
                  ChecklistJson       = dto.ChecklistJson ?? "[]",
                  ObservacoesTecnicas = dto.ObservacoesTecnicas ?? string.Empty,
                  TecnicoNome         = dto.TecnicoNome,
                  TecnicoEmail        = dto.TecnicoEmail
            };

           _context.PmocRegistros.Add(registro);
            await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(ObterPorId), new { id = registro.Id }, registro);
           }

            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message} - {ex.InnerException?.Message}");
            }
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
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            Response.Headers["Access-Control-Allow-Methods"] = "GET,POST,OPTIONS";
            Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
            return Ok();
        }
    }
}
