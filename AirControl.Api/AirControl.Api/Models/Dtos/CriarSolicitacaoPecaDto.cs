namespace AirControl.Api.Models.Dtos
{
    public class CriarSolicitacaoPecaDto
    {
        public int OrdemServicoId { get; set; }
        public string NomePeca { get; set; } = string.Empty;
        public int Quantidade { get; set; } = 1;
        public string? Cliente { get; set; }
        public string? Unidade { get; set; }
        public string? TecnicoNome { get; set; }
        public string? Observacao { get; set; }
        public string? Status { get; set; }   // opcional – se vier nulo vira "Pendente"
    }

    public class AtualizarStatusSolicitacaoPecaDto
    {
        public string Status { get; set; } = "Pendente";
    }

    public class PecaDto
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; } = 1;
    }
}
