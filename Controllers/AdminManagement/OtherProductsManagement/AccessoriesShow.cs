using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ManagerModels;
using MuseMusic.Models.Tables;

namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement;

[Route("admin")]
public class AccessoriesShow : Controller
{
    private readonly ILogger<AccessoriesShow> _logger;

    public AccessoriesShow(ILogger<AccessoriesShow> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("accessories")]
    public IActionResult RecordPlayer()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var accessories = db.Accessories
                // bước 2 load dữ liệu từ db 
            .Include(v => v.Product)
            .Include(v => v.Brand)
            .ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                var listAcessoriesViews = accessories.Select(x => new ListAcessoriesView
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    ProductQuantity = x.Product.Quantity,
                    ProductDescription = x.Product.Description,
                    Price = x.Product.Price,
                    BrandName = x.Brand.Name,
                }).ToList();

                // bước 4 return l 
                return View("~/Views/Admin/AccessoriesManagement/Accessories.cshtml", listAcessoriesViews);
            }
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}