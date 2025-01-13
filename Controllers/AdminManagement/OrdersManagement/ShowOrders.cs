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

namespace MuseMusic.Controllers.AdminManagement.OrdersManagement;
[Authorize(Roles = "Admin")]
[Route("admin")]
public class ShowOrders : Controller
{
    private readonly ILogger<ShowOrders> _logger;

    public ShowOrders(ILogger<ShowOrders> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("orders")]
    public IActionResult Customers()
    {
        {
            using (var db = new shopmanagementContext())
            {
                var orderViewModel = new OrderViewModel();

                // Tách dữ liệu ra để tránh lỗi dịch ngữ cảnh
                var orders = db.Orders
                .Include(o => o.Customer)
                .Include(p => p.Payment)
                .ToList();
                if (orders == null)
                {
                    return NotFound(); // Return 404 if no customers is found
                }
                orderViewModel.Orders = orders.Select(x => new MuseMusic.Models.ManagerModels.Orders
                {
                    OrderId = x.Id,
                    CustomerId = x.Customer.Id,
                    CustomerName = x.Customer.Name,
                    created_at = (DateTime)x.CreatedAt,
                    Status = x.Status,
                    Address = x.Customer.Address,
                    PaymentMethod = x.Payment.Method,
                    total = (decimal)x.Total,
                }).ToList();

                return View("~/Views/Admin/Order/Orders.cshtml", orderViewModel);
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
