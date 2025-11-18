namespace AirControl.Api.Models.Dtos;

public record OrdemCreateDto(
    int ClienteId,
    int TecnicoId,
    string? Descricao,
    string Prioridade,
    string Status,
    string? Observacoes,
    string? Endereco,
    decimal? Lat,
    decimal? Lng
);

public record OrdemUpdateDto(
    string? Descricao,
    string Prioridade,
    string Status,
    string? Observacoes,
    string? Endereco,
    decimal? Lat,
    decimal? Lng,
    DateTime? DataConclusao
);
