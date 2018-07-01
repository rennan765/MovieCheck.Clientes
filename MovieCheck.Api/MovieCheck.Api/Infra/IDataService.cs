using MovieCheck.Api.Models;
using System.Collections.Generic;

namespace MovieCheck.Api.Infra
{
    public interface IDataService
    {
        #region Banco
        void IniciarDb();
        void HigienizaTelefone();
        void HigienizaEndereco();
        void HigienizaAtor();
        void HigienizaDiretor();
        #endregion

        #region Usuario
        Usuario ObterUsuarioPorId(int id);
        Usuario ObterUsuarioPorEmail(string email);
        string ObterTipoUsuario(Usuario usuario);
        bool VerificarUsuarioPorEmail(string email);
        void EditarUsuario(Usuario usuario);
        void ExcluirUsuario(Usuario usuario);
        void AlterarSenha(Usuario usuario, string novaSenha);
        #endregion

        #region Cliente
        IList<Cliente> ObterListaCliente();
        void AdicionarCliente(Cliente cliente);
        bool VerificarClientePorCpf(string cpf);
        IList<string> ObterEmailAdministradores();
        void RemoverCliente(Cliente cliente);
        #endregion

        #region Dependente
        IList<Dependente> ObterListaDependente();
        IList<Dependente> ObterListaDependente(Cliente cliente);
        Dependente ObterDependente(int id);
        void AdicionarDependente(Cliente responsavel, Dependente dependente);
        void RemoverDependente(Dependente dependente);
        void EditarDependente(int idDependente, Dependente dependente);
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

        #region Pendencia
        IList<Pendencia> ObterListaPendencia();
        bool ExistePendencia(Filme filme);
        IList<Pendencia> ObterPendenciaPorUsuario(Usuario usuario);
        Pendencia ObterPendenciaPorId(int id);
        void AdicionarPendencia(Pendencia pendencia);
        void EditarPendencia(Pendencia pendencia);
        void RemoverPendencia(Pendencia pendencia);
        #endregion
    }
}