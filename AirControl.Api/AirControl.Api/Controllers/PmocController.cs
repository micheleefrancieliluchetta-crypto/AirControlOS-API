using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using AirControl.Api.Models;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]  // se suas outras rotas usam Auth, pode descomentar depois
    public class PmocController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PmocController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/pmoc/local/5
        [HttpGet("local/{localId:int}")]
        public async Task<ActionResult<IEnumerable<PmocPlano>>> GetPorLocal(int localId)
        {
            var lista = await _context.PmocPlanos
                .Where(p => p.LocalId == localId)
                .OrderBy(p => p.DataReferencia)
                .ToListAsync();

            return Ok(lista);
        }

        // POST api/pmoc
        [HttpPost]
        public async Task<ActionResult<PmocPlano>> Criar([FromBody] PmocPlano dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            // validações simples
            if (dto.LocalId <= 0)
                return BadRequest("LocalId obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.NomePlano))
                return BadRequest("Nome do plano obrigatório.");

            _context.PmocPlanos.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorLocal),
                new { localId = dto.LocalId },
                dto);
        }
    }
}
