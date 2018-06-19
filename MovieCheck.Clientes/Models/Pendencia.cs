using MovieCheck.Clientes.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private IDictionary<string, string> dicionarioStatus;
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
        public Pendencia()
        {
            IniciarDicionarioStatus();
            this.dataReserva = DateTime.Now;
        }

        public Pendencia(Usuario usuario, Filme filme)
        {
            IniciarDicionarioStatus();
            this.dataReserva = DateTime.Now;
            NovaReserva(usuario, filme);
        }
        #endregion

        #region Métodos
        private void IniciarDicionarioStatus()
        {
            this.dicionarioStatus = new Dictionary<string, string>();

            this.dicionarioStatus.Add("0", "Reservado");
            this.dicionarioStatus.Add("1", "Cancelamento automático");
            this.dicionarioStatus.Add("2", "Alugado");
            this.dicionarioStatus.Add("3", "Devolvido");
            this.dicionarioStatus.Add("4", "Cancelado");
            this.dicionarioStatus.Add("5", "Perda, roubo ou furto");
            this.dicionarioStatus.Add("6", "Mídia danificada");
        }

        public void NovaReserva(Usuario usuario, Filme filme)
        {
            this.usuario = usuario;
            this.filme = filme;
            this.status = dicionarioStatus.Where(ds => ds.Value == "Reservado").First().Key;
        }

        public void CancelarReservaAutomaticamente()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Cancelamento automático").First().Key;
        }

        public void AlugarFilme()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Alugado").First().Key;
        }

        public void DevolverFilme()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Devolvido").First().Key;
        }

        public void CancelarReserva()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Cancelado").First().Key;
        }

        public void PerdeuRoubouFilme()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Perda, roubo ou furto").First().Key;
        }

        public void DanificouMidia()
        {
            this.status = dicionarioStatus.Where(ds => ds.Value == "Mídia danificada").First().Key;
        }

        public string RetornarDescricaoReserva()
        {
            string descricao = "";
            ValidarStatus(this.status);
            foreach (var ds in dicionarioStatus)
            {
                if (ds.Key == this.status)
                {
                    descricao = ds.Value;
                }
            }
            return descricao;
        }

        private void ValidarStatus(string status)
        {
            if (dicionarioStatus is null)
            {
                IniciarDicionarioStatus();
            }

            if (!dicionarioStatus.ContainsKey(status))
            {
                throw new NewPendenciaFailedException("Status de pendência inválida.");
            }
        }

        public bool Disponivel()
        {
            return this.status != "0"
                && this.status != "2"
                && this.status != "5"
                && this.status != "6";
        }
        #endregion
    }
}
