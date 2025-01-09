using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly shopmanagementContext _shopcontext;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Cus()
    {
        return View();
    }


    public IActionResult BlogDetail()
    {
        return View();
    }

    public IActionResult BlogList()
    {
        return View();
    }

    public IActionResult MusicGenre()
    {
        return View();
    }
    public IActionResult Editus()
    {
        return View();
    }
    public IActionResult History()
    {
        return View();
    }

    public IActionResult MusicMood()
    {
        return View();
    }


// Artists
        public IActionResult Artist()
    {
        return View();
    }

        public IActionResult Artist_Yoasobi()
    {
        return View();
    }

        public IActionResult Payment()
    {
        return View();
    }

    public IActionResult Wish()
    {
        return View();
    }
    public IActionResult Bank()
    {
        return View();
    }
    public IActionResult Store()
    {
        return View();
    }

    public IActionResult Record_Player()
    {
        return View();
    }
    public IActionResult Accessories()
    {
        return View();
    }
    public IActionResult AccessoriesDetails()
    {
        return View();
    }
    public IActionResult RecordPlayer_Details()
    {
        return View();
    }

    public IActionResult MusicEra()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}