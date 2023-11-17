using Microsoft.AspNetCore.Mvc;
using StealLyrics.Models;
using StealLyrics.Service.Interface;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace StealLyrics.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetMusica([FromBody]string nomeMusica)
        {
            nomeMusica.Replace(" ", "+");
            var url = $"https://www.google.com/search?q={nomeMusica}+lyrics&oq={nomeMusica}+lyrics&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIJCAEQABgTGIAEMgoIAhAAGBMYFhgeMgoIAxAAGBMYFhgeMgoIBBAAGBMYFhgeMgoIBRAAGBMYFhgeMgoIBhAAGBMYFhgeMgoIBxAAGBMYFhgeMgoICBAAGBMYFhgeMgoICRAAGBMYFhge0gEIMjcwM2owajeoAgCwAgA&sourceid=chrome&ie=UTF-8";
            
            var letra = await _homeService.GetLyrics(url, nomeMusica);

            ViewBag.VideoEmbedUrl = "https://www.youtube.com/watch?v=ygTZZpVkmKg&pp=ygULYWZ0ZXIgaG91cnM%3D";

            return PartialView("~/Views/Partials/_Letra.cshtml", letra);

        }
    }
}