namespace AirControl.Api.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Endereco { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }

    public ICollection<OrdemServico>? Ordens { get; set; }
}

