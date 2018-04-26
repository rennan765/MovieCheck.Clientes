using MovieCheck.Clientes.Models;
using System.Linq;

namespace MovieCheck.Clientes.Infra.Factory
{
    public static class MovieFactory
    {
        public static void ExisteClassificacao(Classificacao classificacao)
        {
            bool resposta = false;

            using (MovieCheckContext contexto = new MovieCheckContext())
            {
                if (contexto.Classificacao.Any(c => c.Id == (!string.IsNullOrEmpty(classificacao.Id.ToString()) ? classificacao.Id : 0) || (c.ClassificacaoIndicativa == classificacao.ClassificacaoIndicativa && c.Descricao == classificacao.Descricao)))
                {
                    resposta = true;
                }
            }

            if (!resposta)
            {
                throw new NewMovieFailedException("Filme cadastrado sem classificação indicativa. Necessário haver uma classificação indicativa.");
            }
        }
    }
}
