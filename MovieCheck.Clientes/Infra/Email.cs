using MovieCheck.Clientes.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MovieCheck.Clientes.Infra
{
    public class Email : IDisposable
    {
        #region Atributos
        private IDataService _dataService;
        private MailMessage mailMessage;
        private SmtpClient smtpClient;
        private IList<string> tipoEmail;
        #endregion

        #region Propriedades
        public IList<string> TipoEmail
        {
            get { return this.tipoEmail; }
        }
        #endregion

        #region Construtores
        public Email(IDataService dataService)
        {
            this._dataService = dataService;
            ConfigurarMensagem();
            ConfigurarSmtp();
            PreencherTipoEmail();
        }
        #endregion

        #region Métodos
        private void PreencherTipoEmail()
        {
            this.tipoEmail = new List<string>()
            {
                "0", "1", "2"
            };
        }

        private void ConfigurarMensagem()
        {
            mailMessage = new MailMessage();
            AdicionarRemetente("r.rezende@devloopers.com.br", "Rennan Rezende - DevLoopers");
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;
        }

        private void ConfigurarSmtp()
        {
            SmtpClient smtpClient = new SmtpClient("smtp.devloopers.com.br");
            smtpClient.EnableSsl = false;
            smtpClient.Credentials = new NetworkCredential("movie-check@devloopers.com.br", "asd,123");
        }

        private void ValidarTipoEmail(string tipoEmail)
        {
            if (!this.tipoEmail.Contains(tipoEmail))
            {
                throw new EmailFailedException("Tipo de corpo de e-mail inválido.");
            }
        }

        private void AdicionarAssunto(string tipoEmail)
        {
            try
            {
                ValidarTipoEmail(tipoEmail);

                switch (tipoEmail)
                {
                    case "0":
                        mailMessage.Subject = "MovieCheck - Novo Cliente Cadastrado.";
                        break;
                    case "1":
                        mailMessage.Subject = "MovieCheck - Solicitação de redefinição de senha.";
                        break;
                    case "2":
                        mailMessage.Subject = "MovieCheck - Novo Dependente Cadastrado.";
                        break;
                }
            }
            catch (EmailFailedException e)
            {
                throw e;
            }
        }

        private void PrepararCorpoEmail(string tipoMensagem, Usuario usuario)
        {
            /*
                Método criado para encapsular os tipos diferentes de mensagens de e-mail.
                Os tipos podem ser:
                    0: Novo Cliente Cadastrado.
                    1: Redefinição de Senha.
                    2: Novo Dependente Cadastrado.
                Todos estes tipos estão cadastrados na propriedade TipoEmail, na classe Email. 
            */
            string mensagem = "";

            try
            {
                ValidarTipoEmail(tipoMensagem);
                using (CorpoEmail corpoEmail = new CorpoEmail(_dataService))
                {
                    switch (tipoMensagem)
                    {
                        case "0":
                            mensagem = corpoEmail.NovoCliente((Cliente)usuario);
                            break;
                        case "1":
                            mensagem = corpoEmail.RedefinirSenha(usuario);
                            break;
                        case "2":
                            mensagem = corpoEmail.NovoDependente((Dependente)usuario);
                            break;
                    }
                }

                this.mailMessage.Body = mensagem;
            }
            catch (EmailFailedException e)
            {
                throw e;
            }
        }

        public void AdicionarRemetente(string email, string nome)
        {
            mailMessage.From = new MailAddress(email, nome);
        }

        public void AdicionarDestinatarios(string tipoMensagem, Usuario usuario = null)
        {
            IList<string> listaEmail = null;

            try
            {
                ValidarTipoEmail(tipoMensagem);

                switch (tipoMensagem)
                {
                    case "0":
                        listaEmail = _dataService.ObterEmailAdministradores();
                        break;
                    case "1":
                        listaEmail = new List<string>() { usuario.Email };
                        break;
                    case "2":
                        Dependente dependente = (Dependente)usuario;
                        string email = (!(dependente.Cliente is null) ? dependente.Cliente.Email : _dataService.ObterUsuarioPorId(dependente.ClienteId).Email);

                        listaEmail = new List<string>() { email };
                        break;
                }

                if (listaEmail.Count > 0)
                {
                    foreach (string email in listaEmail)
                    {
                        mailMessage.To.Add(email);
                    }
                }
            }
            catch (EmailFailedException e)
            {
                throw e;
            }
        }

        public void EnviarEmail(string tipoEmail, Usuario usuario = null)
        {
            try
            {
                ValidarTipoEmail(tipoEmail);
                AdicionarDestinatarios(tipoEmail, usuario);
                PrepararCorpoEmail(tipoEmail, usuario);
                smtpClient.Send(mailMessage);
            }
            catch (EmailFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new EmailFailedException(e.Message);
            }
        }
        
        public void Dispose()
        {
            mailMessage.Dispose();
            smtpClient.Dispose();
        }
        #endregion
    }
}
