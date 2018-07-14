using System;

namespace MovieCheck.Site.Models.ViewModels
{
    public class MensagemViewModel : IDisposable
    {
        #region Atributos
        private string tipo;
        private string mensagem;
        private string operacao;
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
        public string Operacao
        {
            get { return this.operacao; }
            set { this.operacao = value; }
        }
        #endregion

        #region Construtores
        public MensagemViewModel()
        {
            this.tipo = "";
            this.mensagem = "";
            this.operacao = "";
        }

        public MensagemViewModel(string tipo, string mensagem, string operacao)
        {
            this.tipo = tipo;
            this.mensagem = mensagem;
            this.operacao = operacao;
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
            this.tipo = "";
            this.mensagem = "";
            this.operacao = "";
        }
        #endregion
    }
}
