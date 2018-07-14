using System;

namespace MovieCheck.Site.Infra
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