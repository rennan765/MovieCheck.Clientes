IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [TB_Endereco] (
    [UsuarioId] int NOT NULL IDENTITY,
    [Bairro] nvarchar(max) NULL,
    [Cep] nvarchar(max) NULL,
    [Cidade] nvarchar(max) NULL,
    [Complemento] nvarchar(max) NULL,
    [Estado] nvarchar(max) NULL,
    [Logradouro] nvarchar(max) NULL,
    [Numero] int NOT NULL,
    CONSTRAINT [PK_TB_Endereco] PRIMARY KEY ([UsuarioId])
);

GO

CREATE TABLE [TB_Telefone] (
    [Id] int NOT NULL IDENTITY,
    [Ddd] int NOT NULL,
    [Numero] nvarchar(max) NULL,
    [Tipo] int NOT NULL,
    CONSTRAINT [PK_TB_Telefone] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TB_Usuario] (
    [Id] int NOT NULL IDENTITY,
    [Discriminator] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    [EnderecoUsuarioId] int NULL,
    [Nome] nvarchar(max) NULL,
    [Senha] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Cpf] nvarchar(max) NULL,
    [Tipo] int NULL,
    [ClienteId] int NULL,
    CONSTRAINT [PK_TB_Usuario] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_Usuario_TB_Endereco_EnderecoUsuarioId] FOREIGN KEY ([EnderecoUsuarioId]) REFERENCES [TB_Endereco] ([UsuarioId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TB_Usuario_Telefone] (
    [UsuarioId] int NOT NULL,
    [TelefoneId] int NOT NULL,
    CONSTRAINT [PK_TB_Usuario_Telefone] PRIMARY KEY ([UsuarioId], [TelefoneId]),
    CONSTRAINT [FK_TB_Usuario_Telefone_TB_Telefone_TelefoneId] FOREIGN KEY ([TelefoneId]) REFERENCES [TB_Telefone] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TB_Usuario_Telefone_TB_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [TB_Usuario] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_TB_Usuario_EnderecoUsuarioId] ON [TB_Usuario] ([EnderecoUsuarioId]);

GO

CREATE INDEX [IX_TB_Usuario_ClienteId] ON [TB_Usuario] ([ClienteId]);

GO

CREATE INDEX [IX_TB_Usuario_Telefone_TelefoneId] ON [TB_Usuario_Telefone] ([TelefoneId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171207001437_Usuario', N'2.0.1-rtm-125');

GO

CREATE TABLE [TB_Ator] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NULL,
    CONSTRAINT [PK_TB_Ator] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Tb_Classificacao] (
    [Id] int NOT NULL IDENTITY,
    [ClassificacaoIndicativa] nvarchar(max) NULL,
    [Descricao] nvarchar(max) NULL,
    CONSTRAINT [PK_Tb_Classificacao] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TB_Diretor] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NULL,
    CONSTRAINT [PK_TB_Diretor] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TB_Genero] (
    [Id] int NOT NULL IDENTITY,
    [Descricao] nvarchar(max) NULL,
    CONSTRAINT [PK_TB_Genero] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TB_Filme] (
    [Id] int NOT NULL IDENTITY,
    [Ano] int NOT NULL,
    [ClassificacaoId] int NOT NULL,
    [Midia] nvarchar(max) NULL,
    [Poster] nvarchar(max) NULL,
    [Sinopse] nvarchar(max) NULL,
    [Titulo] nvarchar(max) NULL,
    CONSTRAINT [PK_TB_Filme] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_Filme_Tb_Classificacao_ClassificacaoId] FOREIGN KEY ([ClassificacaoId]) REFERENCES [Tb_Classificacao] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TB_Ator_Filme] (
    [AtorId] int NOT NULL,
    [FilmeId] int NOT NULL,
    CONSTRAINT [PK_TB_Ator_Filme] PRIMARY KEY ([AtorId], [FilmeId]),
    CONSTRAINT [FK_TB_Ator_Filme_TB_Ator_AtorId] FOREIGN KEY ([AtorId]) REFERENCES [TB_Ator] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TB_Ator_Filme_TB_Filme_FilmeId] FOREIGN KEY ([FilmeId]) REFERENCES [TB_Filme] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TB_Diretor_Filme] (
    [DiretorId] int NOT NULL,
    [FilmeId] int NOT NULL,
    CONSTRAINT [PK_TB_Diretor_Filme] PRIMARY KEY ([DiretorId], [FilmeId]),
    CONSTRAINT [FK_TB_Diretor_Filme_TB_Diretor_DiretorId] FOREIGN KEY ([DiretorId]) REFERENCES [TB_Diretor] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TB_Diretor_Filme_TB_Filme_FilmeId] FOREIGN KEY ([FilmeId]) REFERENCES [TB_Filme] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TB_Genero_Filme] (
    [GeneroId] int NOT NULL,
    [FilmeId] int NOT NULL,
    CONSTRAINT [PK_TB_Genero_Filme] PRIMARY KEY ([GeneroId], [FilmeId]),
    CONSTRAINT [FK_TB_Genero_Filme_TB_Filme_FilmeId] FOREIGN KEY ([FilmeId]) REFERENCES [TB_Filme] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TB_Genero_Filme_TB_Genero_GeneroId] FOREIGN KEY ([GeneroId]) REFERENCES [TB_Genero] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TB_Pendencia] (
    [Id] int NOT NULL IDENTITY,
    [DataReserva] datetime2 NOT NULL,
    [FilmeId] int NOT NULL,
    [Status] nvarchar(max) NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_TB_Pendencia] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TB_Pendencia_TB_Filme_FilmeId] FOREIGN KEY ([FilmeId]) REFERENCES [TB_Filme] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TB_Pendencia_TB_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [TB_Usuario] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_TB_Ator_Filme_FilmeId] ON [TB_Ator_Filme] ([FilmeId]);

GO

CREATE INDEX [IX_TB_Diretor_Filme_FilmeId] ON [TB_Diretor_Filme] ([FilmeId]);

GO

CREATE INDEX [IX_TB_Filme_ClassificacaoId] ON [TB_Filme] ([ClassificacaoId]);

GO

CREATE INDEX [IX_TB_Genero_Filme_FilmeId] ON [TB_Genero_Filme] ([FilmeId]);

GO

CREATE INDEX [IX_TB_Pendencia_FilmeId] ON [TB_Pendencia] ([FilmeId]);

GO

CREATE INDEX [IX_TB_Pendencia_UsuarioId] ON [TB_Pendencia] ([UsuarioId]);

GO


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
                END

GO


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
                    END

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180424131440_Filme', N'2.0.1-rtm-125');

GO

