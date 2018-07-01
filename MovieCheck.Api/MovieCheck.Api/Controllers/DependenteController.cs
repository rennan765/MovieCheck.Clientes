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
    public class DependenteController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public DependenteController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Dependente
        [HttpGet]
        public IList<Dependente> Get()
        {
            return _dataService.ObterListaDependente();
        }

        // GET: api/Dependente/5
        [HttpGet("{id}", Name = "GetById")]
        public Dependente GetById(int id)
        {
            return (Dependente)this._dataService.ObterUsuarioPorId(id);
        }

        [HttpGet("{idCliente}", Name = "GetByIdCliente")]
        public IList<Dependente> GetByIdCliente(int idCliente)
        {
            return this._dataService.ObterListaDependente((Cliente)this._dataService.ObterUsuarioPorId(idCliente));
        }

        // POST: api/Dependente
        [HttpPost]
        public void Post([FromBody]Dependente dependente)
        {
            _dataService.AdicionarDependente((Cliente)_dataService.ObterUsuarioPorId(dependente.ClienteId), dependente);
        }

        // PUT: api/Dependente/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Dependente dependente)
        {
            _dataService.EditarDependente(id, dependente);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _dataService.RemoverDependente((Dependente)_dataService.ObterUsuarioPorId(id));
        }
        #endregion
    }
}
