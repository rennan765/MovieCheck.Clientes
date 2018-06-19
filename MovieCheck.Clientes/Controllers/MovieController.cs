using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Infra.Factory;
using MovieCheck.Clientes.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Controllers
{
    public class MovieController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public MovieController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Movie");
        }

        [HttpPost]
        public IActionResult Movie()
        {
            if (_dataService.VerificarSecao() && !DefaultFactory._mensagemViewModel.SemMensagem())
            {
                ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                DefaultFactory._mensagemViewModel.Dispose();
                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        public IActionResult Movie(IList<FilmeViewModel> listaFilmeViewModel)
        {
            if (_dataService.VerificarSecao())
            {
                if (!DefaultFactory._mensagemViewModel.SemMensagem())
                {
                    ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                    ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                    DefaultFactory._mensagemViewModel.Dispose();
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #region Pesquisa
        public IActionResult VerEstoqueCompleto()
        {
            var listaFilmeViewModel = _dataService.VerEstoqueCompleto();

            ValidaListaFilme(listaFilmeViewModel);

            return View("Movie", listaFilmeViewModel);
        }

        [HttpGet]
        public IActionResult PesquisaSimples(string movieInfo)
        {
            IList<FilmeViewModel> listaFilmeViewModel;

            try
            {
                if (!string.IsNullOrEmpty(movieInfo))
                {
                    listaFilmeViewModel = _dataService.EfetuarPesquisaSimples(movieInfo);

                    ValidaListaFilme(listaFilmeViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (NewPendenciaFailedException e)
            {
                listaFilmeViewModel = new List<FilmeViewModel>();
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro($"Erro ao carregar lista de filmes. {e.Descricao}");
            }

            if (!DefaultFactory._mensagemViewModel.SemMensagem())
            {
                ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                DefaultFactory._mensagemViewModel.Dispose();
            }

            return View("Movie", listaFilmeViewModel);
        }

        [HttpPost]
        public IActionResult PesquisaAvancada(IFormCollection formCollection)
        {
            IList<FilmeViewModel> listaFilmeViewModel;

            try
            {
                if (!(formCollection is null))
                {
                    listaFilmeViewModel = _dataService.EfetuarPesquisaAvancada(formCollection["movieName"], !string.IsNullOrEmpty(formCollection["movieYear"]) ? Convert.ToInt32(formCollection["movieYear"]) : 0, formCollection["movieActor"], formCollection["movieDirector"], formCollection["movieClass"].ToString() != "null" ? formCollection["movieClass"].ToString() : null, formCollection["movieGender"]);

                    ValidaListaFilme(listaFilmeViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (NewPendenciaFailedException e)
            {
                listaFilmeViewModel = new List<FilmeViewModel>();
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro($"Erro ao carregar lista de filmes. {e.Descricao}");
            }

            if (!DefaultFactory._mensagemViewModel.SemMensagem())
            {
                ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                DefaultFactory._mensagemViewModel.Dispose();
            }

            return View("Movie", listaFilmeViewModel);
        }
        #endregion

        [HttpPost]
        public IActionResult ReservarFilme(IFormCollection formCollection)
        {
            try
            {
                _dataService.EfetuarReserva(_dataService.ObterFilmePorId(Convert.ToInt32(formCollection["idMovie"])));

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso("Reserva efetuada com sucesso.");
            }
            catch (NewPendenciaFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro($"Erro ao efetuar reserva. {e.Descricao}");
            }

            return RedirectToAction("Movie");
        }

        private void ValidaListaFilme(IList<FilmeViewModel> listaFilmeViewModel)
        {
            if (listaFilmeViewModel.Count <= 0)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemAtencao("Nenhum filme encontrado com os parâmetros informados.");
            }
        }
    }
}