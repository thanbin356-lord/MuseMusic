using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ManagerModels;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement;

[Route("admin")]
public class Edit_Accessories : Controller
{
    private readonly ILogger<Edit_Accessories> _logger;

    public Edit_Accessories(ILogger<Edit_Accessories> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("editaccessories/{id}")]
    public IActionResult EditAccessories(int id)
    {
        using (var db = new shopmanagementContext())
        {

            var accessories = db.Accessories
                .Include(v => v.Product)

                .FirstOrDefault(v => v.ProductId == id);

            if (accessories == null)
            {
                return NotFound(); // Return 404 if no accessories is found
            }
            var images = db.ImageUrls
                        .Where(img => img.ProductId == accessories.Product.Id)
                        .ToList();

            var viewModel = new AccessoriesViewModel
            {
                SelectedAccessories = new Models.ManagerModels.ProductAccessories
                {
                    ProductId = accessories.Product.Id,
                    ProductName = accessories.Product.Name,
                    ProductDescription = accessories.Product.Description,
                    Price = accessories.Product.Price,
                    ProductQuantity = accessories.Product.Quantity ?? 0,

                },
                AllBrands = db.Brands
                    .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                    .ToList(),
                SelectedBrandId = accessories.Product.BrandId ?? 0,

                AllImages = images.Select(img => new ImageUrl { Id = img.Id, Url = img.Url }).ToList(),
                SelectedImageId = images.Any() ? images.First().Id : 0 // Use the primary image if available
            };

            return View("~/Views/Admin/AccessoriesManagement/editaccessories.cshtml", viewModel);
        }
    }

    [HttpPost("editaccessories/{id}")]
    public IActionResult EditAccessories(EditAccessoriesModel model, int id)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                var accessories = db.Accessories
                    .Include(v => v.Product)
                    .FirstOrDefault(v => v.ProductId == id);

                if (accessories == null)
                {
                    return NotFound(); // Return 404 if accessories not found
                }

                accessories.Product.Name = model.SelectedAccessories.ProductName;
                accessories.Product.Price = model.SelectedAccessories.Price;
                accessories.Product.Quantity = model.SelectedAccessories.ProductQuantity;
                accessories.Product.Description = model.SelectedAccessories.ProductDescription;
                accessories.Product.BrandId = model.SelectedBrandId;

                db.SaveChanges(); // Save the changes to the database
            }

            return RedirectToAction("EditAccessories", new { id = id });
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View("~/Views/Admin/AccessoriesManagement/editaccessories.cshtml", model); // Return the model with validation errors if the form is not valid
        }

    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
