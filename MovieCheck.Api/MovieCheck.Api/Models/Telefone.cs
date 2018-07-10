using System;
using System.Collections.Generic;

namespace MovieCheck.Api.Models
{
    [Serializable]
    public class Telefone
    {
        #region Atributos
        private int id;
        private int tipo;   //0 = FIXO - 1 = CELULAR
        private int ddd;
        private string numero;
        private List<UsuarioTelefone> usuarios;
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Tipo
        {
            get { return this.tipo; }
            set { this.tipo = value; }
        }
        public int Ddd
        {
            get { return this.ddd; }
            set { this.ddd = value; }
        }
        public string Numero
        {
            get { return this.numero; }
            set { this.numero = value; }
        }
        public List<UsuarioTelefone> Usuarios
        {
            get { return this.usuarios; }
            set { this.usuarios = value; }
        }
        #endregion

        #region Metodos
        public bool Fixo()
        {
            return this.Tipo == 0;
        }

        public bool Celular()
        {
            return this.Tipo == 1;
        }
        #endregion
    }
}
