namespace AirControl.Api.Models;

public static class Cargos
{
    public const string Admin = "Admin";
    public const string Tecnico = "Tecnico";
    public const string Ajudante = "Ajudante";
    public const string MeioOficial = "MeioOficial";
    public const string Mecanico = "Mecanico";

    public static readonly string[] Todos = { Admin, Tecnico, Ajudante, MeioOficial, Mecanico };
}
