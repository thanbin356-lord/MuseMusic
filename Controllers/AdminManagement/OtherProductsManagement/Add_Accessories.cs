using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ManagerModels;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement;
[Authorize(Roles = "Admin")]
[Route("admin")]
public class Add_Accessories : Controller
{
    private readonly ILogger<Add_Accessories> _logger;

    public Add_Accessories(ILogger<Add_Accessories> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("addaccessories")]
    public IActionResult AddAccessories()
    {
        using (var db = new shopmanagementContext())
        {

            var allBrands = db.Brands
                .Select(b => new MuseMusic.Models.ManagerModels.Brand
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

            // Create a new instance of VinylViewModel to pass to the view
            var accessoriesModel = new AccessoriesViewModel
            {
                AllBrands = allBrands,
                SelectedAccessories = new MuseMusic.Models.ManagerModels.ProductAccessories(), // Initialize an empty product for the form
                SelectedBrandId = 0 // Default to no brand selected
            };

            return View("~/Views/Admin/AccessoriesManagement/AddAccessories.cshtml", accessoriesModel);
        }
    }

    [HttpPost("addaccessories")]
    public IActionResult AddAccessories(AddAccessoriesModel model)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                // Create a new Vinyl entry
                var newAccessories = new MuseMusic.Models.Tables.Accessory
                {
                    Product = new MuseMusic.Models.Tables.Product
                    {
                        Id = model.SelectedAccessories.ProductId,
                        Name = model.SelectedAccessories.ProductName,
                        Description = model.SelectedAccessories.ProductDescription,
                        Price = model.SelectedAccessories.Price,
                        Quantity = model.SelectedAccessories.ProductQuantity,
                        BrandId = model.SelectedBrandId,
                    },
                };

                // Add the new Vinyl to the database
                db.Accessories.Add(newAccessories);
                db.SaveChanges(); // Save changes to the database
            }
            return Redirect("/admin/accessories");
        }
        else
        {
            // If the form is invalid, log the errors and return the view with validation messages
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            // Return to the AddVinyl view with validation errors
            return View("~/Views/Admin/AccessoriesManagement/AddAccessories.cshtml", model);
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}