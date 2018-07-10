using System;

namespace MovieCheck.Api.Infra.Exceptions
{
    public class NotFoundException : Exception 
    {
        public string Descricao { get; set; }

        public NotFoundException(string descricao)
        {
            this.Descricao = descricao;
        }
    }
}
