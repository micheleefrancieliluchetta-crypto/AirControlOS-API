using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AllowNull_TecnicoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Clientes_ClienteId",
                table: "OrdensServico");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Tecnicos_TecnicoId",
                table: "OrdensServico");

            migrationBuilder.DropIndex(
                name: "IX_Tecnicos_Email",
                table: "Tecnicos");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Tecnicos");

            migrationBuilder.RenameColumn(
                name: "NivelAcesso",
                table: "Tecnicos",
                newName: "Cargo");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Tecnicos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "TecnicoId",
                table: "OrdensServico",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "OrdensServico",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataConclusao",
                table: "OrdensServico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "OrdensServico",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "OrdensServico",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "OrdensServico",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdemServicoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Conteudo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NomeArquivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_OrdensServico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdensServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tecnicos_Email",
                table: "Tecnicos",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_OrdemServicoId",
                table: "Fotos",
                column: "OrdemServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Clientes_ClienteId",
                table: "OrdensServico",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Tecnicos_TecnicoId",
                table: "OrdensServico",
                column: "TecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Clientes_ClienteId",
                table: "OrdensServico");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Tecnicos_TecnicoId",
                table: "OrdensServico");

            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Tecnicos_Email",
                table: "Tecnicos");

            migrationBuilder.DropColumn(
                name: "DataConclusao",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "OrdensServico");

            migrationBuilder.RenameColumn(
                name: "Cargo",
                table: "Tecnicos",
                newName: "NivelAcesso");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Tecnicos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Tecnicos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TecnicoId",
                table: "OrdensServico",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "OrdensServico",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tecnicos_Email",
                table: "Tecnicos",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Clientes_ClienteId",
                table: "OrdensServico",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Tecnicos_TecnicoId",
                table: "OrdensServico",
                column: "TecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
