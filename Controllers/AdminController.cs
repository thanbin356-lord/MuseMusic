using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
namespace MuseMusic.Controllers;

public class AdminController : Controller{
    private readonly ILogger<HomeController> _logger;
    public AdminController(ILogger<HomeController> logger){
        _logger = logger;
    }
    public IActionResult Login(){
        return View();
    }
    public IActionResult Categories(){
        return View();
    }
    public IActionResult Vinylmanage(){
        return View();
    }
    public IActionResult Usermanage(){
        return View();
    }
     public IActionResult Ordermanage(){
        return View();
    }
     public IActionResult Blogmanage(){
        return View();
    }
     public IActionResult Phukienmanage(){
        return View();
    }
     public IActionResult Daudiamanage(){
        return View();
    }
       public IActionResult Profile(){
        return View();
    }
        public IActionResult Addvinyl(){
        return View();
    }
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}