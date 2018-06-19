using Microsoft.EntityFrameworkCore;
using MovieCheck.Clientes.Models;
//using System.IO;
//using Microsoft.Extensions.Configuration;

namespace MovieCheck.Clientes.Infra
{
    public class MovieCheckContext : DbContext
    {
        #region Propriedades
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Dependente> Dependente { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Filme> Filme { get; set; }
        public DbSet<Ator> Ator { get; set; }
        public DbSet<Diretor> Diretor { get; set; }
        public DbSet<Genero> Genero { get; set; }
        public DbSet<Classificacao> Classificacao { get; set; }
        public DbSet<Pendencia> Pendencia { get; set; }
        #endregion

        #region Construtores
        public MovieCheckContext()
        {

        }

        public MovieCheckContext(DbContextOptions<MovieCheckContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        #endregion

        #region Métodos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //USUARIO
            modelBuilder.Entity<Usuario>().ToTable("TB_Usuario");
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>().Property<string>("Discriminator").HasColumnType("nvarchar(-1)");

            //TELEFONE
            modelBuilder.Entity<Telefone>().ToTable("TB_Telefone");
            modelBuilder.Entity<UsuarioTelefone>().ToTable("TB_Usuario_Telefone");
            modelBuilder.Entity<UsuarioTelefone>().HasKey(ut => new { ut.UsuarioId, ut.TelefoneId });

            //ENDERECO
            modelBuilder.Entity<Endereco>().ToTable("TB_Endereco");
            modelBuilder.Entity<Endereco>().Property<int>("UsuarioId");
            modelBuilder.Entity<Endereco>().HasKey("UsuarioId");

            //FILME
            modelBuilder.Entity<Filme>().ToTable("TB_Filme");
            modelBuilder.Entity<Filme>().HasKey(f => f.Id);

            //ATOR
            modelBuilder.Entity<Ator>().ToTable("TB_Ator");
            modelBuilder.Entity<AtorFilme>().ToTable("TB_Ator_Filme");
            modelBuilder.Entity<AtorFilme>().HasKey(af => new { af.AtorId, af.FilmeId });

            //DIRETOR
            modelBuilder.Entity<Diretor>().ToTable("TB_Diretor");
            modelBuilder.Entity<DiretorFilme>().ToTable("TB_Diretor_Filme");
            modelBuilder.Entity<DiretorFilme>().HasKey(df => new { df.DiretorId, df.FilmeId });

            //GENERO
            modelBuilder.Entity<Genero>().ToTable("TB_Genero");
            modelBuilder.Entity<GeneroFilme>().ToTable("TB_Genero_Filme");
            modelBuilder.Entity<GeneroFilme>().HasKey(gf => new { gf.GeneroId, gf.FilmeId });

            //CLASSIFICACAO
            modelBuilder.Entity<Classificacao>().ToTable("Tb_Classificacao");
            modelBuilder.Entity<Classificacao>().HasKey(c => c.Id);

            //PENDENCIA
            modelBuilder.Entity<Pendencia>().ToTable("TB_Pendencia");
            modelBuilder.Entity<Pendencia>().HasKey(pen => new { pen.Id });
        }

        //Este método foi substituído por um serviço.
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json");

        //    var configuration = builder.Build();

        //    optionsBuilder.UseSqlServer(configuration["ConnectionStrings:Default"]);
        //}
        #endregion
    }
}
