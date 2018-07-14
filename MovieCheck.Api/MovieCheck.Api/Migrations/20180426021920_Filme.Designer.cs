﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MovieCheck.Api.Infra;
using System;

namespace MovieCheck.Api.Migrations
{
    [DbContext(typeof(MovieCheckContext))]
    [Migration("20180426021920_Filme")]
    partial class Filme
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieCheck.Site.Models.Ator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("TB_Ator");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.AtorFilme", b =>
                {
                    b.Property<int>("AtorId");

                    b.Property<int>("FilmeId");

                    b.HasKey("AtorId", "FilmeId");

                    b.HasIndex("FilmeId");

                    b.ToTable("TB_Ator_Filme");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Classificacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassificacaoIndicativa");

                    b.Property<string>("Descricao");

                    b.HasKey("Id");

                    b.ToTable("Tb_Classificacao");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Diretor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("TB_Diretor");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.DiretorFilme", b =>
                {
                    b.Property<int>("DiretorId");

                    b.Property<int>("FilmeId");

                    b.HasKey("DiretorId", "FilmeId");

                    b.HasIndex("FilmeId");

                    b.ToTable("TB_Diretor_Filme");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Endereco", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("Cep");

                    b.Property<string>("Cidade");

                    b.Property<string>("Complemento");

                    b.Property<string>("Estado");

                    b.Property<string>("Logradouro");

                    b.Property<int>("Numero");

                    b.HasKey("UsuarioId");

                    b.ToTable("TB_Endereco");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Filme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ano");

                    b.Property<int>("ClassificacaoId");

                    b.Property<string>("Midia");

                    b.Property<string>("Poster");

                    b.Property<string>("Sinopse");

                    b.Property<string>("Titulo");

                    b.HasKey("Id");

                    b.HasIndex("ClassificacaoId");

                    b.ToTable("TB_Filme");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao");

                    b.HasKey("Id");

                    b.ToTable("TB_Genero");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.GeneroFilme", b =>
                {
                    b.Property<int>("GeneroId");

                    b.Property<int>("FilmeId");

                    b.HasKey("GeneroId", "FilmeId");

                    b.HasIndex("FilmeId");

                    b.ToTable("TB_Genero_Filme");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Pendencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataReserva");

                    b.Property<int>("FilmeId");

                    b.Property<string>("Status");

                    b.Property<int>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("FilmeId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TB_Pendencia");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Telefone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ddd");

                    b.Property<string>("Numero");

                    b.Property<int>("Tipo");

                    b.HasKey("Id");

                    b.ToTable("TB_Telefone");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(-1)");

                    b.Property<string>("Email");

                    b.Property<int?>("EnderecoUsuarioId");

                    b.Property<string>("Nome");

                    b.Property<string>("Senha");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoUsuarioId");

                    b.ToTable("TB_Usuario");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Usuario");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.UsuarioTelefone", b =>
                {
                    b.Property<int>("UsuarioId");

                    b.Property<int>("TelefoneId");

                    b.HasKey("UsuarioId", "TelefoneId");

                    b.HasIndex("TelefoneId");

                    b.ToTable("TB_Usuario_Telefone");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Cliente", b =>
                {
                    b.HasBaseType("MovieCheck.Site.Models.Usuario");

                    b.Property<string>("Cpf");

                    b.Property<int>("Tipo");

                    b.ToTable("Cliente");

                    b.HasDiscriminator().HasValue("Cliente");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Dependente", b =>
                {
                    b.HasBaseType("MovieCheck.Site.Models.Usuario");

                    b.Property<int>("ClienteId");

                    b.HasIndex("ClienteId");

                    b.ToTable("Dependente");

                    b.HasDiscriminator().HasValue("Dependente");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.AtorFilme", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Ator", "Ator")
                        .WithMany("Filmes")
                        .HasForeignKey("AtorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Site.Models.Filme", "Filme")
                        .WithMany("Atores")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.DiretorFilme", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Diretor", "Diretor")
                        .WithMany("Filmes")
                        .HasForeignKey("DiretorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Site.Models.Filme", "Filme")
                        .WithMany("Diretores")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Filme", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Classificacao", "ClassificacaoIndicativa")
                        .WithMany("Filmes")
                        .HasForeignKey("ClassificacaoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.GeneroFilme", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Filme", "Filme")
                        .WithMany("Generos")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Site.Models.Genero", "Genero")
                        .WithMany("Filmes")
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Pendencia", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Filme", "Filme")
                        .WithMany("Pendencias")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Site.Models.Usuario", "Usuario")
                        .WithMany("Pendencias")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Usuario", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoUsuarioId");
                });

            modelBuilder.Entity("MovieCheck.Site.Models.UsuarioTelefone", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Telefone", "Telefone")
                        .WithMany("Usuarios")
                        .HasForeignKey("TelefoneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Site.Models.Usuario", "Usuario")
                        .WithMany("Telefones")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Site.Models.Dependente", b =>
                {
                    b.HasOne("MovieCheck.Site.Models.Cliente", "Cliente")
                        .WithMany("Dependentes")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
