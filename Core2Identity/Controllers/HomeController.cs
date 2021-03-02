using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core2Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace Core2Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[Authorize] //Eğer ki kullanıcı login olduysa çalışır. Login olmadıysa ve Account/login controller'ı yoksa 404 hatası alırız.
        //[Authorize] ifadesi,eğer ki kullanıcı login olmadıysa bizi direkt Account/login controller'ına yönlendirir.
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles ="Admin")]//Rolü sadece Admin olanlar bu fonksiyona ulaşabilirler.
        public IActionResult Sample()
        {
            return View();
        }
    }
}
