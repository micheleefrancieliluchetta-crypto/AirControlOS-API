using System;

namespace AirControl.Api.Models
{
    public class SolicitacaoPeca
    {
        public int Id { get; set; }

        // vínculo com OS
        public int OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }   // põe ? pra não encher de warning

        // dados principais
        public string NomePeca { get; set; } = string.Empty;
        public int Quantidade { get; set; } = 1;

        // infos para tela de peças
        public string? Cliente { get; set; }
        public string? Unidade { get; set; }
        public string? TecnicoNome { get; set; }
        public string? Observacao { get; set; }

        // status / datas
        public string Status { get; set; } = "Pendente";
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}
