namespace AirControl.Api.Models
{
    public class SolicitacaoPeca
    {
        public int Id { get; set; }

        // vínculo com OS
        public int OrdemServicoId { get; set; }

        // 👇 diz pro compilador “relaxa, o EF vai preencher isso”
        public OrdemServico OrdemServico { get; set; } = null!;

        // dados principais
        public string NomePeca { get; set; } = string.Empty;
        public int Quantidade { get; set; }

        public string? Cliente { get; set; }    // se puder ser vazio, coloca ? 
        public string? Observacao { get; set; }

        public string Status { get; set; } = "Pendente";
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}

