using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
using System.ComponentModel.DataAnnotations;
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
    public IActionResult AddVinyl()
    {
        using (var db = new shopmanagementContext())
        {
            var allMoods = db.Moods
                .Select(m => new Mood
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToList();

            var allBrands = db.Brands
                .Select(b => new Brand
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToList();

            var allCategories = db.Categories
                .Select(c => new Categories
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            var allArtists = db.Artists
                .Select(a => new Artist
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
                SelectedProduct = new Product(), // Initialize an empty product for the form
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
                    },
                    Tracklist = model.SelectedProduct.Tracklist,
                    Years = model.SelectedProduct.Years,
                    DiskId = model.SelectedProduct.DiskId,
                    BrandId = model.SelectedBrandId
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
            return RedirectToAction("Vinylmanage");
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




    [HttpGet("editvinyl/{id}")]
    public IActionResult EditVinyl(int id)
    {
        using (var db = new shopmanagementContext())
        {
            // Fetch the Vinyl and related data
            var vinyl = db.Vinyls
                .Include(v => v.Product)
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

            // Map Vinyl data to the view model
            var viewModel = new VinylViewModel
            {
                SelectedProduct = new Product
                {
                    ProductId = vinyl.Product.Id,
                    ProductName = vinyl.Product.Name,
                    ProductDescription = vinyl.Product.Description,
                    Price = vinyl.Product.Price,
                    ProductQuantity = vinyl.Product.Quantity ?? 0,
                    DiskId = vinyl.DiskId,
                    Years = vinyl.Years ?? 0,
                    Tracklist = vinyl.Tracklist
                },
                AllArtists = db.Artists
                    .Select(a => new Artist { Id = a.Id, Name = a.Name })
                    .ToList(),
                SelectedArtistIds = vinyl.ArtistVinyls
                    .Select(av => av.ArtistId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllMoods = db.Moods
                    .Select(m => new Mood { Id = m.Id, Name = m.Name })
                    .ToList(),
                SelectedMood = vinyl.MoodVinyls
                    .Select(mv => mv.MoodId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllCategories = db.Categories
                    .Select(c => new Categories { Id = c.Id, Name = c.Name })
                    .ToList(),
                SelectedCategories = vinyl.CategoriesVinyls
                    .Select(cv => cv.CategoryId)
                    .Where(id => id.HasValue)   // Filter out null values
                    .Select(id => id.Value)    // Cast to non-nullable int
                    .ToList(),
                AllBrands = db.Brands
                    .Select(b => new Brand { Id = b.Id, Name = b.Name })
                    .ToList(),
                SelectedBrandId = vinyl.BrandId ?? 0
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
                vinyl.BrandId = model.SelectedBrandId;

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

    [HttpGet("vinylmanage")] //Hiện thị bảng data Vinyl
    public IActionResult Vinylmanage()
    {
        using (var db = new shopmanagementContext())
        {
            var vinyls = db.Vinyls
                .Include(v => v.Product)
                .Include(v => v.Brand)
                .Include(v => v.ArtistVinyls)
                    .ThenInclude(av => av.Artist)
                .Include(v => v.MoodVinyls)
                    .ThenInclude(mv => mv.Mood)
                .Include(v => v.CategoriesVinyls)
                    .ThenInclude(cv => cv.Category)
                .ToList();

            var listVinylViewModel = vinyls.Select(v => new ListVinylViewModel
            {
                ProductId = v.Product.Id,
                DiskId = v.DiskId,
                ProductName = v.Product.Name,
                ProductDescription = v.Product.Description,
                ProductQuantity = v.Product.Quantity,
                Price = v.Product.Price,
                Years = v.Years,
                BrandName = v.Brand.Name,
                MoodNames = string.Join(", ", v.MoodVinyls.Select(mv => mv.Mood.Name)),
                CategoriesName = string.Join(", ", v.CategoriesVinyls.Select(cv => cv.Category.Name)),
                ArtistNames = string.Join(", ", v.ArtistVinyls.Select(av => av.Artist.Name))
            }).ToList();

            return View("~/Views/Admin/Vinylmanagement/Vinylmanage.cshtml", listVinylViewModel);
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

        [Required]
        public Product SelectedProduct { get; set; }
        public List<Artist> AllArtists { get; set; } // All artists for dropdown

        [Required]
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        public List<Mood> AllMoods { get; set; }

        [Required]
        public List<int> SelectedMood { get; set; }

        public List<Categories> AllCategories { get; set; } // All artists for dropdown

        [Required]
        public List<int> SelectedCategories { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }

        public List<Brand> AllBrands { get; set; }

    }

    public partial class AddVinylModel
    {
        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        [Required]
        public List<int> SelectedMood { get; set; }

        [Required]
        public List<int> SelectedCategories { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class EditVinylModel
    {
        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        [Required]
        public List<int> SelectedMood { get; set; }

        [Required]
        public List<int> SelectedCategories { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class ListVinylViewModel
    {
        public string DiskId;
        public int? ProductId;
        public string ProductName;
        public string ProductDescription;
        public int? ProductQuantity;
        public decimal Price;
        public int? Years;
        public string MoodNames;
        public string CategoriesName;
        public string ArtistNames;
        public string BrandName;
    }


    public class Vinyl
    {
        public int Id { get; set; }
        public string DiskId { get; set; }
        public int? Years { get; set; }
        public string Tracklist { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // New property
        public decimal Price { get; set; }

        public int ProductQuantity { get; set; }
        public string ProductDescription { get; set; }

        public List<Mood> Moods { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Artists { get; set; }  // This will hold artist names for the view
        public Product Product { get; set; }
        public int BrandId { get; internal set; }
    }
    public class Product
    {
        // public List<Vinyl> Vinyls { get; set; }
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
        public int Years { get; set; }
        public string Tracklist { get; set; }
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