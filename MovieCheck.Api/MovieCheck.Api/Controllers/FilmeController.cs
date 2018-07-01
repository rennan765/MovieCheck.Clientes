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
    public class FilmeController : Controller
    {
        #region Atributos
        private readonly IDataService _dataService;
        #endregion

        #region Construtores
        public FilmeController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion

        #region Actions
        // GET: api/Filme
        [HttpGet]
        public IList<Filme> Get()
        {
            return _dataService.ObterCatalogoCompleto();
        }

        // GET: api/Filme/5
        [HttpGet("{id}", Name = "GetById")]
        public Filme GetById(int id)
        {
            return _dataService.ObterFilmePorId(id);
        }
        
        // POST: api/Filme
        [HttpPost]
        public void Post([FromBody]Filme filme)
        {
            var listaAtor = new List<Ator>();
            var listaDiretor = new List<Diretor>();
            var listaGenero = new List<Genero>();

            foreach (var ator in filme.Atores)
            {
                listaAtor.Add(ator.Ator);
            }

            foreach (var diretor in filme.Diretores)
            {
                listaDiretor.Add(diretor.Diretor);
            }

            foreach (var genero in filme.Generos)
            {
                listaGenero.Add(genero.Genero);
            }

            if (_dataService.TituloJaExiste(filme.Titulo))
            {
                _dataService.AdicionarExemplar(filme, filme.Midia);
            }
            else
            {
                _dataService.AdicionarFilme(filme, listaAtor, listaDiretor, listaGenero, filme.ClassificacaoIndicativa);
            }
        }
        
        // PUT: api/Filme/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Filme filme)
        {
            var listaAtor = new List<Ator>();
            var listaDiretor = new List<Diretor>();
            var listaGenero = new List<Genero>();

            foreach (var ator in filme.Atores)
            {
                listaAtor.Add(ator.Ator);
            }

            foreach (var diretor in filme.Diretores)
            {
                listaDiretor.Add(diretor.Diretor);
            }

            foreach (var genero in filme.Generos)
            {
                listaGenero.Add(genero.Genero);
            }

            _dataService.EditarTitulo(filme, listaAtor, listaDiretor, listaGenero);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _dataService.ExcluirExemplar(_dataService.ObterFilmePorId(id));
        }
        #endregion
    }
}
