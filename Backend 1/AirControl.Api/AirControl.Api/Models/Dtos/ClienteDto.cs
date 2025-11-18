namespace AirControl.Api.Models.Dtos;

public record ClienteCreateDto(string Nome, string? Endereco, string? Telefone, string? Email);
public record ClienteUpdateDto(string Nome, string? Endereco, string? Telefone, string? Email);
