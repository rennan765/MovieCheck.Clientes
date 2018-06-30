using MovieCheck.Api.Infra;
using System;
using System.Collections.Generic;

namespace MovieCheck.Api.Models
{
    public abstract class Usuario : IDisposable
    {
        #region Atributos
        private int id;
        private string email;
        private string senha;
        private string nome;
        private Endereco endereco;
        private IList<UsuarioTelefone> telefones;
        private int status; //0 = A AUTORIZAR - 1 = DESBLOQUEADO - 2 - BLOQUEADO
        private IList<Pendencia> pendencias;
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
            set
            {
                this.email = value; 
            }
        }
        public string Senha
        {
            get { return this.senha; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.senha = value;
                }
                else
                {
                    this.senha = Util.HashPassword("");
                }
            }
        }
        public string Nome
        {
            get { return this.nome; }
            set { this.nome = value; }
        }
        public Endereco Endereco
        {
            get { return this.endereco; }
            set
            {
                if (!(value is null))
                {
                    this.endereco = value;
                }
                else
                {
                    this.endereco = new Endereco()
                    {
                        Logradouro = "",
                        Numero = 0,
                        Complemento = "",
                        Bairro = "",
                        Cidade = "",
                        Estado = "",
                        Cep = ""
                    };
                }
            }
        }
        public IList<UsuarioTelefone> Telefones
        {
            get { return this.telefones; }
            set { this.telefones = value; }
        }
        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public IList<Pendencia> Pendencias
        {
            get { return this.pendencias; }
            set { this.pendencias = value; }
        }
        #endregion

        #region Metodos
        public override bool Equals(object usuario)
        {
            Usuario u = (Usuario)usuario;
            return u.Email == this.Email && u.Nome == this.Nome;
        }

        //CHANGE STATUS: THE FIRST IS FOR LOCK/UNLOCK AND THE SECOND IS FOR ALLOW
        public void ChangeStatus()
        {
            if (this.Status == 1)
            {
                this.Status = 2;
            }
            else
            {
                this.Status = 1;
            }
        }

        public void ChangeStatus(string operation)
        {
            if (this.status == 0)
            {
                switch (operation)
                {
                    case "allow":
                        this.status = 1;
                        break;
                    default:
                        this.status = 2;
                        break;
                }
            }
            else
            {
                this.ChangeStatus();
            }
        }

        public string VerificarSenha(string senha)
        {
            return Util.HashPassword(senha);
        }

        public bool VerificaSeTrocouEmail(string email)
        {
            if (this.Email != email)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AdicionarTelefone(Telefone telefone)
        {
            if (telefone.Id < 0)
            {
                this.Telefones.Add(new UsuarioTelefone() { Telefone = telefone });
            }
            else
            {
                this.Telefones.Add(new UsuarioTelefone() { TelefoneId = telefone.Id, Telefone = telefone });
            }
        }

        public void EditarTelefoneFixo(Telefone fixo)
        {
            RemoverTelefoneFixo();
            AdicionarTelefone(fixo);
        }

        public void EditarTelefoneCelular(Telefone celular)
        {
            RemoverTelefoneCelular();
            AdicionarTelefone(celular);
        }

        public void RemoverTelefoneFixo()
        {
            var listaRemover = new List<UsuarioTelefone>();
            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Fixo())
                {
                    listaRemover.Add(telefone);
                }
            }

            if (listaRemover.Count > 0)
            {
                foreach (var telefone in listaRemover)
                {
                    this.Telefones.Remove(telefone);
                }
            }
        }

        public void RemoverTelefoneCelular()
        {
            var listaRemover = new List<UsuarioTelefone>();
            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Celular())
                {
                    listaRemover.Add(telefone);
                }
            }

            if (listaRemover.Count > 0)
            {
                foreach (var telefone in listaRemover)
                {
                    this.Telefones.Remove(telefone);
                }
            }
        }

        public Telefone ObterTelefoneFixo()
        {
            Telefone fixo = null;
            
            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Fixo())
                {
                    fixo = telefone.Telefone;
                }
            }

            return fixo;
        }

        public Telefone ObterTelefoneCelular()
        {
            Telefone celular = null;
            
            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Celular())
                {
                    celular = telefone.Telefone;
                }
            }

            return celular;
        }

        public bool ExisteTelefoneFixo()
        {
            Telefone fixo = null;

            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Fixo())
                {
                    fixo = telefone.Telefone;
                }
            }

            if (!(fixo is null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteTelefoneCelular()
        {
            Telefone celular = null;

            foreach (var telefone in this.Telefones)
            {
                if (telefone.Telefone.Celular())
                {
                    celular = telefone.Telefone;
                }
            }

            if (!(celular is null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            this.Endereco = new Endereco()
            {
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep
            };
        }

        public void AlterarEndereco(Endereco endereco)
        {
            if (this.Endereco is null)
            {
                this.Endereco = new Endereco();
            }
            this.Endereco.Logradouro = endereco.Logradouro;
            this.Endereco.Numero = endereco.Numero;
            this.Endereco.Complemento = endereco.Complemento;
            this.Endereco.Bairro = endereco.Bairro;
            this.Endereco.Cidade = endereco.Cidade;
            this.Endereco.Estado = endereco.Estado;
            this.Endereco.Cep = endereco.Cep;
        }

        public void RemoverEndereco()
        {
            this.Endereco = null;
        }

        public virtual void AtualizarUsuario(Usuario novo)
        {
            if(this.Id == novo.Id)
            {
                this.Email = novo.Email;
                this.Nome = novo.Nome;
                if (novo.Senha != Util.HashPassword(""))
                {
                    this.senha = novo.Senha;
                }

                if (!(novo.Endereco is null))
                {
                    this.AlterarEndereco(novo.Endereco);
                }
                else
                {
                    this.RemoverEndereco();
                }
            }
        }

        public void Dispose()
        {
            this.id = 0;
            this.email = null;
            this.nome = null;
            this.RemoverTelefoneCelular();
            this.RemoverTelefoneFixo();
            this.RemoverEndereco();
        }
        #endregion
    }
}