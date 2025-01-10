using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ViewModels;

namespace MuseMusic.Controllers.HomeManagement.OtherProductView;

[Route("home")]
public class RecordPlayerView : Controller
{
    private readonly ILogger<RecordPlayerView> _logger;

    public RecordPlayerView(ILogger<RecordPlayerView> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("record_player")]
    public IActionResult RecordPlayer(int page = 1, int pageSize = 12)
    {
        using (var db = new shopmanagementContext())
        {
            // Query to fetch products with associated record player details and primary images
            var query = from p in db.Products
                        join rc in db.Recordplayers on p.Id equals rc.ProductId
                        join img in db.ImageUrls.Where(img => img.IsPrimary == true) on p.Id equals img.ProductId
                        select new
                        {
                            Product = p,
                            Recordplayer = rc,
                            ImageUrl = img.Url,
                            BrandName = db.Brands.Where(b => b.Id == p.BrandId).Select(b => b.Name).FirstOrDefault() ?? "Unknown"
                        };

            // Get the total number of products
            var totalProducts = query.Count();

            // Apply pagination
            var pagedQuery = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map query results to view model
            var products = pagedQuery.Select(p => new Models.ViewModels.Product
            {
                ProductId = p.Product.Id,
                ProductName = p.Product.Name,
                ProductImage = p.ImageUrl,
                Price = p.Product.Price,
                SelectedBrandName = p.BrandName
            }).ToList();

            // Prepare the view model
            var recordPlayerViewModel = new RecordPlayerViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalProducts = totalProducts,
                PageSize = pageSize
            };

            // Return the view with the populated model
            return PartialView("~/Views/Home/Record_Player.cshtml", recordPlayerViewModel);
        }
    }

    [HttpGet("recordplayer_details/{id}")]
    public IActionResult RecordDetails(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the Vinyl and related data
            var recordPlayer = db.Recordplayers
                .Include(v => v.Product)
                    .ThenInclude(v => v.Brand)

                .FirstOrDefault(v => v.ProductId == id);


            if (recordPlayer == null)
            {
                return NotFound(); // Return 404 if no vinyl is found
            }

#pragma warning disable CS8601

            var Products = db.Products
            .Where(p => p.Recordplayers.Any())
            .Where(p => p.Id != id) // Exclude the selected product
            .Select(p => new Models.ViewModels.Product // Map each product to the ViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Price = p.Price,
                // Join ArtistVinyls to get Artist names
                SelectedBrandName = db.Brands.Where(b => b.Id == p.BrandId).Select(b => b.Name).FirstOrDefault() ?? "Unknown",
                PrimaryImageUrl = db.ImageUrls
                .Where(i => i.ProductId == p.Id && i.IsPrimary == true) // Check for IsPrimary being true
                .Select(i => i.Url)
                .FirstOrDefault()
            }).ToList();

            var imageUrls = db.ImageUrls
            .Where(i => i.ProductId == recordPlayer.Product.Id) // Lọc theo ProductId
            .ToList(); // Lấy danh sách đối tượng ImageUrl

            var primaryImageUrl = imageUrls
                .FirstOrDefault(i => i.IsPrimary == true)?.Url; // Lấy URL ảnh chính

            var allImageUrls = imageUrls
                .Select(i => i.Url) // Chuyển đổi sang danh sách URL
                .ToList();

            var viewModel = new RecordPlayerViewModel
            {
                SelectedProduct = new Models.ViewModels.Product
                {
                    ProductId = recordPlayer.Product.Id,
                    ProductName = recordPlayer.Product.Name,
                    ProductDescription = recordPlayer.Product.Description,
                    Price = recordPlayer.Product.Price,
                    ProductQuantity = recordPlayer.Product.Quantity ?? 0,
                    SelectedBrandName = recordPlayer.Product.Brand?.Name ?? "",
                    PrimaryImageUrl = primaryImageUrl, // Gán URL ảnh chính
                    ImageUrls = allImageUrls // Gán danh sách tất cả URL
                },
                Products = Products
            };

            return View("~/Views/Home/RecordPlayer_Details.cshtml", viewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
