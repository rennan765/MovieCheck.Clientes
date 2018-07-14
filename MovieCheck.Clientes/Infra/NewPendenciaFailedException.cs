using System;

namespace MovieCheck.Site.Infra
{
    public class NewPendenciaFailedException : Exception
    {
        public string Descricao { get; set; }

        public NewPendenciaFailedException(string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
