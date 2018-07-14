using MovieCheck.Core.Models;
using System.Collections.Generic;

namespace MovieCheck.Core.Interface.Services
{
    interface IUsuarioDataService
    {
        #region Usuario
        IList<Usuario> ObterListaUsuario();
        Usuario ObterUsuarioCompletoPorId(int id);
        Usuario ObterUsuarioPorId(int id);
        Usuario ObterUsuarioPorEmail(string email);
        string ObterTipoUsuario(Usuario usuario);
        bool VerificarUsuarioPorEmail(string email);
        void EditarUsuario(Usuario usuario);
        void ExcluirUsuario(Usuario usuario);
        void AlterarSenha(Usuario usuario, string novaSenha);
        #endregion

        #region Cliente
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
    }
}
