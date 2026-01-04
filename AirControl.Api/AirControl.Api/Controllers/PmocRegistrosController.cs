using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]   // removido por enquanto
    public class PmocRegistrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PmocRegistrosController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/PmocRegistros
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPmocRegistroDto dto)
        {
            if (dto == null)
                return BadRequest("Dados do PMOC não enviados.");

            // 1) tenta pegar do token (quando você voltar a usar [Authorize])
            var email = User.Identity?.Name;

            // 2) se não tiver no token, usa o que veio no DTO
            if (string.IsNullOrWhiteSpace(email))
                email = dto.TecnicoEmail;

            // 3) se ainda estiver vazio, usa um valor padrão
            if (string.IsNullOrWhiteSpace(email))
                email = "pmoc@aircontrolos"; // <-- NÃO FICA NULO NO BANCO

            var registro = new PmocRegistro
            {
                AparelhoHdvId = dto.AparelhoHdvId,
                TecnicoEmail = email,                 // nunca nulo
                TecnicoNome = dto.TecnicoNome,       // vem do front
                Data = DateTime.UtcNow,              // data de criação
                ItensJson = dto.ItensJson ?? string.Empty,
                ObservacoesTecnicas = dto.ObservacoesTecnicas
            };

            _context.PmocRegistros.Add(registro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = registro.Id }, registro);
        }

        // GET api/PmocRegistros/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PmocRegistro>> ObterPorId(int id)
        {
            var registro = await _context.PmocRegistros.FindAsync(id);
            if (registro == null) return NotFound();
            return registro;
        }

        // GET api/PmocRegistros/por-aparelho/{aparelhoId}
        [HttpGet("por-aparelho/{aparelhoId}")]
        public async Task<ActionResult<IEnumerable<PmocRegistro>>> ListarPorAparelho(int aparelhoId)
        {
            var registros = await _context.PmocRegistros
                .Where(p => p.AparelhoHdvId == aparelhoId)
                .OrderByDescending(p => p.Data)
                .ToListAsync();

            return registros;
        }
    }

    // DTO que o frontend vai enviar
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }

        // JSON com os itens do checklist
        public string ItensJson { get; set; } = string.Empty;

        // observações gerais do técnico
        public string? ObservacoesTecnicas { get; set; }

        // quem está registrando
        public string? TecnicoEmail { get; set; }
        public string? TecnicoNome { get; set; }
    }
}