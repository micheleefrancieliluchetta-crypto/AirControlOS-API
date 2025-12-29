using System.ComponentModel.DataAnnotations;

namespace AirControl.Api.Models
{
    public class Unidade
    {
        public int Id { get; set; }

        // FK para a empresa dona dessa unidade (Maxi, G&G, etc.)
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }

        // Nome da unidade/projeto: "Hospital Municipal", "UPA Santos", etc.
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        // Cidade onde fica essa unidade
        [MaxLength(80)]
        public string Cidade { get; set; } = string.Empty;

        // Endereço opcional
        [MaxLength(250)]
        public string? Endereco { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
