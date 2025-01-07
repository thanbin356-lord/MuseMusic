using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ViewModels;

namespace MuseMusic.Controllers.HomeManagement.VinylView;

[Route("home")]
public class VinylView : Controller
{
    private readonly ILogger<VinylView> _logger;

    public VinylView(ILogger<VinylView> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("vinyl")]
    public IActionResult Vinyl()
    {
        using (var db = new shopmanagementContext())
        {
            var vinylViewModel = new VinylViewModel
            {
                Products = (from p in db.Products
                            join v in db.Vinyls on p.Id equals v.ProductId
                            join img in db.ImageUrls.Where(img => (bool)img.IsPrimary) on p.Id equals img.ProductId
                            let artistNames = (from av in db.ArtistVinyls
                                               join a in db.Artists on av.ArtistId equals a.Id
                                               where av.VinylId == v.Id
                                               select a.Name).ToList()
                            select new Models.ViewModels.Product
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                ProductImage = img.Url,
                                ProductDescription = p.Description,
                                Price = p.Price.ToString("C") ?? "N/A",
                                DiskId = v.DiskId,
                                Tracklist = v.Tracklist,
                                ArtistNames = string.Join(", ", artistNames)
                                // Status = v.Status // Assume this field exists to indicate preorder, etc.
                            }).ToList()
            };

            return View("~/Views/Home/Vinyl.cshtml", vinylViewModel);
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}

