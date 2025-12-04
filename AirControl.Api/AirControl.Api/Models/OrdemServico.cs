using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Models/OrdemServico.cs
namespace AirControl.Api.Models
{
    public class OrdemServico
    {
        public int Id { get; set; }

        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }   // <— navegação

        public int? TecnicoId { get; set; }
        public Tecnico? Tecnico { get; set; }   // <— navegação

        public int? EmpresaId { get; set; }      // FK opcional
        public Empresa? Empresa { get; set; }    // navegação


        public string? Descricao { get; set; }
        public string? Prioridade { get; set; } // Baixa/Média/Alta/Crítica
        public string? Status { get; set; }     // Aberta/Em Andamento/Concluída
        public string? Observacoes { get; set; }

        public string? Endereco { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }

        public ICollection<Foto> Fotos { get; set; } = new List<Foto>();
    }
}
