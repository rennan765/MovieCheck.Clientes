using System;

namespace MovieCheck.Site.Infra
{
    public class NewMovieFailedException : Exception
    {
        public string Descricao { get; set; }

        public NewMovieFailedException(string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
