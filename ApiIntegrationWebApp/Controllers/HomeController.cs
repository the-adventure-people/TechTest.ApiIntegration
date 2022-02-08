using ApiIntegration.Interfaces;
using ApiIntegrationWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiIntegrationWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITourRepository tourRepository;
        private readonly IImporter importer;

        public HomeController(ITourRepository tourRepository,
            IImporter importer)
        {
            this.tourRepository = tourRepository;
            this.importer = importer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IndexViewModel viewModel)
        {
            viewModel.Tours = await tourRepository.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> IndexPost(IndexViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.SubmitBtn) 
                && viewModel.SubmitBtn == "submit")
            {
                await importer.Execute(viewModel.ProviderId);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}