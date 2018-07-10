using MovieCheck.Api.Models;
using System.Collections.Generic;

namespace MovieCheck.Api.Infra.Factory
{
    public static class TelefoneFactory
    {
        public static IList<Telefone> ObterTelefones(this IDataService dataService, Usuario usuario)
        {
            var listaTelefone = dataService.ObterTelefonesPorUsuario(usuario);

            foreach (var telefone in listaTelefone)
            {
                telefone.Usuarios = null;
            }

            return listaTelefone;
        }
    }
}
