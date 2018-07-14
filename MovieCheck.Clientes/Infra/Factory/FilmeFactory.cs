using MovieCheck.Site.Models;
using MovieCheck.Site.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Site.Infra.Factory
{
    public static class FilmeFactory
    {
        public static IList<string> PrepararGeneroViewModel(this IDataService dataService)
        {
            var listaGenero = new List<string>();

            foreach (var genero in dataService.ObterListaGenero())
            {
                listaGenero.Add(genero.Descricao);
            }

            return listaGenero;
        }

        public static IDictionary<string, string> PrepararClassificacaoViewModel(this IDataService dataService)
        {
            var dicionarioClassificacao = new Dictionary<string, string>();

            foreach (var classificacao in dataService.ObterListaClassificacao())
            {
                dicionarioClassificacao.Add(classificacao.ClassificacaoIndicativa, classificacao.Descricao);
            }

            return dicionarioClassificacao;
        }

        private static IList<FilmeViewModel> MontarFilmeViewModel(this IDataService dataService, IList<Filme> listaFilme)
        {
            IList<FilmeViewModel> listaFilmeViewModel = new List<FilmeViewModel>();
            FilmeViewModel filmeViewModel;

            foreach (var filme in listaFilme)
            {
                if (!listaFilmeViewModel.Any(f => f.Titulo == filme.Titulo && f.TipoMidia == filme.ObterTipoMidia()))
                {
                    filmeViewModel = new FilmeViewModel(filme);
                    listaFilmeViewModel.Add(filmeViewModel);
                }
            }

            return listaFilmeViewModel.OrderBy(f => f.Titulo).ThenBy(f => f.TipoMidia).ToList();
        }

        public static IList<FilmeViewModel> VerEstoqueCompleto(this IDataService dataService)
        {
            return dataService.MontarFilmeViewModel(dataService.ObterCatalogoCompleto());
        }

        public static IList<FilmeViewModel> EfetuarPesquisaSimples(this IDataService dataService, string titulo)
        {
            return dataService.MontarFilmeViewModel(dataService.ObterListaFilmePorTitulo(titulo));
        }

        public static IList<FilmeViewModel> EfetuarPesquisaAvancada(this IDataService dataService, string titulo, int ano, string ator, string diretor, string classificacao, string[] generos)
        {
            IList<string> listaGenero = new List<string>();

            titulo = (!(string.IsNullOrEmpty(titulo)) ? titulo : "");
            ano = (ano > 0 ? ano : 0);
            ator = (!(string.IsNullOrEmpty(ator)) ? ator : "");
            diretor = (!(string.IsNullOrEmpty(diretor)) ? diretor : "");
            classificacao = (!(string.IsNullOrEmpty(classificacao)) ? classificacao : "");
            foreach (string genero in generos)
            {
                listaGenero.Add(genero);
            }

            return dataService.MontarFilmeViewModel(dataService.PesquisaAvancada(titulo, ano, ator, diretor, classificacao, listaGenero));
        }
    }
}
