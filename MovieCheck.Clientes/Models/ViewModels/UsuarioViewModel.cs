using System.Collections.Generic;

namespace MovieCheck.Clientes.Models.ViewModels
{
    public abstract class UsuarioViewModel
    {
        #region Atributos
        private int id;
        private string email;
        private string nome;
        private string enderecoLogradouro;
        private string enderecoNumero;
        private string enderecoComplemento;
        private string enderecoBairro;
        private string enderecoCidade;
        private string enderecoEstado;
        private string enderecoCep;
        private string telefoneFixo;
        private string telefoneCelular;
        private bool status;
        public string tipo;
        public IList<PendenciaViewModel> pendencias;
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public string Nome
        {
            get { return this.nome; }
            set { this.nome = value; }
        }
        public string EnderecoLogradouro
        {
            get { return this.enderecoLogradouro; }
            set { this.enderecoLogradouro = value; }
        }
        public string EnderecoNumero
        {
            get { return this.enderecoNumero; }
            set { this.enderecoNumero = value; }
        }
        public string EnderecoComplemento
        {
            get { return this.enderecoComplemento; }
            set { this.enderecoComplemento = value; }
        }
        public string EnderecoBairro
        {
            get { return this.enderecoBairro; }
            set { this.enderecoBairro = value; }
        }
        public string EnderecoCidade
        {
            get { return this.enderecoCidade; }
            set { this.enderecoCidade = value; }
        }
        public string EnderecoEstado
        {
            get { return this.enderecoEstado; }
            set { this.enderecoEstado = value; }
        }
        public string EnderecoCep
        {
            get { return this.enderecoCep; }
            set { this.enderecoCep = value; }
        }
        public string TelefoneFixo
        {
            get { return this.telefoneFixo; }
            set { this.telefoneFixo = value; }
        }
        public string TelefoneCelular
        {
            get { return this.telefoneCelular; }
            set { this.telefoneCelular = value; }
        }
        public bool Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public string Tipo
        {
            get { return this.tipo; }
            set { this.tipo = value; }
        }
        public IList<PendenciaViewModel> Pendencias
        {
            get { return this.pendencias; }
            set { this.pendencias = value; }
        }
        #endregion

        #region Métodos
        public void EnderecoEmBranco()
        {
            this.EnderecoLogradouro = "";
            this.EnderecoNumero = "";
            this.EnderecoComplemento = "";
            this.EnderecoBairro = "";
            this.EnderecoCidade = "";
            this.EnderecoEstado = "";
            this.EnderecoCep = "";
        }

        public void RemoverEndereco()
        {
            EnderecoEmBranco();
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            this.EnderecoLogradouro = endereco.Logradouro;
            this.EnderecoNumero = endereco.Numero.ToString();
            this.EnderecoComplemento = endereco.Complemento;
            this.EnderecoBairro = endereco.Bairro;
            this.EnderecoCidade = endereco.Cidade;
            this.EnderecoEstado = endereco.Estado;
            this.EnderecoCep = endereco.Cep;
        }

        public void PreencherListaPendencias(IList<Pendencia> pendencias)
        {
            foreach (var pendencia in pendencias)
            {
                this.pendencias.Add(new PendenciaViewModel(pendencia));
            }
        }
        #endregion
    }
}
