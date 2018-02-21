namespace MovieCheck.Clientes.Models.ViewModels
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
        #endregion

        #region Construtores
        public DependenteViewModel()
        {

        }

        public DependenteViewModel(Dependente dependente)
        {
            var telefoneFixo = dependente.ObterTelefoneFixo();
            var telefoneCelular = dependente.ObterTelefoneCelular();

            this.Id = dependente.Id;
            this.Email = dependente.Email;
            this.Nome = dependente.Nome;
            this.Endereco = dependente.Endereco;
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
        }
        #endregion
    }
}
