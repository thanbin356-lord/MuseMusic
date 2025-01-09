using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;
using MuseMusic.Models.ManagerModels;
using Microsoft.EntityFrameworkCore;

namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement;

[Route("admin")]
public class RecordPlayerShow : Controller
{
    private readonly ILogger<RecordPlayerShow> _logger;

    public RecordPlayerShow(ILogger<RecordPlayerShow> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("recordplayer")]
    public IActionResult RecordPlayer()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var recordplayers = db.Recordplayers
                // bước 2 load dữ liệu từ db 
            .Include(v => v.Product)
                .ThenInclude(v => v.Brand)
            .ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                var listrecordPlayerViewModel = recordplayers.Select(x => new ListRecordPlayerView
                    {
                        ProductId = x.Product.Id,
                        ProductName = x.Product.Name,
                        ProductQuantity = x.Product.Quantity,
                        ProductDescription = x.Product.Description,
                        Price = x.Product.Price,
                        BrandName = x.Product.Brand.Name,
                    }).ToList();

                // bước 4 return l 
                return View("~/Views/Admin/RecordPlayerManagement/RecordPlayer.cshtml", listrecordPlayerViewModel);
            }
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}