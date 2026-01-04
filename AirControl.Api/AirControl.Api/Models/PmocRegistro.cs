namespace AirControl.Api.Models
{
    public class PmocRegistro
    {
        public int Id { get; set; }

        public int AparelhoHdvId { get; set; }

        public DateTime Data { get; set; }

        // JSON com o checklist (mesmo campo que já existia no banco)
        public string ItensJson { get; set; } = string.Empty;

        // observações gerais do técnico
        public string? ObservacoesTecnicas { get; set; }

        // quem fez o PMOC
        public string TecnicoEmail { get; set; } = string.Empty;
        public string? TecnicoNome { get; set; }
    }
}
