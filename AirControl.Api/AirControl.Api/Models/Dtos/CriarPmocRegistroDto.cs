using System;

namespace AirControl.Api.Dtos
{
    public class CriarPmocRegistroDto
    {
        public int AparelhoHdvId { get; set; }

        public DateTime Data { get; set; }

        // mesmo nome do front e do modelo
        public string ChecklistJson { get; set; } = string.Empty;

        public string? ObservacoesTecnicas { get; set; }

        public string? TecnicoNome { get; set; }
    }
}
