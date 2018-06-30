using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    public class FilmeController : Controller
    {
        #region Propriedades
        public IDataService _dataService { get; set; }
        #endregion

        #region Construtores
        public FilmeController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion


    }
}