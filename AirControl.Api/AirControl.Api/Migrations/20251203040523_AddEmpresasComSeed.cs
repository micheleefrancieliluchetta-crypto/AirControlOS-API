using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpresasComSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Ativo", "Cnpj", "InscricaoEstadual", "NomeFantasia", "RazaoSocial" },
                values: new object[] { 1, true, "00.000.000/0000-00", null, "Maxi Ar Condicionado", "Maxi Ar Condicionado LTDA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
