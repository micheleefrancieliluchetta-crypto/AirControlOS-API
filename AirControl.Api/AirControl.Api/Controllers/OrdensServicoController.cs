using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using AirControl.Api.Models;
using AirControl.Api.Models.Dtos;

namespace AirControl.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdensServicoController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdensServicoController(AppDbContext db) => _db = db;

    // =========================================================
    //  RESUMO / CONTAGEM  ->  GET /api/OrdensServico/contagem
    // =========================================================
    [HttpGet("contagem")]
    public async Task<ActionResult<object>> GetContagem()
    {
        var total = await _db.OrdensServico.CountAsync();
        var abertas = await _db.OrdensServico.CountAsync(o => o.Status == "Aberta");
        var emAndamento = await _db.OrdensServico.CountAsync(o => o.Status == "Em Andamento");
        var concluidas = await _db.OrdensServico.CountAsync(o => o.Status == "Concluída");

        return Ok(new
        {
            total,
            abertas,
            emAndamento,
            concluidas
        });
    }

    // =========================================================
    //  LISTA PAGINADA  ->  GET /api/OrdensServico
    // =========================================================
    [HttpGet]
    public async Task<ActionResult<object>> Get(
        [FromQuery] string? q,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100)
    {
        var query = _db.OrdensServico
            .Include(o => o.Cliente)
            .Include(o => o.Tecnico)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = q.Trim();
            query = query.Where(o =>
                (o.Cliente != null && (
                    (o.Cliente.Nome != null && o.Cliente.Nome.Contains(like)) ||
                    (o.Cliente.Endereco != null && o.Cliente.Endereco.Contains(like))
                )) ||
                (o.Descricao != null && o.Descricao.Contains(like)) ||
                (o.Observacoes != null && o.Observacoes.Contains(like))
            );
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(o => o.Status == status);

        var total = await query.CountAsync();
        var itens = await query
            .OrderByDescending(o => o.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new { total, itens };
    }

    // =========================================================
    //  DETALHE  ->  GET /api/OrdensServico/{id}
    // =========================================================
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdemServico>> GetById(int id)
    {
        var os = await _db.OrdensServico
            .Include(o => o.Cliente)
            .Include(o => o.Tecnico)
            .Include(o => o.Fotos)
            .FirstOrDefaultAsync(o => o.Id == id);

        return os is null ? NotFound() : os;
    }

    // =========================================================
    //  CRIAR OS (privado, tela interna) -> POST /api/OrdensServico
    // =========================================================
    [HttpPost]
    // [Authorize] // se quiser travar depois
    public async Task<IActionResult> Post([FromBody] OrdemServico os)
    {
        if (os == null)
            return BadRequest("Dados da OS inválidos.");

        os.Id = 0;
        os.DataAbertura = DateTime.Now;

        if (string.IsNullOrWhiteSpace(os.Status))
            os.Status = "Aberta";

        _db.OrdensServico.Add(os);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = os.Id }, os);
    }

    // =========================================================
    //  PRE-FLIGHT (OPTIONS) PÚBLICO -> OPTIONS /api/OrdensServico/publico
    // (nem seria obrigatório, CORS global já atende, mas deixei)
    // =========================================================
    [HttpOptions("publico")]
    [AllowAnonymous]
    public IActionResult OptionsPublic()
    {
        return Ok();
    }

    // =========================================================
    //  CRIAR OS PÚBLICA (abrir-os.html) -> POST /api/OrdensServico/publico
    // =========================================================
    [HttpPost("publico")]
    [AllowAnonymous]
    public async Task<IActionResult> PostPublic([FromBody] CriarOSDto dto)
    {
        int? clienteId = null;

        if (dto.ClienteId.HasValue && dto.ClienteId.Value > 0)
        {
            var ok = await _db.Clientes.AnyAsync(c => c.Id == dto.ClienteId.Value);
            if (!ok) return BadRequest("Cliente inexistente.");
            clienteId = dto.ClienteId.Value;
        }

        var os = new OrdemServico
        {
            ClienteId = clienteId,
            TecnicoId = null, // público não define
            Descricao = dto.Descricao,
            Prioridade = dto.Prioridade ?? "Baixa",
            Status = dto.Status ?? "Aberta",
            Observacoes = dto.Observacoes,
            Endereco = dto.Endereco,
            Lat = dto.Lat,
            Lng = dto.Lng,
            DataAbertura = DateTime.Now
        };

        _db.OrdensServico.Add(os);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = os.Id }, os);
    }

    // =========================================================
    //  ALTERAR STATUS (dashboard) -> PUT /api/OrdensServico/{id}/status
    // =========================================================
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> PutStatus(int id, [FromBody] StatusDto body)
    {
        var os = await _db.OrdensServico.FindAsync(id);
        if (os is null) return NotFound();

        var novo = body?.Status;
        if (!string.IsNullOrWhiteSpace(novo))
        {
            os.Status = novo;

            // se marcar como concluída, grava DataConclusao
            os.DataConclusao = novo.Contains("Conclu", StringComparison.OrdinalIgnoreCase)
                ? DateTime.Now
                : null;

            await _db.SaveChangesAsync();
        }

        return NoContent();
    }

    // =========================================================
    //  EXCLUIR OS  -> DELETE /api/OrdensServico/{id}
    // =========================================================
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var os = await _db.OrdensServico
            .Include(o => o.Fotos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (os is null)
            return NotFound();

        _db.OrdensServico.Remove(os);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

// DTO simples só para o PUT de status
public class StatusDto
{
    public string? Status { get; set; }
}

