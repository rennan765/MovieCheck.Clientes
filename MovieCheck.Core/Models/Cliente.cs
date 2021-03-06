﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Core.Models
{
    [Serializable]
    public class Cliente : Usuario
    {
        #region Atributos
        private string cpf;
        private int tipo;   //0 = NORMAL - 1 = ADMINISTRADOR
        private List<Dependente> dependentes;
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
        public List<Dependente> Dependentes
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
            //else
            //{
            //    throw new NewUserFailedException("Este dependente não existe para este usuário.");
            //}
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
