using System;
using System.Linq;
using System.Threading.Tasks;
using AirControl.Api.Data;
using AirControl.Api.Models;
using AirControl.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacoesPecaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitacoesPecaController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/SolicitacoesPeca
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarSolicitacaoPecaDto dto)
        {
            if (dto == null) return BadRequest("Dados não enviados.");
            if (dto.OrdemServicoId <= 0) return BadRequest("OrdemServicoId inválido.");
            if (string.IsNullOrWhiteSpace(dto.NomePeca)) return BadRequest("Nome da peça é obrigatório.");

            var obj = new SolicitacaoPeca
            {
                OrdemServicoId = dto.OrdemServicoId,
                NomePeca = dto.NomePeca,
                Quantidade = dto.Quantidade <= 0 ? 1 : dto.Quantidade,
                Cliente = dto.Cliente,
                Unidade = dto.Unidade,
                TecnicoNome = dto.TecnicoNome,
                Observacao = dto.Observacao,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Pendente" : dto.Status,
                DataCriacao = DateTime.UtcNow
            };

            _context.SolicitacoesPeca.Add(obj);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = obj.Id }, obj);
        }

        // GET api/SolicitacoesPeca/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SolicitacaoPeca>> ObterPorId(int id)
        {
            var item = await _context.SolicitacoesPeca.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        // GET api/SolicitacoesPeca?status=Pendente
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? status = null)
        {
            var query = _context.SolicitacoesPeca
                .Include(p => p.OrdemServico)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status == status);

            var lista = await query
                .OrderByDescending(p => p.DataCriacao)
                .Select(p => new
                {
                    p.Id,
                    p.NomePeca,
                    p.Quantidade,
                    p.Status,
                    p.Cliente,
                    p.Unidade,
                    Tecnico = p.TecnicoNome,
                    NumeroOS = p.OrdemServico != null ? p.OrdemServico.Id : 0,   // ajusta se tiver outro campo
                    DataOS = p.OrdemServico != null ? p.OrdemServico.DataAbertura : (DateTime?)null
                })
                .ToListAsync();

            return Ok(lista);
        }

        // PUT api/SolicitacoesPeca/{id}/status
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusSolicitacaoPecaDto dto)
        {
            var item = await _context.SolicitacoesPeca.FindAsync(id);
            if (item == null) return NotFound();

            item.Status = dto.Status;
            item.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ------------- MÉTODO AUXILIAR PARA SINCRONIZAR COM A OS -------------

        // você chama isso na ação que salva a OS (Criar/Atualizar)
        public async Task AtualizarSolicitacoesPecaAsync(
            int ordemServicoId,
            System.Collections.Generic.List<PecaDto> pecas,
            string? cliente,
            string? unidade,
            string? tecnicoNome)
        {
            var antigos = _context.SolicitacoesPeca
                .Where(p => p.OrdemServicoId == ordemServicoId);

            _context.SolicitacoesPeca.RemoveRange(antigos);

            foreach (var peca in pecas)
            {
                var nova = new SolicitacaoPeca
                {
                    OrdemServicoId = ordemServicoId,
                    NomePeca = peca.Nome,
                    Quantidade = peca.Quantidade,
                    Cliente = cliente,
                    Unidade = unidade,
                    TecnicoNome = tecnicoNome,
                    Status = "Pendente",
                    DataCriacao = DateTime.UtcNow
                };

                _context.SolicitacoesPeca.Add(nova);
            }

            await _context.SaveChangesAsync();
        }
    }
}
