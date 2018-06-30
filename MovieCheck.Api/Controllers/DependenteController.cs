using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;

namespace MovieCheck.Api.Controllers
{
    [Route("api/[controller]")]
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


    }
}