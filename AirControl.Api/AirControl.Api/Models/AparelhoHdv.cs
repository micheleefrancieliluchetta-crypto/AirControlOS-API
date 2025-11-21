using System.Collections.Generic;

namespace AirControl.Api.Models
{
    public class AparelhoHdv
    {
        public int? UnidadeExterna { get; set; }
        public string? Local { get; set; }
        public string? Marca { get; set; }
        public int? Btu { get; set; }
        public string? Modelo { get; set; }
    }

    // Massa de teste baseada na planilha HDV
    public static class AparelhosHdvSeed
    {
        public static readonly List<AparelhoHdv> Itens = new()
        {
            // Peguei as primeiras linhas da planilha como exemplo.
            // Se quiser, depois é só continuar preenchendo nesse mesmo padrão.

            new AparelhoHdv
            {
                UnidadeExterna = 2,
                Local = "POSTO DE ENFERMAGEM",
                Marca = null,
                Btu = null,
                Modelo = null
            },
            new AparelhoHdv
            {
                UnidadeExterna = 3,
                Local = "POSTO DE ENFERMAGEM",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = null,     // na planilha está "---"
                Local = "POSTO DE ENFERMAGEM",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 5,
                Local = "SALA DE LIMPEZA",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 6,
                Local = "SUPERVISÃO ADMINISTRATIVA",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 7,
                Local = "CONFORTO ENFERMAGEM",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 8,
                Local = "CONFORTO ENFERMAGEM",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 9,
                Local = "CONFORTO ENFERMEIRA",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 10,
                Local = "FATURAMENTO 01",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            },
            new AparelhoHdv
            {
                UnidadeExterna = 11,
                Local = "FATURAMENTO 02",
                Marca = "MIDEA",
                Btu = 9000,
                Modelo = "HW"
            }
        };
    }
}
