using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ManagerModels;

namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement;

[Route("admin")]
public class Edit_RecordPlayer : Controller
{
    private readonly ILogger<Edit_RecordPlayer> _logger;

    public Edit_RecordPlayer(ILogger<Edit_RecordPlayer> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("editrecordplayer/{id}")]
    public IActionResult EditRecordPlayer(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the Vinyl and related data
            var recordplayer = db.Recordplayers
                .Include(v => v.Product)
                .FirstOrDefault(v => v.ProductId == id);

            if (recordplayer == null)
            {
                return NotFound(); // Return 404 if no vinyl is found
            }
            var images = db.ImageUrls
                        .Where(img => img.ProductId == recordplayer.Product.Id)
                        .ToList();
            // Map Vinyl data to the view model
            var viewModel = new RecordPlayerViewModel
            {
                SelectedProduct = new Models.ManagerModels.Product
                {
                    ProductId = recordplayer.Product.Id,
                    ProductName = recordplayer.Product.Name,
                    ProductDescription = recordplayer.Product.Description,
                    Price = recordplayer.Product.Price,

                    ProductQuantity = recordplayer.Product.Quantity ?? 0,
                },
                AllBrands = db.Brands
                    .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                    .ToList(),
                SelectedBrandId = recordplayer.BrandId ?? 0,

                AllImages = images.Select(img => new ImageUrl { Id = img.Id, Url = img.Url }).ToList(),
                SelectedImageId = images.Any() ? images.First().Id : 0 // Use the primary image if available
            };

            return View("~/Views/Admin/RecordPlayerManagement/EditRecordPlayer.cshtml", viewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
