namespace AirControl.Api.Models
{
    public class PmocRegistro
    {
        public int Id { get; set; }
        public int AparelhoHdvId { get; set; }
        public string TecnicoEmail { get; set; } = string.Empty;
        public DateTime Data {  get; set; }

        // Json com todos os itens do checklist
        public string ItensJson { get; set; } = string.Empty;
    }
}
