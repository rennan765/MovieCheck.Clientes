using System;

namespace MovieCheck.Site.Infra
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
