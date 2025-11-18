using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using AirControl.Api.Models;
using AirControl.Api.Models.Dtos;

namespace AirControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private readonly AppDbContext _db;
    public TecnicosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tecnico>>> Get() =>
        await _db.Tecnicos.AsNoTracking().OrderBy(t => t.Nome).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Tecnico>> GetById(int id)
    {
        var t = await _db.Tecnicos.FindAsync(id);
        return t is null ? NotFound() : t;
    }

    [HttpPost]
    public async Task<ActionResult<Tecnico>> Post([FromBody] TecnicoCreateDto dto)
    {
        if (!Cargos.Todos.Contains(dto.Cargo))
            return BadRequest("Cargo inválido. Use: Admin, Tecnico, Ajudante, MeioOficial, Mecanico.");

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            await _db.Tecnicos.AnyAsync(x => x.Email == dto.Email))
            return Conflict("E-mail já cadastrado para outro técnico.");

        var t = new Tecnico { Nome = dto.Nome, Email = dto.Email, Telefone = dto.Telefone, Cargo = dto.Cargo };
        _db.Tecnicos.Add(t);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = t.Id }, t);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] TecnicoUpdateDto dto)
    {
        if (!Cargos.Todos.Contains(dto.Cargo))
            return BadRequest("Cargo inválido.");

        var t = await _db.Tecnicos.FindAsync(id);
        if (t is null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            await _db.Tecnicos.AnyAsync(x => x.Email == dto.Email && x.Id != id))
            return Conflict("E-mail já cadastrado para outro técnico.");

        t.Nome = dto.Nome; t.Email = dto.Email; t.Telefone = dto.Telefone; t.Cargo = dto.Cargo;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Tecnicos.FindAsync(id);
        if (t is null) return NotFound();
        _db.Tecnicos.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
