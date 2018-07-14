using Microsoft.EntityFrameworkCore;
using MovieCheck.Core.Context;
using MovieCheck.Core.Interface.Services;
using MovieCheck.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Core.Services
{
    public class TelefoneDataService : ITelefoneDataService
    {
        #region Atributos
        private readonly MovieCheckContext _contexto;
        #endregion

        #region Construtores
        public TelefoneDataService(MovieCheckContext contexto)
        {
            this._contexto = contexto;
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
            return _contexto.Telefone.Where(t => t.Tipo == telefone.Tipo && t.Ddd == telefone.Ddd && t.Numero == telefone.Numero).FirstOrDefault();
        }

        public Telefone ObterTelefonePorId(int id)
        {
            return _contexto.Telefone.Find(id);
        }

        public IList<Telefone> ObterTelefonesPorUsuario(Usuario usuario)
        {
            var listaTelefone = new List<Telefone>();

            if (usuario.ExisteTelefoneFixo())
            {
                listaTelefone.Add(usuario.ObterTelefoneFixo());
            }

            if (usuario.ExisteTelefoneCelular())
            {
                listaTelefone.Add(usuario.ObterTelefoneCelular());
            }

            return listaTelefone;
        }
        #endregion
    }
}
