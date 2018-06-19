using MovieCheck.Clientes.Models;
using MovieCheck.Clientes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieCheck.Clientes.Infra.Factory
{
    public static class UsuarioFactory
    {
        public static Usuario _usuarioViewModel;

        public static bool ExisteUsuarioViewModel()
        {
            if (!(_usuarioViewModel is null))
            {
                if (!string.IsNullOrEmpty(_usuarioViewModel.Email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void AdicionarUsuarioViewModel(this IDataService dataService, Usuario usuario)
        {
            if (dataService.TipoCliente(usuario) != "Dependente")
            {
                _usuarioViewModel = (Cliente)usuario;
            }
            else
            {
                _usuarioViewModel = (Dependente)usuario;
            }
        }

        public static string HashPassword(string senha)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA1.Create())
            {
                hashBytes = hash.ComputeHash(encoding.GetBytes(senha));
            }

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }
            
            return hashValue.ToString();
        }

        public static string SenhaBranco()
        {
            return HashPassword(string.Empty);
        }

        private static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
        }

        public static string TipoCliente(this IDataService dataService, Usuario usuario)
        {
            return dataService.ObterTipoUsuario(usuario);
        }

        public static void CompararSenha(string senha, string compararSenha)
        {
            if (UsuarioFactory.HashPassword(senha) != UsuarioFactory.HashPassword(compararSenha))
            {
                throw new NewUserFailedException("As senhas não conferem.");
            }
        }
        
        public static void ValidaEmail(string email)
        {
            if (!IsEmail(email))
            {
                throw new NewUserFailedException("E-mail inválido.");
            }
        }

        public static void ExisteEmail(this IDataService dataService, string email)
        {
            if (dataService.VerificarUsuarioPorEmail(email))
            {
                throw new NewUserFailedException("Já existe um usuário cadastrado com este email.");
            }
        }

        public static void ExisteCpf(this IDataService dataService, string cpf)
        {
            if (dataService.VerificarClientePorCpf(cpf))
            {
                throw new NewUserFailedException("Já existe um usuário cadastrado com este CPF.");
            }
        }

        public static Usuario EfetuarLogin(this IDataService dataService, string email, string senha)
        {
            if (IsEmail(email))
            {
                if (senha != string.Empty)
                {
                    var usuario = dataService.ObterUsuarioPorEmail(email);

                    if (!(usuario is null))
                    {
                        if (usuario.Senha == UsuarioFactory.HashPassword(senha))
                        {
                            if (usuario.Status == 1)
                            {
                                dataService.IniciarSessao(usuario);
                                return usuario;
                            }
                            else
                            {
                                throw new LoginFailedException("Usuário bloqueado. Contate o administrador do sistema.");
                            }
                        }
                        else
                        {
                            throw new LoginFailedException("Senha incorreta.");
                        }
                    }
                    else
                    {
                        throw new LoginFailedException("Este e-mail não está cadastrado.");
                    }
                }
                else
                {
                    throw new LoginFailedException("A senha não foi preenchdia.");
                }
            }
            else
            {
                throw new LoginFailedException("E-mail inválido.");
            }
        }

        public static void EfetuarLogOff(this IDataService dataService)
        {
            dataService.FinalizarSessao();
        }

        public static void EditarUsuarioLogado(this IDataService dataService, Usuario usuario)
        {
            try
            {
                dataService.EditarUsuario(usuario);
            } catch (Exception e)
            {
                throw new NewUserFailedException($"Falha ao atualizar o usuário: {e.ToString()}");
            }
        }

        public static void AtualizarUsuario(this IDataService dataService, Usuario antigo, string email, string senha, Endereco endereco, string telefoneFixo, string telefoneCelular)
        {
            try
            {
                Usuario novo = null;

                string tipoCliente = dataService.TipoCliente(antigo);
                if (tipoCliente == "Cliente" || tipoCliente == "Administrador")
                {
                    novo = new Cliente(tipoCliente);
                }
                else
                {
                    novo = new Dependente();
                }

                novo.Id = antigo.Id;
                novo.Nome = antigo.Nome;

                //EMAIL
                if (antigo.VerificaSeTrocouEmail(email))
                {
                    UsuarioFactory.ValidaEmail(email);
                    novo.Email = email;
                }
                else
                {
                    novo.Email = antigo.Email;
                }

                //SENHA
                if (!string.IsNullOrEmpty(senha))
                {
                    novo.Senha = senha;
                }
                else
                {
                    novo.Senha = "";
                }

                //ENDERECO
                if (!(endereco is null))
                {
                    novo.AdicionarEndereco(endereco);
                }

                antigo.AtualizarUsuario(novo);

                if (telefoneFixo != string.Empty)
                {
                    var fixo = TelefoneFactory.ValidaTelefone("fixo", telefoneFixo);

                    if (dataService.ExisteTelefone(fixo))
                    {
                        fixo = dataService.ObterTelefone(fixo);
                    }

                    if (antigo.ObterTelefoneFixo() is null)
                    {
                        antigo.AdicionarTelefone(fixo);
                    }
                    else
                    {
                        if (!antigo.ObterTelefoneFixo().Equals(fixo))
                        {
                            antigo.EditarTelefoneFixo(fixo);
                        }
                    }
                }
                else
                {
                    antigo.RemoverTelefoneFixo();
                }

                if (telefoneCelular != string.Empty)
                {
                    var celular = TelefoneFactory.ValidaTelefone("celular", telefoneCelular);

                    if (dataService.ExisteTelefone(celular))
                    {
                        celular = dataService.ObterTelefone(celular);
                    }

                    if (antigo.ObterTelefoneCelular() is null)
                    {
                        antigo.AdicionarTelefone(celular);
                    }
                    else
                    {
                        if (!antigo.ObterTelefoneCelular().Equals(celular))
                        {
                            antigo.EditarTelefoneCelular(celular);
                        }
                    }
                }
                else
                {
                    antigo.RemoverTelefoneCelular();
                }

                dataService.EditarUsuario(antigo);
            }
            catch (NewUserFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new NewUserFailedException($"Falha ao atualizar o usuário: {e.ToString()}");
            }
        }

        public static void AlterarStatusUsuario(this IDataService dataService, Usuario usuario)
        {
            usuario.ChangeStatus();
            try
            {
                dataService.EditarUsuario(usuario);
            }
            catch (NewUserFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new NewUserFailedException($"Falha ao atualizar o usuário: {e.ToString()}");
            }
        }

        public static IList<DependenteViewModel> ObterListaDependenteViewModelPorCliente(this IDataService dataService, Cliente cliente)
        {
            IList<DependenteViewModel> listaDependenteViewModel = new List<DependenteViewModel>();
            
            foreach (Dependente dependente in dataService.ObterListaDependente(cliente))
            {
                listaDependenteViewModel.Add(new DependenteViewModel(dependente));
            }

            return listaDependenteViewModel;
        }

        public static Dependente ObterDependentePorUsuarioId(this IDataService dataService, Cliente cliente, int id)
        {
            try
            {
                Dependente dependente = (Dependente)dataService.ObterUsuarioPorId(id);
                VerificarResponsavelDependente(cliente, dependente);
                return dependente;
            }
            catch (NewUserFailedException e)
            {
                throw e;
            }
        }

        public static void VerificarResponsavelDependente(Cliente cliente, Dependente dependente)
        {
            if (!cliente.Dependentes.Any(d => d.Email == dependente.Email))
            {
                throw new NewUserFailedException($"O dependente {dependente.Email} não pertence ao cliente {cliente.Email}.");
            }
        }

        public static void AdicionarDependente(this IDataService dataService, Dependente dependente, string telefoneFixo, string telefoneCelular)
        {
            dataService.ExisteEmail(dependente.Email);

            if (telefoneFixo != string.Empty)
            {
                var fixo = TelefoneFactory.ValidaTelefone("fixo", telefoneFixo);

                if (dataService.ExisteTelefone(fixo))
                {
                    dependente.AdicionarTelefone(dataService.ObterTelefone(fixo));
                }
                else
                {
                    dependente.AdicionarTelefone(fixo);
                }
            }

            if (telefoneCelular != string.Empty)
            {
                var celular = TelefoneFactory.ValidaTelefone("celular", telefoneCelular);

                if (dataService.ExisteTelefone(celular))
                {
                    dependente.AdicionarTelefone(dataService.ObterTelefone(celular));
                }
                else
                {
                    dependente.AdicionarTelefone(celular);
                }
            }

            dataService.AdicionarDependente(dependente.Cliente, dependente);
        }

        public static void ExcluirDependente(this IDataService dataService, Dependente dependente)
        {
            try
            {
                dataService.ExcluirUsuario(dependente);
            }
            catch (Exception e)
            {
                throw new NewUserFailedException($"Falha ao atualizar o usuário: {e.ToString()}");
            }
        }

        public static void AdicionarCliente(this IDataService dataService, Cliente cliente, string telefoneFixo, string telefoneCelular)
        {
            dataService.ExisteEmail(cliente.Email);

            if (telefoneFixo != string.Empty)
            {
                var fixo = TelefoneFactory.ValidaTelefone("fixo", telefoneFixo);

                if (dataService.ExisteTelefone(fixo))
                {
                    cliente.AdicionarTelefone(dataService.ObterTelefone(fixo));
                }
                else
                {
                    cliente.AdicionarTelefone(fixo);
                }
            }

            if (telefoneCelular != string.Empty)
            {
                var celular = TelefoneFactory.ValidaTelefone("celular", telefoneCelular);

                if (dataService.ExisteTelefone(celular))
                {
                    cliente.AdicionarTelefone(dataService.ObterTelefone(celular));
                }
                else
                {
                    cliente.AdicionarTelefone(celular);
                }
            }

            dataService.AdicionarCliente(cliente);
        }
        
        public static void RedefinirSenha(this IDataService dataService, string email)
        {
            try
            {
                if (!dataService.VerificarUsuarioPorEmail(email))
                {
                    dataService.AlterarSenha(dataService.ObterUsuarioPorEmail(email), "0000");
                }
                else
                {
                    throw new NewUserFailedException("Este e-mail não está cadastrado");
                }
            }
            catch (NewUserFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new NewUserFailedException($"Erro ao  redefinir a senha: {e.Message}");
            }
        }

        public static void EnviarEmail(this IDataService dataService, string tipoEmail, Usuario usuario)
        {
            /*
                Este método foi criado para enviar todos os e-mails do sistema que tenham relação com usuário.
                Os tipos de e-mail podem ser:
                    0: Novo Cliente Cadastrado.
                    1: Redefinição de Senha.
                    2: Novo Dependente Cadastrado.
                Todos estes tipos estão cadastrados na propriedade TipoEmail, na classe Email. 
            */

            try
            {
                using (Email email = new Email(dataService))
                {
                    email.EnviarEmail(tipoEmail, usuario);
                }
            }
            catch (EmailFailedException e)
            {
                throw e;
            }
        }
    }
}
