using System;

namespace MovieCheck.Clientes.Infra
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
