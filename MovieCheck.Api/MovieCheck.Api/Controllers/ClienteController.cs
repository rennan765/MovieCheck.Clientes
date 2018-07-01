using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Models;

namespace MovieCheck.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public ClienteController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Cliente
        [HttpGet]
        public IList<Cliente> Get()
        {
            return this._dataService.ObterListaCliente();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}", Name = "GetById")]
        public Cliente GetById(int id)
        {
            return (Cliente)this._dataService.ObterUsuarioPorId(id);
        }
        
        // POST: api/Cliente
        [HttpPost]
        public void Post([FromBody]Cliente cliente)
        {
            this._dataService.AdicionarCliente(cliente);
        }
        
        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Cliente cliente)
        {
            this._dataService.EditarUsuario(cliente);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _dataService.RemoverCliente((Cliente)_dataService.ObterUsuarioPorId(id));
        }
        #endregion
    }
}
