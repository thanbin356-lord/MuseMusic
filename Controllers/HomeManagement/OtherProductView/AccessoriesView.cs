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
public class AccessoriesView : Controller
{
    private readonly ILogger<AccessoriesView> _logger;

    public AccessoriesView(ILogger<AccessoriesView> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("accessories")]
    public IActionResult Accessories(int page = 1, int pageSize = 12)
    {
        using (var db = new shopmanagementContext())
        {
            // Query to fetch products with associated record player details and primary images
            var query = from p in db.Products
                        join rc in db.Accessories on p.Id equals rc.ProductId
                        join img in db.ImageUrls.Where(img => img.IsPrimary == true) on p.Id equals img.ProductId
                        select new
                        {
                            Product = p,
                            Accessories = rc,
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
            var accessoriesViewModel = new AccessoriesViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalProducts = totalProducts,
                PageSize = pageSize
            };

            // Return the view with the populated model
            return PartialView("~/Views/Home/Accessories.cshtml", accessoriesViewModel);
        }
    }

    [HttpGet("accessoriesdetails/{id}")]
    public IActionResult AccessoriesDetails(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the Vinyl and related data
            var accessories = db.Accessories
                .Include(v => v.Product)
                    .ThenInclude(v => v.Brand)

                .FirstOrDefault(v => v.ProductId == id);


            if (accessories == null)
            {
                return NotFound(); // Return 404 if no vinyl is found
            }

#pragma warning disable CS8601

            var Products = db.Products
            .Where(p => p.Accessories.Any())
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
            .Where(i => i.ProductId == accessories.Product.Id) // Lọc theo ProductId
            .ToList(); // Lấy danh sách đối tượng ImageUrl

            var primaryImageUrl = imageUrls
                .FirstOrDefault(i => i.IsPrimary == true)?.Url; // Lấy URL ảnh chính

            var allImageUrls = imageUrls
                .Select(i => i.Url) // Chuyển đổi sang danh sách URL
                .ToList();

            var viewModel = new AccessoriesViewModel
            {
                SelectedProduct = new Models.ViewModels.Product
                {
                    ProductId = accessories.Product.Id,
                    ProductName = accessories.Product.Name,
                    ProductDescription = accessories.Product.Description,
                    Price = accessories.Product.Price,
                    ProductQuantity = accessories.Product.Quantity ?? 0,
                    SelectedBrandName = accessories.Product.Brand?.Name ?? "",
                    PrimaryImageUrl = primaryImageUrl, // Gán URL ảnh chính
                    ImageUrls = allImageUrls // Gán danh sách tất cả URL
                },
                Products = Products
            };

            return View("~/Views/Home/AccessoriesDetails.cshtml", viewModel);
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}