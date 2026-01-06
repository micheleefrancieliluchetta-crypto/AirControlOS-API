using System;

namespace AirControl.Api.Models
{
    public class PmocRegistro
    {
        public int Id { get; set; }

        // ID do aparelho (HDV) lá do front
        public int AparelhoHdvId { get; set; }

        public DateTime Data { get; set; }

        // <<< ESSA É A PROPRIEDADE QUE ESTAVA FALTANDO
        public string ChecklistJson { get; set; } = string.Empty;

        // Observações técnicas gerais (caixa grande)
        public string? ObservacoesTecnicas { get; set; }

        // Nome do técnico que fez o PMOC
        public string? TecnicoNome { get; set; }

        // Se depois você quiser salvar email/usuário, usa aqui
        public string? TecnicoEmail { get; set; }
    }
}
