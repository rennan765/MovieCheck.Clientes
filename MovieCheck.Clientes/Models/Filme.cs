using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Infra.Factory;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Models
{
    public class Filme
    {
        #region Atributos
        private int id;
        private string titulo;
        private int ano;
        private string poster;
        private string sinopse;
        private string midia;     //0: DVD / 1: Blu-Ray
        private IList<AtorFilme> atores;
        private IList<DiretorFilme> diretores;
        private int classificacaoId;
        private Classificacao classificacaoIndicativa;
        private IList<GeneroFilme> generos;
        private IList<Pendencia> pendencias;
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Titulo
        {
            get { return this.titulo; }
            set { this.titulo = value; }
        }
        public int Ano
        {
            get { return this.ano; }
            set { this.ano = value; }
        }
        public string Poster
        {
            get { return this.poster; }
            set { this.poster = value; }
        }
        public string Sinopse
        {
            get
            {
                if (!string.IsNullOrEmpty(this.sinopse))
                {
                    return this.sinopse;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.sinopse = value;
                }
                else
                {
                    this.sinopse = "";
                }
            }
        }
        public string Midia
        {
            get { return this.midia; }
            set
            {
                if (value == "0" || value == "1")
                {
                    this.midia = value;
                }
                else
                {
                    throw new NewMovieFailedException("Mídia inválida");
                }
            }
        }   //0: DVD / 1: Blu-Ray
        public IList<AtorFilme> Atores
        {
            get { return this.atores; }
            set { this.atores = value; }
        }
        public IList<DiretorFilme> Diretores
        {
            get { return this.diretores; }
            set { this.diretores = value; }
        }
        public int ClassificacaoId
        {
            get { return this.classificacaoId; }
            set { this.classificacaoId = value; }
        }
        public Classificacao ClassificacaoIndicativa
        {
            get { return this.classificacaoIndicativa; }
            set
            {
                try
                {
                    if (!(value is null))
                    {
                        MovieFactory.ExisteClassificacao(value);
                        this.classificacaoIndicativa = value;
                    }
                    else
                    {
                        throw new NewMovieFailedException("Filme cadastrado sem classificação indicativa. Necessário haver uma classificação indicativa.");
                    }
                }
                catch (NewMovieFailedException e)
                {
                    throw e;
                }
            }
        }
        public IList<GeneroFilme> Generos
        {
            get { return this.generos; }
            set { this.generos = value; }
        }
        public IList<Pendencia> Pendencias
        {
            get { return this.pendencias; }
            set { this.pendencias = value; }
        }
        #endregion

        #region Construtores
        public Filme()
        {
            this.Atores = new List<AtorFilme>();
            this.Diretores = new List<DiretorFilme>();
            this.Generos = new List<GeneroFilme>();
            this.pendencias = new List<Pendencia>();
        }
        #endregion

        #region Métodos
        public void AdicionarAtor(Ator ator)
        {
            if (ator.Id <= 0)
            {
                this.atores.Add(new AtorFilme() { Ator = ator });
            }
            else
            {
                this.atores.Add(new AtorFilme() { AtorId = ator.Id, Ator = ator });
            }
        }
        
        public void AdicionarDiretor(Diretor diretor)
        {
            if(diretor.Id <= 0)
            {
                this.diretores.Add(new DiretorFilme() { Diretor = diretor });
            }
            else
            {
                this.diretores.Add(new DiretorFilme() { DiretorId = diretor.Id, Diretor = diretor });
            }
        }

        public void AdicionarGenero(Genero genero)
        {
            if (genero.Id <= 0)
            {
                this.generos.Add(new GeneroFilme() { Genero = genero });
            }
            else
            {
                this.generos.Add(new GeneroFilme() { GeneroId = genero.Id, Genero = genero });
            }
        }

        public IList<Ator> ObterAtores()
        {
            IList<Ator> listaAtor = new List<Ator>();

            foreach (AtorFilme atorFilme in this.atores)
            {
                listaAtor.Add(atorFilme.Ator);
            }

            return listaAtor;
        }

        public IList<Diretor> ObterDiretores()
        {
            IList<Diretor> listaDiretor = new List<Diretor>();

            foreach (DiretorFilme diretorFilme in this.diretores)
            {
                listaDiretor.Add(diretorFilme.Diretor);
            }

            return listaDiretor;
        }

        public IList<Genero> ObterGeneros()
        {
            IList<Genero> listaGenero = new List<Genero>();

            foreach (GeneroFilme generoFilme in this.generos)
            {
                listaGenero.Add(generoFilme.Genero);
            }

            return listaGenero;
        }
        #endregion
    }
}
