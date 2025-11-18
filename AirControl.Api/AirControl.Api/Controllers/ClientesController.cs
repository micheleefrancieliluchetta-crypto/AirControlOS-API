using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using AirControl.Api.Models;
using AirControl.Api.Models.Dtos;

namespace AirControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ClientesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Get() =>
        await _db.Clientes.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetById(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        return c is null ? NotFound() : c;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post([FromBody] ClienteCreateDto dto)
    {
        var c = new Cliente { Nome = dto.Nome, Endereco = dto.Endereco, Telefone = dto.Telefone, Email = dto.Email };
        _db.Clientes.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ClienteUpdateDto dto)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        c.Nome = dto.Nome; c.Endereco = dto.Endereco; c.Telefone = dto.Telefone; c.Email = dto.Email;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        _db.Clientes.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
