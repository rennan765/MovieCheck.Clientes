using System;

namespace MovieCheck.Clientes.Infra
{
    public class DeleteUserFailedException : Exception
    {
        public string Descricao { get; set; }

        public DeleteUserFailedException (string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
