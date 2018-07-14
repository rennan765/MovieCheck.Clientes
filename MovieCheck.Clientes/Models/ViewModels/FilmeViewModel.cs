namespace MovieCheck.Site.Models.ViewModels
{
    public class FilmeViewModel
    {
        #region Atributos
        private int id;
        private string titulo;
        private int ano;
        private string poster;
        private string sinopse;
        private string tipoMidia;
        private string iconeMidia;
        private string atores;
        private string diretores;
        private string classificacaoIndicativa;
        private string generos;
        private string situacao;
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
        public string TipoMidia
        {
            get { return this.tipoMidia; }
            set { this.tipoMidia = value; }
            
        }
        public string IconeMidia
        {
            get { return this.iconeMidia; }
            set { this.iconeMidia = value; }

        }
        public string Atores
        {
            get { return this.atores; }
            set { this.atores = value; }
        }
        public string Diretores
        {
            get { return this.diretores; }
            set { this.diretores = value; }
        }
        public string ClassificacaoIndicativa
        {
            get { return this.classificacaoIndicativa; }
            set { this.classificacaoIndicativa = value; }
        }
        public string Generos
        {
            get { return this.generos; }
            set { this.generos = value; }
        }
        public string Situacao
        {
            get { return this.situacao; }
            set { this.situacao = value; }
        }
        #endregion

        #region Construtores
        public FilmeViewModel(Filme filme)
        {
            this.id = (!(filme.Id <= 0) ? filme.Id : 0);
            this.titulo = filme.Titulo;
            this.ano = filme.Ano;
            this.poster = filme.Poster;
            this.sinopse = filme.Sinopse;
            this.tipoMidia = filme.ObterTipoMidia();
            this.iconeMidia = filme.ObterIconeMidia();

            this.atores = "";
            foreach (var ator in filme.Atores)
            {
                this.atores += $"{ator.Ator.Nome} ";
            }

            this.diretores = "";
            foreach (var diretor in filme.Diretores)
            {
                this.diretores += $"{diretor.Diretor.Nome} ";
            }

            this.classificacaoIndicativa = filme.ClassificacaoIndicativa.Descricao;

            this.generos = "";
            foreach (var genero in filme.Generos)
            {
                this.generos += $"{genero.Genero.Descricao} ";
            }

            this.situacao = (filme.Disponivel() ? "Disponível" : "Não disponível");
        }
        #endregion

        #region Métodos
        public bool Disponivel()
        {
            return this.situacao == "Disponível";
        }
        #endregion
    }
}
