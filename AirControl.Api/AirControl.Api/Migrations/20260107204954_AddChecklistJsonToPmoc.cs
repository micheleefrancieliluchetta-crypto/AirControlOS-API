using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddChecklistJsonToPmoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItensJson",
                table: "PmocRegistros",
                newName: "ChecklistJson");

            migrationBuilder.AlterColumn<string>(
                name: "TecnicoEmail",
                table: "PmocRegistros",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ObservacoesTecnicas",
                table: "PmocRegistros",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TecnicoNome",
                table: "PmocRegistros",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservacoesTecnicas",
                table: "PmocRegistros");

            migrationBuilder.DropColumn(
                name: "TecnicoNome",
                table: "PmocRegistros");

            migrationBuilder.RenameColumn(
                name: "ChecklistJson",
                table: "PmocRegistros",
                newName: "ItensJson");

            migrationBuilder.AlterColumn<string>(
                name: "TecnicoEmail",
                table: "PmocRegistros",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
