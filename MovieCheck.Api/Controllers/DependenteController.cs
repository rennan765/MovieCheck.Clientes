using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Models;
using System;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenteController : Controller
    {
        #region Propriedades
        public IDataService _dataService { get; set; }
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
        public ActionResult<string> Get()
        {
            return NotFound("Método não implementado.");
        }

        // GET: api/Dependente/5
        [HttpGet("{id}", Name = "ObterDependentePorId")]
        public ActionResult<string> ObterDependentePorId(int id)
        {
            try
            {
                var dependente = (Dependente)this._dataService.ObterUsuarioPorId(id);

                if (!(dependente is null))
                {
                    return Ok(dependente);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Dependente
        [HttpPost]
        public ActionResult<string> AdicionarDependente([FromBody]Dependente dependente)
        {
            try
            {
                this._dataService.AdicionarDependente(dependente.Cliente, dependente);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Dependente/5
        [HttpPut("{id}")]
        public ActionResult<string> EditarCliente(int id, [FromBody]Dependente dependente)
        {
            try
            {
                if (!(_dataService.ObterUsuarioPorId(id) is null))
                {
                    this._dataService.EditarUsuario(dependente);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<string> RemoverCliente(int id)
        {
            try
            {
                Dependente dependente = (Dependente)_dataService.ObterUsuarioPorId(id);
                if (!(dependente is null))
                {
                    _dataService.RemoverDependente(dependente);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}