using System.Collections.Generic;

namespace MovieCheck.Clientes.Models
{
    public class Classificacao
    {
        #region Atributos
        private int id;
        private string classificacao;
        private string descricao;
        private IList<Filme> filmes;
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string ClassificacaoIndicativa
        {
            get { return this.classificacao; }
            set { this.classificacao = value; }
        }
        public string Descricao
        {
            get { return this.descricao; }
            set { this.descricao = value; }
        }
        public IList<Filme> Filmes
        {
            get { return this.filmes; }
            set { this.filmes = value; }
        }
        #endregion

        #region Construtores
        public Classificacao()
        {

        }

        public Classificacao(string classificacao, string descricao)
        {
            this.classificacao = classificacao;
            this.descricao = descricao;
        }

        public Classificacao(int id, string classificacao, string descricao)
        {
            this.id = id;
            this.classificacao = classificacao;
            this.descricao = descricao;
        }
        #endregion

        #region Métodos
        public static IList<Classificacao> RetornaClassificacoes()
        {
            return new List<Classificacao>()
            {
                new Classificacao("l", "Livre"),
                new Classificacao("10", "Não recomendado para menores de 10 anos"),
                new Classificacao("12", "Não recomendado para menores de 12 anos"),
                new Classificacao("14", "Não recomendado para menores de 14 anos"),
                new Classificacao("16", "Não recomendado para menores de 16 anos"),
                new Classificacao("18", "Não recomendado para menores de 18 anos")
            };
        }
        #endregion
    }
}