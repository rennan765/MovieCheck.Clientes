using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Infra.Factory;
using MovieCheck.Clientes.Models;
using MovieCheck.Clientes.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Controllers
{
    public class UserController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public UserController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Usuário
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region Dependentes
        public IActionResult Guests()
        {
            if (_dataService.VerificarSecao())
            {
                var usuario = _dataService.ObterUsuarioSessao();
                if (_dataService.TipoCliente(usuario) != "Dependente")
                {
                    if (!DefaultFactory._mensagemViewModel.SemMensagem())
                    {
                        ViewBag.TipoErro = DefaultFactory._mensagemViewModel.Tipo;
                        ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                        ViewBag.Operacao = DefaultFactory._mensagemViewModel.Operacao;
                        DefaultFactory._mensagemViewModel.Dispose();
                    }

                    if (UsuarioFactory.ExisteUsuarioViewModel())
                    {
                        ViewBag.DependenteSelecionado = new DependenteViewModel((Dependente)UsuarioFactory._usuarioViewModel);
                        UsuarioFactory._usuarioViewModel.Dispose();
                    }

                    return View(_dataService.ObterListaDependenteViewModelPorCliente((Cliente)usuario));
                }
                else
                {
                    return RedirectToAction("Main", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult InformacaoDependente(int id)
        {
            try
            {
                var dependente = _dataService.ObterDependente(id);
                if (_dataService.TipoCliente(dependente) != "Dependente")
                {
                    throw new NewUserFailedException("Este usuário não é um dependente.");
                }
                else
                {
                    _dataService.AdicionarUsuarioViewModel(dependente);
                }

                return RedirectToAction("Guests");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Informacao";
                return RedirectToAction("Guests");
            }
        }

        public IActionResult BloquearDependente(int id)
        {
            try
            {
                Dependente dependente = _dataService.ObterDependentePorUsuarioId((Cliente)_dataService.ObterUsuarioSessao(), id);
                _dataService.AlterarStatusUsuario(dependente);
                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Dependente {dependente.Email} {(dependente.Status == 1 ? "desbloqueado" : "bloqueado")} com sucesso.");
                DefaultFactory._mensagemViewModel.Operacao = "Bloquear";
                return RedirectToAction("Guests");
            }
            catch(NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Bloquear";
                return RedirectToAction("Guests");
            }
        }

        public IActionResult ExcluirDependente(int id)
        {
            try
            {
                Dependente dependente = _dataService.ObterDependentePorUsuarioId((Cliente)_dataService.ObterUsuarioSessao(), id);
                _dataService.ExcluirDependente(dependente);
                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Dependente {dependente.Email} excluído com sucesso.");
                DefaultFactory._mensagemViewModel.Operacao = "Excluir";
                return RedirectToAction("Guests");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Excluir";
                return RedirectToAction("Guests");
            }
        }

        [HttpPost]
        public IActionResult AdicionarDependente(IFormCollection formCollection)
        {
            try
            {
                Dependente dependente = new Dependente();
                var usuario = _dataService.ObterUsuarioSessao();

                if (_dataService.TipoCliente(usuario) == "Dependente")
                {
                    throw new NewUserFailedException("Este usuário é um dependente. Dependentes não podem possuir dependentes.");
                }
                else
                {
                    dependente.Cliente = (Cliente)usuario;
                }

                dependente.Email = formCollection["email"];
                dependente.Nome = formCollection["name"];
                UsuarioFactory.CompararSenha(formCollection["pass"], formCollection["repass"]);
                dependente.Senha = formCollection["pass"];

                //ENDERECO
                if (formCollection["zipCode"] != string.Empty)
                {
                    EnderecoFactory.ValidaEstado(formCollection["state"]);
                    EnderecoFactory.ValidaNumero(formCollection["numAddress"]);
                    dependente.Endereco = new Endereco()
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
                else
                {
                    dependente.Endereco = null;
                }

                _dataService.AdicionarDependente(dependente, formCollection["phoneHome"], formCollection["phoneCel"]);

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Dependente adicionado para o usuário {dependente.Cliente.Email} com sucesso.");
                DefaultFactory._mensagemViewModel.Operacao = "Adicionar Dependente";
                return RedirectToAction("Guests");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Adicionar Dependente";
                return RedirectToAction("Guests");
            }
        }

        [HttpPost]
        public IActionResult AtualizarDependente(IFormCollection formCollection)
        {
            ViewData["Mensagem"] = "";
            try
            {
                var antigo = _dataService.ObterUsuarioPorId(Convert.ToInt32(formCollection["id"]));

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

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso("Dependente atualizado com sucesso.");
                return RedirectToAction("Guests");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                return RedirectToAction("Guests");
            }
        }
        #endregion
    }
}