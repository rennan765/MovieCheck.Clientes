using System.Collections.Generic;

namespace MovieCheck.Site.Models.ViewModels
{
    public class DependenteViewModel : UsuarioViewModel
    {
        #region Atributos
        private int clienteId;
        private string clienteNome;
        #endregion

        #region Propriedades
        public int ClienteId
        {
            get { return this.clienteId; }
            set { this.clienteId = value; }
        }
        public string ClienteNome
        {
            get { return this.clienteNome; }
            set { this.clienteNome = value; }
        }

        public Endereco Endereco { get; internal set; }
        #endregion

        #region Construtores
        public DependenteViewModel()
        {
            this.pendencias = new List<PendenciaViewModel>();
        }

        public DependenteViewModel(Dependente dependente)
        {
            this.pendencias = new List<PendenciaViewModel>();

            var telefoneFixo = dependente.ObterTelefoneFixo();
            var telefoneCelular = dependente.ObterTelefoneCelular();

            this.Id = dependente.Id;
            this.Email = dependente.Email;
            this.Nome = dependente.Nome;
            if (!(dependente.Endereco is null))
            {
                this.AdicionarEndereco(dependente.Endereco);
            }
            else
            {
                this.EnderecoEmBranco();
            }
            if (dependente.ExisteTelefoneFixo())
            {
                this.TelefoneFixo = $"{telefoneFixo.Ddd.ToString()}{telefoneFixo.Numero}";
            }
            else
            {
                this.TelefoneFixo = "";
            }
            if (dependente.ExisteTelefoneCelular())
            {
                this.TelefoneCelular = $"{telefoneCelular.Ddd.ToString()}{telefoneCelular.Numero}";
            }
            else
            {
                this.TelefoneCelular = "";
            }
            this.clienteId = dependente.ClienteId;
            this.clienteNome = dependente.Cliente.Nome;
            this.tipo = "Dependente";
            this.Status = dependente.Status == 1;

            this.PreencherListaPendencias(dependente.Pendencias);
        }
        #endregion
    }
}
