namespace AirControl.Api.Models;

public class Foto
{
    public int Id { get; set; }
    public int OrdemServicoId { get; set; }
    public OrdemServico? OrdemServico { get; set; }

    public string Tipo { get; set; } = null!; // Antes | Depois
    public byte[] Conteudo { get; set; } = null!;
    public string? ContentType { get; set; }
    public string? NomeArquivo { get; set; }
}
