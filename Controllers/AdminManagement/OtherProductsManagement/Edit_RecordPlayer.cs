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

            var recordplayer = db.Recordplayers
                .Include(v => v.Product)

                .FirstOrDefault(v => v.ProductId == id);

            if (recordplayer == null)
            {
                return NotFound(); // Return 404 if no recordplayer is found
            }
            var images = db.ImageUrls
                        .Where(img => img.ProductId == recordplayer.Product.Id)
                        .ToList();

            var viewModel = new RecordPlayerViewModel
            {
                SelectedRecordPlayer = new Models.ManagerModels.ProductRecordPlayer
                {
                    ProductId = recordplayer.Product.Id,
                    ProductName = recordplayer.Product.Name,
                    ProductDescription = recordplayer.Product.Description,
                    Price = recordplayer.Product.Price,
                    Motor = recordplayer.Motor,
                    Speed = recordplayer.Speed,
                    ProductQuantity = recordplayer.Product.Quantity ?? 0,
                    
                },
                AllBrands = db.Brands
                    .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                    .ToList(),
                SelectedBrandId = recordplayer.Product.BrandId ?? 0,

                AllImages = images.Select(img => new ImageUrl { Id = img.Id, Url = img.Url }).ToList(),
                SelectedImageId = images.Any() ? images.First().Id : 0 // Use the primary image if available
            };

            return View("~/Views/Admin/RecordPlayerManagement/EditRecordPlayer.cshtml", viewModel);
        }
    }

    [HttpPost("editrecordplayer/{id}")]
    public IActionResult EditRecordPlayer(EditRecordPlayerModel model, int id)
    {
        if (ModelState.IsValid)
        {
            using (var db = new shopmanagementContext())
            {
                var recordplayer = db.Recordplayers
                    .Include(v => v.Product)
                    .FirstOrDefault(v => v.ProductId == id);

                if (recordplayer == null)
                {
                    return NotFound(); // Return 404 if recordplayer not found
                }

                recordplayer.Product.Name = model.SelectedRecordPlayer.ProductName;
                recordplayer.Product.Price = model.SelectedRecordPlayer.Price;
                recordplayer.Product.Quantity = model.SelectedRecordPlayer.ProductQuantity;
                recordplayer.Product.Description = model.SelectedRecordPlayer.ProductDescription;
                recordplayer.Product.BrandId = model.SelectedBrandId;
                recordplayer.Motor = model.SelectedRecordPlayer.Motor;
                recordplayer.Speed = model.SelectedRecordPlayer.Speed;

                db.SaveChanges(); // Save the changes to the database
            }

            return RedirectToAction("EditRecordPlayer", new { id = id });
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View("~/Views/Admin/RecordPlayerManagement/editrecordplayer.cshtml",model); // Return the model with validation errors if the form is not valid
        }

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
