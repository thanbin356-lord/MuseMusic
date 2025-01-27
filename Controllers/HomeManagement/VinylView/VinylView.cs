using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuseMusic.Helpers;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ViewModels;

namespace MuseMusic.Controllers.HomeManagement.VinylView
{
    [Route("home")]
    public class VinylView : Controller
    {
        private readonly ILogger<VinylView> _logger;

        public VinylView(ILogger<VinylView> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("vinyl")]
        public IActionResult Vinyl(int page = 1, int pageSize = 12, List<int> categoryIds = null, List<int> artistIds = null, List<string> selectedEras = null, List<int> moodIds = null)
        {
            using (var db = new shopmanagementContext())
            {
                // Fetching the categories, artists, and moods
                var categories = db.Categories.ToList();
                var artists = db.Artists.ToList();
                var moods = db.Moods.ToList();

                // Building the query and applying filters
                var query = from p in db.Products
                            join v in db.Vinyls on p.Id equals v.ProductId
                            join img in db.ImageUrls.Where(img => (bool)img.IsPrimary) on p.Id equals img.ProductId
                            select new
                            {
                                Product = p,
                                Vinyl = v,
                                ImageUrl = img.Url,
                                Years = v.Years
                            };

                // Apply category filter if categoryIds are provided
                if (categoryIds != null && categoryIds.Any())
                {
                    query = query.Where(p => p.Vinyl.CategoriesVinyls.Any(cv => categoryIds.Contains((int)cv.CategoryId)));
                }

                // Apply other filters similarly
                // ...

                var productsData = query.ToList();
                var products = productsData.Select(p => new Models.ViewModels.Product
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    ProductImage = p.ImageUrl,
                    Price = p.Product.Price,
                    ArtistNames = string.Join(", ", db.ArtistVinyls
                        .Join(db.Artists, av => av.ArtistId, a => a.Id, (av, a) => new { av, a })
                        .Where(av => av.av.VinylId == p.Vinyl.Id)
                        .Select(av => av.a.Name)
                        .ToList()),
                    Years = p.Years.HasValue ? GetEra(p.Years.Value) : 0
                }).ToList();

                var totalProducts = query.Count();
                var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var vinylViewModel = new VinylViewModel
                {
                    Products = pagedProducts,
                    CurrentPage = page,
                    TotalProducts = totalProducts,
                    SelectedCategoryIds = categoryIds ?? new List<int>(),
                    SelectedArtistIds = artistIds ?? new List<int>(),
                    SelectedEraIds = selectedEras ?? new List<string>(),
                    SelectedMoodIds = moodIds ?? new List<int>(),
                    AllCategories = categories,
                    AllArtists = artists,
                    AllMoods = moods
                };

                // Check if the request is AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("~/Views/Home/_VinylProductListPartial.cshtml", vinylViewModel);
                }

                return View("~/Views/Home/Vinyl.cshtml", vinylViewModel);
            }
        }

        private int GetEra(int year)
        {
            if (year >= 1980 && year < 1990) return 1980;
            if (year >= 1990 && year < 2000) return 1990;
            if (year >= 2000 && year < 2010) return 2000;
            if (year >= 2010 && year < 2020) return 2010;
            if (year >= 2020) return 2020;
            return 0;  // For years that don't match any defined era
        }

        [HttpGet("vinyldetails/{id}")]
        public IActionResult VinylDetails(int id)
        {
            using (var db = new shopmanagementContext())
            {
                var vinyl = db.Vinyls
                    .Include(v => v.Product)
                        .ThenInclude(v => v.Brand)
                    .Include(v => v.ArtistVinyls)
                        .ThenInclude(av => av.Artist)
                    .Include(v => v.MoodVinyls)
                        .ThenInclude(mv => mv.Mood)
                    .Include(v => v.CategoriesVinyls)
                        .ThenInclude(cv => cv.Category)
                    .FirstOrDefault(v => v.ProductId == id);

                if (vinyl == null)
                {
                    return NotFound(); // Return 404 if no vinyl is found
                }

                var Products = db.Products
                    .Where(p => p.Vinyls.Any())
                    .Where(p => p.Id != id) // Exclude the selected product
                    .Select(p => new Models.ViewModels.Product // Map each product to the ViewModel
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Price = p.Price,
                        ArtistNames = p.Vinyls
                            .Select(v => v.ArtistVinyls
                            .Select(av => av.Artist.Name)
                            .FirstOrDefault()) // Get the first artist name associated with the vinyl
                            .FirstOrDefault(), // Assuming you want the first artist name
                        PrimaryImageUrl = db.ImageUrls
                            .Where(i => i.ProductId == p.Id && i.IsPrimary == true)
                            .Select(i => i.Url)
                            .FirstOrDefault() // Lấy URL của ảnh chính
                    }).ToList();

                var imageUrls = db.ImageUrls
                    .Where(i => i.ProductId == vinyl.Product.Id)
                    .ToList(); // Lấy danh sách đối tượng ImageUrl

                var primaryImageUrl = imageUrls
                    .FirstOrDefault(i => i.IsPrimary == true)?.Url; // Lấy URL ảnh chính

                var allImageUrls = imageUrls
                    .Select(i => i.Url)
                    .ToList();

                string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "music", id.ToString());
                var musicFiles = Directory.Exists(musicDirectory)
                    ? Directory.GetFiles(musicDirectory, "*.mp3").Select(Path.GetFileName).ToList()
                    : new List<string>(); // Trả về danh sách rỗng nếu thư mục không tồn tại

                var viewModel = new VinylViewModel
                {
                    SelectedProduct = new Models.ViewModels.Product
                    {
                        ProductId = vinyl.Product.Id,
                        ProductName = vinyl.Product.Name,
                        ProductDescription = vinyl.Product.Description,
                        Price = vinyl.Product.Price,
                        ProductQuantity = vinyl.Product.Quantity ?? 0,
                        DiskId = vinyl.DiskId,
                        Years = vinyl.Years ?? 0,
                        Tracklist = vinyl.Tracklist,
                        SelectedBrandName = vinyl.Product.Brand?.Name ?? "",
                        PrimaryImageUrl = primaryImageUrl,
                        ImageUrls = allImageUrls
                    },
                    SelectedArtistNames = vinyl.ArtistVinyls
                        .Select(av => av.Artist.Name)  // Select the artist names instead of IDs
                        .ToList(),
                    SelectedMoodNames = vinyl.MoodVinyls
                        .Select(mv => mv.Mood.Name)  // Select the mood names instead of IDs
                        .ToList(),
                    SelectedCategoryNames = vinyl.CategoriesVinyls
                        .Select(cv => cv.Category.Name)  // Select the category names instead of IDs
                        .ToList(),
                    Products = Products,
                    MusicFiles = musicFiles.Select(m => $"/music/{id}/{m}").ToList()
                };

                return View("~/Views/Home/VinylDetails.cshtml", viewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
