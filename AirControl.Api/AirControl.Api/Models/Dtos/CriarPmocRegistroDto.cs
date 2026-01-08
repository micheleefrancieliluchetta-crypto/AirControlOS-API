namespace AirControl.Api.Dtos
{
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }

        // Data vem como string do front (ou vazia). Vamos tratar no controller.
        public string? Data { get; set; }

        // JSON com os itens do checklist
        public string ChecklistJson { get; set; } = "[]";

        // Observações técnicas gerais (textarea no fim do PMOC)
        public string? ObservacoesTecnicas { get; set; }

        // Dados do técnico (opcionais)
        public string? TecnicoNome { get; set; }
        public string? TecnicoEmail { get; set; }
    }
}
