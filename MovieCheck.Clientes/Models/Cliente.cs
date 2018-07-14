using MovieCheck.Site.Infra;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Site.Models
{
    public class Cliente : Usuario
    {
        #region Atributos
        private string cpf;
        private int tipo;   //0 = NORMAL - 1 = ADMINISTRADOR
        private IList<Dependente> dependentes;
        #endregion

        #region Propriedades
        public string Cpf
        {
            get { return this.cpf; }
            set { this.cpf = value; }
        }
        public int Tipo
        {
            get { return this.tipo; }
            set
            {
                switch (value)
                {
                    case 0:
                        this.tipo = value;
                        break;
                    case 1:
                        this.tipo = value;
                        break;
                    default:
                        this.tipo = 0;
                        break;
                }
            }
        }
        public IList<Dependente> Dependentes
        {
            get { return this.dependentes; }
            set { this.dependentes = value; }
        }
        #endregion

        #region Construtores
        public Cliente()
        {
            this.Telefones = new List<UsuarioTelefone>();
            this.Dependentes = new List<Dependente>();
            this.Pendencias = new List<Pendencia>();
        }

        public Cliente(string tipo)
        {
            this.Telefones = new List<UsuarioTelefone>();
            this.Dependentes = new List<Dependente>();
            this.Pendencias = new List<Pendencia>();

            switch (tipo)
            {
                case "Cliente":
                    this.Tipo = 0;
                    break;
                case "Administrador":
                    this.Tipo = 1;
                    break;
                default:
                    this.Tipo = 0;
                    break;
            }
        }
        #endregion

        #region Metodos
        public override bool Equals(object usuario)
        {
            var c = (Cliente)usuario;
            return c.Email == this.Email && c.Nome == this.Nome && c.cpf == this.Cpf;
        }

        public void AdicionarDependente(Dependente dependente)
        {
            this.Dependentes.Add(dependente);
        }

        public void ExcluirDependente(Dependente dependente)
        {
            if (this.Dependentes.Any(d => d.Equals(dependente)))
            {
                this.Dependentes.Remove(dependente);
            }
            else
            {
                throw new NewUserFailedException("Este dependente não existe para este usuário.");
            }
        }

        public void ConverterParaAdministrador()
        {
            this.Tipo = 1;
        }

        public void ConverterParaNormal()
        {
            this.Tipo = 0;
        }
        #endregion
    }
}
