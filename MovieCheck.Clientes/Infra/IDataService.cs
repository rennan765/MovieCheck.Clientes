using MovieCheck.Clientes.Models;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Infra
{
    public interface IDataService
    {
        #region Banco
        void IniciarDb();
        #endregion

        #region Secao
        void IniciarSessao(Usuario usuario);
        bool VerificarSecao();
        void FinalizarSessao();
        Usuario ObterUsuarioSessao();
        #endregion

        #region Usuario
        Usuario ObterUsuarioPorId(int id);
        Usuario ObterUsuarioPorEmail(string email);
        string ObterTipoUsuario(Usuario usuario);
        bool VerificarUsuarioPorEmail(string email);
        void EditarUsuario(Usuario usuario);
        void ExcluirUsuario(Usuario usuario);
        #endregion

        #region Cliente
        bool VerificarClientePorCpf(string cpf);
        #endregion

        #region Dependente
        IList<Dependente> ObterListaDependente(Cliente cliente);
        Dependente ObterDependente(int id);
        void AdicionarDependente(Cliente responsavel, Dependente dependente);
        #endregion

        #region Telefone
        bool ExisteTelefone(Telefone telefone);
        bool ExisteOutroUsuarioTelefone(Usuario usuario, Telefone telefone);
        Telefone ObterTelefone(Telefone telefone);
        #endregion

        #region Filme
        void AdicionarFilme(Filme filme, IList<Ator> atores, IList<Diretor> diretores, IList<Genero> generos, Classificacao classificacao);
        void ExcluirTitulo(string titulo);
        bool TituloJaExiste(string titulo);
        void AdicionarExemplar(Filme filme, string midia);
        void ExcluirExemplar(Filme filme);
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
        Genero ObterGeneroPorDescricao(string descricao);
        void AdicionarGenero(Genero genero);
        #endregion

        #region Classificacao
        Classificacao ObterClassificacaoPorSigla(string sigla);
        Classificacao ObterClassificacaoPorDescricao(string descricao);
        void AdicionarClassificacao(Classificacao classificacao);
        bool ExisteClassificacao(Classificacao classificacao);
        #endregion
    }
}