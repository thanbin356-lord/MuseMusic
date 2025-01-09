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

namespace MuseMusic.Controllers.AdminManagement.CustomerManagement;

[Route("admin")]
public class CustomerShow : Controller
{
    private readonly ILogger<CustomerShow> _logger;

    public CustomerShow(ILogger<CustomerShow> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("customers")]
    public IActionResult Customers()
    {
        {
            using (var db = new shopmanagementContext())
            {
                var customerViewModel = new OrderViewModel();

                // Tách dữ liệu ra để tránh lỗi dịch ngữ cảnh
                var customers = db.Orders
                .Include(o => o.Customer)
                .ToList();
                if (customers == null)
                {
                    return NotFound(); // Return 404 if no customers is found
                }
                customerViewModel.Orders = customers.Select(x => new MuseMusic.Models.ManagerModels.Orders
                {
                    CustomerId = x.Customer.Id,
                    CustomerName = x.Customer.Name,
                    Email = db.Accounts.FirstOrDefault(a => a.Id == x.Customer.AccountId)?.Email,
                    Phone = x.Customer.Phone,
                    Address = x.Customer.Address,
                    OrderCount = db.Orders.Count(o => o.CustomerId == x.Customer.Id)
                }).ToList();

                return View("~/Views/Admin/CustomerManagement/Customers.cshtml", customerViewModel);
            }
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
