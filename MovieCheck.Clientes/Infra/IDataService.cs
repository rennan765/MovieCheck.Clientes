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
        #endregion

        #region Usuario
        Usuario ObterUsuarioPorEmail(string email);
        Usuario ObterUsuarioSessao();
        string ObterTipoUsuario(Usuario usuario);
        bool VerificarUsuarioPorEmail(string email);
        void EditarUsuario(Usuario usuario);
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
    }
}