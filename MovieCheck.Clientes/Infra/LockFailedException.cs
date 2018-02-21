using System;

namespace MovieCheck.Clientes.Infra
{
    public class LockFailedException : Exception
    {
        public string Descricao { get; set; }

        public LockFailedException(string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
