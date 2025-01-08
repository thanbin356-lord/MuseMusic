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

// Artist    
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
    public IActionResult Vinyl()
    {
        using (var db = new shopmanagementContext())
        {
            var vinylViewModel = new VinylViewModel
            {
                Products = (from p in db.Products
                            join v in db.Vinyls on p.Id equals v.ProductId
                            join img in db.ImageUrls.Where(img => (bool)img.IsPrimary) on p.Id equals img.ProductId
                              let artistNames = (from av in db.ArtistVinyls
                                           join a in db.Artists on av.ArtistId equals a.Id
                                           where av.VinylId == v.Id
                                           select a.Name).ToList()
                            select new Product
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                ProductImage = img.Url,
                                ProductDescription = p.Description,
                                Price = p.Price.ToString("C") ?? "N/A",
                                DiskId = v.DiskId,
                                Tracklist = v.Tracklist,
                                ArtistNames = string.Join(", ",artistNames)
                                // Status = v.Status // Assume this field exists to indicate preorder, etc.
                            }).ToList()
            };

            return View(vinylViewModel);
        }
    }
    public IActionResult VinylDetails(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the specific product along with its vinyl details by using the provided ID
            var vinylDetails = (from p in db.Products
                                join v in db.Vinyls on p.Id equals v.ProductId
                                where p.Id == id // Filter by the passed product ID
                                select new Product
                                {
                                    ProductId = p.Id,
                                    ProductName = p.Name,
                                    ProductDescription = p.Description,
                                    Price = p.Price.ToString("C") ?? "N/A",
                                    DiskId = v.DiskId,
                                    Tracklist = v.Tracklist,
                                    Years = (int)v.Years,
                                    // Add any other properties you need from the Vinyl model, like Status
                                }).FirstOrDefault(); // Only one product will be found since you're filtering by ID

            if (vinylDetails == null)
            {
                return NotFound(); // Return 404 if no product is found with the given ID
            }

            // Return the model with the selected product
            var vinylDetailsViewModel = new VinylViewModel
            {
                SelectedProduct = vinylDetails, // Set the SelectedProduct property to the fetched product
                Products = (from p in db.Products
                            join v in db.Vinyls on p.Id equals v.ProductId
                            where p.Id != id  // Exclude the selected product
                            select new Product
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                ProductDescription = p.Description,
                                Price = p.Price.ToString("C") ?? "N/A",
                                DiskId = v.DiskId,
                                Tracklist = v.Tracklist,
                                Years = (int)v.Years,
                            }).ToList()
            };
            return View(vinylDetailsViewModel); // Pass the VinylViewModel to the view
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public class VinylViewModel
    {
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ArtistNames { get; set; }
        public string ProductDescription { get; set; }
        public string Price { get; set; }
        public string Tracklist { get; set; }
        public string Status { get; set; } // For preorder or other statuses
        public int Years { get; set; }

    }

}
