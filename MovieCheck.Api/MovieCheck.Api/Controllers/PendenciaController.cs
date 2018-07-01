using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Models;

namespace MovieCheck.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PendenciaController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public PendenciaController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Pendencia
        [HttpGet]
        public IList<Pendencia> Get()
        {
            return _dataService.ObterListaPendencia();
        }

        // GET: api/Pendencia/5
        [HttpGet("{id}", Name = "GetById")]
        public Pendencia GetById(int id)
        {
            return _dataService.ObterPendenciaPorId(id);
        }

        // GET: api/Pendencia/5
        [HttpGet("{usuarioId}", Name = "GetByUsuarioId")]
        public IList<Pendencia> GetByUsuarioId(int usuarioId)
        {
            return _dataService.ObterPendenciaPorUsuario(_dataService.ObterUsuarioPorId(usuarioId));
        }

        [HttpGet("{id}", Name = "GetVerify")]
        public bool GetVerify(Filme filme)
        {
            return _dataService.ExistePendencia(filme);
        }

        // POST: api/Pendencia
        [HttpPost]
        public void Post([FromBody]Pendencia pendencia)
        {
            _dataService.AdicionarPendencia(pendencia);
        }
        
        // PUT: api/Pendencia/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Pendencia pendencia)
        {
            if (pendencia.Id == id)
            {
                _dataService.EditarPendencia(pendencia);
            }
            else
            {
                var pendenciaEditar = _dataService.ObterPendenciaPorId(id);
                pendenciaEditar.AtualizarPendencia(pendencia);
                _dataService.EditarPendencia(pendenciaEditar);
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _dataService.RemoverPendencia(_dataService.ObterPendenciaPorId(id));
        }
        #endregion
    }
}
