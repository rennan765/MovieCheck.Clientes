using System;

namespace MovieCheck.Clientes.Infra
{
    public class NewUserFailedException : Exception
    {
        public string Desricao { get; set; }

        public NewUserFailedException(string descricao)
        {
            this.Desricao = descricao;
        }
    }
}