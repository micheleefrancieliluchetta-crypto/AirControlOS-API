namespace AirControl.Api.Models.Dtos;

public record TecnicoCreateDto(string Nome, string? Email, string? Telefone, string Cargo);
public record TecnicoUpdateDto(string Nome, string? Email, string? Telefone, string Cargo);
