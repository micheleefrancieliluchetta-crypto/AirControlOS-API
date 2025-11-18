// Models/Dtos/CriarOSDto.cs
namespace AirControl.Api.Models.Dtos
{
    public class CriarOSDto
    {
        public int? ClienteId { get; set; } // pode vir 0/null
        public int? TecnicoId { get; set; } // opcional (público não define)
        public string? LocalNome { get; set; } // quando não vier ClienteId
        public string Descricao { get; set; } = string.Empty;
        public string? Prioridade { get; set; }
        public string? Status { get; set; }
        public string? Observacoes { get; set; }
        public string? Endereco { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }
}

public class AtualizarOSDto
    {
        public string? Descricao { get; set; }
        public string? Prioridade { get; set; }
        public string? Status { get; set; }     // Aberta/Em Andamento/Concluída
        public string? Observacoes { get; set; }
        public string? Endereco { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public DateTime? DataConclusao { get; set; }
}

