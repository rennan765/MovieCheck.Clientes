using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Models;
using System;
using System.Net;
using System.Net.Http;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        #region Atributos
        private IDataService _dataService;
        private HttpResponseMessage response;
        #endregion

        #region Construtores
        public ClienteController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Métodos
        [HttpGet("{clienteId}")]
        public HttpResponseMessage Get(int clienteId)
        {
            try
            {
                var usuario = _dataService.ObterUsuarioPorId(clienteId);

                if (usuario is null)
                {
                    throw new NewUserFailedException("Usuário não encontrado. ");
                }

                response = Request.CreateResponse(HttpStatusCode.OK, usuario);
            }
            catch (NewUserFailedException e)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new HttpError(e.Descricao));
            }
            catch (Exception e)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(e.Descricao));
            }

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "cliente", id = clienteId }));

            return response;
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Cliente cliente)
        {
            try
            {
                this._dataService.AdicionarCliente(cliente);
                
                response = Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (NewUserFailedException e)
            {
                response = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, new HttpError(e.Desricao));
            }
            catch (Exception e)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(e.Message));
            }

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "cliente", id = cliente.Id }));

            return response;
        }
        #endregion
    }
}