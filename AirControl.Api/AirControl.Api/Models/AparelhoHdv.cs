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

    // Massa de teste baseada na planilha HDV – todos os aparelhos
    public static class AparelhosHdvSeed
    {
        public static readonly List<AparelhoHdv> Itens = new()
        {
            // 2 a 11 – primeiros que você já tinha
            new AparelhoHdv { UnidadeExterna = 2,  Local = "POSTO DE ENFERMAGEM",           Marca = null,     Btu = null,   Modelo = null },
            new AparelhoHdv { UnidadeExterna = 3,  Local = "POSTO DE ENFERMAGEM",           Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = null, Local = "POSTO DE ENFERMAGEM",         Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 5,  Local = "SALA DE LIMPEZA",              Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 6,  Local = "SUPERVISÃO ADMINISTRATIVA",    Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 7,  Local = "CONFORTO ENFERMAGEM",          Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 8,  Local = "CONFORTO ENFERMAGEM",          Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 9,  Local = "CONFORTO ENFERMEIRA",          Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 10, Local = "FATURAMENTO 01",               Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 11, Local = "FATURAMENTO 02",               Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },

            // 12 em diante – continuação da planilha
            new AparelhoHdv { UnidadeExterna = 12, Local = "RECURSOS HUMANOS",             Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 13, Local = "APOIO ADMINISTRATIVO",         Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 14, Local = "ADMINISTRAÇÃO",                Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 15, Local = "DIRETORIA MÉDICA",             Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 16, Local = "GERENCIA DE ENFERMAGEM",       Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 17, Local = "HOSPITAL DIA",                 Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 18, Local = "NIR",                          Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 19, Local = "NÚCLEO DE VIGILÂNCIA",         Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 20, Local = "TOMOGRAFIA",                   Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 21, Local = "ISOLAMENTO 01",                Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 22, Local = "ENDOSCOPIA 1",                 Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 23, Local = "VACINA CCIH",                  Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 24, Local = "EXPURGO CONSULTÓRIO BUCAL",    Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 26, Local = "ESPAÇO ECUMÊNICO",             Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 27, Local = "ENDOSCOPIA 3",                 Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 28, Local = "PSICOLOGIA",                   Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 29, Local = "NUTRIÇÃO",                     Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 30, Local = "ASSISTÊNCIA SOCIAL",           Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 31, Local = "SALA DE GESSO",                Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 32, Local = "VOLUNTÁRIOS",                  Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 33, Local = "PRÉ-ANESTÉSICO",               Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 34, Local = "DEPÓSITO DE EQUIPAMENTO",      Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 35, Local = "AG. TRANSFUSIONAL OPERACIONAL",Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 36, Local = "AG. TRANSFUSIONAL ADMINISTRATIVO", Marca = "MIDEA", Btu = 9000, Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 37, Local = "MONITORAMENTO",                Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 38, Local = "ROUPARIA",                     Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 39, Local = "DEPÓSITO DE EQUIPAMENTO",      Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 40, Local = "CONFORTO MÉDICO",              Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 41, Local = "CONFORTO MÉDICO",              Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 42, Local = "CONFORTO ENFERMAGÉM",          Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 43, Local = "CONFORTO ENFERMAGÉM",          Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 44, Local = "REUNIÃO",                      Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 45, Local = "APOIO DIRETORIA MÉDICA",       Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 46, Local = "ENDOSCOPIA 2",                 Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 47, Local = "CONSULTÓRIO BUCOMAXLOFACIAL",  Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 48, Local = "ORTOPEDIA",                    Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 49, Local = "COPA",                         Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 50, Local = "SALA ROUPA SUJA",              Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 51, Local = "PEDIATRIA",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "DUTO" },
            new AparelhoHdv { UnidadeExterna = 52, Local = "QUARTO B2",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 55, Local = "QUARTO A2",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 56, Local = "QUARTO A",                     Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 57, Local = "QUARTO D",                     Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 58, Local = "QUARTO E",                     Marca = "MIDEA",  Btu = 48000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 59, Local = "QUARTO B",                     Marca = "ELGIN",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 60, Local = "ENGENHARIA CLÍNICA",           Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 61, Local = "VESTIÁRIO",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 62, Local = "QUARTO B",                     Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 63, Local = "SME",                          Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 64, Local = "EXPEDIENTE",                   Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 65, Local = "HOSPITAL DIA",                 Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 66, Local = "TOMOGRAFIA",                   Marca = "ELGIN",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 67, Local = "QUARTO D",                     Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 68, Local = "RAIO-X",                       Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 69, Local = "OUVIDORIA",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 70, Local = "CORREDOR SALA GESSO",          Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 71, Local = "CORREDOR EXPEDIENTE",          Marca = "CARRIEL",Btu = 24000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 72, Local = "PEDIATRIA",                    Marca = "MIDEA",  Btu = 18000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = 73, Local = "LABORATÓRIO",                  Marca = "CARRIEL",Btu = 24000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 74, Local = "LABORATÓRIO",                  Marca = "CARRIEL",Btu = 24000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 75, Local = "QUARTO C CLÍNICA MASCULINA",   Marca = "MIDEA",  Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 76, Local = "QUARTO E",                     Marca = "MIDEA",  Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 77, Local = "FARMÁCIA",                     Marca = "MIDEA",  Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 78, Local = "REFEITÓRIO",                   Marca = "MIDEA",  Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 79, Local = "SALA 18",                      Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 80, Local = "SALA 18",                      Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 81, Local = "SALA 18 (clínica med. masculina)", Marca = "CARRIEL", Btu = 36000, Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 82, Local = "SALA 18",                      Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 83, Local = "ESPERA LABORATÓRIO",           Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 84, Local = "ESPERA LABORATÓRIO",           Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 85, Local = "RECEPÇÃO",                     Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 86, Local = "RECEPÇÃO",                     Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 87, Local = "SEM ID",                       Marca = "CARRIEL",Btu = 46000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 88, Local = "CANTINA RECEPÇÃO",             Marca = "CARRIEL",Btu = 36000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 89, Local = "QUARTO C2",                    Marca = "MIDEA",  Btu = 38000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = 90, Local = "QUARTO E",                     Marca = "CARRIEL",Btu = 48000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = null, Local = "SEM ID",                     Marca = "CARRIEL",Btu = 48000,  Modelo = "K7" },
            new AparelhoHdv { UnidadeExterna = null, Local = "FISIOTERAPIA",               Marca = "MIDEA",  Btu = 12000,  Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = null, Local = "C. MASCULINO",               Marca = "MIDEA",  Btu = 9000,   Modelo = "HW" },
            new AparelhoHdv { UnidadeExterna = null, Local = "ALMOXARIFADO",               Marca = "ELGIN",  Btu = 60000,  Modelo = "PT" },
            new AparelhoHdv { UnidadeExterna = null, Local = "ESTABILIZAÇÃO",              Marca = null,     Btu = null,   Modelo = null },
        };
    }
}
