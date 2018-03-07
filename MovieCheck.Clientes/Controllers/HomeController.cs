using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Infra.Factory;
using MovieCheck.Clientes.Models;
using MovieCheck.Clientes.Models.ViewModels;
using System;

namespace MovieCheck.Clientes.Controllers
{
    public class HomeController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        private string mensagem;
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
                    usuarioViewModel = new DependenteViewModel((Dependente)usuario);
                }

                if (!DefaultFactory._mensagemViewModel.SemMensagem())
                {
                    ViewBag.TipoMensagem = DefaultFactory._mensagemViewModel.Tipo;
                    ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                    DefaultFactory._mensagemViewModel.Dispose();
                }

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
                //PEGA USUARIO DO BANCO
                var usuario = _dataService.ObterUsuarioSessao();
                Usuario novo = null;

                string tipoCliente = _dataService.TipoCliente(usuario);
                if (tipoCliente == "Cliente" || tipoCliente == "Administrador")
                {
                    novo = new Cliente(tipoCliente);
                }
                else
                {
                    novo = new Dependente();
                }

                novo.Id = usuario.Id;
                novo.Nome = usuario.Nome;

                //EMAIL
                if (usuario.VerificaSeTrocouEmail(formCollection["email"]))
                {
                    UsuarioFactory.ValidaEmail(formCollection["email"]);
                    novo.Email = formCollection["email"];
                }
                else
                {
                    novo.Email = usuario.Email;
                }

                //SENHA
                var pass = formCollection["pass"];
                var repass = formCollection["repass"];

                if (!string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(repass))
                {
                    UsuarioFactory.CompararSenha(pass, repass);
                    novo.Senha = pass;
                }
                else
                {
                    novo.Senha = "";
                }

                //ENDERECO
                if (formCollection["zipCode"] != string.Empty)
                {
                    EnderecoFactory.ValidaEstado(formCollection["state"]);
                    EnderecoFactory.ValidaNumero(formCollection["numAddress"]);
                    novo.Endereco = new Endereco()
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

                usuario.AtualizarUsuario(novo);

                if (formCollection["phoneHome"] != string.Empty)
                {
                    var fixo = TelefoneFactory.ValidaTelefone("fixo", formCollection["phoneHome"]);

                    if (_dataService.ExisteTelefone(fixo))
                    {
                        fixo = _dataService.ObterTelefone(fixo);
                    }

                    if (usuario.ObterTelefoneFixo() is null)
                    {
                        usuario.AdicionarTelefone(fixo);
                    }
                    else
                    {
                        if (!usuario.ObterTelefoneFixo().Equals(fixo))
                        {
                            usuario.EditarTelefoneFixo(fixo);
                        }
                    }
                }
                else
                {
                    usuario.RemoverTelefoneFixo();
                }

                if (formCollection["phoneCel"] != string.Empty)
                {
                    var celular = TelefoneFactory.ValidaTelefone("celular", formCollection["phoneCel"]);

                    if (_dataService.ExisteTelefone(celular))
                    {
                        celular = _dataService.ObterTelefone(celular);
                    }

                    if (usuario.ObterTelefoneCelular() is null)
                    {
                        usuario.AdicionarTelefone(celular);
                    }
                    else
                    {
                        if (!usuario.ObterTelefoneCelular().Equals(celular))
                        {
                            usuario.EditarTelefoneCelular(celular);
                        }
                    }
                }
                else
                {
                    usuario.RemoverTelefoneCelular();
                }

                _dataService.EditarUsuarioLogado(usuario);

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso("Usuário atualizado com sucesso.");
                return RedirectToAction("Main");
            } catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                return RedirectToAction("Main");
            }
        }
        #endregion
    }
}
