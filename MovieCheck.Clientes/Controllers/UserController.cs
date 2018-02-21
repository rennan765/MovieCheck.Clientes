using Microsoft.AspNetCore.Mvc;
using MovieCheck.Clientes.Infra;
using MovieCheck.Clientes.Models;
using MovieCheck.Clientes.Models.ViewModels;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Controllers
{
    public class UserController : Controller
    {
        private readonly IDataService _dataService;

        public UserController(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        //public IActionResult Guests()
        //{
        //    bool status;
        //    var listaDependente = new List<DependenteViewModel>();
        //    foreach (var dependente in this._dataService.ObterListaDependente((Cliente)this._dataService.ObterUsuarioPorEmail("rennan765@gmail.com")))
        //    {
        //        if (dependente.Status == 1)
        //        {
        //            status = true;
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //        listaDependente.Add(new DependenteViewModel()
        //        {
        //            Id = dependente.Id,
        //            Email = dependente.Email,
        //            Nome = dependente.Nome,
        //            Endereco = dependente.Endereco,
        //            TelefoneFixo = dependente.ObterTelefoneFixo(),
        //            TelefoneCelular = dependente.ObterTelefoneCelular(),
        //            Status = status,
        //            ClienteId = dependente.ClienteId,
        //            ClienteNome = dependente.Cliente.Nome
        //        });
        //    }
        //    return View(listaDependente);
        //}

        //[HttpGet]
        //public IActionResult Guests(int dependenteId)
        //{
        //    bool status;
        //    var listaDependente = new List<DependenteViewModel>();
        //    foreach (var dependente in this._dataService.ObterListaDependente((Cliente)this._dataService.ObterUsuarioPorEmail("rennan765@gmail.com")))
        //    {
        //        if (dependente.Status == 1)
        //        {
        //            status = true;
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //        listaDependente.Add(new DependenteViewModel()
        //        {
        //            Id = dependente.Id,
        //            Email = dependente.Email,
        //            Senha = dependente.Senha,
        //            Nome = dependente.Nome,
        //            Endereco = dependente.Endereco,
        //            Telefones = dependente.Telefones,
        //            Status = status,
        //            ClienteId = dependente.ClienteId,
        //            ClienteNome = dependente.Cliente.Nome
        //        });
        //    }

        //    var dependente = _dataService.ObterDependente(dependenteId);
        //    var dependenteViewModel = new DependenteViewModel()
        //    {

        //    }

        //    return View(listaDependente);
        //}
    }
}