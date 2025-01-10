using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ManagerModels;
using MuseMusic.Models.Tables;

namespace MuseMusic.Controllers.AdminManagement.VinylManagement;


[Authorize(Roles = "Admin")]
[Route("admin")]
public class Add_Vinyl : Controller
{
    private readonly ILogger<Add_Vinyl> _logger;

    public Add_Vinyl(ILogger<Add_Vinyl> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("addvinyl")]
    public IActionResult AddVinyl()
    {
        using (var db = new shopmanagementContext())
        {
            var allMoods = db.Moods
                .Select(m => new MuseMusic.Models.ManagerModels.Mood
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToList();

            var allBrands = db.Brands
                .Select(b => new MuseMusic.Models.ManagerModels.Brand
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

            var allCategories = db.Categories
                .Select(c => new MuseMusic.Models.ManagerModels.Categories
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            var allArtists = db.Artists
                .Select(a => new MuseMusic.Models.ManagerModels.Artist
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            // Create a new instance of VinylViewModel to pass to the view
            var viewModel = new VinylViewModel
            {
                AllMoods = allMoods,
                AllBrands = allBrands,
                AllCategories = allCategories,
                AllArtists = allArtists,
                SelectedProduct = new MuseMusic.Models.ManagerModels.Product(), // Initialize an empty product for the form
                SelectedMood = new List<int>(),
                SelectedCategories = new List<int>(),
                SelectedArtistIds = new List<int>(),
                SelectedBrandId = 0 // Default to no brand selected
            };

            return View("~/Views/Admin/VinylManagement/AddVinyl.cshtml", viewModel);
        }
    }

    [HttpPost("addvinyl")]
    public IActionResult AddVinyl(AddVinylModel model)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                // Create a new Vinyl entry
                var newVinyl = new MuseMusic.Models.Tables.Vinyl
                {
                    Product = new MuseMusic.Models.Tables.Product
                    {
                        Id = model.SelectedProduct.ProductId,
                        Name = model.SelectedProduct.ProductName,
                        Description = model.SelectedProduct.ProductDescription,
                        Price = model.SelectedProduct.Price,
                        Quantity = model.SelectedProduct.ProductQuantity,
                        BrandId = model.SelectedBrandId
                    },
                    Tracklist = model.SelectedProduct.Tracklist,
                    Years = model.SelectedProduct.Years,
                    DiskId = model.SelectedProduct.DiskId,

                };

                // Add the associated artists
                foreach (var artistId in model.SelectedArtistIds)
                {
                    var artistVinyl = new ArtistVinyl
                    {
                        Vinyl = newVinyl,
                        ArtistId = artistId
                    };
                    newVinyl.ArtistVinyls.Add(artistVinyl);
                }

                // Add the associated categories
                foreach (var categoryId in model.SelectedCategories)
                {
                    var categoriesVinyl = new CategoriesVinyl
                    {
                        Vinyl = newVinyl,
                        CategoryId = categoryId
                    };
                    newVinyl.CategoriesVinyls.Add(categoriesVinyl);
                }

                // Add the associated moods
                foreach (var moodId in model.SelectedMood)
                {
                    var moodVinyl = new MoodVinyl
                    {
                        Vinyl = newVinyl,
                        MoodId = moodId
                    };
                    newVinyl.MoodVinyls.Add(moodVinyl);
                }

                // Add the new Vinyl to the database
                db.Vinyls.Add(newVinyl);
                db.SaveChanges(); // Save changes to the database
            }

            // Redirect to the Vinyl management page after the new vinyl is added
            return Redirect("/admin/vinylmanage");
        }
        else
        {
            // If the form is invalid, log the errors and return the view with validation messages
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            // Return to the AddVinyl view with validation errors
            return View("~/Views/Admin/VinylManagement/AddVinyl.cshtml", model);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
