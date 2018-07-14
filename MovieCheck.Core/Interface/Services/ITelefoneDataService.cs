using MovieCheck.Core.Models;
using System.Collections.Generic;

namespace MovieCheck.Core.Interface.Services
{
    public interface ITelefoneDataService
    {
        #region Telefone
        bool ExisteTelefone(Telefone telefone);
        bool ExisteOutroUsuarioTelefone(Usuario usuario, Telefone telefone);
        Telefone ObterTelefone(Telefone telefone);
        Telefone ObterTelefonePorId(int id);
        IList<Telefone> ObterTelefonesPorUsuario(Usuario usuario);
        #endregion
    }
}
