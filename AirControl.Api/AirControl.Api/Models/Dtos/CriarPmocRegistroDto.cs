namespace AirControl.Api.Dtos
{
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }
        public string? Data { get; set; }
        public string? ChecklistJson { get; set; }
        public string? ObservacoesTecnicas { get; set; }

        // 🔹 ADICIONAR ESSES DOIS:
        public string? TecnicoNome { get; set; }
        public string? TecnicoEmail { get; set; }
    }
}
