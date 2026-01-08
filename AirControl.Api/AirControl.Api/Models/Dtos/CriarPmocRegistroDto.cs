namespace AirControl.Api.Dtos
{
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }

        // data em string, vinda do front (pmoc.html)
        public string? Data { get; set; }

        // JSON com os itens do checklist
        public string? ChecklistJson { get; set; }

        // observações técnicas gerais
        public string? ObservacoesTecnicas { get; set; }
    }
}
