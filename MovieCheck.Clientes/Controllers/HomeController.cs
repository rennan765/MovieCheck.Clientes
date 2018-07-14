using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Site.Infra;
using MovieCheck.Site.Infra.Factory;
using MovieCheck.Site.Models;
using MovieCheck.Site.Models.ViewModels;
using System;

namespace MovieCheck.Site.Controllers
{
    public class HomeController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public HomeController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions Index
        [HttpGet]
        public IActionResult Index()
        {
            if (!_dataService.VerificarSecao())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Main");
            }
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            try
            {
                var usuario = _dataService.EfetuarLogin(formCollection["email"], formCollection["pass"]);
                return RedirectToAction("Main");
            }
            catch (LoginFailedException e)
            {
                ViewBag.Error = e.Desricao;
                return View();
            }
        }

        #endregion

        #region Actions Main
        public IActionResult Main()
        {
            if (_dataService.VerificarSecao())
            {
                UsuarioViewModel usuarioViewModel;
                var usuario = _dataService.ObterUsuarioSessao();
                if (_dataService.TipoCliente(usuario) != "Dependente") 
                {
                    usuarioViewModel = new ClienteViewModel((Cliente)usuario);
                }
                else
                {
                    usuarioViewModel = new DependenteViewModel(_dataService.ObterDependente(usuario.Id));
                }

                if (!DefaultFactory._mensagemViewModel.SemMensagem())
                {
                    ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                    ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                    DefaultFactory._mensagemViewModel.Dispose();
                }

                ViewBag.ListaPendenciaViewModel = _dataService.ListarPendenciasUsuarioLogado();
                ViewBag.DicionarioClassificacao = _dataService.PrepararClassificacaoViewModel();
                ViewBag.ListaGenero = _dataService.PrepararGeneroViewModel();

                return View(usuarioViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult AtualizarUsuario(IFormCollection formCollection)
        {
            ViewData["Mensagem"] = "";
            try
            {
                var antigo = _dataService.ObterUsuarioSessao();

                UsuarioFactory.ValidaEmail(formCollection["email"]);

                //SENHA
                var senha = "";

                if (!string.IsNullOrEmpty(formCollection["pass"]) && !string.IsNullOrEmpty(formCollection["repass"]))
                {
                    UsuarioFactory.CompararSenha(formCollection["pass"], formCollection["repass"]);
                    senha = formCollection["pass"];
                }

                //ENDERECO
                Endereco endereco = null;

                if (formCollection["zipCode"] != string.Empty)
                {
                    EnderecoFactory.ValidaEstado(formCollection["state"]);
                    EnderecoFactory.ValidaNumero(formCollection["numAddress"]);
                    endereco = new Endereco()
                    {
                        Logradouro = formCollection["street"],
                        Numero = Convert.ToInt32(formCollection["numAddress"]),
                        Complemento = formCollection["complement"],
                        Bairro = formCollection["province"],
                        Cidade = formCollection["city"],
                        Estado = formCollection["state"],
                        Cep = formCollection["zipCode"]
                    };
                }

                _dataService.AtualizarUsuario(antigo, formCollection["email"], senha, endereco, formCollection["phoneHome"], formCollection["phoneCel"]);

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso("Usuário atualizado com sucesso.");
            } catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
            }

            return RedirectToAction("Main");
        }

        public IActionResult CancelarReserva(IFormCollection formCollection)
        {
            try
            {
                if (!(formCollection is null))
                {
                    _dataService.CancelarReserva(Convert.ToInt32(formCollection["id"]));

                    DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso("Reserva cancelada com sucesso.");
                }
                else
                {
                    throw new NewPendenciaFailedException("Erro ao localizar pendência para cancelamento.");
                }
            }
            catch (NewPendenciaFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Descricao);
            }

            return RedirectToAction("Main");
        }
        #endregion

        public IActionResult LogOff()
        {
            if (_dataService.VerificarSecao())
            {
                _dataService.EfetuarLogOff();
            }

            return RedirectToAction("Index");
        }
    }
}
