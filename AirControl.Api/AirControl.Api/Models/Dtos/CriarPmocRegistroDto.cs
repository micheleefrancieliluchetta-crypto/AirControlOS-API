namespace AirControl.Api.Dtos
{
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }
        public string Data { get; set; }  // string ISO 8601
        public string ChecklistJson { get; set; }
        public string ObservacoesTecnicas { get; set; }
        public string TecnicoNome { get; set; }
        public string TecnicoEmail { get; set; }
    }
}
