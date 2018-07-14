using MovieCheck.Core.Models;
using System.Collections.Generic;

namespace MovieCheck.Core.Interface.Services
{
    public interface IFilmeDataService
    {
        #region Filme
        void AdicionarFilme(Filme filme, IList<Ator> atores, IList<Diretor> diretores, IList<Genero> generos, Classificacao classificacao);
        void ExcluirTitulo(string titulo);
        bool TituloJaExiste(string titulo);
        void AdicionarExemplar(Filme filme, string midia);
        void EditarTitulo(Filme filme, IList<Ator> atores, IList<Diretor> diretores, IList<Genero> generos);
        void ExcluirExemplar(Filme filme);
        Filme ObterFilmePorId(int id);
        IList<Filme> ObterCatalogoCompleto();
        IList<Filme> ObterListaDvds();
        IList<Filme> ObterListaBluRays();
        IList<Filme> ObterListaFilmePorTitulo(string titulo);
        IList<Filme> PesquisaAvancada(string titulo, int ano, string ator, string diretor, string classificacao, IList<string> listaGenero);
        #endregion

        #region Ator
        Ator ObterAtorPorNome(string nome);
        void AdicionarAtor(Ator ator);
        #endregion

        #region Diretor
        Diretor ObterDiretorPorNome(string nome);
        void AdicionarDiretor(Diretor diretor);
        #endregion

        #region Genero 
        IList<Genero> ObterListaGenero();
        Genero ObterGeneroPorDescricao(string descricao);
        void AdicionarGenero(Genero genero);
        #endregion

        #region Classificacao
        IList<Classificacao> ObterListaClassificacao();
        Classificacao ObterClassificacaoPorSigla(string sigla);
        Classificacao ObterClassificacaoPorDescricao(string descricao);
        void AdicionarClassificacao(Classificacao classificacao);
        bool ExisteClassificacao(Classificacao classificacao);
        #endregion
    }
}
