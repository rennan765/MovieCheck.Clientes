namespace MovieCheck.Clientes.Models.ViewModels
{
    public abstract class UsuarioViewModel
    {
        #region Atributos
        private int id;
        private string email;
        private string nome;
        private Endereco endereco;
        private string telefoneFixo;
        private string telefoneCelular;
        private bool status;
        public string tipo;
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
        public Endereco Endereco
        {
            get
            {
                if (!(this.endereco is null))
                {
                    return this.endereco;
                }
                else
                {
                    return new Endereco("semEndereco");
                }
            }
            set
            {
                if (!(value is null))
                {
                    this.endereco = value;
                }
                else
                {
                    this.endereco = new Endereco("semEndereco");
                }
            }
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
        #endregion
    }
}
