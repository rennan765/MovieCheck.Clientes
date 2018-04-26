using System.Collections.Generic;

namespace MovieCheck.Clientes.Models
{
    public class Ator
    {
        #region Atributos
        private int id;
        private string nome;
        private IList<AtorFilme> filmes;
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
        public IList<AtorFilme> Filmes
        {
            get { return this.filmes; }
            set { this.filmes = value; }
        }
        #endregion

        #region Construtores
        public Ator()
        {
            this.Filmes = new List<AtorFilme>();
        }

        public Ator(string nome)
        {
            this.Filmes = new List<AtorFilme>();
            this.Nome = nome;
        }

        public Ator(int id, string nome)
        {
            this.Filmes = new List<AtorFilme>();
            this.Id = id;
            this.Nome = nome;
        }
        #endregion
    }
}
