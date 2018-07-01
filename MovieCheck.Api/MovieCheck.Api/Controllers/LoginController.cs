using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Models;
using MovieCheck.Api.Models.ViewModel;

namespace MovieCheck.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public LoginController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Login/5
        [HttpGet("{id}", Name = "GetById")]
        public Usuario GetById(int id)
        {
            var usuario = _dataService.ObterUsuarioPorId(id);

            return usuario;
        }
        
        // POST: api/Login
        [HttpPost]
        public Usuario Post([FromBody]UsuarioViewModel usuarioViewModel)
        {
            var usuario = _dataService.ObterUsuarioPorEmail(usuarioViewModel.Email);

            if (!(usuario is null))
            {
                if (usuario.VerificarSenha(usuarioViewModel.Senha))
                {
                    if (usuario.Status == 1)
                    {
                        return usuario;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        
        // PUT: api/Login/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UsuarioViewModel usuarioViewModel)
        {
            _dataService.AlterarSenha(_dataService.ObterUsuarioPorId(id), usuarioViewModel.Senha);
        }
        #endregion
    }
}
