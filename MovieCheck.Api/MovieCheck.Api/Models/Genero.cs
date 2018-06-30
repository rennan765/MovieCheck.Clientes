using System.Collections.Generic;

namespace MovieCheck.Api.Models
{
    public class Genero
    {
        #region Atributos
        private int id;
        private string descricao;
        private IList<GeneroFilme> filmes;
        #endregion

        #region Propriedades
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public string Descricao
        {
            get
            {
                return this.descricao;
            }
            set
            {
                this.descricao = value;
            }
        }
        public IList<GeneroFilme> Filmes
        {
            get { return this.filmes; }
            set { this.filmes = value; }
        }
        #endregion

        #region Construtores
        public Genero()
        {
            this.filmes = new List<GeneroFilme>();
        }

        public Genero(string descricao)
        {
            this.filmes = new List<GeneroFilme>();
            this.Descricao = descricao;
        }

        public Genero(int id, string descricao)
        {
            this.filmes = new List<GeneroFilme>();
            this.Id = id;
            this.Descricao = descricao;
        }
        #endregion
    }
}
