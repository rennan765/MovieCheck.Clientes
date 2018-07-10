using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Infra.Exceptions;
using MovieCheck.Api.Infra.Factory;
using MovieCheck.Api.Models;
using System;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
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
        public ActionResult<string> Get()
        {
            return NotFound("Método não implementado.");
        }

        // GET: api/Cliente/5
        [HttpGet("{id}", Name = "ObterClientePorId")]
        //[Route("/ObterClientePorId/{id}")]
        public ActionResult<string> ObterClientePorId(int id)
        {
            try
            {
                return Ok(_dataService.ObterCliente(id));
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // POST: api/Cliente
        [HttpPost]
        public ActionResult<string> AdicionarCliente([FromBody]Cliente cliente)
        {
            try
            {
                this._dataService.AdicionarCliente(cliente);

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public ActionResult<string> EditarCliente(int id, [FromBody]Cliente cliente)
        {
            try
            {
                if (!(_dataService.ObterUsuarioPorId(id) is null))
                {
                    this._dataService.EditarUsuario(cliente);
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
                Cliente cliente = (Cliente)_dataService.ObterUsuarioPorId(id);
                if (!(cliente is null))
                {
                    _dataService.RemoverCliente(cliente);
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
