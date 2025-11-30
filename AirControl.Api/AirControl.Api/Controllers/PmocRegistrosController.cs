using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]   // removido por enquanto\\
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
            // pega o email do usuário logado (do token JWT)
            var email = User.Identity?.Name ?? dto.TecnicoEmail;

            var registro = new PmocRegistro
            {
                AparelhoHdvId = dto.AparelhoHdvId,
                TecnicoEmail = email!,
                Data = DateTime.UtcNow,
                ItensJson = dto.ItensJson
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

        // GET api/PmocRegistros/por-aparelho/5
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
        public string ItensJson { get; set; } = string.Empty;

        // opcional, só para testes
        public string? TecnicoEmail { get; set; }
    }
}
