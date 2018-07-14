using System;
using System.Collections.Generic;

namespace MovieCheck.Core.Models
{
    [Serializable]
    public class Dependente : Usuario
    {
        #region Atributos
        private int clienteId;
        private Cliente cliente;
        #endregion

        #region Propriedades
        public int ClienteId
        {
            get { return this.clienteId; }
            set { this.clienteId = value; }
        }
        public Cliente Cliente
        {
            get { return this.cliente; }
            set { this.cliente = value; }
        }
        #endregion

        #region Construtores
        public Dependente()
        {
            this.Telefones = new List<UsuarioTelefone>();
            this.Pendencias = new List<Pendencia>();
        }
        #endregion

        #region Metodos
        public void AtribuirResponsavel(Cliente cliente)
        {
            this.Cliente = cliente;
        }
        #endregion
    }
}
