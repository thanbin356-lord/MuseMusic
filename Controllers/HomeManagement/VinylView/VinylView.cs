using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ViewModels;

namespace MuseMusic.Controllers.HomeManagement.VinylView;


[Authorize]
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
            // Start with an empty query and apply filters only if parameters exist
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

            // Apply filters only if parameters are passed
            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(p => p.Vinyl.CategoriesVinyls.Any(cv => categoryIds.Contains((int)cv.CategoryId)));
            }
            if (artistIds != null && artistIds.Any())
            {
                query = query.Where(p => p.Vinyl.ArtistVinyls.Any(av => artistIds.Contains((int)av.ArtistId)));
            }
            if (moodIds != null && moodIds.Any())
            {
                query = query.Where(p => p.Vinyl.MoodVinyls.Any(mv => moodIds.Contains((int)mv.MoodId)));
            }

            // Execute the query
            var productsData = query.ToList();

            // Map the result to your view model
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
                SelectedMoodIds = moodIds ?? new List<int>()
            };

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
            // Fetch the Vinyl and related data
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

#pragma warning disable CS8601

            var Products = db.Products
            .Where(p => p.Vinyls.Any())
            .Where(p => p.Id != id) // Exclude the selected product
            .Select(p => new Models.ViewModels.Product // Map each product to the ViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Price = p.Price,
                // Join ArtistVinyls to get Artist names
                ArtistNames = p.Vinyls
                    .Select(v => v.ArtistVinyls
                    .Select(av => av.Artist.Name)
                    .FirstOrDefault()) // Get the first artist name associated with the vinyl
                    .FirstOrDefault()// Assuming you want the first artist name
            }).ToList();

            var imageUrls = db.ImageUrls
            .Where(i => i.ProductId == vinyl.Product.Id) // Lọc theo ProductId
            .ToList(); // Lấy danh sách đối tượng ImageUrl

            var primaryImageUrl = imageUrls
                .FirstOrDefault(i => i.IsPrimary == true)?.Url; // Lấy URL ảnh chính

            var allImageUrls = imageUrls
                .Select(i => i.Url) // Chuyển đổi sang danh sách URL
                .ToList();

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
                    PrimaryImageUrl = primaryImageUrl, // Gán URL ảnh chính
                    ImageUrls = allImageUrls // Gán danh sách tất cả URL
                },
                SelectedArtistNames = vinyl.ArtistVinyls
                .Select(av => av.Artist.Name)  // Select the artist names instead of IDs
                .ToList(),
                // Select mood names directly
                SelectedMoodNames = vinyl.MoodVinyls
                .Select(mv => mv.Mood.Name)  // Select the mood names instead of IDs
                .ToList(),
                // Select category names directly
                SelectedCategoryNames = vinyl.CategoriesVinyls
                .Select(cv => cv.Category.Name)  // Select the category names instead of IDs
                .ToList(),
                Products = Products
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

