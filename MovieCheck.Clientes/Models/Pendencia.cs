using MovieCheck.Clientes.Infra;
using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Models
{
    public class Pendencia
    {
        #region Atributos
        private int id;
        private int usuarioId;
        private Usuario usuario;
        private int filmeId;
        private Filme filme;
        private DateTime dataReserva;
        private string status;    //0: Reservado; 1: Cancelamento automático; 2: Alugado; 3: Devolvido; 4: Cancelado; 5: Perda, roubo ou furto; 6: Mídia danificada
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int UsuarioId
        {
            get { return this.usuarioId; }
            set { this.usuarioId = value; }
        }

        public Usuario Usuario
        {
            get { return this.usuario; }
            set { this.usuario = value; }
        }

        public int FilmeId
        {
            get { return this.filmeId; }
            set { this.filmeId = value; }
        }

        public Filme Filme
        {
            get { return this.filme; }
            set { this.filme = value; }
        }

        public DateTime DataReserva
        {
            get { return this.dataReserva; }
            set { this.dataReserva = value; }
        }

        public string Status  //0: Reservado; 1: Cancelamento automático; 2: Alugado; 3: Devolvido; 4: Cancelado; 5: Perda, roubo ou furto; 6: Mídia danificada
        {
            get { return this.status; }
            set
            {
                ValidarStatus(value);
                this.status = value;
            }
        }
        #endregion

        #region Construtores
        //Construtor vazio para a criação de uma nova pendência
        public Pendencia()
        {
            this.dataReserva = DateTime.Now;
        }
        #endregion

        #region Métodos
        public void NovaReserva(Usuario usuario, Filme filme)
        {
            this.usuario = usuario;
            this.filme = filme;
            this.status = "0";
        }

        public void CancelarReservaAutomaticamente()
        {
            this.status = "1";
        }

        public void AlugarFilme()
        {
            this.status = "2";
        }

        public void DevolverFilme()
        {
            this.status = "3";
        }

        public void CancelarReserva()
        {
            this.status = "4";
        }

        public void PerdeuRoubouFilme()
        {
            this.status = "5";
        }

        public void DanificouMidia()
        {
            this.status = "6";
        }
        
        public string RetornarDescricaoReserva()
        {
            string descricao = "";
            switch (this.status)
            {
                case "0":
                    descricao = "Reservado";
                    break;
                case "1":
                    descricao = "Cancelamento automático";
                    break;
                case "2":
                    descricao = "Alugado";
                    break;
                case "3":
                    descricao = "Devolvido";
                    break;
                case "4":
                    descricao = "Cancelado";
                    break;
                case "5":
                    descricao = "Perda, roubo ou furto";
                    break;
                case "6":
                    descricao = "Mídia danificada";
                    break;
                default:
                    ValidarStatus(this.status);
                    break;
            }

            return descricao;
        }

        private void ValidarStatus(string status)
        {
            var listaStatus = new List<string>()
                {
                    "0", "1", "2", "3", "4", "5", "6"
                };

            if (!listaStatus.Contains(status))
            {
                throw new NewPendenciaFailedException("Status de pendência inválida.");
            }
        }
        #endregion
    }
}
