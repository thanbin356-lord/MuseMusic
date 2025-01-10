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
public class Edit_Vinyl : Controller
{
    private readonly ILogger<Edit_Vinyl> _logger;

    public Edit_Vinyl(ILogger<Edit_Vinyl> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("editvinyl/{id}")]
    public IActionResult EditVinyl(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the Vinyl and related data
            var vinyl = db.Vinyls
                .Include(v => v.Product)
                    .ThenInclude(br => br.Brand)
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
            var images = db.ImageUrls
                        .Where(img => img.ProductId == vinyl.Product.Id)
                        .ToList();
            // Map Vinyl data to the view model
            var viewModel = new VinylViewModel
            {
                SelectedProduct = new MuseMusic.Models.ManagerModels.Product
                {
                    ProductId = vinyl.Product.Id,
                    ProductName = vinyl.Product.Name,
                    ProductDescription = vinyl.Product.Description,
                    Price = vinyl.Product.Price,
                    ProductQuantity = vinyl.Product.Quantity ?? 0,
                    DiskId = vinyl.DiskId,
                    Years = vinyl.Years ?? 0,
                    Tracklist = vinyl.Tracklist,

                },
                AllArtists = db.Artists
                    .Select(a => new MuseMusic.Models.ManagerModels.Artist { Id = a.Id, Name = a.Name })
                    .ToList(),
                SelectedArtistIds = vinyl.ArtistVinyls
                    .Select(av => av.ArtistId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllMoods = db.Moods
                    .Select(m => new MuseMusic.Models.ManagerModels.Mood { Id = m.Id, Name = m.Name })
                    .ToList(),
                SelectedMood = vinyl.MoodVinyls
                    .Select(mv => mv.MoodId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllCategories = db.Categories
                    .Select(c => new MuseMusic.Models.ManagerModels.Categories { Id = c.Id, Name = c.Name })
                    .ToList(),
                SelectedCategories = vinyl.CategoriesVinyls
                    .Select(cv => cv.CategoryId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllBrands = db.Brands
                    .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                    .ToList(),
                SelectedBrandId = vinyl.Product.BrandId ?? 0,

                AllImages = images.Select(img => new ImageUrl { Id = img.Id, Url = img.Url }).ToList(),
                SelectedImageId = images.Any() ? images.First().Id : 0 // Use the primary image if available
            };

            return View("~/Views/Admin/VinylManagement/EditVinyl.cshtml", viewModel);
        }
    }


    [HttpPost("editvinyl/{id}")]
    public IActionResult EditVinyl(EditVinylModel model, int id)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                var vinyl = db.Vinyls
                    .Include(v => v.Product)
                    .Include(v => v.ArtistVinyls)
                    .Include(v => v.CategoriesVinyls)
                    .Include(v => v.MoodVinyls)
                    .FirstOrDefault(v => v.ProductId == id);

                if (vinyl == null)
                {
                    return NotFound(); // Return 404 if vinyl not found
                }

                // Update the Vinyl details
                vinyl.Product.Name = model.SelectedProduct.ProductName;
                vinyl.DiskId = model.SelectedProduct.DiskId;
                vinyl.Product.Price = model.SelectedProduct.Price;
                vinyl.Years = model.SelectedProduct.Years;
                vinyl.Tracklist = model.SelectedProduct.Tracklist;
                vinyl.Product.Quantity = model.SelectedProduct.ProductQuantity;
                vinyl.Product.Description = model.SelectedProduct.ProductDescription;
                vinyl.Product.BrandId = model.SelectedBrandId;

                vinyl.ArtistVinyls.Clear();
                // Add new ArtistVinyls based on the provided artistIds
                foreach (var artistId in model.SelectedArtistIds)
                {
                    var artistVinyl = new ArtistVinyl
                    {
                        VinylId = vinyl.Id,
                        ArtistId = artistId
                    };
                    vinyl.ArtistVinyls.Add(artistVinyl);
                }

                vinyl.CategoriesVinyls.Clear();
                foreach (var categoryId in model.SelectedCategories)
                {
                    var categoriesVinyl = new CategoriesVinyl
                    {
                        VinylId = vinyl.Id,
                        CategoryId = categoryId
                    };
                    vinyl.CategoriesVinyls.Add(categoriesVinyl);
                }

                vinyl.MoodVinyls.Clear();
                foreach (var moodIds in model.SelectedMood)
                {
                    var moodVinyl = new MoodVinyl
                    {
                        VinylId = vinyl.Id,
                        MoodId = moodIds
                    };
                    vinyl.MoodVinyls.Add(moodVinyl);
                }

                db.SaveChanges(); // Save the changes to the database
            }

            return RedirectToAction("EditVinyl", new { id = id });
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View("~/Views/Admin/Vinylmanagement/editvinyl.cshtml"); // Return the model with validation errors if the form is not valid
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
