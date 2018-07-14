using System;

namespace MovieCheck.Site.Infra
{
    public class LoginFailedException : Exception
    {
        public string Desricao { get; set; }

        public LoginFailedException (string descricao)
        {
            this.Desricao = descricao;
        }
    }
}
