using System;

namespace MovieCheck.Clientes.Models.ViewModels
{
    public class MensagemViewModel : IDisposable
    {
        #region Atrinutos
        private string tipo;
        private string mensagem;
        #endregion

        #region Propriedades
        public string Tipo
        {
            get { return this.tipo; }
            set { this.tipo = value; }
        }
        public string Mensagem
        {
            get { return this.mensagem; }
            set { this.mensagem = value; }
        }
        #endregion

        #region Construtores
        public MensagemViewModel()
        {

        }

        public MensagemViewModel(string tipo, string mensagem)
        {
            this.tipo = tipo;
            this.mensagem = mensagem;
        }
        #endregion

        #region Métodos
        public void AtribuirMensagemSucesso(string mensagem)
        {
            this.tipo = "Sucesso";
            this.mensagem = mensagem;
        }

        public void AtribuirMensagemErro(string mensagem)
        {
            this.tipo = "Erro";
            this.mensagem = mensagem;
        }

        public void AtribuirMensagemAtencao(string mensagem)
        {
            this.tipo = "Atenção";
            this.mensagem = mensagem;
        }

        public bool SemMensagem()
        {
            if (string.IsNullOrEmpty(this.mensagem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            this.tipo = null;
            this.mensagem = null;
        }
        #endregion
    }
}
