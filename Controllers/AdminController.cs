using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public AdminController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Categories()
    {
        return View();
    }
    public IActionResult Vinyl()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var vinylViewModel = new VinylViewModel();
                // bước 2 load dữ liệu từ db 
                var product = (from p in db.Products
                               join v in db.Vinyls on p.Id equals v.ProductId
                               select new
                               {
                                   Product = p,
                                   Vinyl = v
                               }).ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                vinylViewModel.Products = product.Select(x => new Product
            {
                ProductId = x.Product.Id,
                ProductName = x.Product.Name,
                ProductDescription = x.Product.Description,
                Price = x.Product.Price.ToString("C"),
                VinylName = x.Vinyl.Name,
                DiskId = x.Vinyl.DiskId,
                Years = (int)x.Vinyl.Years,
                Tracklist = x.Vinyl.Tracklist
            }).ToList();

                // bước 4 return l 
                return View(vinylViewModel);
            }
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public partial class VinylViewModel
    {
        public List<Product> Products { get; set; }

    }
    public class Product
    {
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string VinylName { get; set; }
        public string Price { get; set; }
        public int Years { get; set; }
        public string Tracklist { get; set; }
    }
}