using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using System.Net.Http;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        #region Propriedades
        public IDataService _dataService { get; set; }
        public HttpResponseMessage response { get; set; }
        #endregion

        #region Construtores
        public LoginController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Métodos
        // PUT api/values/5
        [HttpPut("{email}")]
        public void Put(string email)
        {
            this._dataService.AlterarSenha(_dataService.ObterUsuarioPorEmail(email), "0000");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string email, string senha)
        {
            var usuario = this._dataService.ObterUsuarioPorEmail(email);
        }
        #endregion
    }
}