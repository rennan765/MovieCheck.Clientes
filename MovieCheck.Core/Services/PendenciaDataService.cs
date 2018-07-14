using Microsoft.EntityFrameworkCore;
using MovieCheck.Core.Context;
using MovieCheck.Core.Interface.Services;
using MovieCheck.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Core.Services
{
    public class PendenciaDataService : IPendenciaDataService
    {
        #region Atributos
        private readonly MovieCheckContext _contexto;
        #endregion

        #region Construtores
        public PendenciaDataService(MovieCheckContext contexto)
        {
            this._contexto = contexto;
        }
        #endregion

        #region Pendencia
        public IList<Pendencia> ObterListaPendencia()
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Include(u => u.Usuario)
                .ToList();
        }

        public bool ExistePendencia(Filme filme)
        {
            //O filme possui disponibilidade quando existe um filme 
            //com o mesmo título e mídia com situação que não seja 
            //reservado ou alugado.
            return _contexto.Pendencia.Any(p => p.Filme.Titulo == filme.Titulo &&
                                                p.Filme.Midia == filme.Midia &&
                                                (p.Status == "0" || p.Status == "2"));
        }

        public IList<Pendencia> ObterPendenciaPorUsuario(Usuario usuario)
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Where(p => p.UsuarioId == usuario.Id)
                .ToList();
        }

        public Pendencia ObterPendenciaPorId(int id)
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Include(u => u.Usuario)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void AdicionarPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Add(pendencia);
            _contexto.SaveChanges();
        }

        public void EditarPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Update(pendencia);
            _contexto.SaveChanges();
        }

        public void RemoverPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Remove(pendencia);
            _contexto.SaveChanges();
        }
        #endregion
    }
}
