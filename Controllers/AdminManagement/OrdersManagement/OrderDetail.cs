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

namespace MuseMusic.Controllers.AdminManagement.OrdersManagement;

[Route("admin")]
public class OrderDetail : Controller
{
    private readonly ILogger<OrderDetail> _logger;

    public OrderDetail(ILogger<OrderDetail> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("orderdetail/{id}")]
    public IActionResult OrderDetails(int id)
    {
        using (var db = new shopmanagementContext())
        {

            var customers = db.Orders
            .Include(o => o.Customer)
                .ThenInclude(ac => ac.Account)
            .Include(od => od.OrderDetails)
                .ThenInclude(p => p.Product)
                .ThenInclude(v => v.Vinyls)
                    .ThenInclude(a => a.ArtistVinyls)
                        .ThenInclude(av => av.Artist)
            .Include(od => od.OrderDetails)
                .ThenInclude(p => p.Product)
                    .ThenInclude(b => b.Brand)
            .FirstOrDefault(o => o.Id == id);
            if (customers == null)
            {
                return NotFound(); // Return 404 if no customers is found
            }
            // var images = db.ImageUrls
            //             .Where(img => img.CustomerId == customers.Customer.Id)
            //             .ToList()
            var orderCount = db.Orders
                       .Where(o => o.CustomerId == id)
                       .Count();
            var viewModel = new OrderViewModel
            {
                SelectedCustomerOrders = new Models.ManagerModels.Orders
                {
                    CustomerId = customers.Customer.Id,
                    CustomerName = customers.Customer.Name,
                    Phone = customers.Customer.Phone,
                    Email = customers.Customer.Account.Email,
                    Address = customers.Customer.Address,
                    OrderCount = orderCount,
                    OrderId = customers.Id,
                    created_at = (DateTime)customers.CreatedAt,
                    total = (decimal)customers.Total,


                    AllProducts = customers.OrderDetails.Select(od => new Models.ManagerModels.ListProduct
                    {
                        ProductName = od.Product.Name,
                        BrandName = od.Product.Brand?.Name,
                        ArtistNames = od.Product.Vinyls
                        .SelectMany(v => v.ArtistVinyls)
                        .Where(av => av.Artist != null)
                        .Select(av => av.Artist.Name)
                        .ToList(),
                        Price = od.Price,  // Price from OrderDetail
                        Quantity = od.Quantity  // Quantity from OrderDetail
                    }).ToList()
                }
                // AllBrands = db.Brands
                //     .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                //     .ToList(),
                // SelectedBrandId = customers.BrandId ?? 0,
            };

            return View("~/Views/Admin/Order/OrderDetail.cshtml", viewModel);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
