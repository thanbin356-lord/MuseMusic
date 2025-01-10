using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ViewModels;
using MuseMusic.Models.ManagerModels;
namespace MuseMusic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly shopmanagementContext _shopcontext;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int page = 1, int pageSize = 8) // Set default pageSize to 8
    {
        using (var db = new shopmanagementContext())
        {
            var totalProducts = (from p in db.Products
                                 join v in db.Vinyls on p.Id equals v.ProductId
                                 select p).Count();

            var products = (from p in db.Products
                            join v in db.Vinyls on p.Id equals v.ProductId
                            join img in db.ImageUrls.Where(img => (bool)img.IsPrimary) on p.Id equals img.ProductId
                            let artistNames = (from av in db.ArtistVinyls
                                               join a in db.Artists on av.ArtistId equals a.Id
                                               where av.VinylId == v.Id
                                               select a.Name).ToList()
                            select new Models.ViewModels.Product
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                ProductImage = img.Url,
                                ProductDescription = p.Description,
                                Price = p.Price,
                                DiskId = v.DiskId,
                                Tracklist = v.Tracklist,
                                ArtistNames = string.Join(", ", artistNames),

                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            var artists = (from a in db.Artists
                           select new
                           {
                               ArtistName = a.Name,
                               ArtistImageUrl = a.Artisitmageturl
                           }).Distinct().Take(5).ToList();

            ViewBag.Artists = artists;

            var categories = (from a in db.Categories
                              select new
                              {
                                  CategoriesName = a.Name,
                                  CategoriesImageUrl = a.CategoryImage,
                              }).Take(7).ToList();

            ViewBag.Categories = categories;

            ViewBag.Categories = categories;
            var vinylViewModel = new MuseMusic.Models.ViewModels.VinylViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalProducts = totalProducts
            };

            ViewBag.Message = TempData["Message"];
            return View("~/Views/Home/Index.cshtml", vinylViewModel);
        }
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
