using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MovieCheck.Api.Infra;

namespace MovieCheck.Api.Migrations
{
    [DbContext(typeof(MovieCheckContext))]
    [Migration("20171207001437_Usuario")]
    partial class Usuario
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Endereco", b =>
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

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Telefone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ddd");

                    b.Property<string>("Numero");

                    b.Property<int>("Tipo");

                    b.HasKey("Id");

                    b.ToTable("TB_Telefone");
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

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

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.UsuarioTelefone", b =>
                {
                    b.Property<int>("UsuarioId");

                    b.Property<int>("TelefoneId");

                    b.HasKey("UsuarioId", "TelefoneId");

                    b.HasIndex("TelefoneId");

                    b.ToTable("TB_Usuario_Telefone");
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Cliente", b =>
                {
                    b.HasBaseType("MovieCheck.Administradores.Modeling.Usuario");

                    b.Property<string>("Cpf");

                    b.Property<int>("Tipo");

                    b.ToTable("Cliente");

                    b.HasDiscriminator().HasValue("Cliente");
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Dependente", b =>
                {
                    b.HasBaseType("MovieCheck.Administradores.Modeling.Usuario");

                    b.Property<int>("ClienteId");

                    b.HasIndex("ClienteId");

                    b.ToTable("Dependente");

                    b.HasDiscriminator().HasValue("Dependente");
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Usuario", b =>
                {
                    b.HasOne("MovieCheck.Administradores.Modeling.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoUsuarioId");
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.UsuarioTelefone", b =>
                {
                    b.HasOne("MovieCheck.Administradores.Modeling.Telefone", "Telefone")
                        .WithMany("Usuarios")
                        .HasForeignKey("TelefoneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieCheck.Administradores.Modeling.Usuario", "Usuario")
                        .WithMany("Telefones")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieCheck.Administradores.Modeling.Dependente", b =>
                {
                    b.HasOne("MovieCheck.Administradores.Modeling.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
