using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarSolicitacoesPeca : Migration
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

            migrationBuilder.CreateTable(
                name: "SolicitacoesPeca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdemServicoId = table.Column<int>(type: "integer", nullable: false),
                    NomePeca = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    Cliente = table.Column<string>(type: "text", nullable: true),
                    Unidade = table.Column<string>(type: "text", nullable: true),
                    TecnicoNome = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pendente"),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesPeca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitacoesPeca_OrdensServico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdensServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesPeca_OrdemServicoId",
                table: "SolicitacoesPeca",
                column: "OrdemServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacoesPeca");

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
