using System.Collections.Generic;

namespace MovieCheck.Clientes.Models.ViewModels
{
    public class ClienteViewModel : UsuarioViewModel
    {
        #region Atributos
        private string cpf;
        private IList<DependenteViewModel> dependentes;
        private IList<string> nomeDependentes;
        #endregion

        #region Propriedades
        public string Cpf
        {
            get { return this.cpf; }
            set { this.cpf = value; }
        }

        public IList<DependenteViewModel> Dependentes
        {
            get { return this.dependentes; }
            set { this.dependentes = value; }
        }

        public IList<string> NomeDependentes
        {
            get { return this.nomeDependentes; }
            set{ this.nomeDependentes = value; }
        }
        #endregion

        #region Construtores
        public ClienteViewModel(Cliente cliente)
        {
            var telefoneFixo = cliente.ObterTelefoneFixo();
            var telefoneCelular = cliente.ObterTelefoneCelular();

            this.Id = cliente.Id;
            this.Email = cliente.Email;
            this.Nome = cliente.Nome;
            this.Cpf = cliente.Cpf;
            if (!(cliente.Endereco is null))
            {
                this.AdicionarEndereco(cliente.Endereco);
            }
            else
            {
                this.EnderecoEmBranco();
            }
            if (!(telefoneFixo is null))
            {
                this.TelefoneFixo = $"{telefoneFixo.Ddd.ToString()}{telefoneFixo.Numero}";
            }
            else
            {
                this.TelefoneFixo = "";
            }
            if (!(telefoneCelular is null))
            {
                this.TelefoneCelular = $"{telefoneCelular.Ddd.ToString()}{telefoneCelular.Numero}";
            }
            else
            {
                this.TelefoneCelular = "";
            }
            if (cliente.Tipo == 0)
            {
                this.tipo = "Cliente";
            }
            else
            {
                this.tipo = "Administrador";
            }

            this.Dependentes = new List<DependenteViewModel>();
            this.NomeDependentes = new List<string>();

            this.PreencherListaDependentes(cliente);
        }
        #endregion

        #region Metodos
        private void PreencherListaDependentes(Cliente cliente)
        {
            this.Dependentes.Clear();
            this.NomeDependentes.Clear();
            foreach (var dependente in cliente.Dependentes)
            {
                this.Dependentes.Add(new DependenteViewModel(dependente));
                this.NomeDependentes.Add(dependente.Nome);
            }
        }
        #endregion
    }
}
