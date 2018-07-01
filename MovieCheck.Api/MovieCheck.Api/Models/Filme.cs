using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Api.Models
{
    public class Filme
    {
        #region Atributos
        private int id;
        private string titulo;
        private int ano;
        private string poster;
        private string sinopse;
        private string midia;
        private IList<AtorFilme> atores;
        private IList<DiretorFilme> diretores;
        private int classificacaoId;
        private Classificacao classificacaoIndicativa;
        private IList<GeneroFilme> generos;
        private IList<Pendencia> pendencias;
        private IDictionary<string, string> dicionarioMidia;
        private IDictionary<string, string> dicionarioIconeMidia;
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
                //ValidarMidia(value);
                this.midia = value;
            }
        }
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
            set { this.classificacaoIndicativa = value; }
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
            IniciarDicionarioMidia();
            IniciarDicionarioIconeMidia();
            this.Atores = new List<AtorFilme>();
            this.Diretores = new List<DiretorFilme>();
            this.Generos = new List<GeneroFilme>();
            this.pendencias = new List<Pendencia>();
        }
        #endregion

        #region Métodos
        private void IniciarDicionarioMidia()
        {
            this.dicionarioMidia = new Dictionary<string, string>();

            this.dicionarioMidia.Add("0", "DVD");
            this.dicionarioMidia.Add("1", "Blu-Ray");
        }

        private void IniciarDicionarioIconeMidia()
        {
            this.dicionarioIconeMidia = new Dictionary<string, string>();

            this.dicionarioIconeMidia.Add("0", "http://icons.iconseeker.com/png/fullsize/ivista-2-os-x-icons/dvd-52.png");
            this.dicionarioIconeMidia.Add("1", "https://cdn.icon-icons.com/icons2/143/PNG/256/blu_ray_21074.png");
        }

        //private void ValidarMidia(string midia)
        //{
        //    IniciarDicionarioMidia();
        //    IniciarDicionarioIconeMidia();

        //    if (!dicionarioMidia.ContainsKey(midia))
        //    {
        //        throw new NewMovieFailedException("Mídia inválida");
        //    }
        //}

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
        
        public void RemoverAtores()
        {
            foreach (var ator in this.atores)
            {
                this.atores.Remove(ator);
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

        public void RemoverDiretores()
        {
            foreach (var diretor in this.diretores)
            {
                this.diretores.Remove(diretor);
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

        public void RemoverGeneros()
        {
            foreach (var genero in this.generos)
            {
                this.generos.Remove(genero);
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

        public string ObterTipoMidia()
        {
            string tipoMidia = "";
            //ValidarMidia(this.midia);
            foreach (var dm in dicionarioMidia)
            {
                if (dm.Key == this.midia)
                {
                    tipoMidia = dm.Value;
                    break;
                }
            }
            return tipoMidia;
        }

        public string ObterIconeMidia()
        {
            string iconeMidia = "";
            //ValidarMidia(this.midia);
            foreach (var di in dicionarioIconeMidia)
            {
                if (di.Key == this.midia)
                {
                    iconeMidia = di.Value;
                    break;
                }
            }
            return iconeMidia;
        }

        public bool Disponivel()
        {
            return this.pendencias.Count <= 0 || this.pendencias.Any(p => p.Disponivel());
        }

        public void AtualizarTitulo(Filme filme)
        {
            this.titulo = filme.titulo;
            this.ano = filme.ano;
            this.poster = filme.poster;
            this.sinopse = filme.sinopse;
            this.midia = filme.midia;
            this.classificacaoIndicativa = filme.ClassificacaoIndicativa;
        }
        #endregion
    }
}
