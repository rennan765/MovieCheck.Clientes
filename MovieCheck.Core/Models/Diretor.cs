using System.Collections.Generic;

namespace MovieCheck.Core.Models
{
    public class Diretor
    {
        #region Atributos
        private int id;
        private string nome;
        private IList<DiretorFilme> filmes;
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
        public string Nome
        {
            get
            {
                return this.nome;
            }
            set
            {
                this.nome = value;
            }
        }
        public IList<DiretorFilme> Filmes
        {
            get { return this.filmes; }
            set { this.filmes = value; }
        }
        #endregion

        #region Construtores
        public Diretor()
        {
            this.Filmes = new List<DiretorFilme>();
        }

        public Diretor(string nome)
        {
            this.Filmes = new List<DiretorFilme>();
            this.Nome = nome;
        }

        public Diretor(int id, string nome)
        {
            this.Filmes = new List<DiretorFilme>();
            this.Id = id;
            this.Nome = nome;
        }
        #endregion
    }
}
