using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Models
{
    public class Telefone
    {
        private int id;
        private int tipo;   //0 = FIXO - 1 = CELULAR
        private int ddd;
        private string numero;
        private IList<UsuarioTelefone> usuarios;

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
        public IList<UsuarioTelefone> Usuarios
        {
            get { return this.usuarios; }
            set { this.usuarios = value; }
        }

        /********** FUNCTIONS **********/

        public override bool Equals(object telefone)
        {
            Telefone tel = (Telefone)telefone;
            return this.Tipo == tel.Tipo && this.Ddd == tel.Ddd && this.Numero == tel.Numero;
        }

        public bool Fixo()
        {
            return this.Tipo == 0;
        }

        public bool Celular()
        {
            return this.Tipo == 1;
        }
    }
}
