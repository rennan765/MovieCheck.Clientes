using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Migrations
{
    public partial class Filme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Ator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Ator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Classificacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassificacaoIndicativa = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Classificacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Diretor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Diretor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Genero",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Genero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Filme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ano = table.Column<int>(nullable: false),
                    ClassificacaoId = table.Column<int>(nullable: false),
                    Midia = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Sinopse = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Filme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Filme_Tb_Classificacao_ClassificacaoId",
                        column: x => x.ClassificacaoId,
                        principalTable: "Tb_Classificacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Ator_Filme",
                columns: table => new
                {
                    AtorId = table.Column<int>(nullable: false),
                    FilmeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Ator_Filme", x => new { x.AtorId, x.FilmeId });
                    table.ForeignKey(
                        name: "FK_TB_Ator_Filme_TB_Ator_AtorId",
                        column: x => x.AtorId,
                        principalTable: "TB_Ator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Ator_Filme_TB_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "TB_Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Diretor_Filme",
                columns: table => new
                {
                    DiretorId = table.Column<int>(nullable: false),
                    FilmeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Diretor_Filme", x => new { x.DiretorId, x.FilmeId });
                    table.ForeignKey(
                        name: "FK_TB_Diretor_Filme_TB_Diretor_DiretorId",
                        column: x => x.DiretorId,
                        principalTable: "TB_Diretor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Diretor_Filme_TB_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "TB_Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Genero_Filme",
                columns: table => new
                {
                    GeneroId = table.Column<int>(nullable: false),
                    FilmeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Genero_Filme", x => new { x.GeneroId, x.FilmeId });
                    table.ForeignKey(
                        name: "FK_TB_Genero_Filme_TB_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "TB_Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Genero_Filme_TB_Genero_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "TB_Genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Pendencia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataReserva = table.Column<DateTime>(nullable: false),
                    FilmeId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Pendencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Pendencia_TB_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "TB_Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Pendencia_TB_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Ator_Filme_FilmeId",
                table: "TB_Ator_Filme",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Diretor_Filme_FilmeId",
                table: "TB_Diretor_Filme",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Filme_ClassificacaoId",
                table: "TB_Filme",
                column: "ClassificacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Genero_Filme_FilmeId",
                table: "TB_Genero_Filme",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Pendencia_FilmeId",
                table: "TB_Pendencia",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Pendencia_UsuarioId",
                table: "TB_Pendencia",
                column: "UsuarioId");

            //Higieniza Telefone
            migrationBuilder.Sql(
                @"
                CREATE PROCEDURE sp_higieniza_telefone AS
                BEGIN
	                /*
		                Esta SP é utilizada para excluir os telefones os quais não possuem mais usuários vinculados, além de excluir os registros da tabela de JOIN que não possuem pai.
	                */
	                CREATE TABLE #telefonesExcluir (id int, feito int)
	                CREATE TABLE #usuarioTelefoneExcluir (usuarioId int, telefoneId int, feito int)

	                CREATE CLUSTERED INDEX ix_telefonesExcluir on #telefonesExcluir (feito)
	                CREATE CLUSTERED INDEX ix_usuarioTelefonesExcluir on #usuarioTelefoneExcluir (feito)

	                -- ********** Excluir telefones ********** --
	                INSERT INTO #telefonesExcluir(id, feito)
	                SELECT TB_Telefone.Id, 0
	                FROM TB_Telefone WITH (NOLOCK)
	                WHERE EXISTS (SELECT *
				                  FROM TB_Usuario_Telefone WITH (NOLOCK)
				                  WHERE TB_Usuario_Telefone.TelefoneId = TB_Telefone.Id
				                  AND NOT EXISTS (SELECT *
								                  FROM TB_Usuario WITH (NOLOCK)
								                  WHERE TB_Usuario.Id = TB_Usuario_Telefone.UsuarioId))
	                OR NOT EXISTS (SELECT *
				                   FROM TB_Usuario_Telefone WITH (NOLOCK)
				                   WHERE TB_Usuario_Telefone.TelefoneId = TB_Telefone.Id)
	
	                WHILE EXISTS (SELECT * FROM #telefonesExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Telefone
		                FROM TB_Telefone
		                CROSS APPLY (SELECT TOP 500 id 
					                 FROM #telefonesExcluir
					                 WHERE feito = 0
					                 ORDER BY id) telefones_excluir
		                WHERE TB_Telefone.Id = telefones_excluir.id

		                UPDATE TEMP1 
		                SET feito = 1
		                FROM #telefonesExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #telefonesExcluir temp2
					                WHERE feito = 0
					                AND temp2.id = temp1.id
					                ORDER BY id)
	                END

	                -- ********** Excluir registros JOIN ********** --
	                INSERT INTO #usuarioTelefoneExcluir (usuarioId, telefoneId, feito)
	                SELECT UsuarioId, TelefoneId, 0
	                FROM TB_Usuario_Telefone WITH (NOLOCK)
	                WHERE NOT EXISTS (SELECT *
					                  FROM TB_Usuario WITH (NOLOCK)
					                  WHERE TB_Usuario.Id = TB_Usuario_Telefone.UsuarioId)
	                OR NOT EXISTS (SELECT *
				                   FROM TB_Telefone WITH (NOLOCK) 
				                   WHERE TB_Telefone.Id = TB_Telefone.Id)

	                WHILE EXISTS (SELECT * FROM #usuarioTelefoneExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Usuario_Telefone
		                FROM TB_Usuario_Telefone
		                CROSS APPLY (SELECT TOP 500 usuarioId, telefoneId 
					                 FROM #usuarioTelefoneExcluir
					                 WHERE feito = 0
					                 ORDER BY usuarioId, telefoneId) usuario_telefones_excluir
		                WHERE TB_Usuario_Telefone.UsuarioId = usuario_telefones_excluir.usuarioId
		                AND TB_Usuario_Telefone.TelefoneId = usuario_telefones_excluir.telefoneId

		                UPDATE temp1 
		                SET feito = 1
		                FROM #usuarioTelefoneExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #usuarioTelefoneExcluir temp2
					                WHERE feito = 0
					                AND temp2.usuarioId = temp1.usuarioId
					                AND temp2.telefoneId = temp1.telefoneId
					                ORDER BY usuarioId, telefoneId)
	                END
                END");

            //Higieniza Endereço
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_higieniza_endereco
                AS
                BEGIN
	                /*
		                Esta SP é utilizada para excluir os endereços os quais não possuem mais usuários vinculados.
	                */

	                CREATE TABLE #temp_enderecos (id INT, feito BIT DEFAULT 0)
	                CREATE CLUSTERED INDEX ix_temp_enderecos ON #temp_enderecos (feito)

	                INSERT INTO #temp_enderecos
	                SELECT TB_Endereco.UsuarioId AS id, 0 AS feito
	                FROM TB_Endereco (NOLOCK)
	                WHERE NOT EXISTS (SELECT *
					                  FROM TB_Usuario (NOLOCK)
					                  WHERE TB_Usuario.EnderecoUsuarioId = TB_Endereco.UsuarioId)

	                WHILE EXISTS (SELECT * FROM #temp_enderecos WHERE FEITO = 0)
	                BEGIN
		                DELETE 
		                FROM TB_Endereco 
		                WHERE TB_Endereco.UsuarioId IN (SELECT TOP 500 id
										                FROM #temp_enderecos 
										                WHERE feito = 0
										                ORDER BY id) 

		                UPDATE temp1
		                SET FEITO = 1
		                FROM #temp_enderecos temp1
		                CROSS APPLY (SELECT TOP 500 *
					                 FROM #temp_enderecos 
					                 WHERE feito = 0
					                 ORDER BY id) temp2
		                WHERE temp1.id = temp2.id
	                END
                END");
            
            //Higieniza Ator
            migrationBuilder.Sql(
                @"
                CREATE PROCEDURE sp_higieniza_ator AS
                BEGIN
	                /*
		                Esta SP é utilizada para excluir os atores os quais não possuem mais filmes vinculados, além de excluir os registros da tabela de JOIN que não possuem pai.
	                */
	                CREATE TABLE #atoresExcluir (id int, feito int)
	                CREATE TABLE #atorFilmeExcluir (atorId int, filmeId int, feito int)

	                CREATE CLUSTERED INDEX ix_telefonesExcluir on #atoresExcluir (feito)
	                CREATE CLUSTERED INDEX ix_usuarioTelefonesExcluir on #atorFilmeExcluir (feito)

	                -- ********** Excluir telefones ********** --
	                INSERT INTO #atoresExcluir(id, feito)
	                SELECT TB_Ator.Id, 0
	                FROM TB_Ator WITH (NOLOCK)
	                WHERE EXISTS (SELECT *
				                    FROM TB_Ator_Filme WITH (NOLOCK)
				                    WHERE TB_Ator_Filme.AtorId = TB_Ator.Id
				                    AND NOT EXISTS (SELECT *
								                    FROM TB_Filme WITH (NOLOCK)
								                    WHERE TB_Filme.Id = TB_Ator_Filme.FilmeId))
	                OR NOT EXISTS (SELECT *
				                    FROM TB_Ator_Filme WITH (NOLOCK)
				                    WHERE TB_Ator_Filme.AtorId = TB_Ator.Id)
	
	                WHILE EXISTS (SELECT * FROM #atoresExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Ator
		                FROM TB_Ator
		                CROSS APPLY (SELECT TOP 500 id 
					                    FROM #atoresExcluir
					                    WHERE feito = 0
					                    ORDER BY id) atores_excluir
		                WHERE TB_Ator.Id = atores_excluir.id

		                UPDATE TEMP1 
		                SET feito = 1
		                FROM #atoresExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #atoresExcluir temp2
					                WHERE feito = 0
					                AND temp2.id = temp1.id
					                ORDER BY id)
	                END

	                -- ********** Excluir registros JOIN ********** --
	                INSERT INTO #atorFilmeExcluir (atorId, filmeId, feito)
	                SELECT AtorId, FilmeId, 0
	                FROM TB_Ator_Filme WITH (NOLOCK)
	                WHERE NOT EXISTS (SELECT *
					                    FROM TB_Ator WITH (NOLOCK)
					                    WHERE TB_Ator.Id = TB_Ator_Filme.AtorId)
	                OR NOT EXISTS (SELECT *
				                    FROM TB_Filme WITH (NOLOCK) 
				                    WHERE TB_Filme.Id = TB_Ator_Filme.FilmeId)

	                WHILE EXISTS (SELECT * FROM #atorFilmeExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Ator_Filme
		                FROM TB_Ator_Filme
		                CROSS APPLY (SELECT TOP 500 atorId, filmeId 
					                    FROM #atorFilmeExcluir
					                    WHERE feito = 0
					                    ORDER BY atorId, filmeId) ator_filmes_excluir
		                WHERE TB_Ator_Filme.AtorId = ator_filmes_excluir.atorId
		                AND TB_Ator_Filme.FilmeId = ator_filmes_excluir.filmeId

		                UPDATE temp1 
		                SET feito = 1
		                FROM #atorFilmeExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #atorFilmeExcluir temp2
					                WHERE feito = 0
					                AND temp2.atorId = temp1.atorId
					                AND temp2.filmeId = temp1.filmeId
					                ORDER BY atorId, filmeId)
	                END
                END");

            //Higieniza Diretor
            migrationBuilder.Sql(
                @"
                CREATE PROCEDURE sp_higieniza_diretor AS
                BEGIN
	                /*
		                Esta SP é utilizada para excluir os diretores os quais não possuem mais filmes vinculados, além de excluir os registros da tabela de JOIN que não possuem pai.
	                */
	                CREATE TABLE #diretoresExcluir (id int, feito int)
	                CREATE TABLE #diretorFilmeExcluir (diretorId int, filmeId int, feito int)

	                CREATE CLUSTERED INDEX ix_telefonesExcluir on #diretoresExcluir (feito)
	                CREATE CLUSTERED INDEX ix_usuarioTelefonesExcluir on #diretorFilmeExcluir (feito)

	                -- ********** Excluir telefones ********** --
	                INSERT INTO #diretoresExcluir(id, feito)
	                SELECT TB_Diretor.Id, 0
	                FROM TB_Diretor WITH (NOLOCK)
	                WHERE EXISTS (SELECT *
				                  FROM TB_Diretor_Filme WITH (NOLOCK)
				                  WHERE TB_Diretor_Filme.DiretorId = TB_Diretor.Id
				                  AND NOT EXISTS (SELECT *
								                  FROM TB_Filme WITH (NOLOCK)
								                  WHERE TB_Filme.Id = TB_Diretor_Filme.FilmeId))
	                OR NOT EXISTS (SELECT *
				                   FROM TB_Diretor_Filme WITH (NOLOCK)
				                   WHERE TB_Diretor_Filme.DiretorId = TB_Diretor.Id)
	
	                WHILE EXISTS (SELECT * FROM #diretoresExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Diretor
		                FROM TB_Diretor
		                CROSS APPLY (SELECT TOP 500 id 
					                 FROM #diretoresExcluir
					                 WHERE feito = 0
					                 ORDER BY id) diretores_excluir
		                WHERE TB_Diretor.Id = diretores_excluir.id

		                UPDATE TEMP1 
		                SET feito = 1
		                FROM #diretoresExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #diretoresExcluir temp2
					                WHERE feito = 0
					                AND temp2.id = temp1.id
					                ORDER BY id)
	                END

	                -- ********** Excluir registros JOIN ********** --
	                INSERT INTO #diretorFilmeExcluir (diretorId, filmeId, feito)
	                SELECT DiretorId, FilmeId, 0
	                FROM TB_Diretor_Filme WITH (NOLOCK)
	                WHERE NOT EXISTS (SELECT *
					                  FROM TB_Diretor WITH (NOLOCK)
					                  WHERE TB_Diretor.Id = TB_Diretor_Filme.DiretorId)
	                OR NOT EXISTS (SELECT *
				                   FROM TB_Filme WITH (NOLOCK) 
				                   WHERE TB_Filme.Id = TB_Diretor_Filme.FilmeId)

	                WHILE EXISTS (SELECT * FROM #diretorFilmeExcluir WHERE feito = 0)
	                BEGIN
		                DELETE TB_Diretor_Filme
		                FROM TB_Diretor_Filme
		                CROSS APPLY (SELECT TOP 500 diretorId, filmeId 
					                 FROM #diretorFilmeExcluir
					                 WHERE feito = 0
					                 ORDER BY diretorId, filmeId) diretor_filme_excluir
		                WHERE TB_Diretor_Filme.DiretorId = diretor_filme_excluir.diretorId
		                AND TB_Diretor_Filme.FilmeId = diretor_filme_excluir.filmeId

		                UPDATE temp1 
		                SET feito = 1
		                FROM #diretorFilmeExcluir temp1
		                WHERE feito = 0
		                AND EXISTS (SELECT TOP 500 *
					                FROM #diretorFilmeExcluir temp2
					                WHERE feito = 0
					                AND temp2.diretorId = temp1.diretorId
					                AND temp2.filmeId = temp1.filmeId
					                ORDER BY diretorId, filmeId)
	                END
                END");

            //Varchar Para Tabela
            migrationBuilder.Sql(
                @"
                    CREATE FUNCTION uf_varchar_para_tabela
                    (	
	                    @String      AS VARCHAR(max),
	                    @Delimitador AS CHAR(1)
                    )
                    RETURNS @Output TABLE(data VARCHAR(256) COLLATE Latin1_General_CI_AS)
                    BEGIN
                        DECLARE @start INT, @end INT
                        SELECT @start = 1, @end = CHARINDEX(@Delimitador, @String)
                        WHILE @start < LEN(@String) + 1 BEGIN
                            IF @end = 0 
                                SET @end = LEN(@String) + 1

                            INSERT INTO @Output (data) 
                            VALUES(SUBSTRING(@String, @start, @end - @start))
                            SET @start = @end + 1
                            SET @end = CHARINDEX(@Delimitador, @String, @start)
                        END
                        RETURN
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Ator_Filme");

            migrationBuilder.DropTable(
                name: "TB_Diretor_Filme");

            migrationBuilder.DropTable(
                name: "TB_Genero_Filme");

            migrationBuilder.DropTable(
                name: "TB_Pendencia");

            migrationBuilder.DropTable(
                name: "TB_Ator");

            migrationBuilder.DropTable(
                name: "TB_Diretor");

            migrationBuilder.DropTable(
                name: "TB_Genero");

            migrationBuilder.DropTable(
                name: "TB_Filme");

            migrationBuilder.DropTable(
                name: "Tb_Classificacao");

            //Higieniza Telefone
            migrationBuilder.Sql(@"DROP PROCEDURE sp_higieniza_telefone");

            //Higieniza Endereço
            migrationBuilder.Sql(@"DROP PROCEDURE sp_higieniza_endereco");

            //Higieniza Ator
            migrationBuilder.Sql(@"DROP PROCEDURE sp_higieniza_ator");

            //Higieniza Diretor
            migrationBuilder.Sql(@"DROP PROCEDURE sp_higieniza_diretor");

            //Varchar para Tabela
            migrationBuilder.Sql(@"DROP FUNCTION uf_varchar_para_tabela");
        }
    }
}
