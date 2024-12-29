using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers.AdminManagement.VinylManagement;

[Route("admin")]
public class VinylController : Controller
{
    private readonly ILogger<VinylController> _logger;

    public VinylController(ILogger<VinylController> logger)
    {
        _logger = logger;
    }


    // Render the Add Vinyl form
    [HttpGet("addvinyl")]
    public IActionResult Addvinyl()
    {
        return View("~/Views/Admin/Vinylmanagement/Addvinyl.cshtml");
    }

    // Handle form submission
    [HttpPost("addvinyl")]
    public IActionResult Addvinyl(MuseMusic.Models.Tables.Vinyl vinyl) // Ensure correct Vinyl type
    {
        if (ModelState.IsValid) // Ensure the submitted data is valid
        {
            try
            {
                using (var db = new shopmanagementContext())
                {
                    // Save the vinyl record to the database
                    db.Vinyls.Add(vinyl);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Vinyl record added successfully!";
                    return RedirectToAction("Vinylmanage");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding vinyl: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while adding the vinyl record.";
            }
        }

        // If validation fails or an error occurs, return to the form
        return View("~/Views/Admin/Vinylmanagement/Addvinyl.cshtml", vinyl);
    }

    [HttpGet("editvinyl/{id}")]
    public IActionResult EditVinyl(int id)
    {
        using (var db = new shopmanagementContext())
        {
            var vinylDetails = (from v in db.Vinyls
                                join p in db.Products on v.ProductId equals p.Id
                                join b in db.Brands on v.BrandId equals b.Id
                                where p.Id == id
                                select new
                                {
                                    ProductId = p.Id,
                                    ProductName = p.Name,
                                    ProductDescription = p.Description,
                                    Price = (decimal)p.Price,
                                    DiskId = v.DiskId,
                                    ProductQuantity = (int)p.Quantity,
                                    Years = (int)v.Years,
                                    Tracklist = v.Tracklist,
                                    BrandId = b.Id,
                                    BrandName = b.Name,
                                    ArtistIds = db.ArtistVinyls
                                        .Where(av => av.VinylId == v.ProductId)
                                        .Select(av => av.ArtistId)
                                        .ToList(),
                                    MoodIds = db.MoodVinyls
                                        .Where(av => av.VinylId == v.ProductId)
                                        .Select(av => av.MoodId)
                                        .ToList(),
                                    CategoriesIds = db.CategoriesVinyls
                                        .Where(av => av.VinylId == v.ProductId)
                                        .Select(av => av.CategoryId)
                                        .ToList(),
                                }).FirstOrDefault();

            if (vinylDetails == null)
            {
                return NotFound(); // Return 404 if no vinyl is found
            }

            // Convert List<int?> to List<int>, excluding null values
            var artistIds = vinylDetails.ArtistIds
                .Where(id => id.HasValue) // Filter out nulls
                .Select(id => id.Value)  // Unwrap nullable values
                .ToList();

            // Map the database Artist to the custom Artist class
            var allArtists = db.Artists
                .Select(a => new VinylController.Artist
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            // Convert List<int?> to List<int>, excluding null values
            var moodIds = vinylDetails.MoodIds
                .Where(id => id.HasValue) // Filter out nulls
                .Select(id => id.Value)  // Unwrap nullable values
                .ToList();

            var allMoods = db.Moods
                .Select(a => new VinylController.Mood
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            var allBrands = db.Brands
                .Select(b => new VinylController.Brand
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

            // Convert List<int?> to List<int>, excluding null values
            var CategoriesIds = vinylDetails.CategoriesIds
                .Where(id => id.HasValue) // Filter out nulls
                .Select(id => id.Value)  // Unwrap nullable values
                .ToList();

            var allCategories = db.Categories
                .Select(a => new VinylController.Categories
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            // Ensure SelectedArtistIds is not null
            var viewModel = new VinylViewModel
            {
                SelectedProduct = new Product
                {
                    ProductId = vinylDetails.ProductId,
                    ProductName = vinylDetails.ProductName,
                    ProductDescription = vinylDetails.ProductDescription,
                    Price = vinylDetails.Price,
                    DiskId = vinylDetails.DiskId,
                    ProductQuantity = vinylDetails.ProductQuantity,
                    Years = vinylDetails.Years,
                    Tracklist = vinylDetails.Tracklist,
                    BrandName = vinylDetails.BrandName
                },
                AllArtists = allArtists,
                SelectedArtistIds = artistIds ?? new List<int>(),  // Ensure it's not null
                AllCategories = allCategories,
                SelectedCategories = CategoriesIds ?? new List<int>(),
                AllMoods = allMoods,
                SelectedMood = moodIds ?? new List<int>(),
                AllBrands = allBrands,
                SelectedBrandId = vinylDetails.BrandId 
            };

            return View("~/Views/Admin/VinylManagement/EditVinyl.cshtml", viewModel);
        }
    }


    [HttpGet("vinylmanage")] //Hiện thị bảng data Vinyl
    public IActionResult Vinylmanage()
    {
        using (var db = new shopmanagementContext())
        {
            var vinyls = (from v in db.Vinyls
                          join p in db.Products on v.ProductId equals p.Id
                          join b in db.Brands on v.BrandId equals b.Id
                          select new
                          {
                              ProductId = p.Id,
                              ProductName = p.Name,
                              ProductDescription = p.Description,
                              Price = (decimal)p.Price,
                              DiskId = v.DiskId,
                              ProductQuantity = (int)p.Quantity,
                              Years = (int)v.Years,
                              Tracklist = v.Tracklist,
                              BrandName = b.Name
                          }).Distinct().OrderBy(p => p.ProductId).ToList();

            var productList = vinyls.Select(vinyl => new Product
            {
                ProductId = vinyl.ProductId,
                ProductName = vinyl.ProductName,
                ProductDescription = vinyl.ProductDescription,
                Price = vinyl.Price,
                DiskId = vinyl.DiskId,
                ProductQuantity = vinyl.ProductQuantity,
                Years = vinyl.Years,
                Tracklist = vinyl.Tracklist,
                BrandName = vinyl.BrandName,
                ArtistNames = string.Join(", ", db.ArtistVinyls
                    .Where(av => av.VinylId == vinyl.ProductId)
                    .Select(av => av.Artist.Name)
                    .ToList()),
                MoodNames = string.Join(", ", db.MoodVinyls
                    .Where(mv => mv.VinylId == vinyl.ProductId)
                    .Select(mv => mv.Mood.Name)
                    .ToList()),
                CategoriesName = string.Join(", ", db.CategoriesVinyls
                    .Where(cv => cv.VinylId == vinyl.ProductId)
                    .Select(cv => cv.Category.Name)
                    .ToList())
            }).ToList();

            var vinylViewModel = new VinylViewModel
            {
                Products = productList
            };

            return View("~/Views/Admin/Vinylmanagement/Vinylmanage.cshtml", vinylViewModel);
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public partial class VinylViewModel
    {
        public List<Product> Products { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Mood> Moods { get; set; }
        public List<Category> Categories { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Artist> AllArtists { get; set; } // All artists for dropdown
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        public List<Mood> AllMoods { get; set; }
        public List<int> SelectedMood { get; set; }

        public List<Categories> AllCategories { get; set; } // All artists for dropdown
        public List<int> SelectedCategories { get; set; }
        public int SelectedBrandId { get; set; }

        public List<Brand> AllBrands { get; set; }

    }
    public class Vinyl
    {
        public int Id { get; set; }
        public string DiskId { get; set; }
        public int? Years { get; set; }
        public string Tracklist { get; set; }

        public List<Mood> Moods { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Artists { get; set; }  // This will hold artist names for the view
    }
    public class Product
    {
        public List<Vinyl> Vinyls { get; set; }
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
        public int Years { get; set; }
        public string Tracklist { get; set; }
        public string BrandName { get; set; }
        public string ArtistNames { get; set; }
        public string MoodNames { get; set; }
        public string CategoriesName { get; set; }
    }
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Mood
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
