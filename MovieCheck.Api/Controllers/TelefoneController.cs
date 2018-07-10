using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCheck.Api.Infra;
using MovieCheck.Api.Infra.Factory;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelefoneController : ControllerBase
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public TelefoneController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Telefone/5
        //[HttpGet("{id}", Name = "ObterPorId")]
        //[Route("/ObterTelefonePorId/{id}")]
        //public ActionResult<string> ObterTelefonePorId(int id)
        //{
        //    try
        //    {
        //        var telefone = _dataService.ObterTelefonePorId(id);

        //        if (!(telefone is null))
        //        {
        //            return Ok(telefone);
        //        }
        //        else
        //        {
        //            return NotFound("Telefone não encontrado");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet("{usuarioId}", Name = "ObterPorUsuarioId")]
        [Route("/ObterTelefonePorUsuarioId/{usuarioId}")]
        public ActionResult<string> ObterTelefonePorUsuarioId(int usuarioId)
        {
            try
            {
                var usuario = _dataService.ObterUsuarioCompleto(usuarioId);

                if (!(usuario is null))
                {
                    var listaTelefone = _dataService.ObterTelefones(usuario);

                    if (!(listaTelefone is null) && listaTelefone.Count > 0)
                    {
                        return Ok(listaTelefone);
                    }
                    else
                    {
                        return NotFound("Telefone (s) não encontrado (s).");
                    }
                }
                else
                {
                    return NotFound("Usuário não encontrado.");
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
