using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Infra.Factory;
using MovieCheck.Clientes.Models;
using MovieCheck.Clientes.Models.ViewModels;
using System;

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
        [HttpGet]
        public IActionResult Register()
        {
            if (!DefaultFactory._mensagemViewModel.SemMensagem())
            {
                ViewBag.TipoErro = DefaultFactory._mensagemViewModel.Tipo;
                ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                ViewBag.Operacao = DefaultFactory._mensagemViewModel.Operacao;
                DefaultFactory._mensagemViewModel.Dispose();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(IFormCollection formCollection)
        {
            try
            {
                UsuarioFactory.CompararSenha(formCollection["pass"], formCollection["repass"]);
                var cliente = new Cliente()
                {
                    Email = formCollection["email"],
                    Cpf = formCollection["cpf"],
                    Nome = formCollection["name"],
                    Senha = formCollection["pass"]
                };

                EnderecoFactory.ValidaEstado(formCollection["state"]);
                EnderecoFactory.ValidaNumero(formCollection["numAddress"]);
                var endereco = new Endereco()
                {
                    Logradouro = formCollection["street"],
                    Numero = Convert.ToInt32(formCollection["numAddress"]),
                    Complemento = formCollection["complement"],
                    Bairro = formCollection["province"],
                    Cidade = formCollection["city"],
                    Estado = formCollection["state"],
                    Cep = formCollection["zipCode"]
                };
                cliente.AdicionarEndereco(endereco);

                _dataService.AdicionarCliente(cliente, formCollection["phoneHome"], formCollection["phoneCel"]);

                _dataService.EnviarEmail("0", cliente);

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Usuário {cliente.Email} cadastrado com sucesso. Aguarde o contato do administrador do sistema.");
                DefaultFactory._mensagemViewModel.Operacao = "Criar usuario";
                return RedirectToAction("Register");
            }
            catch(EmailFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"O usuário foi cadastrado com sucesso, porém o e-mail não foi enviado para o administrador do sistema. Favor entrar em contato solicitando a liberação do usuário.");
                DefaultFactory._mensagemViewModel.Operacao = "Criar usuario";
                return RedirectToAction("Register");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Criar Usuário";
                return RedirectToAction("Register");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            if (!DefaultFactory._mensagemViewModel.SemMensagem())
            {
                ViewBag.TipoErro = DefaultFactory._mensagemViewModel.Tipo;
                ViewBag.Mensagem = DefaultFactory._mensagemViewModel.Mensagem;
                ViewBag.Operacao = DefaultFactory._mensagemViewModel.Operacao;
                DefaultFactory._mensagemViewModel.Dispose();
            }

            return View();
        }
        
        [HttpPost]
        public IActionResult ForgotPassword(IFormCollection formCollection)
        {
            try
            {
                _dataService.RedefinirSenha(formCollection["email"]);

                _dataService.EnviarEmail("1", _dataService.ObterUsuarioPorEmail(formCollection["email"]));

                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Solicitação de restauração de senha enviada para {formCollection["email"]}.");
                return RedirectToAction("ForgotPassword");
            }
            catch (EmailFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"A senha deste usuário foi alterada para '0000' (sem aspas). Favor efetuar o login e alterar para a senha desejada.");
                return RedirectToAction("ForgotPassword");
            }
            catch (NewUserFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemErro(e.Desricao);
                DefaultFactory._mensagemViewModel.Operacao = "Redefinir Senha";
                return RedirectToAction("ForgotPassword");
            }
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
                        ViewBag.DependenteSelecionado = new DependenteViewModel(_dataService.ObterDependente(UsuarioFactory._usuarioViewModel.Id));
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
                _dataService.EnviarEmail("2", dependente);

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
            catch (EmailFailedException e)
            {
                DefaultFactory._mensagemViewModel.AtribuirMensagemSucesso($"Dependente adicionado com sucesso.");
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