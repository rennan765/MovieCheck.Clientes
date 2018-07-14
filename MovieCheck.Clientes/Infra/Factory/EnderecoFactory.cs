using MovieCheck.Site.Models;
using System.Linq;

namespace MovieCheck.Site.Infra.Factory
{
    public static class EnderecoFactory
    {
        public static void ValidaEstado(string siglaEstado)
        {
            if (!Estado.ListState().Any(e => e.NomeAbreviado == siglaEstado))
            {
                throw new NewUserFailedException("Estado inválido.");
            }
        }

        public static void ValidaNumero(string numero)
        {
            if (!DefaultFactory.IsNumeric(numero))
            {
                throw new NewUserFailedException("Número do endereço inválido.");
            }
        }
    }
}
