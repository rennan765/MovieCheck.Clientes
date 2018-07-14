using System;
using System.Collections.Generic;

namespace MovieCheck.Site.Models
{
    public class Endereco
    {
        #region Atributos
        private string logradouro;
        private int numero;
        private string complemento;
        private string bairro;
        private string cidade;
        private string estado;
        private string cep;
        #endregion

        #region Propriedades
        public string Logradouro
        {
            get { return this.logradouro;}
            set { this.logradouro = value; }
        }
        public int Numero
        {
            get { return this.numero; }
            set { this.numero = value; }
        }
        public string Complemento
        {
            get { return this.complemento; }
            set { this.complemento = value; }
        }
        public string Bairro
        {
            get { return this.bairro; }
            set { this.bairro = value; }
        }
        public string Cidade
        {
            get { return this.cidade; }
            set { this.cidade = value; }
        }
        public string Estado
        {
            get { return this.estado; }
            set { this.estado = value; }
        }
        public string Cep
        {
            get { return this.cep; }
            set { this.cep = value; }
        }
        #endregion

        #region Construtores
        public Endereco()
        {

        }
        public Endereco(string parametro)
        {
            if (parametro == "semEndereco")
            {
                this.logradouro = "";
                this.numero = 0;
                this.complemento = "";
                this.bairro = "";
                this.cidade = "";
                this.estado = "";
                this.cep = "";
            }
        }
        #endregion
    }
}
