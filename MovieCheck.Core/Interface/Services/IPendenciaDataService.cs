using MovieCheck.Core.Models;
using System.Collections.Generic;

namespace MovieCheck.Core.Interface.Services
{
    interface IPendenciaDataService
    {
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
