using MovieCheck.Clientes.Models;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieCheck.Clientes.Infra.Factory
{
    public static class UsuarioFactory
    {
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
            if (UsuarioFactory.IsEmail(email))
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

        public static void AdicionarDependente(this IDataService dataService, Dependente dependente)
        {
            var usuario = dataService.ObterUsuarioSessao();

            if (dataService.ObterTipoUsuario(usuario) == "Cliente")
            {
                if (UsuarioFactory.IsEmail(dependente.Email))
                {
                    dataService.ExisteEmail(dependente.Email);

                    dataService.AdicionarDependente((Cliente)usuario, dependente);
                }
                else
                {
                    throw new NewUserFailedException("E-mail inválido.");
                }
            }
            else
            {
                throw new NewUserFailedException("Este usuário é um dependente. Dependentes não podem possuir dependentes.");
            }
            
        }
    }
}
