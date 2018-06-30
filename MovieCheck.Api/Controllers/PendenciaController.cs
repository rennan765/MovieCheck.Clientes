using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
    public class PendenciaController : Controller
    {
        #region Propriedades
        public IDataService _dataService { get; set; }
        #endregion

        #region Construtores
        public PendenciaController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        #endregion


    }
}