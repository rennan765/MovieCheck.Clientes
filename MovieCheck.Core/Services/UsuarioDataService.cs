using Microsoft.EntityFrameworkCore;
using MovieCheck.Core.Context;
using MovieCheck.Core.Interface.Services;
using MovieCheck.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Core.Services
{
    public class UsuarioDataService : IUsuarioDataService
    {
        #region Atributos
        private readonly MovieCheckContext _contexto;
        private readonly ITelefoneDataService _telefoneDataService;
        #endregion

        #region Construtores
        public UsuarioDataService(MovieCheckContext contexto, ITelefoneDataService telefoneDataService)
        {
            this._telefoneDataService = telefoneDataService;
            this._contexto = contexto;
        }
        #endregion

        #region Usuario
        public IList<Usuario> ObterListaUsuario()
        {
            return _contexto.Usuario
                .Include(e => e.Endereco)
                .ToList();
        }

        public Usuario ObterUsuarioCompletoPorId(int id)
        {
            var usuario = this._contexto.Usuario.Find(id);

            if (!(usuario is null))
            {
                if (this.ObterTipoUsuario(usuario) == "Cliente")
                {
                    Cliente cliente = _contexto.Cliente
                        .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                        .Include(e => e.Endereco)
                        .Include(d => d.Dependentes)
                        .Include(p => p.Pendencias).ThenInclude(f => f.Filme)
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
            else
            {
                return null;
            }
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return this._contexto.Usuario
                .Include(e => e.Endereco)
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            return this._contexto.Usuario
                .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                .Include(e => e.Endereco)
                .Include(p => p.Pendencias).ThenInclude(f => f.Filme)
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
                    if (_telefoneDataService.ExisteTelefone(usuarioEditado.ObterTelefoneFixo()))
                    {
                        var fixo = _telefoneDataService.ObterTelefone(usuarioEditado.ObterTelefoneFixo());
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
                    if (_telefoneDataService.ExisteTelefone(usuarioEditado.ObterTelefoneCelular()))
                    {
                        var celular = _telefoneDataService.ObterTelefone(usuarioEditado.ObterTelefoneCelular());
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

        public void AlterarSenha(Usuario usuario, string novaSenha)
        {
            usuario.Senha = Util.HashPassword(novaSenha);
            _contexto.Usuario.Update(usuario);
            _contexto.SaveChanges();
        }
        #endregion

        #region Cliente
        public bool VerificarClientePorCpf(string cpf)
        {
            return _contexto.Cliente.Any(c => c.Cpf == cpf);
        }

        public void AdicionarCliente(Cliente cliente)
        {
            //VERIFICA TELEFONE
            //FIXO
            if (cliente.ExisteTelefoneFixo())
            {
                if (_telefoneDataService.ExisteTelefone(cliente.ObterTelefoneFixo()))
                {
                    var fixo = _telefoneDataService.ObterTelefone(cliente.ObterTelefoneFixo());
                    cliente.EditarTelefoneFixo(fixo);
                }
            }

            //CELULAR
            if (cliente.ExisteTelefoneCelular())
            {
                if (_telefoneDataService.ExisteTelefone(cliente.ObterTelefoneCelular()))
                {
                    var celular = _telefoneDataService.ObterTelefone(cliente.ObterTelefoneCelular());
                    cliente.EditarTelefoneFixo(celular);
                }
            }

            //ATUALIZA CONTEXTO
            _contexto.Cliente.Add(cliente);
            _contexto.SaveChanges();
        }

        public IList<string> ObterEmailAdministradores()
        {
            IList<string> listaEmail = new List<string>();
            var listaAdministradores = _contexto.Cliente.Where(c => c.Tipo == 1).ToList();

            foreach (Cliente administrador in listaAdministradores)
            {
                listaEmail.Add(administrador.Email);
            }

            return listaEmail;
        }

        public void RemoverCliente(Cliente cliente)
        {
            cliente.RemoverTelefoneFixo();
            cliente.RemoverTelefoneCelular();
            cliente.RemoverEndereco();
            foreach (Dependente dependente in cliente.Dependentes)
            {
                _contexto.Dependente.Remove(dependente);
            }
            _contexto.Cliente.Remove(cliente);
            _contexto.SaveChanges();
        }
        #endregion

        #region Dependente
        public IList<Dependente> ObterListaDependente()
        {
            return _contexto.Dependente
                .ToList();
        }

        public IList<Dependente> ObterListaDependente(Cliente cliente)
        {
            return _contexto.Dependente
                .Where(d => d.ClienteId == cliente.Id).ToList();
        }

        public Dependente ObterDependente(int id)
        {
            return _contexto.Dependente.Find();
        }

        public void AdicionarDependente(Cliente responsavel, Dependente dependente)
        {
            //VERIFICA TELEFONE
            //FIXO
            if (dependente.ExisteTelefoneFixo())
            {
                if (_telefoneDataService.ExisteTelefone(dependente.ObterTelefoneFixo()))
                {
                    var fixo = _telefoneDataService.ObterTelefone(dependente.ObterTelefoneFixo());
                    dependente.EditarTelefoneFixo(fixo);
                }
            }
            //CELULAR
            if (dependente.ExisteTelefoneCelular())
            {
                if (_telefoneDataService.ExisteTelefone(dependente.ObterTelefoneCelular()))
                {
                    var celular = _telefoneDataService.ObterTelefone(dependente.ObterTelefoneCelular());
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

        public void RemoverDependente(Dependente dependente)
        {
            dependente.RemoverTelefoneFixo();
            dependente.RemoverTelefoneCelular();
            dependente.RemoverEndereco();
            _contexto.Dependente.Remove(dependente);
            _contexto.SaveChanges();
        }

        public void EditarDependente(int idDependente, Dependente dependenteEditado)
        {
            var dependenteBanco = ObterDependente(idDependente);

            dependenteBanco.AtualizarUsuario(dependenteEditado);

            //VERIFICA TELEFONE
            //FIXO
            if (dependenteEditado.ExisteTelefoneFixo())
            {
                if (!dependenteBanco.ObterTelefoneFixo().Equals(dependenteEditado.ObterTelefoneFixo()))
                {
                    if (_telefoneDataService.ExisteTelefone(dependenteEditado.ObterTelefoneFixo()))
                    {
                        var fixo = _telefoneDataService.ObterTelefone(dependenteEditado.ObterTelefoneFixo());
                        dependenteBanco.EditarTelefoneFixo(fixo);
                    }
                    else
                    {
                        dependenteBanco.AdicionarTelefone(dependenteEditado.ObterTelefoneFixo());
                    }
                }
            }
            else
            {
                dependenteBanco.RemoverTelefoneFixo();
            }

            //CELULAR
            if (dependenteEditado.ExisteTelefoneCelular())
            {
                if (!dependenteBanco.ObterTelefoneCelular().Equals(dependenteEditado.ObterTelefoneCelular()))
                {
                    if (_telefoneDataService.ExisteTelefone(dependenteEditado.ObterTelefoneCelular()))
                    {
                        var celular = _telefoneDataService.ObterTelefone(dependenteEditado.ObterTelefoneCelular());
                        dependenteBanco.EditarTelefoneCelular(celular);
                    }
                    else
                    {
                        dependenteBanco.AdicionarTelefone(dependenteEditado.ObterTelefoneCelular());
                    }
                }
            }
            else
            {
                dependenteBanco.RemoverTelefoneCelular();
            }

            dependenteBanco.AtribuirResponsavel(dependenteEditado.Cliente);

            //ATUALIZAR CONTEXTO
            _contexto.Dependente.Update(dependenteBanco);
            _contexto.SaveChanges();
        }
        #endregion
    }
}
