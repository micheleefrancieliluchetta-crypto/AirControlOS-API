namespace AirControl.Api.Models
{
    public class PmocPlano
    {
        public int Id { get; set; }

        // Id da unidade/projeto/cliente que você já tem no sistema
        public int LocalId { get; set; }

        public string NomePlano { get; set; } = string.Empty;   // "PMOC Torre A"
        public string Tipo { get; set; } = "PMOC";              // "PMOC" ou "POP"
        public string Descricao { get; set; } = string.Empty;   // orientações gerais
        public DateTime? DataReferencia { get; set; }           // mês/ano base (01/2025)
    }
}
