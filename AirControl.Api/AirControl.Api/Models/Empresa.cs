using System.Collections.Generic;

namespace AirControl.Api.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        public string NomeFantasia { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? InscricaoEstadual { get; set; }

        // se quiser “desativar” uma empresa sem apagar
        public bool Ativo { get; set; } = true;

        // Navegação – todas as OS daquela empresa
        public ICollection<OrdemServico> Ordens { get; set; } = new List<OrdemServico>();
    }
}
