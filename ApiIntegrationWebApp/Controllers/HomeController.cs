using ApiIntegration.Interfaces;
using ApiIntegrationWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiIntegrationWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITourRepository tourRepository;
        public HomeController(ITourRepository tourRepository)
        {
            this.tourRepository = tourRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IndexViewModel viewModel)
        {
            viewModel.Tours = await tourRepository.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}