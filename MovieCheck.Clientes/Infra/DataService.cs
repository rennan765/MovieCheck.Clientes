using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieCheck.Clientes.Models;

namespace MovieCheck.Clientes.Infra
{
    public class DataService : IDataService
    {
        private readonly MovieCheckContext _contexto;
        private readonly IHttpContextAccessor _contextAccessor;

        public DataService(MovieCheckContext contexto, IHttpContextAccessor contextAccessor)
        {
            this._contexto = contexto;
            this._contextAccessor = contextAccessor;
        }

        #region Banco
        public void IniciarDb()
        {
            this._contexto.Database.EnsureCreated();
            this._contexto.Database.Migrate();
        }
        #endregion

        #region Secao
        public bool VerificarSecao()
        {
            return this._contextAccessor.HttpContext.Session.GetInt32("Id").HasValue;
        }

        public void IniciarSessao(Usuario usuario)
        {
            _contextAccessor.HttpContext.Session.SetInt32("Id", usuario.Id);
        }

        public void FinalizarSessao()
        {
            _contextAccessor.HttpContext.Session.Clear();
        }

        public Usuario ObterUsuarioSessao()
        {
            return this.ObterUsuarioPorId((int)this._contextAccessor.HttpContext.Session.GetInt32("Id"));
        }
        #endregion

        #region Usuario
        public Usuario ObterUsuarioPorId(int id)
        {
            var usuario = this._contexto.Usuario.Find(id);

            if (this.ObterTipoUsuario(usuario) == "Cliente")
            {
                Cliente cliente = _contexto.Cliente
                    .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                    .Include(e => e.Endereco)
                    .Include(d => d.Dependentes)
                    .Where(c => c.Id == usuario.Id)
                    .FirstOrDefault();

                return cliente;
            }
            else
            {
                Dependente dependente = _contexto.Dependente
                    .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                    .Include(e => e.Endereco)
                    .Include(d => d.Cliente)
                    .Where(c => c.Id == usuario.Id)
                    .FirstOrDefault();

                return dependente;
            }
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            return this._contexto.Usuario
                .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                .Include(e => e.Endereco)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public string ObterTipoUsuario(Usuario usuario)
        {
            return _contexto.Entry(usuario).Property("Discriminator").CurrentValue.ToString();
        }

        public bool VerificarUsuarioPorEmail(string email)
        {
            return _contexto.Usuario.Any(u => u.Email == email);
        }

        public void EditarUsuario(Usuario usuarioEditado)
        {
            var usuarioBanco = this.ObterUsuarioPorId(usuarioEditado.Id);

            usuarioBanco.AtualizarUsuario(usuarioEditado);

            //VERIFICA TELEFONE
            //FIXO
            if (usuarioEditado.ExisteTelefoneFixo())
            {
                if (!usuarioBanco.ObterTelefoneFixo().Equals(usuarioEditado.ObterTelefoneFixo()))
                {
                    if (this.ExisteTelefone(usuarioEditado.ObterTelefoneFixo()))
                    {
                        var fixo = this.ObterTelefone(usuarioEditado.ObterTelefoneFixo());
                        usuarioBanco.EditarTelefoneFixo(fixo);
                    }
                    else
                    {
                        usuarioBanco.AdicionarTelefone(usuarioEditado.ObterTelefoneFixo());
                    }
                }
            }
            else
            {
                usuarioBanco.RemoverTelefoneFixo();
            }

            //CELULAR
            if (usuarioEditado.ExisteTelefoneCelular())
            {
                if (!usuarioBanco.ObterTelefoneCelular().Equals(usuarioEditado.ObterTelefoneCelular()))
                {
                    if (this.ExisteTelefone(usuarioEditado.ObterTelefoneCelular()))
                    {
                        var celular = this.ObterTelefone(usuarioEditado.ObterTelefoneCelular());
                        usuarioBanco.EditarTelefoneCelular(celular);
                    }
                    else
                    {
                        usuarioBanco.AdicionarTelefone(usuarioEditado.ObterTelefoneCelular());
                    }
                }
            }
            else
            {
                usuarioBanco.RemoverTelefoneCelular();
            }

            //ATUALIZAR CONTEXTO
            _contexto.Usuario.Update(usuarioBanco);
            _contexto.SaveChanges();
        }

        public void ExcluirUsuario(Usuario usuario)
        {
            if (this.ObterTipoUsuario(usuario) != "Dependente")
            {
                var cliente = (Cliente)usuario;
                if (cliente.Dependentes.Count > 0)
                {
                    foreach (var dependente in cliente.Dependentes)
                    {
                        _contexto.Dependente.Remove(dependente);
                    }
                }
            }
            _contexto.Usuario.Remove(usuario);
            _contexto.SaveChanges();
        }
        #endregion

        #region Cliente
        public bool VerificarClientePorCpf(string cpf)
        {
            return _contexto.Cliente.Any(c => c.Cpf == cpf);
        }
        #endregion

        #region Dependente
        public IList<Dependente> ObterListaDependente(Cliente cliente)
        {
            return _contexto.Dependente.Where(d => d.ClienteId == cliente.Id).ToList();
        }

        public Dependente ObterDependente(int id)
        {
            return _contexto.Dependente
                .Include(t => t.Telefones)
                .ThenInclude(ut => ut.Telefone)
                .Include(e => e.Endereco)
                .Include(c => c.Cliente)
                .Where(d => d.Id == id).FirstOrDefault();
        }

        public void AdicionarDependente(Cliente responsavel, Dependente dependente)
        {
            //VERIFICA TELEFONE
            //FIXO
            if (dependente.ExisteTelefoneFixo())
            {
                if (this.ExisteTelefone(dependente.ObterTelefoneFixo()))
                {
                    var fixo = ObterTelefone(dependente.ObterTelefoneFixo());
                    dependente.EditarTelefoneFixo(fixo);
                }
            }
            //CELULAR
            if (dependente.ExisteTelefoneCelular())
            {
                if (this.ExisteTelefone(dependente.ObterTelefoneCelular()))
                {
                    var celular = ObterTelefone(dependente.ObterTelefoneCelular());
                    dependente.EditarTelefoneFixo(celular);
                }
            }

            //ATRIBUI DEPENDENTE AO CLIENTE
            responsavel.AdicionarDependente(dependente);
            dependente.AtribuirResponsavel(responsavel);

            //ATUALIZA CONTEXTO
            _contexto.Dependente.Add(dependente);
            _contexto.Cliente.Update(responsavel);
            _contexto.SaveChanges();
        }
        #endregion

        #region Telefone
        public bool ExisteTelefone(Telefone telefone)
        {
            if (_contexto.Telefone.Any(t => t.Tipo == telefone.Tipo && t.Ddd == telefone.Ddd && t.Numero == telefone.Numero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteOutroUsuarioTelefone(Usuario usuario, Telefone telefone)
        {
            var tel = _contexto.Telefone.Include(u => u.Usuarios).ThenInclude(ut => ut.Usuario).Where(t => t.Equals(telefone)).FirstOrDefault();
            if (tel.Usuarios.Any(u => !u.Usuario.Equals(usuario)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Telefone ObterTelefone(Telefone telefone)
        {
            return _contexto.Telefone.Where(t => t.Tipo == telefone.Tipo && t.Ddd == telefone.Ddd && t.Numero == telefone.Numero ).FirstOrDefault();
        }
        #endregion
    }
}
