using MovieCheck.Clientes.Models;
using System;

namespace MovieCheck.Clientes.Infra
{
    public class CorpoEmail : IDisposable
    {
        #region Atributos
        private IDataService _dataService;
        private string cabecalho;
        private string rodape;
        #endregion

        #region Construtores
        public CorpoEmail(IDataService dataService)
        {
            this._dataService = dataService;
            this.PreencheCabecalho();
            this.PreencheRodape();
        }

        public CorpoEmail(IDataService dataService, Usuario usuario)
        {
            this._dataService = dataService;
            this.PreencheCabecalho(usuario);
            this.PreencheRodape();
        }
        #endregion

        #region Métodos
        private void PreencheCabecalho()
        {
            this.cabecalho = "Prezado Usuario (a), \n\n";
        }

        private void PreencheCabecalho(Usuario usuario)
        {
            this.cabecalho = $"Prezado {_dataService.ObterTipoUsuario(usuario)}, \n\n";
        }

        private void PreencheRodape()
        {
            this.rodape = "\nAtenciosamente,\n" +
                          "Equipe MovieCheck. \n\n" +
                          "Obs.: Este é um e-mail automático. Favor não responder.";
        }

        public string NovoCliente(Cliente cliente)
        {
            return this.cabecalho +
                   "Foi efetuado o cadastro de um novo cliente através do portal MovieCheck. \n" +
                   $"Nome: {cliente.Nome}\nE-mail: {cliente.Email}\n" +
                   "Favor acessar o painel de administradores e tomar as ações necessárias. \n" +
                   this.rodape;
        }

        public string RedefinirSenha(Usuario usuario)
        {
            return this.cabecalho +
                   "Foi solicitada uma redefinição de senha para o seu usuário através do portal MovieCheck. \n" +
                   "A senha foi alterada para '0000' (sem aspas). Favor acessar o portal e alterar a senha para a nova senha desejada. \n" +
                   this.rodape;
        }

        public string NovoDependente(Dependente dependente)
        {
            var cliente = (!(dependente.Cliente is null) ? dependente.Cliente : (Cliente)_dataService.ObterUsuarioPorId(dependente.ClienteId));

            return this.cabecalho +
                   $"Foi efetuado o cadastro de um novo dependente com o usuário de e-mail {cliente.Email} como responsável: \n" +
                   $"Nome: {dependente.Nome}\nE-mail: {dependente.Email}\n" +
                   "Caso não tenha efetuado ou solicitado a criação deste dependente, favor acessar o portal para bloquear o acesso do mesmo e entrar em contato com um administrador para excluir este usuário.\n" +
                   this.rodape;
        }

        public void Dispose()
        {
            this.cabecalho = null;
            this.rodape = null;
        }
        #endregion
    }
}