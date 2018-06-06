using System;

namespace MovieCheck.Clientes.Infra
{
    public class EmailFailedException : Exception
    {
        public string Descricao { get; set; }

        public EmailFailedException(string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
