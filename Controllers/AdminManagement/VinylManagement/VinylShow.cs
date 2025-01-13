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
public class VinylShow : Controller
{
    private readonly ILogger<VinylShow> _logger;

    public VinylShow(ILogger<VinylShow> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    [HttpGet("vinylmanage")] //Hiện thị bảng data Vinyl
    public IActionResult Vinylmanage()
    {
        using (var db = new shopmanagementContext())
        {
            var vinyls = db.Vinyls
                .Include(v => v.Product)
                    .ThenInclude(v => v.Brand)
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
                BrandName = v.Product.Brand.Name,
                MoodNames = string.Join(", ", v.MoodVinyls.Select(mv => mv.Mood.Name)),
                CategoriesName = string.Join(", ", v.CategoriesVinyls.Select(cv => cv.Category.Name)),
                ArtistNames = string.Join(", ", v.ArtistVinyls.Select(av => av.Artist.Name))
            }).ToList();

            return View("~/Views/Admin/Vinylmanagement/Vinylmanage.cshtml", listVinylViewModel);
        }
    }
    [HttpPost("delete-vinyls")]
    public IActionResult DeleteVinyls([FromBody] List<int> productIds)
    {
        if (productIds == null || productIds.Count == 0)
        {
            return BadRequest("No products selected for deletion.");
        }

        try
        {
            using (var db = new shopmanagementContext())
            {
                var vinylsToDelete = db.Vinyls.Where(v => productIds.Contains(v.Product.Id)).ToList();

                if (vinylsToDelete.Count == 0)
                {
                    return NotFound("No matching products found for deletion.");
                }

                db.Vinyls.RemoveRange(vinylsToDelete);
                db.SaveChanges();
            }

            return Ok("Products deleted successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error deleting vinyls: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}