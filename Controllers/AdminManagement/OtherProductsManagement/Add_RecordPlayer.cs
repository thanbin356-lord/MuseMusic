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
public class Add_RecordPlayer : Controller
{
    private readonly ILogger<Add_RecordPlayer> _logger;

    public Add_RecordPlayer(ILogger<Add_RecordPlayer> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("addrecordplayer")]
    public IActionResult AddRecordPlayer()
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
            var recordModel = new RecordPlayerViewModel
            {
                AllBrands = allBrands,
                SelectedRecordPlayer = new MuseMusic.Models.ManagerModels.ProductRecordPlayer(), // Initialize an empty product for the form
                SelectedBrandId = 0 // Default to no brand selected
            };

            return View("~/Views/Admin/RecordPlayerManagement/AddRecordPlayer.cshtml", recordModel);
        }
    }

    [HttpPost("addrecordplayer")]
    public IActionResult AddRecordPlayer(AddRecordPlayerModel model)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                // Create a new Vinyl entry
                var newRecordPlayer = new MuseMusic.Models.Tables.Recordplayer
                {
                    Product = new MuseMusic.Models.Tables.Product
                    {
                        Id = model.SelectedRecordPlayer.ProductId,
                        Name = model.SelectedRecordPlayer.ProductName,
                        Description = model.SelectedRecordPlayer.ProductDescription,
                        Price = model.SelectedRecordPlayer.Price,
                        Quantity = model.SelectedRecordPlayer.ProductQuantity,
                        BrandId = model.SelectedBrandId,
                    },
                        Motor = model.SelectedRecordPlayer.Motor,
                        Speed = model.SelectedRecordPlayer.Speed,
                        
                };

                // Add the new Vinyl to the database
                db.Recordplayers.Add(newRecordPlayer);
                db.SaveChanges(); // Save changes to the database
            }
            return Redirect("/admin/recordplayer");
        }
        else
        {
            // If the form is invalid, log the errors and return the view with validation messages
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            // Return to the AddVinyl view with validation errors
            return View("~/Views/Admin/RecordPlayerManagement/AddRecordPlayer.cshtml", model);
        }
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
