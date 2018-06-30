using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieCheck.Api.Migrations
{
    public partial class Usuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Endereco",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bairro = table.Column<string>(nullable: true),
                    Cep = table.Column<string>(nullable: true),
                    Cidade = table.Column<string>(nullable: true),
                    Complemento = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Endereco", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "TB_Telefone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ddd = table.Column<int>(nullable: false),
                    Numero = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Telefone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EnderecoUsuarioId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Cpf = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: true),
                    ClienteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Usuario_TB_Endereco_EnderecoUsuarioId",
                        column: x => x.EnderecoUsuarioId,
                        principalTable: "TB_Endereco",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_Usuario_Telefone",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false),
                    TelefoneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Usuario_Telefone", x => new { x.UsuarioId, x.TelefoneId });
                    table.ForeignKey(
                        name: "FK_TB_Usuario_Telefone_TB_Telefone_TelefoneId",
                        column: x => x.TelefoneId,
                        principalTable: "TB_Telefone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Usuario_Telefone_TB_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Usuario_EnderecoUsuarioId",
                table: "TB_Usuario",
                column: "EnderecoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Usuario_ClienteId",
                table: "TB_Usuario",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Usuario_Telefone_TelefoneId",
                table: "TB_Usuario_Telefone",
                column: "TelefoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Usuario_Telefone");

            migrationBuilder.DropTable(
                name: "TB_Telefone");

            migrationBuilder.DropTable(
                name: "TB_Usuario");

            migrationBuilder.DropTable(
                name: "TB_Endereco");
        }
    }
}
