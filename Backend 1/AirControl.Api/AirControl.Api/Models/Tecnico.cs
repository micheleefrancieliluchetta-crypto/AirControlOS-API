namespace AirControl.Api.Models;

public class Tecnico
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string Cargo { get; set; } = null!; // Admin, Tecnico, Ajudante, MeioOficial, Mecanico

    public ICollection<OrdemServico>? Ordens { get; set; }
}
