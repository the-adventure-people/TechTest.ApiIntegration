using ApiIntegration.Interfaces;
using ApiIntegrationWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegrationWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImporter _importer;
        private readonly ITourRepository _tourRepository;

        public HomeController(ILogger<HomeController> logger, IImporter importer, ITourRepository tourRepository)
        {
            _logger = logger;
            _importer = importer;
            _tourRepository = tourRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tours = await _tourRepository.Get(g => g.ProviderId == 1);
            return View(tours.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Index(int ProviderId)
        {
            ViewBag.Message = "Clicked";

            await _importer.Execute(ProviderId);

            var tours = await _tourRepository.Get(g => g.ProviderId == ProviderId);
            return View(tours.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
